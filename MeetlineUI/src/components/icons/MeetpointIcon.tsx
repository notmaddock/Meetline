import type { ComponentProps } from 'react'

const MeetpointIcon = (props: ComponentProps<'svg'>) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    width={512}
    height={512}
    viewBox={'0 0 512 512'}
    fill="none"
    {...props}
  >
    <path
      fill="#0883e8"
      d="M256 0C114.685 0 0 114.541 0 256s114.541 256 256 256 256-114.541 256-256S397.315 0 256 0Zm75.454 384.43c-70.872 0-128.429-57.558-128.429-128.43 0-70.872 57.557-128.43 128.429-128.43 70.873 0 128.43 57.558 128.43 128.43 0 70.872-57.414 128.43-128.43 128.43Z"
    />
  </svg>
)
export default MeetpointIcon
