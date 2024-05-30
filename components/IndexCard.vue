<template>
  <div>
    <VCard 
      color ="#1F7087"
      width="15rem"
      :href="`/unity-docs/${Props.contentName}`"
      hover
    >
      <VImg
        :src="getBackgroundImage(Props.contentName)"
        gradient="to bottom, rgba(0,0,0,.1), rgba(0,0,0,.5)"
        height="100px"
        cover 
      >
        <VCardTitle class="text-white" v-text="Props.title" />
      </VImg>
      <div class="pa-1">
        <VCardSubtitle class="text-white">{{ Props.subtitle }}</VCardSubtitle>
      </div>
      <div class="pa-1">
        <VCardText class="text-white">{{ Props.text }}</VCardText>
      </div>
    </VCard>
  </div>
</template>

<script setup lang="ts">

interface Props {
  title: string,
  contentName: string,
  subtitle: string,
  text: string,
}

const Props = withDefaults(defineProps<Props>(), {
  title: "",
  subtitle: "",
  contentName: "",
  text: "",
})

const getBackgroundImage = (contentName: string) => {
  return `/img/backgrounds/${contentName}.webp`;
}

const router = useRouter()
</script>