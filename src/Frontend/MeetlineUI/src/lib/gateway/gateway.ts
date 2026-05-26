import {
  HubConnectionBuilder,
  HubConnectionState,
  LogLevel,
} from '@microsoft/signalr'
import { env } from '#/env.ts'
import type { HubConnection } from '@microsoft/signalr'

type TokenGetter = () => Promise<string | null>

class Gateway {
  private connection: HubConnection | null = null
  private tokenGetter: TokenGetter | null = null
  private disconnectPromise: Promise<void> | null = null

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
  }

  async connect() {
    if (this.disconnectPromise) {
      await this.disconnectPromise
    }

    if (!this.connection) {
      throw new Error('Gateway not initialized')
    }

    if (this.connection.state !== HubConnectionState.Disconnected) {
      return
    }

    try {
      await this.connection.start()
    } catch (err) {
      console.error(
        'SignalR connection failed to start, retrying in 5s...',
        err,
      )
      const currentConnection = this.connection
      setTimeout(() => {
        if (this.connection === currentConnection) {
          this.connect()
        }
      }, 5000)
    }
  }

  async disconnect() {
    if (!this.connection) return

    const connectionToStop = this.connection
    this.connection = null
    this.tokenGetter = null

    this.disconnectPromise = connectionToStop.stop().then(() => {
      this.disconnectPromise = null
    })

    await this.disconnectPromise
  }
}

export const gateway = new Gateway()
