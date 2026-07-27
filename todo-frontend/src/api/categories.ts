import client from './client'
import type { CategoryDto, CreateCategoryDto, UpdateCategoryDto } from '@/types'

export async function getAll(): Promise<CategoryDto[]> {
  const res = await client.get<CategoryDto[]>('/categories')
  return res.data
}

export async function getById(id: number): Promise<CategoryDto> {
  const res = await client.get<CategoryDto>(`/categories/${id}`)
  return res.data
}

export async function create(dto: CreateCategoryDto): Promise<CategoryDto> {
  const res = await client.post<CategoryDto>('/categories', dto)
  return res.data
}

export async function update(id: number, dto: UpdateCategoryDto): Promise<CategoryDto> {
  const res = await client.put<CategoryDto>(`/categories/${id}`, dto)
  return res.data
}

export async function remove(id: number): Promise<void> {
  await client.delete(`/categories/${id}`)
}
