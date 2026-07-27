<script setup lang="ts">
import type { TaskDto } from '@/types'
import TaskItem from './TaskItem.vue'

defineProps<{
  tasks: TaskDto[]
  loading: boolean
}>()

const emit = defineEmits<{
  toggle: [id: number]
  edit: [task: TaskDto]
  delete: [id: number]
}>()
</script>

<template>
  <div class="task-list">
    <div v-if="loading" class="task-list__skeleton">
      <div class="skeleton-item" v-for="n in 3" :key="n"></div>
    </div>
    <div v-else-if="tasks.length === 0" class="task-list__empty">
      <div class="task-list__empty-icon">☑</div>
      <p class="task-list__empty-text">No tasks yet</p>
      <p class="task-list__empty-sub">Create your first task to get started</p>
    </div>
    <ul v-else class="task-list__items">
      <li v-for="task in tasks" :key="task.id">
        <TaskItem
          :task="task"
          @toggle="emit('toggle', $event)"
          @edit="emit('edit', $event)"
          @delete="emit('delete', $event)"
        />
      </li>
    </ul>
  </div>
</template>

<style scoped>
.task-list__items {
  list-style: none;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

/* ── Empty state ── */

.task-list__empty {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 4rem 1rem;
  text-align: center;
}

.task-list__empty-icon {
  font-size: 2.5rem;
  margin-bottom: 0.75rem;
  opacity: 0.25;
}

.task-list__empty-text {
  font-size: 1rem;
  font-weight: 600;
  color: var(--c-text);
  margin-bottom: 0.25rem;
}

.task-list__empty-sub {
  font-size: 0.875rem;
  color: var(--c-text-muted);
}

/* ── Skeleton ── */

.task-list__skeleton {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.skeleton-item {
  height: 64px;
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
