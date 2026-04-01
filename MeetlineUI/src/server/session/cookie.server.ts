import { useSession } from '@tanstack/react-start/server'

import { env } from '@/env'

type SessionData = {
  id: string
}

type OAuthState = {
  codeVerifier: string
  state: string
}

export function useAppSession() {
  return useSession<SessionData>({
    name: 'sid',
    password: env.COOKIE_SID_SECRET,
    cookie: {
      httpOnly: true,
      sameSite: 'lax',
      secure: process.env.NODE_ENV === 'production',
    },
  })
}

export function useOAuthState() {
  return useSession<OAuthState>({
    name: 'oas',
    password: env.COOKIE_OAS_SECRET,
    cookie: {
      httpOnly: true,
      sameSite: 'lax',
      secure: process.env.NODE_ENV === 'production',
    },
  })
}
