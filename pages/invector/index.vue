<template>
  <div>
    <ContentList path="/invector" v-slot="{ list }">
      <IndexCard
        v-for="article in list" :key="article._path"
        :title="article.title"
        :subtitle="article.description"
        :path="article._path" />
    </ContentList>
  </div>
</template>