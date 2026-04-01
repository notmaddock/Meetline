import { randomBytes } from 'node:crypto'
import { env } from '@/env'
import redis from '@/server/infrastructure/redis.server'

type SessionData = {
  access: string
  refresh: string
  idToken: string
}

export async function saveTokens(tokens: SessionData) {
  const id = randomBytes(env.SESSION_ID_ENTROPY).toString('hex')
  const key = `session:${id}`

  await redis
    .multi()
    .hSet(key, tokens)
    .expire(key, env.SESSION_ROLLING_TTL)
    .exec()

  return id
}

export async function getTokens(sessionId: string) {
  const key = `session:${sessionId}`

  const [data] = await redis
    .multi()
    .hGetAll(key)
    .expire(key, env.SESSION_ROLLING_TTL)
    .exec()

  if (Object.keys(data).length === 0) return null

  return data as unknown as SessionData
}

export async function updateTokens(
  sessionId: string,
  session: Partial<SessionData>,
) {
  const key = `session:${sessionId}`

  const exists = await redis.exists(key)
  if (!exists) return

  await redis
    .multi()
    .hSet(key, session)
    .expire(key, env.SESSION_ROLLING_TTL)
    .exec()
}

export async function clearSession(sessionId: string) {
  const key = `session:${sessionId}`

  await redis.del(key)
}
