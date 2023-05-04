export default defineNuxtPlugin (nuxtApp => {
  const categories = [
    { title: "Invector", contentPath: "/invector", descShort: "TPSコントローラ", text: "" },
  ]
  return {
    provide: {
      categories(message: string) {
        return categories
      }
    }
  }
})