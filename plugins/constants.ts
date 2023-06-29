export default defineNuxtPlugin (nuxtApp => {
  const categories = [
    { title: "Invector", contentPath: "/invector", descShort: "TPSコントローラ", text: "" },
    { title: "ゲームデザイン関連", contentPath: "/game_design", descShort: "ゲームデザイン", text: "" },
    { title: "Quest Machine", contentPath: "/questmachine", descShort: "クエスト管理アセット", text: ""},
  ]
  return {
    provide: {
      categories(message: string) {
        return categories
      }
    }
  }
})