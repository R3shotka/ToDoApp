<script setup lang="ts">
import { ref, watch } from 'vue'
import type { CategoryDto } from '@/types'

const props = defineProps<{
  category?: CategoryDto
  saving?: boolean
  serverError?: string | null
}>()

const emit = defineEmits<{
  save: [name: string]
  cancel: []
}>()

const name = ref(props.category?.name ?? '')
const nameError = ref('')

watch(
  () => props.category,
  (cat) => {
    name.value = cat?.name ?? ''
    nameError.value = ''
  },
)

function submit() {
  nameError.value = ''
  if (!name.value.trim()) {
    nameError.value = 'Name is required.'
    return
  }
  emit('save', name.value.trim())
}
</script>

<template>
  <form class="category-form" @submit.prevent="submit">
    <div class="field">
      <input
        v-model="name"
        type="text"
        placeholder="Category name"
        :disabled="saving"
        class="input"
        :class="{ 'input--error': nameError }"
        autofocus
      />
      <p v-if="nameError" class="field-error">{{ nameError }}</p>
    </div>
    <p v-if="serverError" class="server-error">{{ serverError }}</p>
    <div class="actions">
      <button type="submit" class="btn btn--primary" :disabled="saving">
        {{ saving ? 'Saving...' : (category ? 'Update' : 'Create') }}
      </button>
      <button type="button" class="btn btn--ghost" :disabled="saving" @click="emit('cancel')">
        Cancel
      </button>
    </div>
  </form>
</template>

<style scoped>
.category-form {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.field {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.input {
  padding: 0.5rem 0.75rem;
  border: 1px solid var(--color-border, #ccc);
  border-radius: 6px;
  font-size: 1rem;
  width: 100%;
  box-sizing: border-box;
}

.input--error {
  border-color: #e53e3e;
}

.field-error,
.server-error {
  font-size: 0.85rem;
  color: #e53e3e;
  margin: 0;
}

.actions {
  display: flex;
  gap: 0.5rem;
}

.btn {
  padding: 0.5rem 1.25rem;
  border: none;
  border-radius: 6px;
  font-size: 0.95rem;
  cursor: pointer;
}

.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn--primary {
  background: var(--color-primary, #3b82f6);
  color: #fff;
}

.btn--ghost {
  background: transparent;
  border: 1px solid var(--color-border, #ccc);
  color: var(--color-text, #333);
}
</style>
