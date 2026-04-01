import { jwtDecode } from 'jwt-decode'
import auth0 from '../auth/openid.server'
import { getTokens, updateTokens } from './session-repository.server'
import { env } from '@/env'

export const getAccessToken = async (sessionId: string) => {
  const tokens = await getTokens(sessionId)

  if (!tokens || !tokens.refresh) return null

  if (!isAccessTokenExpired(tokens.access)) return tokens.access

  const newTokens = await auth0.refreshAccessToken(tokens.refresh)

  await updateTokens(sessionId, {
    access: newTokens.accessToken(),
    refresh: newTokens.refreshToken(),
    idToken: newTokens.idToken(),
  })

  return newTokens.accessToken()
}

const isAccessTokenExpired = (token: string) => {
  try {
    const { exp } = jwtDecode(token)
    if (!exp) return true

    const currentUnixTime = Math.floor(Date.now() / 1000)
    return currentUnixTime >= exp - env.TOKEN_REFRESH_GRACE_PERIOD
  } catch {
    return true
  }
}
