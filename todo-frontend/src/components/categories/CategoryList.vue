<script setup lang="ts">
import type { CategoryDto } from '@/types'
import CategoryItem from './CategoryItem.vue'

defineProps<{
  categories: CategoryDto[]
  loading: boolean
}>()

const emit = defineEmits<{
  edit: [category: CategoryDto]
  delete: [id: number]
}>()
</script>

<template>
  <div class="category-list">
    <p v-if="loading" class="state-text">Loading...</p>
    <p v-else-if="categories.length === 0" class="state-text">
      No categories yet. Create your first one.
    </p>
    <ul v-else>
      <CategoryItem
        v-for="cat in categories"
        :key="cat.id"
        :category="cat"
        @edit="emit('edit', $event)"
        @delete="emit('delete', $event)"
      />
    </ul>
  </div>
</template>

<style scoped>
ul {
  list-style: none;
  padding: 0;
  margin: 0;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.state-text {
  color: var(--color-text-muted, #718096);
  text-align: center;
  padding: 2rem 0;
}
</style>
