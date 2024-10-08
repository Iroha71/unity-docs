<template>
  <v-container fluid>
    <VCard v-for="category in $categories()" 
      :key="category.name"
      variant="outlined"
      :color="category.color"
      class="mb-2"
      >
      <VCardItem>
        <div class="text-overline">{{ category.name }}</div>
      </VCardItem>
      <VContainer fluid>
        <v-row >
          <VCol md="3" sm="6" xs="12" v-for="content in category.contents" :key="content.title">
            <IndexCard 
              :key="content.title"
              :title="content.title"
              :subtitle="content.descShort"
              :text="content.text"
              :content-name="content.contentName"
              :color-variant="content.colorVariant"
              :icon="content.prependIcon"
            />
          </VCol>
        </v-row>
      </VContainer>
    </VCard>
  </v-container>  
</template>

<script setup lang="ts">
const { $categories } = useNuxtApp()
</script>