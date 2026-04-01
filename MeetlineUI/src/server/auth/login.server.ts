import { createServerFn } from '@tanstack/react-start'
import * as arctic from 'arctic'
import { redirect } from '@tanstack/react-router'
import { useOAuthState } from '@/server/session/cookie.server.ts'
import auth0 from '@/server/auth/openid.server'
import { env } from '@/env.ts'

export const beginAuthenticationFlow = createServerFn({
  method: 'POST',
}).handler(async () => {
  const oauthState = await useOAuthState()

  await oauthState.clear()

  const state = arctic.generateState()
  const codeVerifier = arctic.generateCodeVerifier()

  await oauthState.update({
    state,
    codeVerifier,
  })

  const url = auth0.createAuthorizationURL(state, codeVerifier, [
    'openid',
    'profile',
    'email',
    'offline_access',
  ])

  url.searchParams.set('audience', env.OIDC_AUDIENCE)

  throw redirect({
    href: url.toString(),
  })
})
