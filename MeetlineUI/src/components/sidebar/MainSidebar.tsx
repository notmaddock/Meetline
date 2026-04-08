import { CalendarDaysIcon, MessageCircleIcon } from 'lucide-react'
import { Link } from '@tanstack/react-router'
import type { ComponentProps } from 'react'
import type { SidebarUser } from '#/components/sidebar/NavUser.tsx'
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarGroup,
  SidebarGroupContent,
  SidebarGroupLabel,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
} from '#/components/ui/sidebar.tsx'
import { NavUser } from '#/components/sidebar/NavUser.tsx'
import MeetpointIcon from '#/components/icons/MeetpointIcon.tsx'
import TeamsIcon from '#/components/icons/TeamsIcon.tsx'
import OutlookIcon from '#/components/icons/OutlookIcon.tsx'
import CanvasLMSIcon from '#/components/icons/CanvasLMSIcon.tsx'
import { UniversalSearchTrigger } from '#/components/universal-search/UniversalSearchTrigger.tsx'
import { useIsMobile } from '#/hooks/use-mobile.ts'

type MainSidebarProps = ComponentProps<typeof Sidebar> & {
  user: SidebarUser | null
  currentPath: string
}

export function MainSidebar({ user, currentPath, ...props }: MainSidebarProps) {
  const isMobile = useIsMobile()

  const mainLinks = [
    {
      name: 'Chats',
      icon: <MessageCircleIcon />,
      to: '/chats',
    },
    {
      name: 'Calendar',
      icon: <CalendarDaysIcon />,
      to: '/calendar',
    },
  ]

  const footerLinks = [
    {
      name: 'MeetPoint',
      icon: <MeetpointIcon />,
      href: 'https://meetpoint.jala.university',
    },
    {
      name: 'Microsoft Teams',
      icon: <TeamsIcon />,
      href: 'https://teams.cloud.microsoft',
    },
    {
      name: 'Microsoft Outlook',
      icon: <OutlookIcon />,
      href: 'https://outlook.cloud.microsoft',
    },
    {
      name: 'Canvas LMS',
      icon: <CanvasLMSIcon />,
      href: 'https://lms.jala.university',
    },
  ]

  return (
    <Sidebar collapsible={'icon'} {...props}>
      <SidebarHeader className="flex">
        {isMobile && <UniversalSearchTrigger className={'max-w-full'} />}
      </SidebarHeader>
      <SidebarContent>
        <SidebarGroup>
          <SidebarGroupLabel>Meetline</SidebarGroupLabel>
          <SidebarMenu>
            {mainLinks.map((link) => (
              <SidebarMenuItem key={link.name}>
                <SidebarMenuButton
                  isActive={link.to === currentPath}
                  render={<Link to={link.to} />}
                  tooltip={link.name}
                >
                  {link.icon}
                  {link.name}
                </SidebarMenuButton>
              </SidebarMenuItem>
            ))}
          </SidebarMenu>
        </SidebarGroup>
        <SidebarGroup className={'mt-auto'}>
          <SidebarGroupLabel>External</SidebarGroupLabel>
          <SidebarMenu>
            {footerLinks.map((link) => (
              <SidebarMenuItem key={link.name}>
                <SidebarMenuButton
                  render={<a href={link.href} target={'_blank'} />}
                  tooltip={link.name}
                >
                  {link.icon}
                  {link.name}
                </SidebarMenuButton>
              </SidebarMenuItem>
            ))}
          </SidebarMenu>
        </SidebarGroup>
      </SidebarContent>
      <SidebarFooter>
        <NavUser user={user} />
      </SidebarFooter>
    </Sidebar>
  )
}
