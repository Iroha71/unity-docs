export default defineNuxtPlugin (nuxtApp => {
  const categories = [
    { title: "Invector", contentPath: "/invector", descShort: "TPSコントローラ", text: "", prependIcon: "mdi-run" },
    { title: "シーンセットアップ", contentPath: "/scene_setup", descShort: "セットアップ手順", text: "", prependIcon: "mdi-tune" },
    { title: "ゲームデザイン関連", contentPath: "/game_design", descShort: "ゲームデザイン", text: "", prependIcon: "mdi-palette" },
    { title: "自作スクリプト", contentPath: "/original", descShort: "自作スクリプト", text: "", prependIcon: "mdi-account" },
    { title: "Quest Machine", contentPath: "/questmachine", descShort: "クエスト管理アセット", text: "", prependIcon: "mdi-book-variant" },
    { title: "Dialogue System", contentPath: "/dialoguesystem", descShort: "会話アセット", text: "", prependIcon: "mdi-forum-outline" },
    { title: "Enviro", contentPath: "/enviro", descShort: "天候管理アセット", text: "", prependIcon: "mdi-cloud" },
    { title: "Gaia2", contentPath: "/gaia", descShort: "地形編集アセット", text: "", prependIcon: "mdi-image-filter-hdr" },
  ]
  return {
    provide: {
      categories() {
        return categories
      }
    }
  }
})