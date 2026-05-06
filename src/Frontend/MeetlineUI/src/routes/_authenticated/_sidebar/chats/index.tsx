import {createFileRoute} from '@tanstack/react-router'
import {
    MessageCircleQuestionMarkIcon,
    UserRoundSearchIcon,
} from 'lucide-react'
import { Button } from '#/components/ui/button'
import {
    Empty,
    EmptyContent,
    EmptyDescription,
    EmptyHeader,
    EmptyMedia,
    EmptyTitle,
} from '#/components/ui/empty'
import { useUniversalSearch } from '#/components/universal-search/UniversalSearch'

export const Route = createFileRoute('/_authenticated/_sidebar/chats/')({
  component: RouteComponent,
})

function RouteComponent() {
    const {setIsOpen: setIsUniversalSearchOpen} = useUniversalSearch()

    const openUniversalSearch = () => setIsUniversalSearchOpen(true)

    return (
        <Empty className="h-full flex items-center justify-center">
            <EmptyHeader>
                <EmptyMedia variant={'icon'}>
                    <MessageCircleQuestionMarkIcon/>
                </EmptyMedia>
                <EmptyTitle>Select a chat</EmptyTitle>
                <EmptyDescription>
                    Use the chat list or Universal Search
                </EmptyDescription>
            </EmptyHeader>

            <EmptyContent>
                <Button variant={'secondary'} onClick={() => openUniversalSearch()}>
                    <UserRoundSearchIcon/>
                    Open Universal Search
                </Button>
            </EmptyContent>
        </Empty>
    )
}
