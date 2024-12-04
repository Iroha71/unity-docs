<template>
  <VContainer fluid>
    <VRow 
      dense
      v-for="artGroup in articleGroup"
      :key="artGroup.category"
    >
      <VCol
        cols="3"
        v-for="article in artGroup.articles"
        :key="article.pageName"
      >
        <ArticleLinkCard
          :title="article.title"
          :icon="article.icon"
          :page-name="`invector/${article.pageName}`"
          :category="artGroup.category"
          :color="getCardColor(artGroup.category)"
          :description="article.addInfo"
        />
      </VCol>
    </VRow>
  </VContainer>
</template>

<script setup lang="ts">
import ArticleLinkCard from '~/components/ArticleLinkCard.vue';

const getCardColor = (category: string) => {
  switch (category) {
    case "melee":
      return "red-lighten-1";
    case "shooter":
      return "indigo-lighten-1";
    case "item manager":
      return "green-lighten-1";
    case "ai":
      return "orange-lighten-1";
    default:
      return "transparent";
  }
}

interface ArticleGroup {
  category: string,
  articles: ArticleConfig[],
}

interface ArticleConfig {
  title: string,
  icon: string,
  pageName: string,
  addInfo: string,
}

const meleeArticles = reactive<ArticleConfig[]>([
  { title: "パリィやガードブレイクしたい", icon: "shield-off-outline", pageName: "guardbreak", addInfo: "ガードブレイク・パリィ" },
  { title: "ローリングキャンセルについて", icon: "rotate-left", pageName: "rolling_cancell", addInfo: "実装方法と硬直の解消などの改善" },
  { title: "スタンさせたい", icon: "creation-outline", pageName: "stun", addInfo: "実装方法" },
  { title: "テイクダウンしたい", icon: "knife-military", pageName: "takedown", addInfo: "実装方法" },
  { title: "長押し処理のまとめ", icon: "controller", pageName: "button_longpress", addInfo: "チャージ攻撃・だし分け" },
  { title: "ダメージに要素を追加したい", icon: "sword-cross", pageName: "damage_config", addInfo: "要素の追加・無敵化・ヘッドショット" },
  { title: "左手でも攻撃したい", icon: "hand-back-left-outline", pageName: "lefthand_attack", addInfo: "武器装備や入力について" },
  { title: "状態異常を追加したい", icon: "bottle-tonic-skull", pageName: "abnormal", addInfo: "状態異常ロジックの提示" }
]);

const shooterArticles = reactive<ArticleConfig[]>([
  { title: "Shooter: セットアップ", icon: "pistol", pageName: "shooter_setup", addInfo: "cover addon含めた設定方法" },
  { title: "武器の設定", icon: "pistol", pageName: "gun_config", addInfo: "WeaponIDやスコープ設定などについて" }
]);

const itemArticles = reactive<ArticleConfig[]>([
  { title: "円形メニューを作りたい", icon: "dots-circle", pageName: "radial_menu", addInfo: "アイテム選択メニューの作り方" },
  { title: "スクリプトからアイテムを制御したい", icon: "flask", pageName: "item_script", addInfo: "アイテム使用スクリプト・アイテム使用制限の方法" },
  { title: "装備スロットを追加したい", icon: "shape-square-plus", pageName: "add_slot", addInfo: "スロット・ショートカット追加" }
]);

const articleGroup = reactive<ArticleGroup[]>([
  { category: "melee", articles: meleeArticles },
  { category: "shooter", articles: shooterArticles },
  { category: "item manager", articles: itemArticles },
]);
</script>