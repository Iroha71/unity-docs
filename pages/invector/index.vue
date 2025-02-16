<template>
  <VContainer fluid>
    <VRow>
      <VCol>
        <VTextField 
          label="記事の検索キーワード"
          append-inner-icon="mdi-magnify"
          v-model="keyword"
        />
      </VCol>
      <VCol>
        <VSelect
          label="カテゴリ検索"
          :items="getCategoryNames()"
          v-model="selectedCategory"
          append-icon="mdi-close"
        />
      </VCol>
    </VRow>
    <VRow dense>
      <VCol
        cols="12"
        lg="3"
        md="4"
        sm="6"
        v-for="article in filteredArticles"
        :key="article.title"
      >
        <ArticleLinkCard 
          :title="article.title"
          :description="article.description"
          :page-name="`invector/${article.pageName}`"
          :category="article.category"
          :thumbnail="article.thumbnail"
          :tags="article.tags"
        />
      </VCol>
    </VRow>
  </VContainer>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { VCol, VTextField } from 'vuetify/components';
import ArticleLinkCard from '~/components/ArticleLinkCard.vue';
const { $articleCategories } = useNuxtApp()

const keyword = ref("");
const selectedCategory = ref("");

const getCategoryNames = (): string[] => {
  let names: string[] = ["*"];
  $articleCategories().forEach(category => {
    names.push(category.name);
  });

  return names;
}

const filteredArticles = computed(() => {
  return articles.filter(article => {
    return isMatchCategory(article.category)
      && isMatchTitle(article.title)
  });
});

const isMatchCategory = (category: string): boolean => {
  if (selectedCategory.value === "" || selectedCategory.value === "*")
    return true;
  else if (selectedCategory.value === category)
    return true;
  else
    return false;
}

const isMatchTitle = (title: string): boolean => {
  return title.includes(keyword.value);
}

interface Article {
  title: string,
  description: string,
  pageName: string,
  category: string,
  thumbnail: string,
  tags: string[],
}

const articles: Article[] = [
  {
    title: "キャラのセットアップ",
    description: "AI / プレイヤーの設定値まとめ",
    pageName: "shooter_setup",
    category: "util",
    thumbnail: "source_code",
    tags: ["初期設定", "戦闘", "銃"]
  },
  { 
    title: "パリィやガードブレイクしたい", 
    description: "ガードパリィ", 
    pageName: "guardbreak", 
    category: "melee",
    thumbnail: "spark", 
    tags: ["戦闘"]
  },
  {
    title: "ローリング",
    description: "設定値・ローリングキャンセル",
    pageName: "rolling_cancel",
    category: "melee",
    thumbnail: "rolling",
    tags: [],
  },
  {
    title: "スタン機能",
    description: "実装方法",
    pageName: "stun",
    category: "melee",
    thumbnail: "blur_light",
    tags: ["戦闘"],
  },
  {
    title: "テイクダウン実装",
    description: "テイクダウンの実装方法",
    pageName: "takedown",
    category: "melee",
    thumbnail: "broken_grass",
    tags: ["戦闘"],
  },
  {
    title: "長押し処理について",
    description: "チャージ攻撃の実装など",
    pageName: "button_longpress",
    category: "util",
    thumbnail: "controllers",
    tags: ["戦闘", "キャラコン"],
  },
  {
    title: "ダメージに要素を追加する方法",
    description: "ヘッドショット・無敵化・追加プロパティ実装方法",
    pageName: "damage_config",
    category: "melee",
    thumbnail: "katana",
    tags: ["戦闘", "銃"],
  },
  {
    title: "左手での攻撃",
    description: "武器の装備方法や入力実装",
    pageName: "lefthand",
    category: "melee",
    thumbnail: "katana_2",
    tags: ["戦闘", "キャラコン"],
  },
  {
    title: "状態異常の実装",
    description: "Factoryパターンによる効率的な実装方法",
    pageName: "abnormal",
    category: "melee",
    thumbnail: "poison",
    tags: ["戦闘"],
  },
  {
    title: "武器の設定値まとめ",
    description: "ShooterのWeapon IDやスコープ設定方法",
    pageName: "gun_config",
    category: "shooter",
    thumbnail: "gun",
    tags: ["戦闘", "銃", "初期設定"],
  },
  {
    title: "弓の自作方法",
    description: "セットアップ方法",
    pageName: "bow",
    category: "shooter",
    thumbnail: "bow",
    tags: ["戦闘", "銃"],
  },
  {
    title: "円形メニューを作る方法",
    description: "アイテム選択メニューの作り方",
    pageName: "radial_menu",
    category: "item",
    thumbnail: "cut_pie",
    tags: ["ui"],
  },
  {
    title: "スクリプトからアイテムを制御する",
    description: "アイテム使用・アイテム使用制限について",
    pageName: "item_script",
    category: "item",
    thumbnail: "bottle",
    tags: [],
  },
  {
    title: "装備スロットの追加方法",
    description: "UIの追加方法",
    pageName: "add_slot",
    category: "item",
    thumbnail: "bottle_tools",
    tags: ["ui"],
  },
  {
    title: "AIへのプロパティ追加方法",
    description: "スタミナの実装など",
    pageName: "ai_props",
    category: "ai",
    thumbnail: "ai",
    tags: ["戦闘"],
  },
  {
    title: "ステートの追加方法",
    description: "参考になるFSM・ステート追加方法",
    pageName: "add_state",
    category: "ai",
    thumbnail: "gear",
    tags: ["ai"],
  },
  {
    title: "音がした方向に向かわせる方法",
    description: "ステートの実装方法",
    pageName: "noise",
    category: "ai",
    thumbnail: "speaker",
    tags: ["ai"],
  },
  {
    title: "AIの挙動改善",
    description: "弾きや予備動作の実装方法",
    pageName: "ai_melee",
    category: "ai",
    thumbnail: "tower",
    tags: ["ai", "戦闘"],
  },
  {
    title: "吹き飛ばしの実装",
    description: "AddForceExplosionの使い方",
    pageName: "explode",
    category: "shooter",
    thumbnail: "explode",
    tags: ["戦闘", "銃"],
  },
  {
    title: "投擲物の軌道計算",
    description: "参考スクリプト",
    pageName: "orbit",
    category: "shooter",
    thumbnail: "orbit",
    tags: ["戦闘", "銃"],
  },
  {
    title: "投擲物の作成",
    description: "投擲物の作成手順",
    pageName: "create_grenade",
    category: "shooter",
    thumbnail: "fireworks",
    tags: ["戦闘", "銃"],
  },
  {
    title: "エイムと投擲を同じボタンにする",
    description: "",
    pageName: "same_throw_input",
    category: "shooter",
    thumbnail: "game_devices",
    tags: ["戦闘", "銃", "キャラコン"],
  },
  {
    title: "UIカーソルとパッドに対応させる",
    description: "カーソルセットアップ方法",
    pageName: "ui_cursor",
    category: "util",
    thumbnail: "mouse",
    tags: ["ui"],
  }
]
</script>