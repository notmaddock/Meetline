import { createServerFn, createServerOnlyFn } from '@tanstack/react-start'
import { redirect } from '@tanstack/react-router'
import { beginAuthenticationFlow } from '../auth/login.server'
import auth0 from '../auth/openid.server'
import { useAppSession, useOAuthState } from './cookie.server'
import { getAccessToken } from './tokens.server'
import {
  clearSession,
  getTokens,
  saveTokens,
} from './session-repository.server'
import { env } from '@/env.ts'

export const ensureValidSession = createServerFn({ method: 'GET' }).handler(
  async () => {
    const session = await useAppSession()

    const sessionId = session.data.id

    if (!sessionId) {
      await beginAuthenticationFlow()
      return
    }

    try {
      const token = await getAccessToken(sessionId)
      if (!token) {
        await session.clear()
        await beginAuthenticationFlow()
      }
    } catch (e) {
      console.error('Error checking session tokens:', e)
      await session.clear()
      await beginAuthenticationFlow()
    }
  },
)

export const logout = createServerFn({ method: 'POST' }).handler(async () => {
  const session = await useAppSession()
  const oauthState = await useOAuthState()

  if (session.data.id) {
    const tokens = await getTokens(session.data.id)

    if (tokens) await auth0.revokeToken(tokens.refresh)

    await clearSession(session.data.id)
  }

  await session.clear()
  await oauthState.clear()

  const auth0LogoutUrl = `https://${env.AUTH0_DOMAIN}/v2/logout?client_id=${env.AUTH0_CLIENT_ID}&returnTo=${encodeURIComponent(env.APP_BASE_URL)}`

  return { url: auth0LogoutUrl }
})

export const handleCallback = createServerOnlyFn(
  async (code: string, state: string) => {
    const oauthState = await useOAuthState()

    const codeVerifier = oauthState.data.codeVerifier
    const expectedState = oauthState.data.state

    if (
      !code ||
      !state ||
      !codeVerifier ||
      !expectedState ||
      expectedState !== state
    ) {
      throw redirect({ to: '/' })
    }

    const tokens = await auth0.validateAuthorizationCode(code, codeVerifier)

    await oauthState.clear()

    const sessionId = await saveTokens({
      access: tokens.accessToken(),
      refresh: tokens.refreshToken(),
      idToken: tokens.idToken(),
    })

    const session = await useAppSession()

    await session.update({
      id: sessionId,
    })

    throw redirect({ to: '/' })
  },
)
