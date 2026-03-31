import posthog from 'posthog-js'
import { PostHogProvider as BasePostHogProvider } from '@posthog/react'
import type { ReactNode } from 'react'

if (
  typeof window !== 'undefined' &&
  import.meta.env.VITE_PUBLIC_POSTHOG_PROJECT_TOKEN
) {
  posthog.init(import.meta.env.VITE_PUBLIC_POSTHOG_PROJECT_TOKEN, {
    api_host:
      import.meta.env.VITE_PUBLIC_POSTHOG_API_HOST ||
      'https://us.i.posthog.com',
    ui_host:
      import.meta.env.VITE_PUBLIC_POSTHOG_UI_HOST || 'https://us.i.posthog.com',
    person_profiles: 'identified_only',
    capture_pageview: true,
    defaults: '2026-01-30',
  })

  console.log(
    `Using ${import.meta.env.VITE_PUBLIC_POSTHOG_PROJECT_TOKEN} as key`,
  )
}

interface PostHogProviderProps {
  children: ReactNode
}

export default function PostHogProvider({ children }: PostHogProviderProps) {
  return <BasePostHogProvider client={posthog}>{children}</BasePostHogProvider>
}
