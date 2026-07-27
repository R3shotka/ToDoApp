<script setup lang="ts">
import { ref, watch } from 'vue'
import type { TaskDto, CategoryDto, CreateTaskDto, UpdateTaskDto } from '@/types'

const props = defineProps<{
  task?: TaskDto
  categories: CategoryDto[]
}>()

const emit = defineEmits<{
  saved: [task: TaskDto]
  cancel: []
}>()

const title = ref('')
const description = ref('')
const dueDate = ref('')
const categoryId = ref<number | ''>('')
const titleError = ref('')
const formError = ref('')
const submitting = ref(false)

watch(
  () => props.task,
  (task) => {
    title.value = task?.title ?? ''
    description.value = task?.description ?? ''
    dueDate.value = task?.dueDate ? task.dueDate.slice(0, 10) : ''
    categoryId.value = task?.categoryId ?? ''
    titleError.value = ''
    formError.value = ''
  },
  { immediate: true },
)

async function onSubmit() {
  titleError.value = ''
  formError.value = ''
  if (!title.value.trim()) {
    titleError.value = 'Title is required'
    return
  }
  submitting.value = true
  const dto: CreateTaskDto | UpdateTaskDto = {
    title: title.value.trim(),
    description: description.value.trim() || undefined,
    dueDate: dueDate.value || undefined,
    categoryId: categoryId.value !== '' ? Number(categoryId.value) : undefined,
  }
  try {
    let result: TaskDto
    if (props.task) {
      const { update } = await import('@/api/tasks')
      result = await update(props.task.id, dto as UpdateTaskDto)
    } else {
      const { create } = await import('@/api/tasks')
      result = await create(dto as CreateTaskDto)
    }
    emit('saved', result)
  } catch (e: unknown) {
    formError.value = extractError(e, props.task ? 'Failed to update task.' : 'Failed to create task.')
  } finally {
    submitting.value = false
  }
}

function extractError(e: unknown, fallback: string): string {
  if (e && typeof e === 'object' && 'response' in e) {
    const r = (e as { response?: { data?: unknown } }).response
    if (typeof r?.data === 'string' && r.data.trim()) return r.data.trim()
    if (r?.data && typeof r.data === 'object' && 'message' in r.data) {
      return String((r.data as { message: unknown }).message)
    }
  }
  return fallback
}
</script>

<template>
  <div class="modal-backdrop" @click.self="emit('cancel')">
    <div class="modal" role="dialog" aria-modal="true">
      <div class="modal__header">
        <h2 class="modal__title">{{ task ? 'Edit task' : 'New task' }}</h2>
        <button class="modal__close" @click="emit('cancel')" aria-label="Close">
          <svg width="16" height="16" viewBox="0 0 16 16" fill="none">
            <path d="M2 2l12 12M14 2L2 14" stroke="currentColor" stroke-width="1.75" stroke-linecap="round"/>
          </svg>
        </button>
      </div>

      <form @submit.prevent="onSubmit" class="modal__form">
        <div class="field">
          <label class="field__label" for="task-title">Title</label>
          <input
            id="task-title"
            v-model="title"
            type="text"
            class="field__input"
            :class="{ 'field__input--error': titleError }"
            placeholder="What needs to be done?"
            :disabled="submitting"
            autofocus
          />
          <span v-if="titleError" class="field__error">{{ titleError }}</span>
        </div>

        <div class="field">
          <label class="field__label" for="task-description">Description <span class="field__optional">optional</span></label>
          <textarea
            id="task-description"
            v-model="description"
            class="field__input field__textarea"
            placeholder="Add more details…"
            rows="3"
            :disabled="submitting"
          />
        </div>

        <div class="modal__row">
          <div class="field">
            <label class="field__label" for="task-due-date">Due date</label>
            <input
              id="task-due-date"
              v-model="dueDate"
              type="date"
              class="field__input"
              :disabled="submitting"
            />
          </div>

          <div class="field">
            <label class="field__label" for="task-category">Category</label>
            <select id="task-category" v-model="categoryId" class="field__input field__select" :disabled="submitting">
              <option value="">No category</option>
              <option v-for="cat in categories" :key="cat.id" :value="cat.id">
                {{ cat.name }}
              </option>
            </select>
          </div>
        </div>

        <div v-if="formError" class="error-block">{{ formError }}</div>

        <div class="modal__actions">
          <button type="button" @click="emit('cancel')" class="btn btn--secondary" :disabled="submitting">
            Cancel
          </button>
          <button type="submit" class="btn btn--primary" :disabled="submitting">
            {{ submitting ? 'Saving…' : (task ? 'Save changes' : 'Create task') }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<style scoped>
.modal-backdrop {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.4);
  backdrop-filter: blur(3px);
  -webkit-backdrop-filter: blur(3px);
  display: flex;
  align-items: flex-end;
  justify-content: center;
  z-index: 100;
  padding: 0;
  animation: backdrop-in var(--t-base) ease;
}

@media (min-width: 540px) {
  .modal-backdrop {
    align-items: center;
    padding: 1.5rem;
  }
}

@keyframes backdrop-in {
  from { opacity: 0; }
  to   { opacity: 1; }
}

.modal {
  background: var(--c-surface);
  width: 100%;
  max-width: 520px;
  border-radius: var(--r-lg) var(--r-lg) 0 0;
  box-shadow: var(--shadow-lg);
  padding: 1.5rem;
  animation: modal-in var(--t-base) ease;
}

@media (min-width: 540px) {
  .modal {
    border-radius: var(--r-lg);
    animation: modal-in-center var(--t-base) ease;
  }
}

@keyframes modal-in {
  from { transform: translateY(100%); opacity: 0; }
  to   { transform: translateY(0);    opacity: 1; }
}

@keyframes modal-in-center {
  from { transform: translateY(12px) scale(0.98); opacity: 0; }
  to   { transform: translateY(0)    scale(1);    opacity: 1; }
}

.modal__header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 1.25rem;
}

.modal__title {
  font-size: 1.0625rem;
  font-weight: 700;
  color: var(--c-text);
  letter-spacing: -0.02em;
}

.modal__close {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 28px;
  height: 28px;
  border: none;
  background: transparent;
  color: var(--c-text-muted);
  border-radius: var(--r-sm);
  cursor: pointer;
  transition: background var(--t-fast), color var(--t-fast);
}

.modal__close:hover {
  background: var(--c-surface-2);
  color: var(--c-text);
}

.modal__form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.modal__row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 0.75rem;
}

.field__optional {
  font-size: 0.6875rem;
  font-weight: 400;
  color: var(--c-text-muted);
  margin-left: 0.25rem;
}

.field__textarea {
  resize: vertical;
  min-height: 80px;
  font-family: var(--font);
  line-height: 1.5;
}

.field__select {
  cursor: pointer;
  appearance: none;
  background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='12' height='12' viewBox='0 0 12 12'%3E%3Cpath d='M2 4l4 4 4-4' stroke='%238e8e93' stroke-width='1.5' fill='none' stroke-linecap='round' stroke-linejoin='round'/%3E%3C/svg%3E");
  background-repeat: no-repeat;
  background-position: right 0.75rem center;
  padding-right: 2rem;
}

.modal__actions {
  display: flex;
  justify-content: flex-end;
  gap: 0.625rem;
  margin-top: 0.5rem;
}
</style>
