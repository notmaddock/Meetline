import { handleUserProfileSynced } from './events/UserProfileSynced'
import type { HubConnection } from '@microsoft/signalr'

export function mountEventHandlers(connection: HubConnection) {
  connection.on('UserProfileSynced', handleUserProfileSynced)
}
