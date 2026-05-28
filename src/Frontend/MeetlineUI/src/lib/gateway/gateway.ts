import {
  HubConnectionBuilder,
  HubConnectionState,
  LogLevel,
} from '@microsoft/signalr'
import { env } from '#/env.ts'
import { mountEventHandlers } from './events'
import type { HubConnection } from '@microsoft/signalr'

type TokenGetter = () => Promise<string | null>

class Gateway {
  private connection: HubConnection | null = null
  private tokenGetter: TokenGetter | null = null
  private disconnectPromise: Promise<void> | null = null
  private reconnectTimeoutId: ReturnType<typeof setTimeout> | null = null

  initialize(getToken: TokenGetter) {
    this.tokenGetter = getToken

    if (this.connection) return

    this.connection = new HubConnectionBuilder()
      .withUrl(`${env.VITE_API_BASE_URL}/api/gateway`, {
        accessTokenFactory: async () => (await this.tokenGetter?.()) ?? '',
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build()

    mountEventHandlers(this.connection)
  }

  async connect() {
    if (this.reconnectTimeoutId) {
      clearTimeout(this.reconnectTimeoutId)
      this.reconnectTimeoutId = null
    }

    if (this.disconnectPromise) {
      await this.disconnectPromise
    }

    const connectionToStart = this.connection
    if (!connectionToStart) {
      throw new Error('Gateway not initialized')
    }

    if (connectionToStart.state !== HubConnectionState.Disconnected) {
      return
    }

    try {
      await connectionToStart.start()
    } catch (err) {
      if (this.connection !== connectionToStart) {
        return
      }

      console.error(
        'SignalR connection failed to start, retrying in 5s...',
        err,
      )

      this.reconnectTimeoutId = setTimeout(() => {
        if (this.connection === connectionToStart) {
          this.connect()
        }
      }, 5000)
    }
  }

  async disconnect() {
    if (this.reconnectTimeoutId) {
      clearTimeout(this.reconnectTimeoutId)
      this.reconnectTimeoutId = null
    }

    if (!this.connection) return

    const connectionToStop = this.connection
    this.connection = null
    this.tokenGetter = null

    this.disconnectPromise = connectionToStop
      .stop()
      .catch((err) => {
        console.error('Failed to stop gateway connection gracefully:', err)
      })
      .finally(() => {
        this.disconnectPromise = null
      })

    await this.disconnectPromise
  }
}

export const gateway = new Gateway()
