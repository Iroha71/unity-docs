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