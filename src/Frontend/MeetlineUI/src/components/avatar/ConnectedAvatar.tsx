import { Avatar } from './Avatar'
import type { SingleAvatarProps } from './Avatar'
import { usePresenceStore } from '#/stores/presence'

type ConnectedAvatarUser = {
  id: string
  name: string
  avatarURL?: string
}

type ConnectedAvatarProps = Omit<SingleAvatarProps, 'presence'> & {
  user: ConnectedAvatarUser
}

export function ConnectedAvatar({ user, ...props }: ConnectedAvatarProps) {
  const presence = usePresenceStore((s) => s.presences[user.id])

  return <Avatar {...props} variant="single" user={user} presence={presence} />
}
