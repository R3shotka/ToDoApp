<script setup lang="ts">
import { ref, onMounted } from 'vue'
import type { TaskDto } from '@/types'
import { useTasks } from '@/composables/useTasks'
import { useCategories } from '@/composables/useCategories'
import TaskFilter from '@/components/tasks/TaskFilter.vue'
import TaskList from '@/components/tasks/TaskList.vue'
import TaskForm from '@/components/tasks/TaskForm.vue'
import BasePagination from '@/components/shared/BasePagination.vue'

const { tasks, loading, error, page, totalPages, search, categoryId, fetchTasks, setSearch, setCategoryId, setPage, addTask, editTask, toggleTask, deleteTask } = useTasks()
const { categories, fetchAll: fetchCategories } = useCategories()

const showForm = ref(false)
const editingTask = ref<TaskDto | null>(null)

onMounted(async () => {
  await Promise.all([fetchTasks(), fetchCategories()])
})

function openCreateForm() {
  editingTask.value = null
  showForm.value = true
}

function openEditForm(task: TaskDto) {
  editingTask.value = task
  showForm.value = true
}

function closeForm() {
  showForm.value = false
  editingTask.value = null
}

async function onSaved(task: TaskDto) {
  if (editingTask.value) {
    const idx = tasks.value.findIndex((t) => t.id === task.id)
    if (idx !== -1) tasks.value[idx] = task
  } else {
    await fetchTasks()
  }
  closeForm()
}

async function onDelete(id: number) {
  if (!window.confirm('Delete this task?')) return
  const ok = await deleteTask(id)
  if (!ok && error.value) {
    // error already set by deleteTask, displayed in template
  }
}
</script>

<template>
  <div class="tasks-view">
    <div class="tasks-view__header">
      <h1 class="tasks-view__title">Tasks</h1>
      <button @click="openCreateForm" class="btn btn--primary">New Task</button>
    </div>

    <TaskFilter
      :search="search"
      :categoryId="categoryId"
      :categories="categories"
      @update:search="setSearch"
      @update:categoryId="setCategoryId"
    />

    <p v-if="error" class="tasks-view__error">{{ error }}</p>

    <TaskList
      :tasks="tasks"
      :loading="loading"
      @toggle="toggleTask"
      @edit="openEditForm"
      @delete="onDelete"
    />

    <BasePagination :page="page" :totalPages="totalPages" @change="setPage" />

    <TaskForm
      v-if="showForm"
      :task="editingTask ?? undefined"
      :categories="categories"
      @saved="onSaved"
      @cancel="closeForm"
    />
  </div>
</template>

<style scoped>
.tasks-view {
  max-width: 720px;
  margin: 0 auto;
  padding: 1rem;
}

.tasks-view__header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 1rem;
}

.tasks-view__title {
  margin: 0;
  font-size: 1.5rem;
}

.tasks-view__error {
  margin: 0 0 1rem;
  padding: 0.6rem 0.75rem;
  background: #fde8e8;
  color: #c81e1e;
  border-radius: 4px;
  font-size: 0.875rem;
}

.btn {
  padding: 0.5rem 1.25rem;
  border: none;
  border-radius: 4px;
  font-size: 1rem;
  cursor: pointer;
}

.btn--primary {
  background: #1a56db;
  color: #fff;
}
</style>
