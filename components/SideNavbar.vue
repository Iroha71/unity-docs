<template>
  <VNavigationDrawer color="#AFD3E2" floating permanent>
    <VList nav>
      <VListItem 
        title="ホーム"
        value="ホーム"
        prepend-icon="mdi-home"
        @click="router.push({path: '/'})"/>
    </VList>
    <VList nav v-for="category in $categories()">
      <VListItem 
        v-for="content in category.contents"
        :title="content.title" 
        :value="content.contentName"
        :prepend-icon="content.prependIcon"
        @click="router.push({path: `/${content.contentName}`})" />
    </VList>    
  </VNavigationDrawer>
</template>

<script setup>
const router = useRouter()
const { $categories } = useNuxtApp()
</script>