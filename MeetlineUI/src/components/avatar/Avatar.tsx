import React, { useMemo } from 'react'
import {
  AvatarBadge,
  AvatarFallback,
  AvatarImage,
  Avatar as BaseAvatar,
} from '../ui/avatar'
import { getAvatarInitials } from '#/lib/utils/avatar-utils'
import { MinusIcon } from 'lucide-react'
import { cn } from '#/lib/utils'

export type AvatarUser = {
  name: string
  avatarURL?: string
}

export type Presence = 'online' | 'dnd' | 'away' | 'busy' | 'offline'

type BaseAvatarProps = React.ComponentPropsWithoutRef<typeof BaseAvatar>

export interface SingleAvatarProps extends BaseAvatarProps {
  variant: 'single'
  user: AvatarUser
  presence?: Presence
}

export interface GroupAvatarProps extends BaseAvatarProps {
  variant: 'group'
  users: AvatarUser[]
}

export interface ChannelAvatarProps extends BaseAvatarProps {
  variant: 'channel'
  name: string
  avatarURL?: string
}

const presenceColors: Record<Presence, string> = {
  online: 'bg-green-600 dark:bg-green-800',
  away: 'bg-yellow-600 dark:bg-yellow-800',
  dnd: 'bg-red-600 dark:bg-red-800',
  busy: 'bg-red-600 dark:bg-red-800',
  offline: 'bg-gray-600 dark:bg-gray-800',
}

export type AvatarProps =
  | SingleAvatarProps
  | GroupAvatarProps
  | ChannelAvatarProps

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

function SingleAvatar({ user, presence, ...props }: SingleAvatarProps) {
  const { name, avatarURL } = user

  const fallback = useMemo(() => getAvatarInitials(name), [name])

  return (
    <BaseAvatar {...props}>
      <AvatarImage src={avatarURL} />
      <AvatarFallback>{fallback}</AvatarFallback>
      {presence && (
        <AvatarBadge className={presenceColors[presence]}>
          {presence === 'dnd' && <MinusIcon className="text-background scale-125" />}
        </AvatarBadge>
      )}
    </BaseAvatar>
  )
}

function GroupAvatar({ users, ...props }: GroupAvatarProps) {
  if (users.length === 1) {
    return (
      <SingleAvatar
        user={users[0]}
        variant="single"
        {...(props as BaseAvatarProps)}
      />
    )
  }

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
            i === 0 ? 'top-0 right-0 z-10' : 'bottom-0 left-0 z-0'
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


function ChannelAvatar({ className, name, avatarURL, ...props }: ChannelAvatarProps) {
  const fallback = useMemo(() => getAvatarInitials(name), [name])

  return (
    <BaseAvatar
      {...props}
      className={cn(
        'rounded-md **:rounded-md after:rounded-md',
        className
      )}
    >
      <AvatarImage src={avatarURL} />
      <AvatarFallback>{fallback}</AvatarFallback>
    </BaseAvatar>
  )
}
