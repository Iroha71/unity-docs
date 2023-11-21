<template>
  <VNavigationDrawer color="#AFD3E2" floating permanent>
    <VList nav v-for="category in $categories()">
      <VListItem 
        :title="category.title" 
        :value="category.contentPath"
        :prepend-icon="category.prependIcon"
        @click="router.push({path: category.contentPath})" />
    </VList>    
  </VNavigationDrawer>
</template>

<script setup>
const router = useRouter()
</script>