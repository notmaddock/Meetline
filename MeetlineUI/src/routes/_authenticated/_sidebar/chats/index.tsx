import { createFileRoute } from '@tanstack/react-router'
import { useQuery } from '@tanstack/react-query'
import { getCurrentUserOptions } from '#/client/@tanstack/react-query.gen.ts'
import { Button } from '#/components/ui/button'

export const Route = createFileRoute('/_authenticated/_sidebar/chats/')({
  component: RouteComponent,
})

function RouteComponent() {
  const { data, refetch, isRefetching } = useQuery({
    ...getCurrentUserOptions(),
  })

  return (
    <div className="flex flex-col">
      <span>Current user info</span>
      <code className="max-w-md w-full border border-primary p-2 m-2 bg-primary/30 overflow-hidden wrap-break-word">
        {JSON.stringify(data) ?? 'no data'}
      </code>
      <Button disabled={isRefetching} onClick={() => refetch()}>
        Refresh{isRefetching ? 'ing...' : ''}
      </Button>
    </div>
  )
}
