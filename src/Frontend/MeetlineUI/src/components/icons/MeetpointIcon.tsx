import type { ComponentProps } from 'react'

const MeetpointIcon = (props: ComponentProps<'svg'>) => (
  <svg
    width={512}
    height={512}
    viewBox="0 0 512 512"
    fill="none"
    xmlns="http://www.w3.org/2000/svg"
    {...props}
  >
    <path
      d="M256 0C114.685 0 0 114.541 0 256C0 397.459 114.541 512 256 512C397.459 512 512 397.459 512 256C512 114.541 397.315 0 256 0ZM331.454 384.43C260.582 384.43 203.025 326.872 203.025 256C203.025 185.128 260.582 127.57 331.454 127.57C402.327 127.57 459.884 185.128 459.884 256C459.884 326.872 402.47 384.43 331.454 384.43Z"
      fill="#0883e8"
    />
  </svg>
)
export default MeetpointIcon
