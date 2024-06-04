import { defineConfig } from 'vite';
import topLevelAwait from 'vite-plugin-top-level-await';
import dtsPlugin from 'vite-plugin-dts';
import wasm from 'vite-plugin-wasm';

export default defineConfig({
  build: {
    lib: {
      entry: 'main.ts',
      name: '@devolutions/devolutions-crypto',
      formats: ['es'],
    },
  },
  plugins: [wasm(), topLevelAwait(), dtsPlugin()],
});