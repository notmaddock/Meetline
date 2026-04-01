import { createFileRoute } from '@tanstack/react-router'
import { Input } from '#/components/ui/input.tsx'
import { Search } from 'lucide-react'
import {
  ResizableHandle,
  ResizablePanel,
  ResizablePanelGroup,
} from '#/components/ui/resizable.tsx'

export const Route = createFileRoute('/_auth/_sidebar/')({
  component: Index,
})

function Index() {
  return (
    <ResizablePanelGroup orientation="horizontal" className="h-full w-full">
      {/* Chats List Pane */}
      <ResizablePanel defaultSize={'30%'} minSize={'20%'} maxSize={'45%'}>
        <section className={'bg-muted/50 flex flex-col h-full'}>
          <header className={'p-4 space-y-4'}>
            <h1 className={'text-xl font-semibold px-1'}>Chats</h1>
            <div className={'relative'}>
              <Search
                className={
                  'absolute left-2.5 top-2 size-4 text-muted-foreground'
                }
              />
              <Input placeholder={'Search chats...'} className={'pl-8'} />
            </div>
          </header>

          <div className={'flex-1 overflow-y-auto px-2 pb-4 space-y-1'}>
            {[1, 2, 3, 4, 5].map((i) => (
              <div
                key={i}
                className={
                  'group flex items-center gap-3 p-3 rounded-lg hover:bg-accent cursor-pointer transition-colors'
                }
              >
                <div
                  className={
                    'size-10 rounded-full bg-primary/10 flex items-center justify-center shrink-0'
                  }
                >
                  <span className={'text-primary font-medium text-sm'}>
                    C{i}
                  </span>
                </div>
                <div className={'flex-1 min-w-0'}>
                  <div className={'flex items-center justify-between gap-2'}>
                    <p className={'text-sm font-semibold truncate'}>
                      Chat Group {i}
                    </p>
                    <span
                      className={'text-[10px] text-muted-foreground shrink-0'}
                    >
                      12:30 PM
                    </span>
                  </div>
                  <p className={'text-xs text-muted-foreground truncate'}>
                    Latest message from this chat group...
                  </p>
                </div>
              </div>
            ))}
          </div>
        </section>
      </ResizablePanel>

      <ResizableHandle withHandle className="!outline-none" />

      {/* Chat Content Pane */}
      <ResizablePanel defaultSize={70}>
        <section className={'flex-1 h-full flex flex-col'}>
          <div
            className={
              'flex-1 flex items-center justify-center text-muted-foreground bg-background'
            }
          >
            <p>Select a chat to start messaging</p>
          </div>
        </section>
      </ResizablePanel>
    </ResizablePanelGroup>
  )
}
