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
    <input
      v-model="localSearch"
      type="text"
      placeholder="Search tasks..."
      class="task-filter__search"
    />
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
  gap: 0.5rem;
  margin-bottom: 1rem;
}

.task-filter__search,
.task-filter__select {
  padding: 0.5rem 0.75rem;
  border: 1px solid #ccc;
  border-radius: 4px;
  font-size: 1rem;
  width: 100%;
  box-sizing: border-box;
}

@media (min-width: 768px) {
  .task-filter {
    flex-direction: row;
  }

  .task-filter__search {
    flex: 1;
  }

  .task-filter__select {
    width: auto;
    min-width: 180px;
  }
}
</style>
