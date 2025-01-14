<template>
  <v-layout>
    <VAppBar color="#19A7CE" >
      <VAppBarTitle @click="pushHome"
        class="cursor-pointer"
      >
        Unity Docs
      </VAppBarTitle>
      <p>{{ currentVersion }}</p>
    </VAppBar>
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