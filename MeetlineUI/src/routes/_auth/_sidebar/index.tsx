import { createFileRoute } from '@tanstack/react-router'
import { Input } from '#/components/ui/input.tsx'
import { Search } from 'lucide-react'
import {
  ResizableHandle,
  ResizablePanel,
  ResizablePanelGroup,
} from '#/components/ui/resizable.tsx'
import { Button } from '#/components/ui/button'

export const Route = createFileRoute('/_auth/_sidebar/')({
  component: Index,
})

function Index() {
  return <div className='flex flex-col gap-2 p-4 w-fit'>
    <span>Debug user info</span>
    <code className='bg-primary/5 border-primary/50 border p-2 rounded-md'>sfd</code>
    <Button variant={'secondary'}>Reload</Button>
  </div>
}
