<template>
  <v-layout>
    <VAppBar color="#19A7CE" >
      <VAppBarNavIcon icon="mdi-home-circle" @click="pushHome" />
      <VAppBarTitle>Unity Docs</VAppBarTitle>
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
const router = useRouter()

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