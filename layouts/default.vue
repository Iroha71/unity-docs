<template>
  <v-layout>
    <VAppBar color="#19A7CE" title="Unity 古エッダ" >
      <VSpacer />
      <p>ver.1.1.0</p>
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
</script>

<style>
.container {
  padding: 1rem 1rem;
}
p {
  margin-right: 1rem;
}
</style>