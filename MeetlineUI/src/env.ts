import { createEnv } from '@t3-oss/env-core'
import { z } from 'zod'

export const env = createEnv({
  server: {
    NODE_ENV: z
      .enum(['development', 'production', 'test'])
      .default('development'),
    REDIS_URL: z.string().url().default('redis://localhost:6379'),

    API_BASE_URL: z.url(),
    APP_BASE_URL: z.url(),

    // Secrets for session encryption (min 32 chars)
    COOKIE_SID_SECRET: z.string().min(32),
    COOKIE_OAS_SECRET: z.string().min(32),

    // Auth0 / OIDC Configuration
    AUTH0_DOMAIN: z.string(),
    AUTH0_CLIENT_ID: z.string(),
    AUTH0_CLIENT_SECRET: z.string(),
    OIDC_REDIRECT_URI: z.url(),
    OIDC_AUDIENCE: z.string(),

    // Session Behavior
    SESSION_ROLLING_TTL: z.coerce.number().positive(),
    SESSION_ID_ENTROPY: z.coerce.number().min(16),
    TOKEN_REFRESH_GRACE_PERIOD: z.coerce.number().positive(),
  },

  clientPrefix: 'VITE_',

  client: {
    VITE_PUBLIC_POSTHOG_PROJECT_TOKEN: z.string().min(1),
    VITE_PUBLIC_POSTHOG_API_HOST: z.url(),
    VITE_PUBLIC_POSTHOG_UI_HOST: z.url(),
  },

  runtimeEnv: process.env,
  emptyStringAsUndefined: true,
})
