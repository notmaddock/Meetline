import { queryOptions } from '@tanstack/react-query'
import { createServerFn } from '@tanstack/react-start'
import { jwtDecode } from 'jwt-decode'
import { useAppSession } from '../session/cookie.server'
import { getTokens } from '../session/session-repository.server'

type IDTokenPayload = {
  given_name?: string
  family_name?: string
  email?: string
  preferred_username?: string
  nickname?: string
}

export const getIdTokenInfoQuery = () =>
  queryOptions({
    queryKey: ['id-token-info'],
    queryFn: () => getIdTokenInfo(),
    staleTime: 'static',
  })

export const getIdTokenInfo = createServerFn({ method: 'GET' }).handler(
  async (): Promise<IDTokenPayload> => {
    const session = await useAppSession()

    if (!session.data.id) return {}

    const tokens = await getTokens(session.data.id)

    if (!tokens?.idToken) return {}

    const decoded = jwtDecode(tokens.idToken)

    if (!decoded.preferred_username && decoded.nickname)
      decoded.preferred_username = decoded.nickname

    return decoded
  },
)
