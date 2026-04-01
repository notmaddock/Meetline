import {
  SidebarGroup,
  SidebarGroupContent,
  SidebarGroupLabel,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
} from '#/components/ui/sidebar.tsx'
import type { ComponentProps } from 'react'
import OutlookIcon from '#/components/icons/OutlookIcon.tsx'
import CanvasIcon from '#/components/icons/CanvasIcon.tsx'
import MeetpointIcon from '#/components/icons/MeetpointIcon.tsx'
import TeamsIcon from '#/components/icons/TeamsIcon.tsx'

const shortcuts = [
  {
    name: 'Canvas',
    icon: <CanvasIcon />,
    href: 'https://lms.jala.university',
  },
  {
    name: 'Meetpoint',
    icon: <MeetpointIcon />,
    href: 'https://meetpoint.jala.university',
  },
  {
    name: 'Outlook',
    icon: <OutlookIcon />,
    href: 'https://outlook.cloud.microsoft',
  },
  {
    name: 'Teams',
    icon: <TeamsIcon />,
    href: 'https://teams.cloud.microsoft',
  },
]

export function SidebarShortcuts({
  showLabel = true,
  ...props
}: ComponentProps<typeof SidebarGroup> & { showLabel?: boolean }) {
  return (
    <SidebarGroup {...props}>
      {showLabel && <SidebarGroupLabel>External</SidebarGroupLabel>}
      <SidebarGroupContent>
        <SidebarMenu>
          {shortcuts.map((shortcut) => (
            <SidebarMenuItem key={shortcut.href}>
              <SidebarMenuButton
                tooltip={!showLabel ? shortcut.name : undefined}
                render={<a href={shortcut.href} target={'_blank'} />}
              >
                {shortcut.icon}
                {showLabel && <span>{shortcut.name}</span>}
              </SidebarMenuButton>
            </SidebarMenuItem>
          ))}
        </SidebarMenu>
      </SidebarGroupContent>
    </SidebarGroup>
  )
}
