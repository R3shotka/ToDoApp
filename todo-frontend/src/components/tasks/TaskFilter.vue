<script setup lang="ts">
import { ref, watch } from 'vue'
import type { CategoryDto } from '@/types'

const props = defineProps<{
  search: string
  categoryId: number | undefined
  categories: CategoryDto[]
}>()

const emit = defineEmits<{
  'update:search': [value: string]
  'update:categoryId': [value: number | undefined]
}>()

const localSearch = ref(props.search)

let debounceTimer: ReturnType<typeof setTimeout>

watch(localSearch, (value) => {
  clearTimeout(debounceTimer)
  debounceTimer = setTimeout(() => {
    emit('update:search', value)
  }, 400)
})

function onCategoryChange(e: Event) {
  const value = (e.target as HTMLSelectElement).value
  emit('update:categoryId', value ? Number(value) : undefined)
}
</script>

<template>
  <div class="task-filter">
    <div class="task-filter__search-wrap">
      <svg class="task-filter__icon" width="16" height="16" viewBox="0 0 16 16" fill="none">
        <circle cx="6.5" cy="6.5" r="4.5" stroke="currentColor" stroke-width="1.5"/>
        <path d="M10 10.5L14 14.5" stroke="currentColor" stroke-width="1.5" stroke-linecap="round"/>
      </svg>
      <input
        v-model="localSearch"
        type="text"
        placeholder="Search tasks…"
        class="task-filter__input"
      />
    </div>
    <select
      :value="categoryId ?? ''"
      @change="onCategoryChange"
      class="task-filter__select"
    >
      <option value="">All categories</option>
      <option v-for="cat in categories" :key="cat.id" :value="cat.id">
        {{ cat.name }}
      </option>
    </select>
  </div>
</template>

<style scoped>
.task-filter {
  display: flex;
  flex-direction: column;
  gap: 0.625rem;
  margin-bottom: 1.25rem;
}

.task-filter__search-wrap {
  position: relative;
  display: flex;
  align-items: center;
}

.task-filter__icon {
  position: absolute;
  left: 0.75rem;
  color: var(--c-text-muted);
  pointer-events: none;
  flex-shrink: 0;
}

.task-filter__input {
  width: 100%;
  padding: 0.625rem 0.875rem 0.625rem 2.375rem;
  background: var(--c-surface);
  border: 1.5px solid var(--c-border);
  border-radius: var(--r-sm);
  font-family: var(--font);
  font-size: 0.9375rem;
  color: var(--c-text);
  transition: border-color var(--t-fast), box-shadow var(--t-fast);
  outline: none;
}

.task-filter__input::placeholder {
  color: var(--c-text-placeholder);
}

.task-filter__input:focus {
  border-color: var(--c-primary);
  box-shadow: 0 0 0 3px var(--c-primary-lt);
}

.task-filter__select {
  padding: 0.625rem 0.875rem;
  background: var(--c-surface);
  border: 1.5px solid var(--c-border);
  border-radius: var(--r-sm);
  font-family: var(--font);
  font-size: 0.9375rem;
  color: var(--c-text);
  cursor: pointer;
  outline: none;
  transition: border-color var(--t-fast), box-shadow var(--t-fast);
  width: 100%;
}

.task-filter__select:focus {
  border-color: var(--c-primary);
  box-shadow: 0 0 0 3px var(--c-primary-lt);
}

@media (min-width: 540px) {
  .task-filter {
    flex-direction: row;
  }

  .task-filter__search-wrap {
    flex: 1;
  }

  .task-filter__select {
    width: auto;
    min-width: 170px;
  }
}
</style>
