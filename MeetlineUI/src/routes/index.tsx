import { createFileRoute } from '@tanstack/react-router'
import ThemeToggle from '#/components/ThemeToggle.tsx'

export const Route = createFileRoute('/')({ component: App })

function App() {
  return <ThemeToggle/>
}
