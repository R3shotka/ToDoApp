import { describe, it, expect, beforeEach, vi } from 'vitest'
import { useTasks } from '@/composables/useTasks'
import type { TaskDto } from '@/types'

vi.mock('@/api/tasks', () => ({
  getAll: vi.fn(),
  create: vi.fn(),
  update: vi.fn(),
  remove: vi.fn(),
}))

import * as tasksApi from '@/api/tasks'

function makeTask(overrides: Partial<TaskDto> = {}): TaskDto {
  return {
    id: 1,
    title: 'Test task',
    description: null,
    isCompleted: false,
    dueDate: null,
    categoryId: null,
    categoryName: null,
    userId: 10,
    createdAt: '2024-01-01',
    updatedAt: '2024-01-01',
    ...overrides,
  }
}

function makePagedResult(items: TaskDto[], totalPages = 1) {
  return { items, totalCount: items.length, page: 1, pageSize: 10, totalPages }
}

beforeEach(() => {
  vi.clearAllMocks()
})

describe('useTasks — fetchTasks', () => {
  it('populates tasks and totalPages from API', async () => {
    const task = makeTask()
    vi.mocked(tasksApi.getAll).mockResolvedValue(makePagedResult([task], 3))
    const { tasks, totalPages, fetchTasks } = useTasks()
    await fetchTasks()
    expect(tasks.value).toEqual([task])
    expect(totalPages.value).toBe(3)
  })

  it('sets loading to true during fetch, false after', async () => {
    let resolveCall!: (v: ReturnType<typeof makePagedResult>) => void
    vi.mocked(tasksApi.getAll).mockReturnValue(new Promise((r) => (resolveCall = r)))
    const { loading, fetchTasks } = useTasks()
    const promise = fetchTasks()
    expect(loading.value).toBe(true)
    resolveCall(makePagedResult([]))
    await promise
    expect(loading.value).toBe(false)
  })

  it('sets error on failure', async () => {
    vi.mocked(tasksApi.getAll).mockRejectedValue(new Error('Network'))
    const { error, fetchTasks } = useTasks()
    await fetchTasks()
    expect(error.value).toBe('Failed to load tasks.')
  })
})

describe('useTasks — setSearch', () => {
  it('resets page to 1 and calls fetchTasks', async () => {
    vi.mocked(tasksApi.getAll).mockResolvedValue(makePagedResult([]))
    const { page, search, setPage, setSearch } = useTasks()
    await setPage(3)
    expect(page.value).toBe(3)
    await setSearch('hello')
    expect(search.value).toBe('hello')
    expect(page.value).toBe(1)
    expect(tasksApi.getAll).toHaveBeenCalledWith(expect.objectContaining({ search: 'hello', page: 1 }))
  })
})

describe('useTasks — setCategoryId', () => {
  it('resets page to 1 and passes categoryId to API', async () => {
    vi.mocked(tasksApi.getAll).mockResolvedValue(makePagedResult([]))
    const { page, setPage, setCategoryId } = useTasks()
    await setPage(5)
    await setCategoryId(7)
    expect(page.value).toBe(1)
    expect(tasksApi.getAll).toHaveBeenCalledWith(expect.objectContaining({ categoryId: 7, page: 1 }))
  })
})

describe('useTasks — setPage', () => {
  it('updates page and re-fetches', async () => {
    vi.mocked(tasksApi.getAll).mockResolvedValue(makePagedResult([]))
    const { page, setPage } = useTasks()
    await setPage(4)
    expect(page.value).toBe(4)
    expect(tasksApi.getAll).toHaveBeenCalledWith(expect.objectContaining({ page: 4 }))
  })
})

describe('useTasks — addTask', () => {
  it('returns the created task and re-fetches list', async () => {
    const task = makeTask({ id: 99, title: 'New' })
    vi.mocked(tasksApi.create).mockResolvedValue(task)
    vi.mocked(tasksApi.getAll).mockResolvedValue(makePagedResult([task]))
    const { addTask, tasks } = useTasks()
    const result = await addTask({ title: 'New' })
    expect(result).toEqual(task)
    expect(tasks.value).toEqual([task])
  })

  it('returns null and sets error on failure', async () => {
    vi.mocked(tasksApi.create).mockRejectedValue({ response: { data: 'Title required' } })
    const { error, addTask } = useTasks()
    const result = await addTask({ title: '' })
    expect(result).toBeNull()
    expect(error.value).toBe('Title required')
  })
})

describe('useTasks — editTask', () => {
  it('updates the task in-place without re-fetching', async () => {
    const original = makeTask({ id: 1, title: 'Old' })
    const updated = makeTask({ id: 1, title: 'Updated' })
    vi.mocked(tasksApi.update).mockResolvedValue(updated)
    const { tasks, editTask } = useTasks()
    tasks.value = [original]
    const result = await editTask(1, { title: 'Updated' })
    expect(result).toEqual(updated)
    expect(tasks.value[0].title).toBe('Updated')
    expect(tasksApi.getAll).not.toHaveBeenCalled()
  })

  it('returns null and sets error on failure', async () => {
    vi.mocked(tasksApi.update).mockRejectedValue(new Error())
    const { error, editTask } = useTasks()
    const result = await editTask(1, { title: 'X' })
    expect(result).toBeNull()
    expect(error.value).toBe('Failed to update task.')
  })
})

describe('useTasks — toggleTask', () => {
  it('optimistically flips isCompleted before API responds', async () => {
    const task = makeTask({ id: 1, isCompleted: false })
    let resolveCall!: (v: TaskDto) => void
    vi.mocked(tasksApi.update).mockReturnValue(new Promise((r) => (resolveCall = r)))
    const { tasks, toggleTask } = useTasks()
    tasks.value = [task]
    const promise = toggleTask(1)
    expect(tasks.value[0].isCompleted).toBe(true)
    resolveCall(makeTask({ id: 1, isCompleted: true }))
    await promise
  })

  it('rolls back isCompleted and preserves error when API fails', async () => {
    const task = makeTask({ id: 1, isCompleted: false })
    vi.mocked(tasksApi.update).mockRejectedValue(new Error('Server error'))
    vi.mocked(tasksApi.getAll).mockResolvedValue(makePagedResult([task]))
    const { tasks, error, toggleTask } = useTasks()
    tasks.value = [task]
    await toggleTask(1)
    expect(tasks.value[0].isCompleted).toBe(false)
    expect(error.value).toBe('Failed to update task.')
  })

  it('does nothing when task id is not in list', async () => {
    const { tasks, toggleTask } = useTasks()
    tasks.value = []
    await toggleTask(999)
    expect(tasksApi.update).not.toHaveBeenCalled()
  })
})

describe('useTasks — deleteTask', () => {
  it('returns true and re-fetches after deletion', async () => {
    vi.mocked(tasksApi.remove).mockResolvedValue(undefined)
    vi.mocked(tasksApi.getAll).mockResolvedValue(makePagedResult([]))
    const { deleteTask, tasks } = useTasks()
    tasks.value = [makeTask()]
    const ok = await deleteTask(1)
    expect(ok).toBe(true)
    expect(tasksApi.getAll).toHaveBeenCalled()
  })

  it('returns false and preserves error on failure', async () => {
    vi.mocked(tasksApi.remove).mockRejectedValue({ response: { data: { message: 'Not found' } } })
    vi.mocked(tasksApi.getAll).mockResolvedValue(makePagedResult([]))
    const { error, deleteTask } = useTasks()
    const ok = await deleteTask(1)
    expect(ok).toBe(false)
    expect(error.value).toBe('Not found')
  })
})
