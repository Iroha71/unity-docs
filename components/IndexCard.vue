<template>
  <VCard
    width="15rem"
    :title="Props.title"
    :subtitle="Props.subtitle"
    :text="Props.text"
    :prepend-icon="Props.prependIcon"
    color="#F6F1F1">
    <VCardActions class="d-flex justify-center">
      <VBtn color="#146C94" variant="outlined" @click="router.push({path: Props.path})">閲覧</VBtn>
    </VCardActions>
  </VCard>
</template>

<script setup lang="ts">

interface Props {
  title: string,
  subtitle: string,
  text: string,
  path: string,
  prependIcon: string,
}

const Props = withDefaults(defineProps<Props>(), {
  title: "",
  subtitle: "",
  text: "",
  path: "",
})

const router = useRouter()
</script>