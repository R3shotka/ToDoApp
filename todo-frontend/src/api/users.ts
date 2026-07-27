import client from './client'
import type { LoginUserDto, RegisterUserDto, UserDto, LoginResponseDto } from '@/types'

export async function login(dto: LoginUserDto): Promise<LoginResponseDto> {
  const response = await client.post<LoginResponseDto>('/users/login', dto)
  return response.data
}

export async function register(dto: RegisterUserDto): Promise<UserDto> {
  const response = await client.post<UserDto>('/users/register', dto)
  return response.data
}
