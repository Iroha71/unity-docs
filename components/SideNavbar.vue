<template>
  <VNavigationDrawer color="#AFD3E2" floating permanent>
    <VList nav v-for="category in $categories()">
      <VListItem 
        :title="category.title" 
        :value="category.contentName"
        :prepend-icon="category.prependIcon"
        @click="router.push({path: `/${category.contentName}`})" />
    </VList>    
  </VNavigationDrawer>
</template>

<script setup>
const router = useRouter()
</script>