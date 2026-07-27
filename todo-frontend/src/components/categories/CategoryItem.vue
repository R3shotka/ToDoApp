<script setup lang="ts">
import type { CategoryDto } from '@/types'

defineProps<{ category: CategoryDto }>()

const emit = defineEmits<{
  edit: [category: CategoryDto]
  delete: [id: number]
}>()
</script>

<template>
  <li class="category-item">
    <span class="category-name">{{ category.name }}</span>
    <div class="category-actions">
      <button class="btn btn--sm" @click="emit('edit', category)">Edit</button>
      <button class="btn btn--sm btn--danger" @click="emit('delete', category.id)">Delete</button>
    </div>
  </li>
</template>

<style scoped>
.category-item {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0.75rem 1rem;
  border: 1px solid var(--color-border, #e2e8f0);
  border-radius: 8px;
  background: var(--color-surface, #fff);
}

.category-name {
  font-size: 1rem;
  color: var(--color-text, #1a202c);
}

.category-actions {
  display: flex;
  gap: 0.5rem;
}

.btn {
  padding: 0.3rem 0.75rem;
  border: 1px solid var(--color-border, #ccc);
  border-radius: 5px;
  font-size: 0.85rem;
  cursor: pointer;
  background: transparent;
  color: var(--color-text, #333);
}

.btn--sm {
  padding: 0.25rem 0.6rem;
}

.btn--danger {
  border-color: #e53e3e;
  color: #e53e3e;
}

.btn--danger:hover {
  background: #e53e3e;
  color: #fff;
}
</style>
