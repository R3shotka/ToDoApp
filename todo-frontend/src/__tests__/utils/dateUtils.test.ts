import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest'
import { isOverdue } from '@/utils/dateUtils'

// Fix "today" to 2024-06-15 for all tests
const TODAY = new Date('2024-06-15T12:00:00.000Z')

beforeEach(() => {
  vi.useFakeTimers()
  vi.setSystemTime(TODAY)
})

afterEach(() => {
  vi.useRealTimers()
})

describe('isOverdue', () => {
  it('returns false when dueDate is null', () => {
    expect(isOverdue(null, false)).toBe(false)
  })

  it('returns false when task is completed, even if date is past', () => {
    expect(isOverdue('2024-06-10', true)).toBe(false)
  })

  it('returns true for a past date', () => {
    expect(isOverdue('2024-06-14', false)).toBe(true)
  })

  it('returns false for today (not yet overdue)', () => {
    expect(isOverdue('2024-06-15', false)).toBe(false)
  })

  it('returns false for a future date', () => {
    expect(isOverdue('2024-06-20', false)).toBe(false)
  })
})
