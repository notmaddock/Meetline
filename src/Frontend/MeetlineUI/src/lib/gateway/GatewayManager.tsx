import { useAuth } from '@clerk/react'
import { useEffect } from 'react'
import { gateway } from './gateway'

export function GatewayManager() {
  const { isLoaded, isSignedIn, getToken } = useAuth()

  useEffect(() => {
    if (isLoaded && isSignedIn) {
      gateway.initialize(getToken)
    }
  }, [isLoaded, isSignedIn, getToken])

  useEffect(() => {
    if (!isLoaded || !isSignedIn) return

    gateway.connect()

    return () => {
      gateway.disconnect()
    }
  }, [isLoaded, isSignedIn])

  return null
}
