import { UniversalSearch } from '#/components/universal-search/UniversalSearch.tsx'
import { Assistant } from './assistant/Assistant'

export function GlobalOverlays() {
  return <>
    <Assistant />
    <UniversalSearch />
  </>
}