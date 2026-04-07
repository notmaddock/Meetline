import { createServerFn } from '@tanstack/react-start'
import { getUserEndpoints } from '../generated/user-endpoints/user-endpoints'
import { queryOptions } from '@tanstack/react-query'

export const getUsersMe = createServerFn({ method: 'GET' }).handler(
  async () => {
    const result = await getUserEndpoints().getApiUsersMe()
    return result
  },
)

export const getUsersMeQuery = () =>
  queryOptions({
    queryKey: ['users', 'me'],
    queryFn: () => getUsersMe(),
  })
