import * as fs from 'node:fs'
import * as path from 'node:path'
import { defineConfig } from 'orval'

const SCHEMA_PATH = './src/server/Web.json'

if (!fs.existsSync(SCHEMA_PATH)) {
  console.warn(
    `Schema file not found at "${path.resolve(SCHEMA_PATH)}". ` +
      `Ensure the backend has generated the JSON schema before running Orval.`,
  )
}

export default defineConfig({
  'meetline_axios': {
    input: {
      target: SCHEMA_PATH,
    },
    output: {
      mode: 'tags-split',
      client: 'axios',
      httpClient: 'axios',
      target: './src/server/api/generated/api-client.ts',
      schemas: './src/server/api/generated/schemas',
      override: {
        mutator: {
          path: './src/server/api/orval-fetcher.ts',
          name: 'fetcher',
        },
      },
    },
  },

  'meetline_zod': {
    input: {
      target: SCHEMA_PATH,
    },
    output: {
      mode: 'tags-split',
      client: 'zod',
      target: './src/server/api/generated/zod',
      fileExtension: '.zod.ts',
    },
  },
})
