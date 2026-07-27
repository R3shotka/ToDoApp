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
    <p v-if="loading" class="task-list__state">Loading...</p>
    <p v-else-if="tasks.length === 0" class="task-list__state">No tasks found.</p>
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
.task-list__state {
  text-align: center;
  color: #888;
  padding: 2rem 0;
}

.task-list__items {
  list-style: none;
  padding: 0;
  margin: 0;
  display: flex;
  flex-direction: column;
  gap: 0.625rem;
}
</style>
