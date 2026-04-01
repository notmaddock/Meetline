import { createFileRoute, Outlet } from '@tanstack/react-router'
import { SidebarInset, SidebarProvider } from '#/components/ui/sidebar.tsx'
import MainSidebar from '#/components/common/MainSidebar/MainSidebar.tsx'

export const Route = createFileRoute('/_auth/_sidebar')({
  component: SidebarLayout,
})

function SidebarLayout() {
  return (
    <SidebarProvider>
      <MainSidebar />
      <SidebarInset className="overflow-hidden">
        <Outlet />
      </SidebarInset>
    </SidebarProvider>
  )
}
