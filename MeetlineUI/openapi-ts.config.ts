import { defineConfig } from '@hey-api/openapi-ts'

export default defineConfig({
  input: 'http://localhost:5200/api/openapi',
  output: 'src/client',
})
