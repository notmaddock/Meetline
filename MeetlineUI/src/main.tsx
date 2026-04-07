import ReactDOM from 'react-dom/client'
import { RouterProvider, createRouter } from '@tanstack/react-router'
import { routeTree } from './routeTree.gen'
import { ClerkProvider, useAuth } from '@clerk/react'
import { env } from '#/env.ts'
import { Spinner } from '#/components/ui/spinner.tsx'
import {
  Empty,
  EmptyDescription,
  EmptyHeader,
  EmptyMedia,
  EmptyTitle,
} from '#/components/ui/empty.tsx'
import type { RouterContext } from '#/routes/__root.tsx'

const router = createRouter({
  routeTree,
  defaultPreload: 'intent',
  scrollRestoration: true,
  context: {} as RouterContext
})

declare module '@tanstack/react-router' {
  interface Register {
    router: typeof router
  }
}

const rootElement = document.getElementById('app')!

if (!rootElement.innerHTML) {
  const root = ReactDOM.createRoot(rootElement)
  root.render(
    <ClerkProvider publishableKey={env.VITE_CLERK_PUBLISHABLE_KEY}>
      <App />
    </ClerkProvider>,
  )
}

function App() {
  const auth = useAuth()

  if (!auth.isLoaded) return <AuthLoadingSpinner />

  return <RouterProvider router={router} context={{ auth }} />
}

function AuthLoadingSpinner() {
  return <main className={'fixed inset-0 flex justify-center'}>
    <Empty>
      <EmptyHeader>
        <EmptyMedia variant={'icon'}>
          <Spinner />
        </EmptyMedia>
        <EmptyTitle>Meetline</EmptyTitle>
        <EmptyDescription>Signing you in...</EmptyDescription>
      </EmptyHeader>
    </Empty>
  </main>
}
