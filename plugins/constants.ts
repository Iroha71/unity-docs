export default defineNuxtPlugin (nuxtApp => {

  interface content {
    title: string,
    contentName: string,
    descShort: string,
    text: string,
    prependIcon: string,
    colorVariant: string,
  }

  interface category {
    name: string,
    color: string,
    contents: content[],
  }

  const categories: category[] = [
    {
      name: 'コントローラ',
      color: 'blue-grey-lighten-1',
      contents: [
        {
          title: "Invector",
          contentName: "invector",
          descShort: "TPSコントローラ",
          text: "拡張に必要な情報",
          prependIcon: "mdi-run",
          colorVariant: "blue-grey-darken-3",
        },
        {
          title: "Horse Animset",
          contentName: "hap",
          descShort: "馬コントローラ",
          text: "セットアップ時の補足情報",
          prependIcon: "mdi-horse",
          colorVariant: "blue-grey-darken-2",
        },
      ],
    },
    {
      name: 'セットアップと自作',
      color: 'indigo-lighten-1',
      contents: [
        {
          title: "シーンセットアップ",
          contentName: "scene_setup",
          descShort: "セットアップ手順",
          text: "初期配置オブジェクトについて",
          prependIcon: "mdi-tune",
          colorVariant: "indigo-darken-1",
        },
        {
          title: "ゲームデザイン関連",
          contentName: "game_design",
          descShort: "ゲームデザイン",
          text: "ダメージ計算式など",
          prependIcon: "mdi-palette",
          colorVariant: "indigo-darken-2",
        },
        {
          title: "自作スクリプト",
          contentName: "original",
          descShort: "自作スクリプト",
          text: "戦闘拡張、他の工夫など",
          prependIcon: "mdi-account",
          colorVariant: "indigo-darken-3",
        },
        {
          title: "役立つTips",
          contentName: "useful_tips",
          descShort: "役立つ情報を記載",
          text: "役立つ情報を記載",
          prependIcon: "mdi-lightbulb-on-10",
          colorVariant: "indigo-darken-4",
        },
        {
          title: "着せ替え機能の実装",
          contentName: "runtime_equipment",
          descShort: "リアルタイムの着せ替え（VRM）",
          text: "リアルタイムの着せ替え（VRM）",
          prependIcon: "mdi-tshirt-crew",
          colorVariant: "indigo-darken-1",
        }
      ],
    },
    {
      name: 'RPG',
      color: 'red-lighten-1',
      contents: [
        {
          title: "Dialogue System",
          contentName: "dialoguesystem",
          descShort: "会話アセット",
          text: "セットアップ方法など",
          prependIcon: "mdi-forum-outline",
          colorVariant: "red-darken-1",
        },
        {
          title: "Quest Machine",
          contentName: "questmachine",
          descShort: "クエスト管理アセット",
          text: "セーブ方法やイベント通知実装",
          prependIcon: "mdi-book-variant",
          colorVariant: "red-darken-2",
        },
      ]
    },
    {
      name: '地形',
      color: 'green-lighten-1',
      contents: [
        {
          title: "Enviro",
          contentName: "enviro",
          descShort: "天候管理アセット",
          text: "タイムラプス等の実装",
          prependIcon: "mdi-cloud",
          colorVariant: "green-darken-1",
        },
        {
          title: "Gaia2",
          contentName: "gaia",
          descShort: "地形編集アセット",
          text: "うまく地形を作るためのTIPS",
          prependIcon: "mdi-image-filter-hdr",
          colorVariant: "green-darken-2",
        },
      ]
    },
    {
      name: 'エフェクト',
      color: 'amber-lighten-1',
      contents: [
        {
          title: "X-Weapon Trail",
          contentName: "xweapon_trail",
          descShort: "武器追従エフェクト",
          text: "セットアップ方法",
          prependIcon: "mdi-sword",
          colorVariant: "amber-darken-1",
        },
        {
          title: "Realtoon",
          contentName: "realtoon",
          descShort: "トゥーンシェーダー",
          text: "Tips",
          prependIcon: "mdi-circle-opacity",
          colorVariant: "amber-darken-2",
        }
      ]
    },
    {
      name: 'UI',
      color: 'deep-purple-lighten-1',
      contents: [
        {
          title: "Ultimate Clean GUI",
          contentName: "ultimate_clean_gui",
          descShort: "GUIの通知等を記載",
          text: "GUIの通知の実装",
          prependIcon: "mdi-animation",
          colorVariant: "deep-purple-darken-1",
        },
      ]
    },
  ];

  interface ArticleCategory {
    name: string,
    color: string,
    iconName: string,
  }

  const articleCategories = [
    { name: 'util', color: 'brown-lighten-5', iconName: 'cursor-default' },
    { name: 'shooter', color: 'indigo-lighten-1', iconName: 'pistol' },
    { name: 'melee', color: 'red-lighten-1', iconName: 'sword-cross' },
    { name: 'item', color: 'green-lighten-1', iconName: 'flask' },
    { name: 'ai', color: 'orange-lighten-1', iconName: 'state-machine' },
  ]

  return {
    provide: {
      categories(): category[] {
        return categories
      },
      articleCategories(): ArticleCategory[] {
        return articleCategories
      }
    }
  }
})