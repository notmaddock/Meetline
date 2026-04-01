import { Auth0 } from 'arctic'
import { env } from '@/env.ts'

const auth0 = new Auth0(
  env.AUTH0_DOMAIN,
  env.AUTH0_CLIENT_ID,
  env.AUTH0_CLIENT_SECRET,
  env.OIDC_REDIRECT_URI,
)

export default auth0
