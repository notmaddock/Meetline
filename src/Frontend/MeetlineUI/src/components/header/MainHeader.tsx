import { AssistantTrigger } from '../assistant/AssistantTrigger'
import { SidebarTrigger } from '#/components/ui/sidebar.tsx'
import { useIsMobile } from '#/hooks/use-mobile.ts'
import { UniversalSearchTrigger } from '#/components/universal-search/UniversalSearchTrigger.tsx'

export function MainHeader() {
  const isMobile = useIsMobile()

  return (
    <header className="h-12 shrink-0 border-b grid grid-cols-3 items-center px-4 bg-sidebar border-sidebar-border">
      <div className="flex items-center">
        {isMobile && <SidebarTrigger className="mr-4" />}
      </div>

      <div className="flex justify-center">
        <UniversalSearchTrigger className={'md:flex hidden'} />
      </div>

      <div className="flex justify-end">
        <AssistantTrigger />
      </div>
    </header>
  )
}
