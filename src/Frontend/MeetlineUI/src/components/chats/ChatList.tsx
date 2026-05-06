import {ArchiveIcon, BanIcon, Trash2Icon} from 'lucide-react'
import {Link} from '@tanstack/react-router'
import { Avatar, AvatarFallback } from '../ui/avatar'
import {
  ContextMenu,
  ContextMenuContent,
  ContextMenuGroup,
  ContextMenuItem,
  ContextMenuSeparator,
  ContextMenuTrigger,
} from '../ui/context-menu'
import type {ComponentProps} from 'react'
import {cn} from '#/lib/utils'

export enum ChatType {
  Direct,
  Group,
  Channel,
}

type ChatParticipant = {
  id: string
  name: string
  avatarUrl: string
}

type ChatMessage = {
  id: string
  content: string
  timestamp: Date
}

type ChatBase = {
  id: string
  type: ChatType
  participants: ChatParticipant[]
  lastMessage?: ChatMessage
}

type DirectChat = ChatBase & {
  type: ChatType.Direct
}

type GroupChat = ChatBase & {
  type: ChatType.Group
}

type ChannelChat = ChatBase & {
  type: ChatType.Channel
}

export type Chat = DirectChat | GroupChat | ChannelChat

type ChatListProps = ComponentProps<'ul'> & {
  chats: Chat[]
  currentUserId: string
}

export function ChatList({
  className,
  chats,
  currentUserId,
  ...props
}: ChatListProps) {
  return (
    <ul className={cn('flex flex-col p-2 gap-1', className)} {...props}>
      {chats.map((chat) => (
        <ChatListItem chat={chat} currentUserId={currentUserId} />
      ))}
    </ul>
  )
}

type ChatListItemProps = ComponentProps<'li'> & {
  chat: Chat
  currentUserId: string
}

function ChatListItem({
  className,
  chat,
  currentUserId,
  ...props
}: ChatListItemProps) {
  return (
    <li className={cn('rounded-sm hover:bg-card', className)} {...props}>
      <ContextMenu>
        <ContextMenuTrigger
          render={
            <div className="flex px-4 py-2 items-center gap-4 cursor-pointer" />
          }
        >
            <Link
                to="/chats/$id"
                params={{id: chat.id}}
                className="flex items-center gap-4 w-full"
            >
            <Avatar>
              <AvatarFallback>AV</AvatarFallback>
            </Avatar>

            <div className="flex flex-col">
              <span className="font-bold">Chat name</span>
              <span className="text-muted-foreground">Contents</span>
            </div>

            <div className="ml-auto">
              <span className="text-muted-foreground text-sm">9:41</span>
            </div>
          </Link>
        </ContextMenuTrigger>

        <ChatListItemContextMenu />
      </ContextMenu>
    </li>
  )
}

function ChatListItemContextMenu() {
  return (
    <ContextMenuContent>
      <ContextMenuGroup>
        <ContextMenuItem>Copy link</ContextMenuItem>
      </ContextMenuGroup>
      <ContextMenuSeparator />
      <ContextMenuGroup>
        <ContextMenuItem>
          <ArchiveIcon />
          Archive
        </ContextMenuItem>
        <ContextMenuItem variant="destructive">
          <BanIcon />
          Block
        </ContextMenuItem>
        <ContextMenuItem variant="destructive">
          <Trash2Icon />
          Delete
        </ContextMenuItem>
      </ContextMenuGroup>
    </ContextMenuContent>
  )
}
