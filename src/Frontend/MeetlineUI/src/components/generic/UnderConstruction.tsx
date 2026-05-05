import { ConstructionIcon } from 'lucide-react'
import type { ComponentProps } from 'react'
import {
  Empty,
  EmptyDescription,
  EmptyHeader,
  EmptyMedia,
  EmptyTitle,
} from '#/components/ui/empty.tsx'

export function UnderConstruction(props: ComponentProps<typeof Empty>) {
  return (
    <Empty {...props}>
      <EmptyHeader>
        <EmptyMedia variant={'icon'}>
          <ConstructionIcon />
        </EmptyMedia>
        <EmptyTitle>Under construction</EmptyTitle>
        <EmptyDescription>
          You've reached a planned, but under development part of Meetline.
          Lucky you!
        </EmptyDescription>
      </EmptyHeader>
    </Empty>
  )
}
