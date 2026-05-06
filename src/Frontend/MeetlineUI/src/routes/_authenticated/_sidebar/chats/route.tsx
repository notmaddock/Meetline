import {Outlet, createFileRoute} from '@tanstack/react-router'
import type {Chat} from '#/components/chats/ChatList'
import {
  ResizableHandle,
  ResizablePanel,
  ResizablePanelGroup,
} from '#/components/ui/resizable'
import {ChatList, ChatType} from '#/components/chats/ChatList'

export const Route = createFileRoute('/_authenticated/_sidebar/chats')({
  component: RouteComponent,
})

function RouteComponent() {
  const chats: Chat[] = [
    {
      id: '1',
      type: ChatType.Direct,
      participants: [
        {
          id: 'u1',
          name: 'Alice Freeman',
          avatarUrl: 'https://i.pravatar.cc/150?u=u1',
        },
        {
          id: 'u2',
          name: 'Bob Smith',
          avatarUrl: 'https://i.pravatar.cc/150?u=u2',
        },
      ],
      lastMessage: {
        id: 'm1',
        content: 'See you at the meeting!',
        timestamp: new Date('2026-04-23T14:30:00'),
      },
    },
    {
      id: '2',
      type: ChatType.Group,
      participants: [
        {
          id: 'u1',
          name: 'Alice Freeman',
          avatarUrl: 'https://i.pravatar.cc/150?u=u1',
        },
        {
          id: 'u3',
          name: 'Charlie Day',
          avatarUrl: 'https://i.pravatar.cc/150?u=u3',
        },
        {
          id: 'u4',
          name: 'Diana Prince',
          avatarUrl: 'https://i.pravatar.cc/150?u=u4',
        },
      ],
      lastMessage: {
        id: 'm2',
        content: 'Who is bringing snacks?',
        timestamp: new Date('2026-04-23T15:45:00'),
      },
    },
    {
      id: '3',
      type: ChatType.Channel,
      participants: [],
      lastMessage: {
        id: 'm3',
        content: 'New policy update pinned.',
        timestamp: new Date('2026-04-23T09:00:00'),
      },
    },
    {
      id: '4',
      type: ChatType.Direct,
      participants: [
        {
          id: 'u1',
          name: 'Alice Freeman',
          avatarUrl: 'https://i.pravatar.cc/150?u=u1',
        },
        {
          id: 'u5',
          name: 'Ethan Hunt',
          avatarUrl: 'https://i.pravatar.cc/150?u=u5',
        },
      ],
      lastMessage: {
        id: 'm4',
        content: 'The mission is a go.',
        timestamp: new Date('2026-04-23T20:10:00'),
      },
    },
    {
      id: '5',
      type: ChatType.Group,
      participants: [
        {
          id: 'u1',
          name: 'Alice Freeman',
          avatarUrl: 'https://i.pravatar.cc/150?u=u1',
        },
        {
          id: 'u6',
          name: 'Fiona Gallagher',
          avatarUrl: 'https://i.pravatar.cc/150?u=u6',
        },
      ],
      lastMessage: {
        id: 'm5',
        content: 'Rent is due tomorrow.',
        timestamp: new Date('2026-04-22T18:20:00'),
      },
    },
    {
      id: '6',
      type: ChatType.Channel,
      participants: [],
      lastMessage: {
        id: 'm6',
        content: 'Welcome to the Dev Portal!',
        timestamp: new Date('2026-04-20T11:11:00'),
      },
    },
    {
      id: '7',
      type: ChatType.Direct,
      participants: [
        {
          id: 'u1',
          name: 'Alice Freeman',
          avatarUrl: 'https://i.pravatar.cc/150?u=u1',
        },
        {
          id: 'u7',
          name: 'George Costanza',
          avatarUrl: 'https://i.pravatar.cc/150?u=u7',
        },
      ],
      lastMessage: {
        id: 'm7',
        content: 'Was that wrong? Should I not have done that?',
        timestamp: new Date('2026-04-23T19:05:00'),
      },
    },
    {
      id: '8',
      type: ChatType.Group,
      participants: [
        {
          id: 'u1',
          name: 'Alice Freeman',
          avatarUrl: 'https://i.pravatar.cc/150?u=u1',
        },
        {
          id: 'u8',
          name: 'Haley Dunphy',
          avatarUrl: 'https://i.pravatar.cc/150?u=u8',
        },
        {
          id: 'u9',
          name: 'Ian Lightfoot',
          avatarUrl: 'https://i.pravatar.cc/150?u=u9',
        },
      ],
      lastMessage: {
        id: 'm8',
        content: 'Photo dump is ready.',
        timestamp: new Date('2026-04-23T20:30:00'),
      },
    },
    {
      id: '9',
      type: ChatType.Direct,
      participants: [
        {
          id: 'u1',
          name: 'Alice Freeman',
          avatarUrl: 'https://i.pravatar.cc/150?u=u1',
        },
        {
          id: 'u10',
          name: 'Joel Miller',
          avatarUrl: 'https://i.pravatar.cc/150?u=u10',
        },
      ],
      lastMessage: {
        id: 'm9',
        content: 'Watch your six.',
        timestamp: new Date('2026-04-23T20:45:00'),
      },
    },
    {
      id: '10',
      type: ChatType.Channel,
      participants: [],
      lastMessage: {
        id: 'm10',
        content: 'Weather Alert: Severe storm warning.',
        timestamp: new Date('2026-04-23T12:00:00'),
      },
    },
  ]

  return (
    <ResizablePanelGroup>
      <ResizablePanel minSize={'25%'} defaultSize={'40%'} collapsible>
        <ChatList chats={chats} currentUserId="1" />
      </ResizablePanel>

      <ResizableHandle />

      <ResizablePanel>
        <Outlet />
      </ResizablePanel>
    </ResizablePanelGroup>
  )
}
