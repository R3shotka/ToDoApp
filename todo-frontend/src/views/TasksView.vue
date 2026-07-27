<script setup lang="ts">
import { ref, onMounted } from 'vue'
import type { TaskDto } from '@/types'
import { useTasks } from '@/composables/useTasks'
import { useCategories } from '@/composables/useCategories'
import AppLayout from '@/components/shared/AppLayout.vue'
import TaskFilter from '@/components/tasks/TaskFilter.vue'
import TaskList from '@/components/tasks/TaskList.vue'
import TaskForm from '@/components/tasks/TaskForm.vue'
import BasePagination from '@/components/shared/BasePagination.vue'

const { tasks, loading, error, page, totalPages, search, categoryId, fetchTasks, setSearch, setCategoryId, setPage, toggleTask, deleteTask } = useTasks()
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
  await deleteTask(id)
}
</script>

<template>
  <AppLayout>
    <div class="tasks-view">
      <div class="tasks-view__header">
        <div>
          <h1 class="tasks-view__title">My Tasks</h1>
          <p class="tasks-view__subtitle" v-if="!loading">
            {{ tasks.length }} task{{ tasks.length !== 1 ? 's' : '' }}
          </p>
        </div>
        <button @click="openCreateForm" class="btn btn--primary">
          <span class="btn-icon">+</span> New Task
        </button>
      </div>

      <TaskFilter
        :search="search"
        :categoryId="categoryId"
        :categories="categories"
        @update:search="setSearch"
        @update:categoryId="setCategoryId"
      />

      <div v-if="error" class="error-block">{{ error }}</div>

      <TaskList
        :tasks="tasks"
        :loading="loading"
        @toggle="toggleTask"
        @edit="openEditForm"
        @delete="onDelete"
      />

      <BasePagination :page="page" :totalPages="totalPages" @change="setPage" />
    </div>

    <TaskForm
      v-if="showForm"
      :task="editingTask ?? undefined"
      :categories="categories"
      @saved="onSaved"
      @cancel="closeForm"
    />
  </AppLayout>
</template>

<style scoped>
.tasks-view {
  max-width: 720px;
  margin: 0 auto;
}

.tasks-view__header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 1rem;
  margin-bottom: 1.5rem;
}

.tasks-view__title {
  font-size: 1.5rem;
  font-weight: 700;
  color: var(--c-text);
  letter-spacing: -0.025em;
  line-height: 1.2;
}

.tasks-view__subtitle {
  font-size: 0.8125rem;
  color: var(--c-text-muted);
  margin-top: 0.125rem;
}

.btn-icon {
  font-size: 1.125rem;
  line-height: 1;
  margin-right: -0.125rem;
}

.error-block {
  margin-bottom: 1rem;
}
</style>
