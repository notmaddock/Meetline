import { create } from 'zustand'

export type Presence = 'online' | 'away' | 'dnd' | 'busy' | 'offline'

type PresenceStore = {
  presences: Record<string, Presence>
  setPresence: (userId: string, presence: Presence) => void
}

export const usePresenceStore = create<PresenceStore>((set) => ({
  presences: {},
  setPresence: (userId, presence) =>
    set((state) => ({
      presences: {
        ...state.presences,
        [userId]: presence,
      },
    })),
}))
