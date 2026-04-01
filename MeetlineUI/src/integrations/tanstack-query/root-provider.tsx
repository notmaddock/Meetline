import { QueryClient } from '@tanstack/react-query'
import { ensureValidSession } from '#/server/session/current.ts'

export function getContext() {
  const queryClient = new QueryClient({
    defaultOptions: {
      queries: {
        refetchOnWindowFocus: true,
        meta: {
          onFocus: async () => {
            try {
              await ensureValidSession()
            } catch (e) {
              console.warn('Session keep-alive failed')
            }
          },
        },
      },
    },
  })

  return {
    queryClient,
  }
}

export default function TanstackQueryProvider() {}
