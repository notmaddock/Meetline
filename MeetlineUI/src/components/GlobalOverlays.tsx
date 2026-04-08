import { Assistant } from './assistant/Assistant'
import { UniversalSearch } from '#/components/universal-search/UniversalSearch.tsx'

export function GlobalOverlays() {
  return (
    <>
      <Assistant />
      <UniversalSearch />
    </>
  )
}
