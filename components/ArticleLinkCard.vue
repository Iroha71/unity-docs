<template>
  <VCard
    hover
    density="compact"
    @click="router.push({path: `/${props.pageName}`})"
  >
    <VImg height="150" :src="`/img/thumbnails/${props.thumbnail}.jpg`" cover>
      
    </VImg>
    <VCardTitle>{{ props.title }}</VCardTitle>
    <VCardText>{{ props.description }}</VCardText>
    <VCardActions>
      <VAvatar :color="badge.color" size="small">
        <VIcon :icon="`mdi-${badge.iconName}`" />
      </VAvatar>
      <VChip v-for="tag in props.tags" label>{{ tag }}</VChip>
    </VCardActions>
  </VCard>
</template>

<script lang="ts" setup>
import { VAvatar, VCardText, VIcon, VImg } from 'vuetify/components';

const categoryConfigs: Category[] = [
  { name: 'util', color: 'brown-lighten-5', iconName: 'cursor-default' },
  { name: 'shooter', color: 'indigo-lighten-1', iconName: 'pistol' },
  { name: 'melee', color: 'red-lighten-1', iconName: 'sword-cross' },
  { name: 'item', color: 'green-lighten-1', iconName: 'flask' },
  { name: 'ai', color: 'orange-lighten-1', iconName: 'state-machine' },
]

interface Props {
  title: string,
  category: string,
  pageName: string,
  description: string,
  thumbnail: string,
  tags?: string[],
}

interface Category {
  name: string,
  color: string,
  iconName: string,
}

const getBadge = (category: string): Category => {
  return categoryConfigs.find(c => c.name === category) ?? categoryConfigs[0];
}

const props = defineProps<Props>()
const router = useRouter()
const badge = getBadge(props.category)
</script>