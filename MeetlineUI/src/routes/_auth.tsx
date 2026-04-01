import { createFileRoute, Outlet } from '@tanstack/react-router'
import { ensureValidSession } from '#/server/session/current.ts'

export const Route = createFileRoute('/_auth')({
  beforeLoad: async () => {
    await ensureValidSession()
  },
  component: AuthLayout,
})

function AuthLayout() {
  return <Outlet />
}
