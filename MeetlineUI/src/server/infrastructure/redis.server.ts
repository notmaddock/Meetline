import { createClient } from 'redis'
import type { RedisClientType } from 'redis'

let redisClient: RedisClientType | null = null

export function getRedisClient(): RedisClientType {
  if (redisClient) return redisClient

  const client = createClient({
    url: process.env.REDIS_URL || 'redis://localhost:6379',
    socket: {
      reconnectStrategy: (retries) => {
        const delay = Math.min(retries * 100, 3000)
        console.warn(`Redis connection lost. Retrying in ${delay}ms...`)
        return delay
      },
      connectTimeout: 5000,
    },
  })

  client.on('error', (err) => {
    console.error('Redis client error', err)
  })

  client.on('connect', () => console.info('Redis connected'))
  client.on('reconnecting', () => console.info('Redis reconnecting...'))
  client.on('ready', () => console.info('Redis ready'))

  client.connect().catch((err) => {
    console.error('Failed to initial connect to Redis:', err)
  })

  redisClient = client as RedisClientType
  return redisClient
}

const client = getRedisClient()
export default client
