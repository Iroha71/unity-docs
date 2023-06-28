<template>
  <div class="d-flex justify-left">
    <IndexCard
      v-for="category in $categories()"
      :key="category.title"
      :title="category.title"
      :subtitle="category.descShort"
      :text="category.text"
      :path="category.contentPath" />
  </div>
</template>

<style lang="scss">
.v-card {
  margin-left: 1rem;
  margin-bottom: 0.5rem;
}
</style>