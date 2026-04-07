import React, { type ComponentProps } from 'react'
import { Button } from '#/components/ui/button'
import { Kbd } from '#/components/ui/kbd.tsx'
import { useUniversalSearch } from './UniversalSearch'
import { cn } from '#/lib/utils.ts'

export function UniversalSearchTrigger({ className, ...props }: ComponentProps<typeof Button>) {
  const { isOpen, setIsOpen } = useUniversalSearch()

  return (
    <>
      <Button
        variant="outline"
        className={cn(
          className,
          'h-8 w-full max-w-xs justify-start text-muted-foreground',
        )}
        onClick={() => setIsOpen(true)}
        {...props}
      >
        <span className="inline-flex">Search all of Meetline...</span>
        <Kbd className="ml-auto">⌘ K</Kbd>
      </Button>
    </>
  )
}
