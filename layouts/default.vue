<template>
  <v-layout>
    <VAppBar color="surface-variant" title="Unity Docs" />
    <VNavigationDrawer color="grey-darken-2" floating permanent >
      <VList nav>
        <VListItem title="inbox" value="inbox" prepend-icon="mdi-email" @click="$router.push({path: '/'})"></VListItem>
      </VList>
    </VNavigationDrawer>
    <VMain>
      <div class="container">
        <NuxtPage />
      </div>
    </VMain>
  </v-layout>
</template>

<script setup>
const router = useRouter()
</script>

<style>
.container {
  padding: 1rem 1rem;
}
</style>