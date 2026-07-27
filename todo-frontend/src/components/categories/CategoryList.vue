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
  <div class="cat-list">
    <div v-if="loading" class="cat-list__skeleton">
      <div class="skeleton-item" v-for="n in 3" :key="n"></div>
    </div>
    <div v-else-if="categories.length === 0" class="cat-list__empty">
      <div class="cat-list__empty-icon">⊞</div>
      <p class="cat-list__empty-text">No categories yet</p>
      <p class="cat-list__empty-sub">Create a category to organise your tasks</p>
    </div>
    <ul v-else class="cat-list__items">
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
.cat-list__items {
  list-style: none;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.cat-list__empty {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 4rem 1rem;
  text-align: center;
}

.cat-list__empty-icon {
  font-size: 2.5rem;
  margin-bottom: 0.75rem;
  opacity: 0.2;
}

.cat-list__empty-text {
  font-size: 1rem;
  font-weight: 600;
  color: var(--c-text);
  margin-bottom: 0.25rem;
}

.cat-list__empty-sub {
  font-size: 0.875rem;
  color: var(--c-text-muted);
}

.cat-list__skeleton {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.skeleton-item {
  height: 52px;
  background: linear-gradient(90deg, var(--c-border) 25%, var(--c-surface-2) 50%, var(--c-border) 75%);
  background-size: 200% 100%;
  border-radius: var(--r-md);
  animation: shimmer 1.4s infinite;
}

@keyframes shimmer {
  0%   { background-position: 200% 0; }
  100% { background-position: -200% 0; }
}
</style>
