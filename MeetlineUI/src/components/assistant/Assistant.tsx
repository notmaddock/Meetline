import { SparklesIcon } from 'lucide-react'
import { create } from 'zustand'
import { Button } from '../ui/button'
import {
  Sheet,
  SheetContent,
  SheetDescription,
  SheetFooter,
  SheetHeader,
  SheetTitle,
  SheetTrigger,
} from '../ui/sheet'
import { Checkbox } from '../ui/checkbox'
import {
  Field,
  FieldContent,
  FieldDescription,
  FieldLabel,
  FieldTitle,
} from '../ui/field'

const ASSISTANT_URL = 'https://ai.valis.jala.university'
const ASSISTANT_TITLE = 'Valis AI'

type AssistantState = {
  isOpen: boolean
  isClosingPrevented: boolean
  setIsOpen: (open: boolean | ((prev: boolean) => boolean)) => void
  setIsClosingPrevented: (open: boolean | ((prev: boolean) => boolean)) => void
}

export const useAssistant = create<AssistantState>((set) => ({
  isOpen: false,
  isClosingPrevented: false,
  setIsOpen: (open) =>
    set((state) => ({
      isOpen:
        state.isClosingPrevented ||
        (typeof open === 'function' ? open(state.isOpen) : open),
    })),
  setIsClosingPrevented: (prevent) =>
    set((state) => ({
      isClosingPrevented:
        typeof prevent === 'function'
          ? prevent(state.isClosingPrevented)
          : prevent,
    })),
}))

export function Assistant() {
  const { isOpen, setIsOpen, isClosingPrevented, setIsClosingPrevented } =
    useAssistant()

  return (
    <Sheet open={isOpen || isClosingPrevented} onOpenChange={setIsOpen}>
      <SheetContent
        className={'max-w-2xl!'}
        showCloseButton={!isClosingPrevented}
      >
        <SheetHeader>
          <SheetTitle>Valis AI</SheetTitle>
          {isClosingPrevented && (
            <SheetDescription>
              This dialog is currently locked and can't close. Use the checkbox
              below to allow closing
            </SheetDescription>
          )}
        </SheetHeader>
        <iframe
          src={ASSISTANT_URL}
          title={ASSISTANT_TITLE}
          width="100%"
          height="100%"
        ></iframe>
        <SheetFooter>
          <FieldLabel>
            <Field orientation="horizontal">
              <Checkbox
                checked={isClosingPrevented}
                onCheckedChange={setIsClosingPrevented}
              />
              <FieldContent>
                <FieldTitle>Lock open</FieldTitle>
                <FieldDescription>
                  Doing something important? Prevent this dialog from closing
                  and don't lose your progress.
                </FieldDescription>
              </FieldContent>
            </Field>
          </FieldLabel>
        </SheetFooter>
      </SheetContent>
    </Sheet>
  )
}
