<script setup lang="ts">
import AppHeader from './AppHeader.vue'
</script>

<template>
  <div class="layout">
    <AppHeader />
    <main class="layout__main">
      <slot />
    </main>
  </div>
</template>

<style scoped>
.layout {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

.layout__main {
  flex: 1;
  padding: 1.5rem 1rem 3rem;
  max-width: 960px;
  width: 100%;
  margin: 0 auto;
}

@media (min-width: 768px) {
  .layout__main {
    padding: 2.5rem 1.5rem 4rem;
  }
}
</style>
