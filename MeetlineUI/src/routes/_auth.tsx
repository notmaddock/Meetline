import { createFileRoute, Outlet } from '@tanstack/react-router'
import { ensureValidSession } from '#/server/session/current.ts'
import { getUsersMeQuery } from '#/server/api/functions/user'

export const Route = createFileRoute('/_auth')({
  beforeLoad: async ({ context }) => {
    await ensureValidSession()
    await context.queryClient.ensureQueryData(getUsersMeQuery())
  },
  component: AuthLayout,
})

function AuthLayout() {
  return <Outlet />
}
