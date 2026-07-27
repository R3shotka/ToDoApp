import { describe, it, expect } from 'vitest'
import { extractError } from '@/utils/extractError'

describe('extractError', () => {
  it('returns string response.data directly', () => {
    const error = { response: { data: 'Title is required' } }
    expect(extractError(error, 'fallback')).toBe('Title is required')
  })

  it('trims whitespace from string response.data', () => {
    const error = { response: { data: '  Bad request  ' } }
    expect(extractError(error, 'fallback')).toBe('Bad request')
  })

  it('returns message from {message} object in response.data', () => {
    const error = { response: { data: { message: 'Invalid credentials' } } }
    expect(extractError(error, 'fallback')).toBe('Invalid credentials')
  })

  it('returns fallback when response.data is empty string', () => {
    const error = { response: { data: '' } }
    expect(extractError(error, 'fallback')).toBe('fallback')
  })

  it('returns fallback when error has no response', () => {
    const error = new Error('Network error')
    expect(extractError(error, 'fallback')).toBe('fallback')
  })

  it('returns fallback for null', () => {
    expect(extractError(null, 'fallback')).toBe('fallback')
  })

  it('returns fallback for unknown object shape', () => {
    const error = { response: { data: { code: 42 } } }
    expect(extractError(error, 'fallback')).toBe('fallback')
  })
})
