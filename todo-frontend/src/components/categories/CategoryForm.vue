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
    nameError.value = 'Name is required'
    return
  }
  emit('save', name.value.trim())
}
</script>

<template>
  <form class="cat-form" @submit.prevent="submit">
    <div class="field">
      <input
        v-model="name"
        type="text"
        placeholder="Category name"
        :disabled="saving"
        class="field__input"
        :class="{ 'field__input--error': nameError }"
        autofocus
      />
      <span v-if="nameError" class="field__error">{{ nameError }}</span>
    </div>
    <div v-if="serverError" class="error-block">{{ serverError }}</div>
    <div class="cat-form__actions">
      <button type="submit" class="btn btn--primary" :disabled="saving">
        {{ saving ? 'Saving…' : (category ? 'Update' : 'Create') }}
      </button>
      <button type="button" class="btn btn--secondary" :disabled="saving" @click="emit('cancel')">
        Cancel
      </button>
    </div>
  </form>
</template>

<style scoped>
.cat-form {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.cat-form__actions {
  display: flex;
  gap: 0.5rem;
}
</style>
