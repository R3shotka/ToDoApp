import { describe, it, expect, beforeEach, vi } from 'vitest'
import { setActivePinia, createPinia } from 'pinia'
import { useAuthStore } from '@/stores/auth'

vi.mock('@/api/users', () => ({
  login: vi.fn(),
  register: vi.fn(),
}))

import * as usersApi from '@/api/users'

const mockLoginResponse = {
  id: 1,
  username: 'testuser',
  email: 'test@example.com',
  token: 'mock-jwt-token',
}

beforeEach(() => {
  localStorage.clear()
  setActivePinia(createPinia())
  vi.clearAllMocks()
})

describe('auth store — initial state', () => {
  it('starts unauthenticated when localStorage is empty', () => {
    const auth = useAuthStore()
    expect(auth.token).toBeNull()
    expect(auth.user).toBeNull()
    expect(auth.isAuthenticated).toBe(false)
  })

  it('restores token and user from localStorage', () => {
    localStorage.setItem('token', 'saved-token')
    localStorage.setItem('user', JSON.stringify({ id: 2, username: 'bob', email: 'b@b.com', createdAt: '' }))
    const auth = useAuthStore()
    expect(auth.token).toBe('saved-token')
    expect(auth.user?.username).toBe('bob')
    expect(auth.isAuthenticated).toBe(true)
  })
})

describe('auth store — login', () => {
  it('sets token and user in state after successful login', async () => {
    vi.mocked(usersApi.login).mockResolvedValue(mockLoginResponse)
    const auth = useAuthStore()
    await auth.login({ username: 'testuser', password: 'pass' })
    expect(auth.token).toBe('mock-jwt-token')
    expect(auth.user?.username).toBe('testuser')
    expect(auth.isAuthenticated).toBe(true)
  })

  it('persists token and user to localStorage', async () => {
    vi.mocked(usersApi.login).mockResolvedValue(mockLoginResponse)
    const auth = useAuthStore()
    await auth.login({ username: 'testuser', password: 'pass' })
    expect(localStorage.getItem('token')).toBe('mock-jwt-token')
    expect(JSON.parse(localStorage.getItem('user')!).username).toBe('testuser')
  })

  it('propagates error without changing state when API fails', async () => {
    vi.mocked(usersApi.login).mockRejectedValue(new Error('Unauthorized'))
    const auth = useAuthStore()
    await expect(auth.login({ username: 'bad', password: 'bad' })).rejects.toThrow()
    expect(auth.token).toBeNull()
    expect(auth.isAuthenticated).toBe(false)
  })
})

describe('auth store — logout', () => {
  it('clears state and localStorage', async () => {
    vi.mocked(usersApi.login).mockResolvedValue(mockLoginResponse)
    const auth = useAuthStore()
    await auth.login({ username: 'testuser', password: 'pass' })
    auth.logout()
    expect(auth.token).toBeNull()
    expect(auth.user).toBeNull()
    expect(auth.isAuthenticated).toBe(false)
    expect(localStorage.getItem('token')).toBeNull()
    expect(localStorage.getItem('user')).toBeNull()
  })
})

describe('auth store — register', () => {
  it('calls register API then auto-logs in', async () => {
    vi.mocked(usersApi.register).mockResolvedValue(undefined as never)
    vi.mocked(usersApi.login).mockResolvedValue(mockLoginResponse)
    const auth = useAuthStore()
    await auth.register({ username: 'testuser', email: 'test@example.com', password: 'pass' })
    expect(usersApi.register).toHaveBeenCalledOnce()
    expect(usersApi.login).toHaveBeenCalledOnce()
    expect(auth.isAuthenticated).toBe(true)
  })
})
