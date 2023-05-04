<template>
  <div>
    <ContentList path="/invector" v-slot="{ list }">
      <div v-for="article in list" :key="article._path">
        <h2>{{ article.title }}</h2>
      </div>
    </ContentList>
  </div>
</template>