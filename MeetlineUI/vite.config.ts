import { defineConfig } from 'vite'
import { devtools } from '@tanstack/devtools-vite'
import tsconfigPaths from 'vite-tsconfig-paths'

import { tanstackRouter } from '@tanstack/router-plugin/vite'

import viteReact from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'

import { heyApiPlugin } from '@hey-api/vite-plugin'
import heyApiConfig from './openapi-ts.config.ts'

const config = defineConfig({
  server: { allowedHosts: ['meetline.maddock.world'] },
  plugins: [
    devtools(),
// @ts-ignore
    heyApiPlugin({
// @ts-ignore
      config: heyApiConfig,
    }),
    tsconfigPaths({ projects: ['./tsconfig.json'] }),
    tailwindcss(),
    tanstackRouter({ target: 'react', autoCodeSplitting: true }),
    viteReact({
      babel: {
        plugins: ['babel-plugin-react-compiler'],
      },
    }),
  ],
  build: {
    chunkSizeWarningLimit: 800 // kB
  },
  resolve: {
    extensions: ['.ts', '.tsx', '.js', '.jsx'],
  }
})

export default config
