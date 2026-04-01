import { Sidebar } from '#/components/ui/sidebar.tsx'
import { SidebarShortcuts } from '#/components/common/MainSidebar/SidebarShortcuts.tsx'

export default function MainSidebar() {
  return (
    <Sidebar
      collapsible="icon"
      variant="inset"
      className="overflow-hidden *:data-[sidebar=sidebar]:flex-row"
    >
      <SidebarShortcuts className={'mt-auto'} />
    </Sidebar>
  )
}
