<template>
  <div class="prose-code">
    <div class="prose-code__file" v-if="filename">
      {{ filename }}
    </div>
    <pre :class="$props.class">
      <slot />
    </pre>
  </div>
</template>

<script setup lang="ts">
defineProps({
  code: {
    type: String,
    default: ''
  },
  language: {
    type: String,
    default: null
  },
  filename: {
    type: String,
    default: null
  },
  highlights: {
    type: Array as () => number[],
    default: () => []
  },
  meta: {
    type: String,
    default: null
  },
  class: {
    type: String,
    default: null
  }
})
</script>

<style>
pre code .line {
  display: block;
}
</style>