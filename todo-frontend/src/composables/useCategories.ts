import { ref } from 'vue'
import type { CategoryDto, CreateCategoryDto, UpdateCategoryDto } from '@/types'
import * as categoriesApi from '@/api/categories'

export function useCategories() {
  const categories = ref<CategoryDto[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)

  async function fetchAll() {
    loading.value = true
    error.value = null
    try {
      categories.value = await categoriesApi.getAll()
    } catch {
      error.value = 'Failed to load categories.'
    } finally {
      loading.value = false
    }
  }

  async function addCategory(dto: CreateCategoryDto): Promise<CategoryDto | null> {
    error.value = null
    try {
      const created = await categoriesApi.create(dto)
      categories.value.push(created)
      return created
    } catch (e: unknown) {
      error.value = extractError(e, 'Failed to create category.')
      return null
    }
  }

  async function editCategory(id: number, dto: UpdateCategoryDto): Promise<CategoryDto | null> {
    error.value = null
    try {
      const updated = await categoriesApi.update(id, dto)
      const idx = categories.value.findIndex((c) => c.id === id)
      if (idx !== -1) categories.value[idx] = updated
      return updated
    } catch (e: unknown) {
      error.value = extractError(e, 'Failed to update category.')
      return null
    }
  }

  async function deleteCategory(id: number): Promise<boolean> {
    error.value = null
    try {
      await categoriesApi.remove(id)
      categories.value = categories.value.filter((c) => c.id !== id)
      return true
    } catch (e: unknown) {
      error.value = extractError(e, 'Failed to delete category.')
      return false
    }
  }

  return { categories, loading, error, fetchAll, addCategory, editCategory, deleteCategory }
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
