<template>
  <VCard
    :title="props.title"
    :subtitle="props.category"
    hover
    @click="router.push({path: `/${props.pageName}`})"
  >
    <template v-slot:prepend>
      <VAvatar :color="getVariant()">
        <VIcon :icon="`mdi-${icon}`" />
      </VAvatar>
    </template>
    <VCardText class="bg-surface-light pt-4">
      {{ props.description }}
    </VCardText>
  </VCard>
</template>

<script lang="ts" setup>
import { VAvatar, VCardText, VIcon, VImg } from 'vuetify/components';


interface Props {
  title: string,
  icon: string,
  category: string,
  pageName: string,
  description: string,
  color?: string,
}

const getVariant = () => {
  return props.color == null ? "transparent" : props.color
}

const props = defineProps<Props>()
const router = useRouter()
</script>