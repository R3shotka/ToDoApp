import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { login as apiLogin, register as apiRegister } from '@/api/users'
import type { LoginUserDto, RegisterUserDto, UserDto } from '@/types'

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(localStorage.getItem('token'))
  const user = ref<UserDto | null>(JSON.parse(localStorage.getItem('user') ?? 'null'))

  const isAuthenticated = computed(() => !!token.value)

  async function login(dto: LoginUserDto) {
    const data = await apiLogin(dto)
    token.value = data.token
    user.value = { id: data.id, username: data.username, email: data.email, createdAt: '' }
    localStorage.setItem('token', data.token)
    localStorage.setItem('user', JSON.stringify(user.value))
  }

  async function register(dto: RegisterUserDto) {
    await apiRegister(dto)
    await login({ username: dto.username, password: dto.password })
  }

  function logout() {
    token.value = null
    user.value = null
    localStorage.removeItem('token')
    localStorage.removeItem('user')
  }

  return { token, user, isAuthenticated, login, register, logout }
})
