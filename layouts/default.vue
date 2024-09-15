<template>
  <v-layout>
    <VAppBar color="#19A7CE" >
      <VAppBarNavIcon icon="mdi-home-circle" @click="pushHome" />
      <VAppBarTitle>Unity Docs</VAppBarTitle>
      <VSpacer/>
      <p>{{ currentVersion }}</p>
    </VAppBar>
    <SideNavbar />
    <VMain>
      <div class="container">
        <NuxtPage />
      </div>
    </VMain>
  </v-layout>
</template>

<script setup>
const config = useRuntimeConfig()

const currentVersion = config.public.gitTag

const pushHome = () => {
  return navigateTo({
    path: '/',
  })
}
</script>

<style>
.container {
  padding: 1rem 1rem;
}
p {
  margin-right: 1rem;
}
</style>