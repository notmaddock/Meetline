import { createFileRoute } from '@tanstack/react-router'
import { UnderConstruction } from '#/components/generic/UnderConstruction.tsx'

export const Route = createFileRoute('/_authenticated/_sidebar/calendar/')({
  component: RouteComponent,
})

function RouteComponent() {
  return (
    <main className={'flex flex-col items-center justify-center h-full'}>
      <UnderConstruction />
    </main>
  )
}
