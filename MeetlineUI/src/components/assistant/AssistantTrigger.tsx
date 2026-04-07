import { SparklesIcon } from "lucide-react"
import { Button } from "../ui/button"
import { useAssistant } from "./Assistant"

export function AssistantTrigger() {
    const { setIsOpen } = useAssistant()

    return <Button variant={'ghost'} onClick={() => setIsOpen(open => !open)}>
        <SparklesIcon />
    </Button>
}