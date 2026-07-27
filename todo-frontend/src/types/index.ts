export interface RegisterUserDto {
  username: string
  email: string
  password: string
}

export interface LoginUserDto {
  username: string
  password: string
}

export interface UserDto {
  id: number
  username: string
  email: string
  createdAt: string
}

export interface LoginResponseDto {
  id: number
  username: string
  email: string
  token: string
}

export interface TaskDto {
  id: number
  title: string
  description: string | null
  isCompleted: boolean
  dueDate: string | null
  categoryId: number | null
  categoryName: string | null
  userId: number
  createdAt: string
  updatedAt: string
}

export interface CreateTaskDto {
  title: string
  description?: string
  dueDate?: string
  categoryId?: number
}

export interface UpdateTaskDto {
  title?: string
  description?: string
  isCompleted?: boolean
  dueDate?: string
  categoryId?: number
}

export interface CategoryDto {
  id: number
  name: string
  userId: number
  createdAt: string
}

export interface CreateCategoryDto {
  name: string
}

export interface UpdateCategoryDto {
  name: string
}

export interface PagedResult<T> {
  items: T[]
  totalCount: number
  page: number
  pageSize: number
  totalPages: number
}
