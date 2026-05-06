import type {Meta, StoryObj} from '@storybook/react-vite'
import type {AvatarProps} from '#/components/avatar/Avatar.tsx'
import {Avatar} from '#/components/avatar/Avatar.tsx'

const meta = {
    title: 'UI/Avatar',
    component: Avatar,
    parameters: {
        layout: 'centered',
    },
} satisfies Meta<typeof Avatar>

export default meta
type Story = StoryObj<typeof Avatar>

const avatarFixtures = {
  single: {
    variant: 'single',
    user: {
      name: 'Maddock',
      avatarURL: 'https://github.com/SowTag.png',
    },
    presence: 'online' as const,
  },
  group: {
    variant: 'group',
    users: [
      { name: 'Maddock', avatarURL: 'https://github.com/SowTag.png' },
      { name: 'Linus', avatarURL: 'https://github.com/torvalds.png' },
      { name: 'Bob' },
    ],
  },
  channel: {
    variant: 'channel',
      channel: {
          name: 'general',
          avatarURL: 'https://picsum.photos/64',
      },
  },
} satisfies {
  [K in AvatarProps['variant']]: Extract<AvatarProps, { variant: K }>
}

export const AllVariants: Story = {
  render: () => (
    <div className="flex flex-col gap-4">
      {Object.entries(avatarFixtures).map(([key, props]) => (
        <div key={key} className="flex items-center gap-2">
          <Avatar {...props} />
          <span>{key}</span>
        </div>
      ))}
    </div>
  ),
}

const presences = ['online', 'away', 'dnd', 'busy', 'offline'] as const

export const SingleStatusVariants: Story = {
  render: () => {
    const base = avatarFixtures.single

    return (
      <div className="flex flex-col gap-4">
        {presences.map((presence) => (
          <div key={presence} className="flex items-center gap-2">
            <Avatar {...base} presence={presence} />
            <span>{presence}</span>
          </div>
        ))}
      </div>
    )
  },
}

export const Sizes: Story = {
  render: () => {
    const sizes = ['sm', 'default', 'lg'] as const
    return (
      <div className="flex flex-col gap-8">
        {sizes.map((size) => (
          <div key={size} className="flex flex-col gap-2">
              <h3 className="text-sm font-medium text-muted-foreground">
                  {size}
              </h3>
            <div className="flex items-center gap-4">
              <Avatar {...avatarFixtures.single} size={size} />
              <Avatar {...avatarFixtures.group} size={size} />
              <Avatar {...avatarFixtures.channel} size={size} />
            </div>
          </div>
        ))}
      </div>
    )
  },
}

export const GroupVariants: Story = {
  render: () => {
    const groupConfigurations = [
      { name: '1 User', users: [avatarFixtures.group.users[0]] },
      { name: '2 Users', users: avatarFixtures.group.users.slice(0, 2) },
      { name: '3 Users', users: avatarFixtures.group.users.slice(0, 3) },
      {
        name: '4 Users',
        users: [...avatarFixtures.group.users, { name: 'Alice' }],
      },
      {
        name: '5+ Users',
        users: [
          ...avatarFixtures.group.users,
          { name: 'Alice' },
          { name: 'Charlie' },
        ],
      },
    ]

    return (
      <div className="flex flex-col gap-4">
        {groupConfigurations.map((config) => (
          <div key={config.name} className="flex items-center gap-2">
            <Avatar variant="group" users={config.users} />
            <span>{config.name}</span>
          </div>
        ))}
      </div>
    )
  },
}

export const Playground: Story = {
  args: {
    variant: 'single',
    size: 'default',
    user: avatarFixtures.single.user,
    users: avatarFixtures.group.users,
    presence: 'online',
      channel: avatarFixtures.channel.channel,
  } as any,
  argTypes: {
    variant: {
      control: 'select',
      options: ['single', 'group', 'channel'],
    },
    size: {
      control: 'select',
      options: ['sm', 'default', 'lg'],
    },
    presence: {
      control: 'select',
      options: ['online', 'away', 'dnd', 'busy', 'offline', undefined],
    },
  },
  render: (args: any) => {
    return <Avatar {...args} />
  },
}
