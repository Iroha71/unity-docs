<template>
  <v-container fluid>
    <VCard v-for="category in $categories()" 
      :key="category.name"
      variant="outlined"
      color="grey-darken-1"
      >
      <VCardItem>
        <div class="text-overline mb-1">{{ category.name }}</div>
      </VCardItem>
      <VContainer fluid>
        <v-row >
          <VCol md="3" sm="6" xs="12" v-for="content in category.contents" :key="content.title">
            <IndexCard 
              :key="content.title"
              :title="content.title"
              :subtitle="content.description"
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

<style lang="scss">
.v-card {
  margin-left: 1rem;
  margin-bottom: 0.5rem;
}
</style>