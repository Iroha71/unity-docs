// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  modules: [
    '@nuxt/content',
  ],
  content: {
    // コードハイライト有効化
    highlight: {
      theme: "github-dark-dimmed",
    },
  },
  build: {
    transpile: ['vuetify'],
  },
  css: ['@/assets/main.scss'],
})
