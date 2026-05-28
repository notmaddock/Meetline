import { Toaster } from 'sonner'
import type { CSSProperties, ComponentProps } from 'react'

export const ToasterIdentifiers = {
  NotificationToaster: 'notifications',
  MessageToaster: 'messages',
} as const

const defaultStyle: CSSProperties = {
  background: 'rgba(0, 0, 0, .4)',
  backdropFilter: 'blur(8px)',
  WebkitBackdropFilter: 'blur(8px)',
  color: '#fff',
  userSelect: 'none',
}

const toasters: Record<string, ComponentProps<typeof Toaster>> = {
  [ToasterIdentifiers.MessageToaster]: {
    style: defaultStyle,
    position: 'bottom-right',
  },

  [ToasterIdentifiers.NotificationToaster]: {
    style: defaultStyle,
    position: 'top-center',
  },
}

export function Toasters() {
  return (
    <>
      {Object.entries(toasters).map(([key, toaster]) => (
        <Toaster key={key} id={key} {...toaster} />
      ))}
    </>
  )
}
