import { describe, it, expect, beforeEach, vi } from 'vitest'
import { useCategories } from '@/composables/useCategories'

vi.mock('@/api/categories', () => ({
  getAll: vi.fn(),
  create: vi.fn(),
  update: vi.fn(),
  remove: vi.fn(),
}))

import * as categoriesApi from '@/api/categories'

const cat1 = { id: 1, name: 'Work', userId: 10, createdAt: '2024-01-01' }
const cat2 = { id: 2, name: 'Personal', userId: 10, createdAt: '2024-01-02' }

beforeEach(() => {
  vi.clearAllMocks()
})

describe('useCategories — fetchAll', () => {
  it('populates categories from API', async () => {
    vi.mocked(categoriesApi.getAll).mockResolvedValue([cat1, cat2])
    const { categories, fetchAll } = useCategories()
    await fetchAll()
    expect(categories.value).toEqual([cat1, cat2])
  })

  it('sets loading to true during fetch, false after', async () => {
    let resolveCall!: (v: typeof cat1[]) => void
    vi.mocked(categoriesApi.getAll).mockReturnValue(new Promise((r) => (resolveCall = r)))
    const { loading, fetchAll } = useCategories()
    const promise = fetchAll()
    expect(loading.value).toBe(true)
    resolveCall([cat1])
    await promise
    expect(loading.value).toBe(false)
  })

  it('sets error and keeps categories empty on failure', async () => {
    vi.mocked(categoriesApi.getAll).mockRejectedValue(new Error('Network'))
    const { categories, error, fetchAll } = useCategories()
    await fetchAll()
    expect(categories.value).toEqual([])
    expect(error.value).toBe('Failed to load categories.')
  })
})

describe('useCategories — addCategory', () => {
  it('appends new category to the list and returns it', async () => {
    const newCat = { id: 3, name: 'Health', userId: 10, createdAt: '2024-01-03' }
    vi.mocked(categoriesApi.create).mockResolvedValue(newCat)
    const { categories, addCategory } = useCategories()
    categories.value = [cat1, cat2]
    const result = await addCategory({ name: 'Health' })
    expect(result).toEqual(newCat)
    expect(categories.value).toHaveLength(3)
    expect(categories.value[2]).toEqual(newCat)
  })

  it('returns null and sets error on failure', async () => {
    vi.mocked(categoriesApi.create).mockRejectedValue({ response: { data: 'Name already taken' } })
    const { error, addCategory } = useCategories()
    const result = await addCategory({ name: 'Work' })
    expect(result).toBeNull()
    expect(error.value).toBe('Name already taken')
  })
})

describe('useCategories — editCategory', () => {
  it('updates the category in-place by id', async () => {
    const updated = { ...cat1, name: 'Work Updated' }
    vi.mocked(categoriesApi.update).mockResolvedValue(updated)
    const { categories, editCategory } = useCategories()
    categories.value = [cat1, cat2]
    const result = await editCategory(1, { name: 'Work Updated' })
    expect(result).toEqual(updated)
    expect(categories.value[0].name).toBe('Work Updated')
    expect(categories.value[1]).toEqual(cat2)
  })

  it('returns null and sets error on failure', async () => {
    vi.mocked(categoriesApi.update).mockRejectedValue(new Error())
    const { error, editCategory } = useCategories()
    const result = await editCategory(1, { name: 'X' })
    expect(result).toBeNull()
    expect(error.value).toBe('Failed to update category.')
  })
})

describe('useCategories — deleteCategory', () => {
  it('removes the category from the local list', async () => {
    vi.mocked(categoriesApi.remove).mockResolvedValue(undefined)
    const { categories, deleteCategory } = useCategories()
    categories.value = [cat1, cat2]
    const ok = await deleteCategory(1)
    expect(ok).toBe(true)
    expect(categories.value).toEqual([cat2])
  })

  it('returns false and sets error on failure', async () => {
    vi.mocked(categoriesApi.remove).mockRejectedValue(new Error())
    const { categories, error, deleteCategory } = useCategories()
    categories.value = [cat1, cat2]
    const ok = await deleteCategory(1)
    expect(ok).toBe(false)
    expect(error.value).toBe('Failed to delete category.')
    expect(categories.value).toHaveLength(2)
  })
})
