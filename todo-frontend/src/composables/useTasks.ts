import { ref } from 'vue'
import type { TaskDto, CreateTaskDto, UpdateTaskDto } from '@/types'
import * as tasksApi from '@/api/tasks'

const PAGE_SIZE = 10

export function useTasks() {
  const tasks = ref<TaskDto[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)
  const page = ref(1)
  const totalPages = ref(1)
  const search = ref('')
  const categoryId = ref<number | undefined>(undefined)

  async function fetchTasks() {
    loading.value = true
    error.value = null
    try {
      const result = await tasksApi.getAll({
        page: page.value,
        pageSize: PAGE_SIZE,
        search: search.value || undefined,
        categoryId: categoryId.value,
      })
      tasks.value = result.items
      totalPages.value = result.totalPages
    } catch {
      error.value = 'Failed to load tasks.'
    } finally {
      loading.value = false
    }
  }

  function setSearch(value: string) {
    search.value = value
    page.value = 1
    fetchTasks()
  }

  function setCategoryId(value: number | undefined) {
    categoryId.value = value
    page.value = 1
    fetchTasks()
  }

  function setPage(value: number) {
    page.value = value
    fetchTasks()
  }

  async function addTask(dto: CreateTaskDto): Promise<TaskDto | null> {
    error.value = null
    try {
      const created = await tasksApi.create(dto)
      await fetchTasks()
      return created
    } catch (e: unknown) {
      error.value = extractError(e, 'Failed to create task.')
      return null
    }
  }

  async function editTask(id: number, dto: UpdateTaskDto): Promise<TaskDto | null> {
    error.value = null
    try {
      const updated = await tasksApi.update(id, dto)
      const idx = tasks.value.findIndex((t) => t.id === id)
      if (idx !== -1) tasks.value[idx] = updated
      return updated
    } catch (e: unknown) {
      error.value = extractError(e, 'Failed to update task.')
      return null
    }
  }

  async function toggleTask(id: number): Promise<void> {
    const task = tasks.value.find((t) => t.id === id)
    if (!task) return

    const original = task.isCompleted
    task.isCompleted = !original

    try {
      const updated = await tasksApi.update(id, { isCompleted: !original })
      const idx = tasks.value.findIndex((t) => t.id === id)
      if (idx !== -1) tasks.value[idx] = updated
    } catch (e: unknown) {
      task.isCompleted = original
      error.value = extractError(e, 'Failed to update task.')
      await fetchTasks()
    }
  }

  async function deleteTask(id: number): Promise<boolean> {
    error.value = null
    try {
      await tasksApi.remove(id)
      await fetchTasks()
      return true
    } catch (e: unknown) {
      error.value = extractError(e, 'Failed to delete task.')
      await fetchTasks()
      return false
    }
  }

  return {
    tasks,
    loading,
    error,
    page,
    totalPages,
    search,
    categoryId,
    fetchTasks,
    setSearch,
    setCategoryId,
    setPage,
    addTask,
    editTask,
    toggleTask,
    deleteTask,
  }
}

function extractError(e: unknown, fallback: string): string {
  if (
    e &&
    typeof e === 'object' &&
    'response' in e &&
    e.response &&
    typeof e.response === 'object' &&
    'data' in e.response
  ) {
    const data = (e.response as { data: unknown }).data
    if (typeof data === 'string' && data.trim()) return data.trim()
    if (data && typeof data === 'object' && 'message' in data) {
      return String((data as { message: unknown }).message)
    }
  }
  return fallback
}
