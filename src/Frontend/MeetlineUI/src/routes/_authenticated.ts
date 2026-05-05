import { Outlet, createFileRoute, redirect } from '@tanstack/react-router'
import { client } from '#/client/client.gen.ts'
import { env } from '#/env.ts'

export const Route = createFileRoute('/_authenticated')({
  beforeLoad: async ({ context }) => {
    if (!context.auth.isSignedIn) {
      throw redirect({
        to: '/welcome/$',
      })
    }

    client.setConfig({
      baseUrl: env.VITE_API_BASE_URL,
      auth: async () => {
        return (await context.auth.getToken()) || ''
      },
    })
  },
  component: Outlet,
})
