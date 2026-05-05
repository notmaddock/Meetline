import { Outlet, createRootRouteWithContext } from '@tanstack/react-router'
import { TanStackRouterDevtoolsPanel } from '@tanstack/react-router-devtools'
import { TanStackDevtools } from '@tanstack/react-devtools'
import { StrictMode } from 'react'
import type { useAuth, useUser } from '@clerk/react'

import '../styles.css'
import type { QueryClient } from '@tanstack/react-query'
import { GlobalErrorComponent } from '#/components/GlobalErrorComponent'

export type RouterContext = {
  auth: ReturnType<typeof useAuth>
  user: ReturnType<typeof useUser>
  queryClient: QueryClient
}

export const Route = createRootRouteWithContext<RouterContext>()({
  component: RootComponent,
  errorComponent: GlobalErrorComponent,
})

function RootComponent() {
  return (
    <>
      <StrictMode>
        <Outlet />
      </StrictMode>
      <TanStackDevtools
        config={{
          position: 'bottom-right',
        }}
        plugins={[
          {
            name: 'TanStack Router',
            render: <TanStackRouterDevtoolsPanel />,
          },
        ]}
      />
    </>
  )
}
