import { execSync } from "child_process"

// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  app: {
    baseURL: '/unity-docs',
    head: {
      link: [
        { rel: 'icon', type: 'image/x-icon', href: '/unity-docs/favicon.ico'},
      ]
    }
  },

  hooks: {
    'build:before': () => {
      const latestTag = execSync('git describe --tags --abbrev=0', { encoding: 'utf-8' }).trim()
      process.env.GIT_TAG = latestTag
    }
  },

  runtimeConfig: {
    public: {
      gitTag: process.env.GIT_TAG || 'No version',
    }
  },

  modules: ['@nuxt/content'],

  content: {
    // コードハイライト有効化
    highlight: {
      theme: "github-dark",
      langs: [
        'csharp',
        'lua',
      ]
    },
  },

  build: {
    transpile: ['vuetify'],
  },

  css: ['vuetify/lib/styles/main.sass', 'mdi/css/materialdesignicons.min.css', '@/assets/main.scss'],

  plugins: [
    '@/plugins/constants.ts',
  ],

  compatibilityDate: '2024-08-13'
})