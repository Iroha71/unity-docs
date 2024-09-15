<template>
  <div>
    <VCard
      :prepend-icon="Props.icon"
      :color="Props.colorVariant"
      min-width="15rem"
      :href="`/unity-docs/${Props.contentName}`"
    >
      <template v-slot:title>{{ Props.title }}</template>
      <template v-slot:subtitle>{{ Props.subtitle }}</template>
      <VCardText class="bg-grey-lighten-4 pt-4">{{ Props.text }}</VCardText>
    </VCard>
  </div>
</template>

<script setup lang="ts">

interface Props {
  title: string,
  contentName: string,
  subtitle: string,
  text: string,
  colorVariant: string,
  icon: string,
}

const Props = defineProps<Props>()
</script>