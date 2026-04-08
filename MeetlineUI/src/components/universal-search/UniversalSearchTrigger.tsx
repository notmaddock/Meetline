import React from 'react'
import { useUniversalSearch } from './UniversalSearch'
import type {ComponentProps} from 'react';
import { Button } from '#/components/ui/button'
import { Kbd } from '#/components/ui/kbd.tsx'
import { cn } from '#/lib/utils.ts'

export function UniversalSearchTrigger({
  className,
  ...props
}: ComponentProps<typeof Button>) {
  const { setIsOpen } = useUniversalSearch()

  return (
    <>
      <Button
        variant="outline"
        className={cn(
          'h-8 w-full max-w-xs justify-start text-muted-foreground',
          className,
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
