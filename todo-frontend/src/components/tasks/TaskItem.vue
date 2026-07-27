<script setup lang="ts">
import type { TaskDto } from '@/types'

defineProps<{ task: TaskDto }>()

const emit = defineEmits<{
  toggle: [id: number]
  edit: [task: TaskDto]
  delete: [id: number]
}>()
</script>

<template>
  <div class="task" :class="{ 'task--done': task.isCompleted }">
    <label class="task__check-wrap" :title="task.isCompleted ? 'Mark incomplete' : 'Mark complete'">
      <input
        type="checkbox"
        class="task__checkbox-native"
        :checked="task.isCompleted"
        @change="emit('toggle', task.id)"
      />
      <span class="task__checkbox"></span>
    </label>

    <div class="task__body">
      <span class="task__title">{{ task.title }}</span>
      <span v-if="task.description" class="task__description">{{ task.description }}</span>
      <div v-if="task.dueDate || task.categoryName" class="task__meta">
        <span v-if="task.categoryName" class="task__tag">{{ task.categoryName }}</span>
        <span v-if="task.dueDate" class="task__date">
          {{ new Date(task.dueDate).toLocaleDateString('en', { month: 'short', day: 'numeric' }) }}
        </span>
      </div>
    </div>

    <div class="task__actions">
      <button class="task__action-btn" @click="emit('edit', task)" title="Edit">
        <svg width="14" height="14" viewBox="0 0 16 16" fill="none">
          <path d="M11.5 1.5a2.121 2.121 0 1 1 3 3L5 14H2v-3L11.5 1.5z" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"/>
        </svg>
      </button>
      <button class="task__action-btn task__action-btn--danger" @click="emit('delete', task.id)" title="Delete">
        <svg width="14" height="14" viewBox="0 0 16 16" fill="none">
          <path d="M2 4h12M5 4V2h6v2M6 7v5M10 7v5M3 4l1 9h8l1-9" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"/>
        </svg>
      </button>
    </div>
  </div>
</template>

<style scoped>
.task {
  display: flex;
  align-items: flex-start;
  gap: 0.75rem;
  padding: 0.875rem 1rem;
  background: var(--c-surface);
  border-radius: var(--r-md);
  box-shadow: var(--shadow-sm);
  transition: box-shadow var(--t-base), transform var(--t-base), opacity var(--t-base);
  position: relative;
}

.task:hover {
  box-shadow: var(--shadow-md);
  transform: translateY(-1px);
}

.task--done {
  opacity: 0.5;
}

/* ── Checkbox ── */

.task__check-wrap {
  flex-shrink: 0;
  margin-top: 1px;
  cursor: pointer;
  display: flex;
}

.task__checkbox-native {
  position: absolute;
  opacity: 0;
  width: 0;
  height: 0;
}

.task__checkbox {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 18px;
  height: 18px;
  border-radius: 50%;
  border: 1.75px solid var(--c-border);
  background: var(--c-surface);
  transition: border-color var(--t-fast), background var(--t-fast);
  flex-shrink: 0;
}

.task__check-wrap:hover .task__checkbox {
  border-color: var(--c-primary);
}

.task__checkbox-native:checked + .task__checkbox {
  background: var(--c-success);
  border-color: var(--c-success);
}

.task__checkbox-native:checked + .task__checkbox::after {
  content: '';
  display: block;
  width: 5px;
  height: 9px;
  border: 2px solid #fff;
  border-top: none;
  border-left: none;
  transform: rotate(45deg) translateY(-1px);
}

/* ── Body ── */

.task__body {
  flex: 1;
  min-width: 0;
  display: flex;
  flex-direction: column;
  gap: 0.2rem;
}

.task__title {
  font-size: 0.9375rem;
  font-weight: 500;
  color: var(--c-text);
  line-height: 1.4;
  word-break: break-word;
}

.task--done .task__title {
  text-decoration: line-through;
  color: var(--c-text-muted);
}

.task__description {
  font-size: 0.8125rem;
  color: var(--c-text-muted);
  line-height: 1.5;
  word-break: break-word;
}

.task__meta {
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  gap: 0.375rem;
  margin-top: 0.25rem;
}

.task__tag {
  font-size: 0.6875rem;
  font-weight: 500;
  color: var(--c-primary);
  background: var(--c-primary-lt);
  padding: 0.125rem 0.5rem;
  border-radius: 99px;
  letter-spacing: 0.01em;
}

.task__date {
  font-size: 0.6875rem;
  color: var(--c-text-muted);
  font-weight: 500;
}

/* ── Actions ── */

.task__actions {
  display: flex;
  gap: 0.25rem;
  flex-shrink: 0;
  opacity: 0;
  transition: opacity var(--t-fast);
}

.task:hover .task__actions,
.task:focus-within .task__actions {
  opacity: 1;
}

@media (pointer: coarse) {
  .task__actions {
    opacity: 1;
  }
}

.task__action-btn {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 28px;
  height: 28px;
  border: none;
  border-radius: var(--r-sm);
  background: transparent;
  color: var(--c-text-muted);
  cursor: pointer;
  transition: background var(--t-fast), color var(--t-fast);
}

.task__action-btn:hover {
  background: var(--c-surface-2);
  color: var(--c-text);
}

.task__action-btn--danger:hover {
  background: var(--c-danger-lt);
  color: var(--c-danger);
}
</style>
