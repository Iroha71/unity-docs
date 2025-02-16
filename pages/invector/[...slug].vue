<template>
  <main>
    <VRow>
      <VCol>
        <VBtn @click="() => {router.go(-1)}">記事一覧へ</VBtn>
      </VCol>
    </VRow>
    <VRow>
      <ContentDoc/>
    </VRow>
  </main>
</template>

<script setup>
const router = useRouter()
</script>