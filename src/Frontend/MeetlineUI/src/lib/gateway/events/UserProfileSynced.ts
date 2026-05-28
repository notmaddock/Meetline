import { toast } from 'sonner'
import { ToasterIdentifiers } from '#/lib/notifications/toasters'

export function handleUserProfileSynced() {
  toast.success('Your user profile has been synced', {
    toasterId: ToasterIdentifiers.NotificationToaster,
    description:
      'Meetline received an update on your profile. Other users will see your changes shortly.',
  })
}
