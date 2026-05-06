export function getAvatarInitials(name?: string): string {
  if (!name || !name.trim()) {
      return '??'
  }

    const parts = name.trim().split(/\s+/)

  if (parts.length > 1) {
      const first = parts[0][0]
      const last = parts[parts.length - 1][0]
      return (first + last).toUpperCase()
  }

    const word = parts[0]

  if (word.length >= 2) {
      return word.slice(0, 2).toUpperCase()
  }

    return word.toLowerCase()
}
