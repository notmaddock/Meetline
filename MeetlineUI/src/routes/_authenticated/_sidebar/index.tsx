import { createFileRoute, redirect } from '@tanstack/react-router'

export const Route = createFileRoute('/_authenticated/_sidebar/')({
  beforeLoad: () => {
    throw redirect({
      to: '/chats',
    })
  },
})
