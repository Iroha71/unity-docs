<template>
  <div>
    <VCard
      :color="Props.colorVariant"
      width="15rem"
      :href="`/unity-docs/${Props.contentName}`"
    >
      <VCardTitle class="text-white" style="height: 50px;" v-text="Props.title" />
      <VCardSubtitle class="text-white" style="margin-bottom: 4px;">{{ Props.subtitle }}</VCardSubtitle>
      <VCardText class="bg-grey-lighten-4">{{ Props.text }}</VCardText>
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
}

const Props = withDefaults(defineProps<Props>(), {
  title: "",
  subtitle: "",
  contentName: "",
  text: "",
})
</script>