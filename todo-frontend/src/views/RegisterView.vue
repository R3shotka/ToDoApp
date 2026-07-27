<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const auth = useAuthStore()

const username = ref('')
const email = ref('')
const password = ref('')
const error = ref('')
const loading = ref(false)

const usernameError = ref('')
const emailError = ref('')
const passwordError = ref('')

const EMAIL_RE = /^[^\s@]+@[^\s@]+\.[^\s@]+$/

function validate(): boolean {
  usernameError.value = ''
  emailError.value = ''
  passwordError.value = ''
  let ok = true

  if (username.value.trim().length < 3) {
    usernameError.value = 'Мінімум 3 символи'
    ok = false
  }
  if (!EMAIL_RE.test(email.value.trim())) {
    emailError.value = 'Некоректний email'
    ok = false
  }
  if (password.value.length < 6) {
    passwordError.value = 'Мінімум 6 символів'
    ok = false
  }
  return ok
}

async function submit() {
  error.value = ''
  if (!validate()) return

  loading.value = true
  try {
    await auth.register({
      username: username.value.trim(),
      email: email.value.trim(),
      password: password.value,
    })
    router.push('/tasks')
  } catch (e: unknown) {
    if (isAxiosError(e)) {
      if (e.response?.status === 400) {
        error.value = typeof e.response.data === 'string'
          ? e.response.data
          : 'Помилка реєстрації'
      } else {
        error.value = 'Сервер недоступний. Спробуй пізніше.'
      }
    } else {
      error.value = 'Сервер недоступний. Спробуй пізніше.'
    }
  } finally {
    loading.value = false
  }
}

function isAxiosError(e: unknown): e is { response?: { status: number; data?: unknown } } {
  return typeof e === 'object' && e !== null && 'response' in e
}
</script>

<template>
  <div class="auth-page">
    <form class="auth-form" @submit.prevent="submit" novalidate>
      <h1 class="auth-title">Реєстрація</h1>

      <div class="field">
        <label for="username">Логін</label>
        <input
          id="username"
          v-model="username"
          type="text"
          autocomplete="username"
          :class="{ invalid: usernameError }"
        />
        <span v-if="usernameError" class="field-error">{{ usernameError }}</span>
      </div>

      <div class="field">
        <label for="email">Email</label>
        <input
          id="email"
          v-model="email"
          type="email"
          autocomplete="email"
          :class="{ invalid: emailError }"
        />
        <span v-if="emailError" class="field-error">{{ emailError }}</span>
      </div>

      <div class="field">
        <label for="password">Пароль</label>
        <input
          id="password"
          v-model="password"
          type="password"
          autocomplete="new-password"
          :class="{ invalid: passwordError }"
        />
        <span v-if="passwordError" class="field-error">{{ passwordError }}</span>
      </div>

      <div v-if="error" class="error-block">{{ error }}</div>

      <button type="submit" :disabled="loading" class="btn-primary">
        {{ loading ? 'Завантаження...' : 'Зареєструватись' }}
      </button>

      <p class="auth-link">
        Вже маєш акаунт?
        <RouterLink to="/login">Увійти</RouterLink>
      </p>
    </form>
  </div>
</template>

<style scoped>
.auth-page {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  padding: 1rem;
}

.auth-form {
  width: 100%;
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.auth-title {
  font-size: 1.5rem;
  font-weight: 600;
  margin-bottom: 0.5rem;
}

.field {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.field label {
  font-size: 0.875rem;
  font-weight: 500;
}

.field input {
  padding: 0.625rem 0.75rem;
  border: 1px solid #d1d5db;
  border-radius: 0.375rem;
  font-size: 1rem;
  outline: none;
  transition: border-color 0.15s;
}

.field input:focus {
  border-color: #6366f1;
}

.field input.invalid {
  border-color: #ef4444;
}

.field-error {
  font-size: 0.75rem;
  color: #ef4444;
}

.error-block {
  padding: 0.75rem 1rem;
  background: #fef2f2;
  border: 1px solid #fecaca;
  border-radius: 0.375rem;
  color: #b91c1c;
  font-size: 0.875rem;
}

.btn-primary {
  padding: 0.625rem 1rem;
  background: #6366f1;
  color: #fff;
  border: none;
  border-radius: 0.375rem;
  font-size: 1rem;
  font-weight: 500;
  cursor: pointer;
  transition: background 0.15s;
}

.btn-primary:hover:not(:disabled) {
  background: #4f46e5;
}

.btn-primary:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.auth-link {
  font-size: 0.875rem;
  text-align: center;
}

.auth-link a {
  color: #6366f1;
  text-decoration: none;
}

.auth-link a:hover {
  text-decoration: underline;
}

@media (min-width: 768px) {
  .auth-form {
    max-width: 400px;
    margin: 0 auto;
  }
}
</style>
