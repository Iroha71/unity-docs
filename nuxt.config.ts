// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  app: {
    baseURL: '/unity-docs',
    head: {
      link: [
        { rel: 'icon', type: 'image/x-icon', href: '/fabicon.ico'},
      ]
    }
  },
  modules: [
    '@nuxt/content',
  ],
  content: {
    // コードハイライト有効化
    highlight: {
      theme: "github-dark",
    },
  },
  build: {
    transpile: ['vuetify'],
  },
  css: ['vuetify/lib/styles/main.sass', 'mdi/css/materialdesignicons.min.css', '@/assets/main.scss'],
  plugins: [
    '@/plugins/constants.ts',
  ]
})
