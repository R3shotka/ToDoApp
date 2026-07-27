<script setup lang="ts">
import type { CategoryDto } from '@/types'

defineProps<{ category: CategoryDto }>()

const emit = defineEmits<{
  edit: [category: CategoryDto]
  delete: [id: number]
}>()
</script>

<template>
  <li class="cat-item">
    <div class="cat-item__left">
      <span class="cat-item__dot"></span>
      <span class="cat-item__name">{{ category.name }}</span>
    </div>
    <div class="cat-item__actions">
      <button class="cat-item__btn" @click="emit('edit', category)" title="Edit">
        <svg width="14" height="14" viewBox="0 0 16 16" fill="none">
          <path d="M11.5 1.5a2.121 2.121 0 1 1 3 3L5 14H2v-3L11.5 1.5z" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"/>
        </svg>
        Edit
      </button>
      <button class="cat-item__btn cat-item__btn--danger" @click="emit('delete', category.id)" title="Delete">
        <svg width="14" height="14" viewBox="0 0 16 16" fill="none">
          <path d="M2 4h12M5 4V2h6v2M6 7v5M10 7v5M3 4l1 9h8l1-9" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"/>
        </svg>
        Delete
      </button>
    </div>
  </li>
</template>

<style scoped>
.cat-item {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 0.75rem;
  padding: 0.875rem 1rem;
  background: var(--c-surface);
  border-radius: var(--r-md);
  box-shadow: var(--shadow-sm);
  transition: box-shadow var(--t-base), transform var(--t-base);
}

.cat-item:hover {
  box-shadow: var(--shadow-md);
  transform: translateY(-1px);
}

.cat-item__left {
  display: flex;
  align-items: center;
  gap: 0.625rem;
  min-width: 0;
}

.cat-item__dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  background: var(--c-primary);
  flex-shrink: 0;
}

.cat-item__name {
  font-size: 0.9375rem;
  font-weight: 500;
  color: var(--c-text);
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.cat-item__actions {
  display: flex;
  gap: 0.375rem;
  flex-shrink: 0;
}

.cat-item__btn {
  display: inline-flex;
  align-items: center;
  gap: 0.3rem;
  padding: 0.3125rem 0.625rem;
  border: 1px solid var(--c-border);
  border-radius: var(--r-xs);
  background: transparent;
  font-family: var(--font);
  font-size: 0.75rem;
  font-weight: 500;
  color: var(--c-text-muted);
  cursor: pointer;
  transition: background var(--t-fast), color var(--t-fast), border-color var(--t-fast);
}

.cat-item__btn:hover {
  background: var(--c-surface-2);
  color: var(--c-text);
  border-color: var(--c-border);
}

.cat-item__btn--danger:hover {
  background: var(--c-danger-lt);
  color: var(--c-danger);
  border-color: var(--c-danger-border);
}
</style>
