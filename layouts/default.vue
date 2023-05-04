<template>
  <v-layout>
    <VAppBar color="surface-variant" title="Unity Docs" />
    <SideNavbar />
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