<script setup lang="ts">
import type { TaskDto } from '@/types'

defineProps<{
  task: TaskDto
}>()

const emit = defineEmits<{
  toggle: [id: number]
  edit: [task: TaskDto]
  delete: [id: number]
}>()
</script>

<template>
  <div class="task-item" :class="{ 'task-item--completed': task.isCompleted }">
    <div class="task-item__left">
      <input
        type="checkbox"
        :checked="task.isCompleted"
        @change="emit('toggle', task.id)"
        class="task-item__checkbox"
      />
      <div class="task-item__content">
        <span class="task-item__title">{{ task.title }}</span>
        <span v-if="task.description" class="task-item__description">{{ task.description }}</span>
        <div class="task-item__meta">
          <span v-if="task.dueDate" class="task-item__meta-item">
            Due: {{ new Date(task.dueDate).toLocaleDateString() }}
          </span>
          <span v-if="task.categoryName" class="task-item__meta-item">
            {{ task.categoryName }}
          </span>
        </div>
      </div>
    </div>
    <div class="task-item__actions">
      <button @click="emit('edit', task)" class="task-item__btn task-item__btn--edit">Edit</button>
      <button @click="emit('delete', task.id)" class="task-item__btn task-item__btn--delete">
        Delete
      </button>
    </div>
  </div>
</template>

<style scoped>
.task-item {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 0.75rem;
  padding: 0.875rem 1rem;
  border: 1px solid #e0e0e0;
  border-radius: 6px;
  background: #fff;
}

.task-item--completed .task-item__title {
  text-decoration: line-through;
  color: #999;
}

.task-item__left {
  display: flex;
  align-items: flex-start;
  gap: 0.75rem;
  flex: 1;
  min-width: 0;
}

.task-item__checkbox {
  margin-top: 3px;
  flex-shrink: 0;
  width: 1rem;
  height: 1rem;
  cursor: pointer;
}

.task-item__content {
  display: flex;
  flex-direction: column;
  gap: 0.2rem;
  min-width: 0;
}

.task-item__title {
  font-size: 1rem;
  font-weight: 500;
  word-break: break-word;
}

.task-item__description {
  font-size: 0.875rem;
  color: #555;
  word-break: break-word;
}

.task-item__meta {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
  margin-top: 0.15rem;
}

.task-item__meta-item {
  font-size: 0.75rem;
  color: #888;
  background: #f3f3f3;
  padding: 0.1rem 0.4rem;
  border-radius: 3px;
}

.task-item__actions {
  display: flex;
  gap: 0.4rem;
  flex-shrink: 0;
}

.task-item__btn {
  padding: 0.3rem 0.65rem;
  border: none;
  border-radius: 4px;
  font-size: 0.8rem;
  cursor: pointer;
}

.task-item__btn--edit {
  background: #e8f0fe;
  color: #1a56db;
}

.task-item__btn--delete {
  background: #fde8e8;
  color: #c81e1e;
}
</style>
