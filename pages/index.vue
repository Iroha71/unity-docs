<template>
  <v-container>
    <v-row no-gutters>
      <IndexCard
        v-for="category in $categories()"
        :key="category.title"
        :title="category.title"
        :subtitle="category.descShort"
        :text="category.text"
        :content-name="category.contentName"
        :color-variant="category.colorVariant"
      />
    </v-row>
  </v-container>
</template>

<style lang="scss">
.v-card {
  margin-left: 1rem;
  margin-bottom: 0.5rem;
}
</style>