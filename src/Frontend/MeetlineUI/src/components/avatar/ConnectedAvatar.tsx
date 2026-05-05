import { Avatar } from "./Avatar";
import type { SingleAvatarProps, AvatarUser } from "./Avatar";
import { usePresenceStore } from "#/stores/presence";

type ConnectedAvatarUser = AvatarUser & {
    id: string;
}

type ConnectedAvatarProps = Omit<SingleAvatarProps, 'presence'> & {
    user: ConnectedAvatarUser;
}

export function ConnectedAvatar({ user, ...props }: ConnectedAvatarProps) {
    const presence = usePresenceStore(s => s.presences[user.id])

    return <Avatar {...props} variant="single" user={user} presence={presence} />
}