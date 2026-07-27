import client from './client'
import type { TaskDto, CreateTaskDto, UpdateTaskDto, PagedResult } from '@/types'

export interface GetTasksParams {
  page: number
  pageSize: number
  search?: string
  categoryId?: number
}

export async function getAll(params: GetTasksParams): Promise<PagedResult<TaskDto>> {
  const query: Record<string, string | number> = {
    page: params.page,
    pageSize: params.pageSize,
  }
  if (params.search) query.search = params.search
  if (params.categoryId !== undefined) query.categoryId = params.categoryId

  const response = await client.get<PagedResult<TaskDto>>('/tasks', { params: query })
  return response.data
}

export async function getById(id: number): Promise<TaskDto> {
  const response = await client.get<TaskDto>(`/tasks/${id}`)
  return response.data
}

export async function create(dto: CreateTaskDto): Promise<TaskDto> {
  const response = await client.post<TaskDto>('/tasks', dto)
  return response.data
}

export async function update(id: number, dto: UpdateTaskDto): Promise<TaskDto> {
  const response = await client.put<TaskDto>(`/tasks/${id}`, dto)
  return response.data
}

export async function remove(id: number): Promise<void> {
  await client.delete(`/tasks/${id}`)
}
