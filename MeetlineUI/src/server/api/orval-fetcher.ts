import type { AxiosInstance, AxiosRequestConfig } from 'axios'
import axios from 'axios'
import { getAccessToken } from '../session/tokens.server'
import { env } from '@/env'
import { useAppSession } from '@/server/session/cookie.server.ts'
import type { ZodSchema } from 'zod'

declare module 'axios' {
  export interface AxiosRequestConfig {
    zodSchema?: ZodSchema
  }
}

export const AXIOS_INSTANCE: AxiosInstance = axios.create({
  baseURL: env.API_BASE_URL,
})

AXIOS_INSTANCE.interceptors.request.use(
  async (config) => {
    const session = await useAppSession()

    if (session.data.id) {
      try {
        const token = await getAccessToken(session.data.id)
        if (token) {
          config.headers.Authorization = `Bearer ${token}`
        }
      } catch (e) {
        console.error('Failed to get access token in axios interceptor:', e)
      }
    }

    return config
  },
  (error) => Promise.reject(error),
)

AXIOS_INSTANCE.interceptors.response.use(
  (response) => {
    if (response.config.zodSchema) {
      const result = response.config.zodSchema.safeParse(response.data)
      if (!result.success) {
        console.error(
          `[API validation error] ${response.config.method?.toUpperCase()} ${response.config.url}:`,
          result.error.flatten(),
        )

        if (process.env.NODE_ENV === 'development') {
          console.warn('Backend schema mismatch detected!')
        }
      }
    }
    return response
  },
  async (error) => Promise.reject(error)
)

export const fetcher = async <T>(config: AxiosRequestConfig): Promise<T> => {
  const source = axios.CancelToken.source()
  const promise = AXIOS_INSTANCE({
    ...config,
    cancelToken: source.token,
  }).then(({ data }) => data)

  // @ts-ignore hackily adding a custom cancel method to the promise
  promise.cancel = () => {
    source.cancel('Query was cancelled by TanStack Query')
  }

  return promise
}
