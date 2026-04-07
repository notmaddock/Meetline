import { createEnv } from '@t3-oss/env-core'
import { z } from 'zod'

export const env = createEnv({
  server: {
    NODE_ENV: z
      .enum(['development', 'production', 'test'])
      .default('development'),

  },

  clientPrefix: 'VITE_',

  client: {
    /**
     * The Posthog project token
     */
    VITE_PUBLIC_POSTHOG_PROJECT_TOKEN: z.string().min(1),

    /**
     * The Posthog API host (where Posthog sends data)
     */
    VITE_PUBLIC_POSTHOG_API_HOST: z.url(),

    /**
     * The Posthog UI host (where Posthog links UI anchors)
     */
    VITE_PUBLIC_POSTHOG_UI_HOST: z.url(),

    /**
     * The API base URL
     */
    VITE_API_BASE_URL: z.url(),

    /**
     * The application's base URL
     */
    VITE_APP_BASE_URL: z.url(),
  },

  runtimeEnv: process.env,
  emptyStringAsUndefined: true,
})
