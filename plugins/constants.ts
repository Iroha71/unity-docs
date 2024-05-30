export default defineNuxtPlugin (nuxtApp => {
  const categories = [
    {
      title: "ゲームデザイン関連",
      contentName: "game_design",
      descShort: "ゲームデザイン",
      text: "ダメージ計算式など",
      prependIcon: "mdi-palette",
    },
    {
      title: "シーンセットアップ",
      contentName: "scene_setup",
      descShort: "セットアップ手順",
      text: "初期配置オブジェクトについて",
      prependIcon: "mdi-tune",
    },
    {
      title: "自作スクリプト",
      contentName: "original",
      descShort: "自作スクリプト",
      text: "戦闘拡張、他の工夫など",
      prependIcon: "mdi-account",
    },
    {
      title: "Dialogue System",
      contentName: "dialoguesystem",
      descShort: "会話アセット",
      text: "セットアップ方法など",
      prependIcon: "mdi-forum-outline",
    },
    {
      title: "Enviro",
      contentName: "enviro",
      descShort: "天候管理アセット",
      text: "タイムラプス等の実装",
      prependIcon: "mdi-cloud",
    },
    {
      title: "Gaia2",
      contentName: "gaia",
      descShort: "地形編集アセット",
      text: "うまく地形を作るためのTIPS",
      prependIcon: "mdi-image-filter-hdr",
    },
    {
      title: "Horse Animset",
      contentName: "hap",
      descShort: "馬コントローラ",
      text: "セットアップ時の補足情報",
      prependIcon: "mdi-horse",
    },
    {
      title: "Invector",
      contentName: "invector",
      descShort: "TPSコントローラ",
      text: "拡張に必要な情報",
      prependIcon: "mdi-run",
    },
    {
      title: "Quest Machine",
      contentName: "questmachine",
      descShort: "クエスト管理アセット",
      text: "セーブ方法やイベント通知実装",
      prependIcon: "mdi-book-variant",
    },
    {
      title: "Ultimate Clean GUI",
      contentName: "ultimate_clean_gui",
      descShort: "GUIの通知等を記載",
      text: "GUIの通知の実装",
      prependIcon: "mdi-animation",
    },
    {
      title: "X-Weapon Trail",
      contentName: "xweapon_trail",
      descShort: "武器追従エフェクト",
      text: "セットアップ方法",
      prependIcon: "mdi-sword",
    },
  ];
  return {
    provide: {
      categories() {
        return categories
      }
    }
  }
})