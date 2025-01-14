<template>
  <VContainer fluid>
    <VRow 
      dense
      v-for="artGroup in articleGroup"
      :key="artGroup.category"
    >
      <VCol
        cols="12"
        lg="3"
        md="4"
        sm="6"
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
import { reactive } from 'vue';
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
    case "magic":
      return "purple-lighten-1";
    case "throwable":
      return "brown-lighten-1";
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

const utilArticles = reactive<ArticleConfig[]>([
  { title: "UIカーソルをパッドに対応させたい", icon: "cursor-default", pageName: "ui_cursor", addInfo: "カーソル画像の設定" },
  { title: "Shooter: セットアップ", icon: "pistol", pageName: "shooter_setup", addInfo: "cover addon含めた設定方法" },
]);

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
  { title: "武器の設定", icon: "pistol", pageName: "gun_config", addInfo: "WeaponIDやスコープ設定などについて" },
  { title: "弓を自作したい", icon: "bow-arrow", pageName: "bow", addInfo: "弓のセットアップ" },
]);

const itemArticles = reactive<ArticleConfig[]>([
  { title: "円形メニューを作りたい", icon: "dots-circle", pageName: "radial_menu", addInfo: "アイテム選択メニューの作り方" },
  { title: "スクリプトからアイテムを制御したい", icon: "flask", pageName: "item_script", addInfo: "アイテム使用スクリプト・アイテム使用制限の方法" },
  { title: "装備スロットを追加したい", icon: "shape-square-plus", pageName: "add_slot", addInfo: "スロット・ショートカット追加" }
]);

const aiArticles = reactive<ArticleConfig[]>([
  { title: "AIにスタミナなど追加したい", icon: "run", pageName: "ai_props", addInfo: "ステータスの追加方法" },
  { title: "参考になるFSMとステート追加方法", icon: "state-machine", pageName: "add_state", addInfo: "ステート追加やFSM一覧" },
  { title: "音がした方向に移動させたい", icon: "ear-hearing", pageName: "noise", addInfo: "ノイズ機能の実装" },
  { title: "予備動作や弾きの実装", icon: "fencing", pageName: "ai_melee", addInfo: "予備動作・弾きスクリプト" },
]);

const throwableArticles = reactive<ArticleConfig[]>([
  { title: "吹き飛ばしを実装したい", icon: "bomb", pageName: "explode", addInfo: "AddForceExplosion使い方" },
  { title: "投擲物の軌道計算", icon: "rotate-orbit", pageName: "orbit", addInfo: "軌道計算サンプル" },
  { title: "投擲物の作成", icon: "cog", pageName: "create_grenade", addInfo: "作成方法の記載" },
  { title: "エイムと投擲を同じボタンにしたい", icon: "hand-coin", pageName: "same_throw_input", addInfo: "射撃ボタンとの統合" },
]);

const articleGroup = reactive<ArticleGroup[]>([
  { category: "melee", articles: meleeArticles },
  { category: "shooter", articles: shooterArticles },
  { category: "item manager", articles: itemArticles },
  { category: "ai", articles: aiArticles },
  { category: "throwable", articles: throwableArticles },
  { category: "util", articles: utilArticles },
]);
</script>