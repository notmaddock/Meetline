import { createFileRoute, redirect } from '@tanstack/react-router'
import { SignIn } from '@clerk/react'
import {
  Empty,
  EmptyHeader,
  EmptyMedia,
  EmptyTitle,
} from '#/components/ui/empty.tsx'
import { Spinner } from '#/components/ui/spinner.tsx'

export const Route = createFileRoute('/welcome/$')({
  beforeLoad: async ({ context }) => {
    if (context.auth.isSignedIn) {
      throw redirect({
        to: '/',
      })
    }
  },
  component: RouteComponent,
})

function RouteComponent() {
  return (
    <main className={'fixed inset-0 flex items-center justify-center'}>
      <SignIn
        path="/welcome"
        routing="path"
        signUpUrl="/welcome"
        fallbackRedirectUrl="/"
        signUpFallbackRedirectUrl="/"
        withSignUp={true}
        fallback={<SignInSpinner />}
      />
    </main>
  )
}

function SignInSpinner() {
  return (
    <Empty>
      <EmptyHeader>
        <EmptyMedia variant={'icon'}>
          <Spinner />
        </EmptyMedia>
        <EmptyTitle>Preparing authentication...</EmptyTitle>
      </EmptyHeader>
    </Empty>
  )
}
