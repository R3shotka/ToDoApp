<script setup lang="ts">
defineProps<{
  page: number
  totalPages: number
}>()

const emit = defineEmits<{
  change: [page: number]
}>()
</script>

<template>
  <div class="pagination" v-if="totalPages > 1">
    <button
      class="pagination__btn"
      :disabled="page <= 1"
      @click="emit('change', page - 1)"
      aria-label="Previous page"
    >
      <svg width="14" height="14" viewBox="0 0 16 16" fill="none">
        <path d="M10 3L5 8l5 5" stroke="currentColor" stroke-width="1.75" stroke-linecap="round" stroke-linejoin="round"/>
      </svg>
    </button>
    <span class="pagination__info">{{ page }} / {{ totalPages }}</span>
    <button
      class="pagination__btn"
      :disabled="page >= totalPages"
      @click="emit('change', page + 1)"
      aria-label="Next page"
    >
      <svg width="14" height="14" viewBox="0 0 16 16" fill="none">
        <path d="M6 3l5 5-5 5" stroke="currentColor" stroke-width="1.75" stroke-linecap="round" stroke-linejoin="round"/>
      </svg>
    </button>
  </div>
</template>

<style scoped>
.pagination {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  margin-top: 1.75rem;
}

.pagination__btn {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 36px;
  height: 36px;
  border: 1.5px solid var(--c-border);
  border-radius: var(--r-sm);
  background: var(--c-surface);
  color: var(--c-text-muted);
  cursor: pointer;
  transition: background var(--t-fast), color var(--t-fast), border-color var(--t-fast);
}

.pagination__btn:hover:not(:disabled) {
  background: var(--c-primary-lt);
  color: var(--c-primary);
  border-color: var(--c-primary);
}

.pagination__btn:disabled {
  opacity: 0.35;
  cursor: default;
}

.pagination__info {
  font-size: 0.8125rem;
  font-weight: 500;
  color: var(--c-text-muted);
  min-width: 3rem;
  text-align: center;
}
</style>
