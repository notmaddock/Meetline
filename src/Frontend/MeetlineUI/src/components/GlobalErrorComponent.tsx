import { RefreshCwIcon, RotateCwIcon } from 'lucide-react'
import { toast } from 'sonner'
import { useState } from 'react'
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from './ui/card'
import { Button } from './ui/button'
import {
  Accordion,
  AccordionContent,
  AccordionItem,
  AccordionTrigger,
} from './ui/accordion'
import type { ErrorComponentProps } from '@tanstack/react-router'

export function GlobalErrorComponent({
  error,
  reset,
  info,
}: ErrorComponentProps) {
  const attemptReset = () => {
    toast.info('Error reset attempted', {
      action: <PageReloadButton />,
      description: 'Nothing changed? Reload the entire page and try again',
    })
    reset()
  }

  return (
    <>
      <main className="fixed inset-0 flex items-center justify-center">
        <Card className="w-full max-w-2xl max-h-screen m-4">
          <CardHeader>
            <CardTitle>Meetline error</CardTitle>
            <CardDescription>
              Something went wrong. This error should be reported automatically.
              You can try and reset the error, or reload the entire page.
            </CardDescription>
          </CardHeader>
          <CardContent className="flex flex-col max-h-full overflow-y-auto">
            <ErrorCode title={error.name} message={error.message} />

            <Accordion className={'mt-2'}>
              <AccordionItem title="Stack trace" disabled={!error.stack}>
                <AccordionTrigger>
                  Stack trace{!error.stack && ' unavailable'}
                </AccordionTrigger>
                <AccordionContent>
                  <ErrorCode
                    message={error.stack ?? 'No stack trace available'}
                  />
                </AccordionContent>
              </AccordionItem>

              <AccordionItem title="Stack trace" disabled={!error.cause}>
                <AccordionTrigger>
                  Cause{!error.cause && ' unavailable'}
                </AccordionTrigger>
                <AccordionContent>
                  <ErrorCode
                    message={(error.cause ?? 'No cause available') as any}
                  />
                </AccordionContent>
              </AccordionItem>

              <AccordionItem
                title="Component stack"
                disabled={!info?.componentStack}
              >
                <AccordionTrigger>
                  Component stack{!info?.componentStack && ' unavailable'}
                </AccordionTrigger>
                <AccordionContent>
                  <ErrorCode
                    message={
                      (info?.componentStack ??
                        'No component stack available')
                    }
                  />
                </AccordionContent>
              </AccordionItem>
            </Accordion>
          </CardContent>
          <CardFooter className="justify-end gap-2">
            <Button variant={'secondary'} onClick={() => attemptReset()}>
              <RotateCwIcon />
              Try to reset error
            </Button>
            <PageReloadButton />
          </CardFooter>
        </Card>
      </main>
    </>
  )
}

function PageReloadButton() {
  const [locked, setLocked] = useState<boolean>(false)

  const reload = () => {
    setLocked(true)
    window.location.reload()
  }

  return (
    <Button onClick={() => reload()} disabled={locked}>
      <RefreshCwIcon className={locked ? 'animate-spin' : ''} />
      {locked ? 'Reloading...' : 'Reload'}
    </Button>
  )
}

function ErrorCode({ title, message }: { title?: string; message: string }) {
  return (
    <span className="flex flex-col border border-destructive bg-destructive/10 rounded-sm p-2 text-destructive">
      {title}
      <code className="text-foreground/70">{message}</code>
    </span>
  )
}
