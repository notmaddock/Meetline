import React, { useMemo } from 'react'
import { MinusIcon } from 'lucide-react'
import {
  AvatarBadge,
  AvatarFallback,
  AvatarGroupCount,
  AvatarImage,
  Avatar as BaseAvatar,
  AvatarGroup as BaseAvatarGroup,
} from '../ui/avatar'
import type { Presence } from '#/stores/presence'
import { getAvatarInitials } from '#/lib/utils/avatar-utils'
import { cn } from '#/lib/utils'

export type AvatarData = {
  name: string
  avatarURL?: string
}

type BaseAvatarProps = React.ComponentPropsWithoutRef<typeof BaseAvatar>
type BaseGroupAvatarProps = React.ComponentPropsWithoutRef<
  typeof BaseAvatarGroup
>

export type SingleAvatarProps = BaseAvatarProps & {
  user: AvatarData
  presence?: Presence
}

type BaseGroupData = {
  users: AvatarData[]
}

export type CompactGroupAvatarProps = BaseGroupData &
  BaseAvatarProps & {
    layout?: 'compact'
  }

export type SpreadGroupAvatarProps = BaseGroupData &
  BaseGroupAvatarProps & {
    layout: 'spread'
  }

export type GroupAvatarProps = CompactGroupAvatarProps | SpreadGroupAvatarProps

export type ChannelAvatarProps = BaseAvatarProps & {
  channel: AvatarData
}

export type AvatarProps =
  | ({ variant: 'single' } & SingleAvatarProps)
  | ({ variant: 'group' } & GroupAvatarProps)
  | ({ variant: 'channel' } & ChannelAvatarProps)

const presenceColors: Record<Presence, string> = {
  online: 'bg-green-600 dark:bg-green-800',
  away: 'bg-yellow-600 dark:bg-yellow-800',
  dnd: 'bg-red-600 dark:bg-red-800',
  busy: 'bg-red-600 dark:bg-red-800',
  offline: 'bg-gray-600 dark:bg-gray-800',
}

export function Avatar(props: AvatarProps) {
  switch (props.variant) {
    case 'single':
      return <SingleAvatar {...props} />
    case 'group':
      return <GroupAvatar {...props} />
    case 'channel':
      return <ChannelAvatar {...props} />
  }
}

export function SingleAvatar({ user, presence, ...props }: SingleAvatarProps) {
  const { name, avatarURL } = user

  const fallback = useMemo(() => getAvatarInitials(name), [name])

  return (
    <BaseAvatar {...props}>
      <AvatarImage src={avatarURL} />
      <AvatarFallback>{fallback}</AvatarFallback>
      {presence && (
        <AvatarBadge className={presenceColors[presence]}>
          {presence === 'dnd' && (
            <MinusIcon className="text-background scale-125" />
          )}
        </AvatarBadge>
      )}
    </BaseAvatar>
  )
}

export function GroupAvatar(props: GroupAvatarProps) {
  const { users, layout = 'compact' } = props

  if (users.length === 1) {
    const { layout: _, ...rest } = props
    return <SingleAvatar user={users[0]} {...(rest as BaseAvatarProps)} />
  }

  if (layout === 'compact') {
    return <CompactGroupAvatar {...(props as CompactGroupAvatarProps)} />
  }

  return <SpreadGroupAvatar {...(props as SpreadGroupAvatarProps)} />
}

function CompactGroupAvatar({ users, ...props }: CompactGroupAvatarProps) {
  const visibleUsers = users.slice(0, 2)

  return (
    <BaseAvatar
      {...props}
      className={cn('bg-transparent after:hidden', props.className)}
    >
      {visibleUsers.map((user, i) => (
        <BaseAvatar
          key={i}
          size={props.size}
          className={cn(
            'absolute h-[75%] w-[75%] border-2 border-background shadow-none after:hidden',
            i === 0 ? 'top-0 right-0 z-10' : 'bottom-0 left-0 z-0',
          )}
        >
          <AvatarImage src={user.avatarURL} alt={user.name} />
          <AvatarFallback className="text-[0.625rem] group-data-[size=lg]/avatar:text-[0.75rem] group-data-[size=sm]/avatar:text-[0.5rem]">
            {getAvatarInitials(user.name)}
          </AvatarFallback>
        </BaseAvatar>
      ))}
    </BaseAvatar>
  )
}

function SpreadGroupAvatar({ users, ...props }: SpreadGroupAvatarProps) {
  const MAX_VISIBLE_USERS = 3

  const visibleUsers = users.slice(0, MAX_VISIBLE_USERS)
  const showCount = users.length > MAX_VISIBLE_USERS

  return (
    <BaseAvatarGroup {...props}>
      {visibleUsers.map((user, index) => (
        <SingleAvatar key={index} user={user} />
      ))}
      {showCount && (
        <AvatarGroupCount>+{users.length - MAX_VISIBLE_USERS}</AvatarGroupCount>
      )}
    </BaseAvatarGroup>
  )
}

export function ChannelAvatar({
  className,
  channel,
  ...props
}: ChannelAvatarProps) {
  const { name, avatarURL } = channel

  const fallback = useMemo(() => getAvatarInitials(name), [name])

  return (
    <BaseAvatar
      {...props}
      className={cn('rounded-md **:rounded-md after:rounded-md', className)}
    >
      <AvatarImage src={avatarURL} />
      <AvatarFallback>{fallback}</AvatarFallback>
    </BaseAvatar>
  )
}
