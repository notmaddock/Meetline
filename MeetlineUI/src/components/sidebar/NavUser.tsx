import { ChevronsUpDown, CircleUserIcon, LogOutIcon } from 'lucide-react'
import { Link } from '@tanstack/react-router'
import type { ComponentProps } from 'react'
import {
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
} from '#/components/ui/sidebar.tsx'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '#/components/ui/dropdown-menu.tsx'
import { Avatar, AvatarFallback, AvatarImage } from '#/components/ui/avatar.tsx'
import { useIsMobile } from '#/hooks/use-mobile.ts'

export type SidebarUser = {
  emailAddress: string | null
  imageUrl: string
  fullName: string | null
  username: string | null
  signOut: () => void
}

type SidebarUserProps = ComponentProps<typeof SidebarMenu> & {
  user: SidebarUser | null
}

function getInitials(fullName: string | null, maxLetters = 2) {
  if (!fullName) return '?'

  return fullName
    .trim()
    .split(/\s+/) // split by spaces
    .filter(Boolean) // remove empty parts
    .slice(0, maxLetters) // limit number of initials
    .map((word) => word[0].toUpperCase())
    .join('')
}

export function NavUser({ user, ...props }: SidebarUserProps) {
  if (!user) return null

  const fallback = getInitials(user.fullName)

  const secondRow = user.username
    ? `@${user.username}`
    : (user.emailAddress ?? null)

  const isMobile = useIsMobile()

  return (
    <SidebarMenu {...props}>
      <SidebarMenuItem>
        <DropdownMenu>
          <DropdownMenuTrigger
            render={
              <SidebarMenuButton
                size={'lg'}
                className={
                  'data-[state=open]:bg-sidebar-accent data-[state=open]:text-sidebar-accent-foreground'
                }
              >
                <Avatar className="h-8 w-8 rounded-lg">
                  <AvatarImage
                    src={user.imageUrl}
                    alt={user.fullName ?? undefined}
                  />
                  <AvatarFallback className="rounded-lg">
                    {fallback}
                  </AvatarFallback>
                </Avatar>
                <div className="grid flex-1 text-left text-sm leading-tight">
                  <span className="truncate font-medium">{user.fullName}</span>
                  {secondRow !== null && (
                    <span className="truncate text-xs">{secondRow}</span>
                  )}
                </div>
                <ChevronsUpDown className="ml-auto size-4" />
              </SidebarMenuButton>
            }
          />

          <DropdownMenuContent
            side={isMobile ? 'top' : 'right'}
            align={'center'}
          >
            <DropdownMenuGroup>
              <DropdownMenuLabel>Account</DropdownMenuLabel>
              <DropdownMenuItem render={<Link to={'/account-settings/$'} />}>
                <CircleUserIcon />
                Account settings
              </DropdownMenuItem>
            </DropdownMenuGroup>
            <DropdownMenuSeparator />
            <DropdownMenuGroup>
              <DropdownMenuItem
                variant={'destructive'}
                onClick={() => user.signOut()}
              >
                <LogOutIcon />
                Log out
              </DropdownMenuItem>
            </DropdownMenuGroup>
          </DropdownMenuContent>
        </DropdownMenu>
      </SidebarMenuItem>
    </SidebarMenu>
  )
}
