import React, { useEffect } from 'react'
import { MailIcon, PhoneOutgoingIcon } from 'lucide-react'
import { create } from 'zustand'
import { Button } from '#/components/ui/button'
import {
  Command,
  CommandDialog,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
  CommandShortcut,
} from '#/components/ui/command'
import { Avatar, AvatarImage } from '#/components/ui/avatar.tsx'

type UniversalSearchState = {
  isOpen: boolean
  setIsOpen: (open: boolean | ((prev: boolean) => boolean)) => void
}

export const useUniversalSearch = create<UniversalSearchState>((set) => ({
  isOpen: false,
  setIsOpen: (open) =>
    set((state) => ({
      isOpen: typeof open === 'function' ? open(state.isOpen) : open,
    })),
}))

export function UniversalSearch() {
  const { isOpen, setIsOpen } = useUniversalSearch()

  useEffect(() => {
    const down = (e: KeyboardEvent) => {
      if ((e.metaKey || e.ctrlKey) && e.key === 'k') {
        e.preventDefault()
        setIsOpen(!isOpen)
      }
    }
    window.addEventListener('keydown', down)
    return () => window.removeEventListener('keydown', down)
  }, [isOpen, setIsOpen])

  return (
    <CommandDialog open={isOpen} onOpenChange={setIsOpen}>
      <Command>
        <CommandInput placeholder="Find people, chats, and more" />
        <CommandList>
          <CommandEmpty>No results found.</CommandEmpty>
          <CommandGroup heading="People">
            {[1, 2, 3, 4, 5, 6, 7, 8, 9].map((x) => (
              <CommandItem key={x}>
                <Avatar>
                  <AvatarImage
                    src={`https://randomuser.me/api/portraits/lego/${x}.jpg`}
                  />
                </Avatar>
                Test user #{x}
                <CommandShortcut>
                  <Button variant={'ghost'}>
                    <PhoneOutgoingIcon />
                  </Button>
                  <Button variant={'ghost'}>
                    <MailIcon />
                  </Button>
                </CommandShortcut>
              </CommandItem>
            ))}
          </CommandGroup>
        </CommandList>
      </Command>
    </CommandDialog>
  )
}
