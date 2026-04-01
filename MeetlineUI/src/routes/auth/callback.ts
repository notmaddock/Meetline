import { createFileRoute } from '@tanstack/react-router'
import { handleCallback } from '#/server/session/current.ts'

export const Route = createFileRoute('/auth/callback')({
  server: {
    handlers: {
      GET: async ({ request }) => {
        const url = new URL(request.url)

        const code = url.searchParams.get('code')
        const state = url.searchParams.get('state')

        if (!code || !state)
          return new Response(null, {
            status: 302,
            headers: {
              Location: '/',
            },
          })

        await handleCallback(code, state)

        return new Response()
      },
    },
  },
})
