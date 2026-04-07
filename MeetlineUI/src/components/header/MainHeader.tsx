import React, { useEffect, useState } from 'react'
import { MailIcon, PhoneOutgoingIcon, SparklesIcon } from 'lucide-react'
import { SidebarTrigger } from '#/components/ui/sidebar.tsx'
import { useIsMobile } from '#/hooks/use-mobile.ts'
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
import { Kbd } from '#/components/ui/kbd.tsx'
import { Avatar, AvatarImage } from '#/components/ui/avatar.tsx'
import {
  Sheet,
  SheetContent,
  SheetHeader,
  SheetTitle,
  SheetTrigger,
} from '#/components/ui/sheet.tsx'
import { UniversalSearchTrigger } from '#/components/universal-search/UniversalSearchTrigger.tsx'

export function MainHeader() {
  const isMobile = useIsMobile()

  useEffect(() => {
    const down = (e: KeyboardEvent) => {
      if (e.key === 'k' && (e.metaKey || e.ctrlKey)) {
        e.preventDefault()
        setOpen((open) => !open)
      }
    }
    document.addEventListener('keydown', down)
    return () => document.removeEventListener('keydown', down)
  }, [])

  return (
    <header className="h-12 shrink-0 border-b grid grid-cols-3 items-center px-4 bg-sidebar border-sidebar-border">
      <div className="flex items-center">
        {isMobile && <SidebarTrigger className="mr-4" />}
      </div>

      <div className="flex justify-center">
        <UniversalSearchTrigger />
      </div>

      <div className="flex justify-end">
        <Sheet>
          <SheetTrigger render={<Button variant={'ghost'} />}>
            <SparklesIcon />
          </SheetTrigger>
          <SheetContent>
            <SheetHeader>
              <SheetTitle>Valis AI</SheetTitle>
            </SheetHeader>
            <iframe
              src="https://ai.valis.jala.university"
              title="Valis AI"
              width="100%"
              height="100%"
            ></iframe>
          </SheetContent>
        </Sheet>
      </div>
    </header>
  )
}
