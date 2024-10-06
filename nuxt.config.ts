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
    markdown: {
      remarkPlugins: ['remark-breaks',],
    },
  },

  build: {
    transpile: ['vuetify'],
  },

  vite: {
    css: {
      preprocessorOptions: {
        sass: {
          api: "modern",
        }
      }
    }
  },

  css: ['vuetify/lib/styles/main.sass', 'mdi/css/materialdesignicons.min.css', '@/assets/main.scss'],

  plugins: [
    '@/plugins/constants',
  ],

  compatibilityDate: '2024-08-13'
})