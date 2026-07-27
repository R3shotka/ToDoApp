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
    titleError.value = 'Title is required.'
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
  if (
    e &&
    typeof e === 'object' &&
    'response' in e &&
    e.response &&
    typeof e.response === 'object' &&
    'data' in e.response
  ) {
    const data = (e.response as { data: unknown }).data
    if (typeof data === 'string' && data.trim()) return data.trim()
    if (data && typeof data === 'object' && 'message' in data) {
      return String((data as { message: unknown }).message)
    }
  }
  return fallback
}
</script>

<template>
  <div class="modal-backdrop" @click.self="emit('cancel')">
    <div class="modal" role="dialog" aria-modal="true">
      <h2 class="modal__title">{{ task ? 'Edit Task' : 'New Task' }}</h2>

      <form @submit.prevent="onSubmit" class="modal__form">
        <div class="field">
          <label class="field__label" for="task-title">Title *</label>
          <input
            id="task-title"
            v-model="title"
            type="text"
            class="field__input"
            :class="{ 'field__input--error': titleError }"
            placeholder="Task title"
            :disabled="submitting"
          />
          <span v-if="titleError" class="field__error">{{ titleError }}</span>
        </div>

        <div class="field">
          <label class="field__label" for="task-description">Description</label>
          <textarea
            id="task-description"
            v-model="description"
            class="field__input field__textarea"
            placeholder="Optional description"
            rows="3"
            :disabled="submitting"
          />
        </div>

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
          <select id="task-category" v-model="categoryId" class="field__input" :disabled="submitting">
            <option value="">No category</option>
            <option v-for="cat in categories" :key="cat.id" :value="cat.id">
              {{ cat.name }}
            </option>
          </select>
        </div>

        <p v-if="formError" class="modal__error">{{ formError }}</p>

        <div class="modal__actions">
          <button type="button" @click="emit('cancel')" class="btn btn--secondary" :disabled="submitting">
            Cancel
          </button>
          <button type="submit" class="btn btn--primary" :disabled="submitting">
            {{ submitting ? 'Saving...' : 'Save' }}
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
  background: rgba(0, 0, 0, 0.45);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 100;
  padding: 1rem;
}

.modal {
  background: #fff;
  border-radius: 8px;
  width: 100%;
  max-width: 480px;
  padding: 1.5rem;
  box-shadow: 0 4px 24px rgba(0, 0, 0, 0.18);
}

.modal__title {
  margin: 0 0 1.25rem;
  font-size: 1.25rem;
}

.modal__form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.field {
  display: flex;
  flex-direction: column;
  gap: 0.3rem;
}

.field__label {
  font-size: 0.875rem;
  font-weight: 500;
  color: #333;
}

.field__input {
  padding: 0.5rem 0.75rem;
  border: 1px solid #ccc;
  border-radius: 4px;
  font-size: 1rem;
  width: 100%;
  box-sizing: border-box;
}

.field__input--error {
  border-color: #c81e1e;
}

.field__textarea {
  resize: vertical;
  font-family: inherit;
}

.field__error {
  font-size: 0.8rem;
  color: #c81e1e;
}

.modal__error {
  margin: 0;
  padding: 0.6rem 0.75rem;
  background: #fde8e8;
  color: #c81e1e;
  border-radius: 4px;
  font-size: 0.875rem;
}

.modal__actions {
  display: flex;
  justify-content: flex-end;
  gap: 0.75rem;
  margin-top: 0.5rem;
}

.btn {
  padding: 0.5rem 1.25rem;
  border: none;
  border-radius: 4px;
  font-size: 1rem;
  cursor: pointer;
}

.btn:disabled {
  opacity: 0.6;
  cursor: default;
}

.btn--primary {
  background: #1a56db;
  color: #fff;
}

.btn--secondary {
  background: #f0f0f0;
  color: #333;
}
</style>
