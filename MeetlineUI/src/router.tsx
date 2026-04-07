import { createRouter as createTanStackRouter } from '@tanstack/react-router'

import { routeTree } from './routeTree.gen'
import { getContext } from './integrations/tanstack-query/root-provider'

export function getRouter() {
  const context = getContext()

  return createTanStackRouter({
    routeTree,
    context,
    scrollRestoration: true,
    defaultPreload: 'intent',
    defaultPreloadStaleTime: 0,
  })
}

declare module '@tanstack/react-router' {
  interface Register {
    router: ReturnType<typeof getRouter>
  }
}
