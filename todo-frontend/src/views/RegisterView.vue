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
    await auth.register({ username: username.value.trim(), email: email.value.trim(), password: password.value })
    router.push('/tasks')
  } catch (e: unknown) {
    if (isAxiosError(e)) {
      error.value = e.response?.status === 400
        ? (typeof e.response.data === 'string' ? e.response.data : 'Помилка реєстрації')
        : 'Сервер недоступний. Спробуй пізніше.'
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
    <div class="auth-card">
      <div class="auth-card__logo">
        <span class="auth-card__logo-icon">✓</span>
      </div>
      <h1 class="auth-card__title">Create account</h1>
      <p class="auth-card__subtitle">Start managing your tasks today</p>

      <form @submit.prevent="submit" novalidate class="auth-form">
        <div class="field">
          <label class="field__label" for="username">Username</label>
          <input
            id="username"
            v-model="username"
            type="text"
            autocomplete="username"
            placeholder="Choose a username"
            class="field__input"
            :class="{ 'field__input--error': usernameError }"
          />
          <span v-if="usernameError" class="field__error">{{ usernameError }}</span>
        </div>

        <div class="field">
          <label class="field__label" for="email">Email</label>
          <input
            id="email"
            v-model="email"
            type="email"
            autocomplete="email"
            placeholder="you@example.com"
            class="field__input"
            :class="{ 'field__input--error': emailError }"
          />
          <span v-if="emailError" class="field__error">{{ emailError }}</span>
        </div>

        <div class="field">
          <label class="field__label" for="password">Password</label>
          <input
            id="password"
            v-model="password"
            type="password"
            autocomplete="new-password"
            placeholder="••••••••"
            class="field__input"
            :class="{ 'field__input--error': passwordError }"
          />
          <span v-if="passwordError" class="field__error">{{ passwordError }}</span>
        </div>

        <div v-if="error" class="error-block">{{ error }}</div>

        <button type="submit" :disabled="loading" class="btn btn--primary auth-form__submit">
          {{ loading ? 'Creating account…' : 'Create account' }}
        </button>
      </form>

      <p class="auth-card__footer">
        Already have an account?
        <RouterLink to="/login">Sign in</RouterLink>
      </p>
    </div>
  </div>
</template>

<style scoped>
.auth-page {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 1.5rem 1rem;
  background: var(--c-bg);
}

.auth-card {
  width: 100%;
  max-width: 400px;
  background: var(--c-surface);
  border-radius: var(--r-lg);
  box-shadow: var(--shadow-md);
  padding: 2.25rem 2rem;
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.auth-card__logo {
  display: flex;
  justify-content: center;
  margin-bottom: 0.75rem;
}

.auth-card__logo-icon {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 48px;
  height: 48px;
  background: var(--c-primary);
  color: #fff;
  border-radius: var(--r-md);
  font-size: 1.375rem;
  font-weight: 700;
  box-shadow: 0 4px 12px rgba(91,91,214,.35);
}

.auth-card__title {
  font-size: 1.375rem;
  font-weight: 700;
  color: var(--c-text);
  text-align: center;
  letter-spacing: -0.02em;
  margin-bottom: 0.125rem;
}

.auth-card__subtitle {
  font-size: 0.875rem;
  color: var(--c-text-muted);
  text-align: center;
  margin-bottom: 1.25rem;
}

.auth-form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.auth-form__submit {
  width: 100%;
  height: 42px;
  font-size: 0.9375rem;
  margin-top: 0.25rem;
}

.auth-card__footer {
  font-size: 0.8125rem;
  color: var(--c-text-muted);
  text-align: center;
  margin-top: 1.25rem;
}

.auth-card__footer a {
  color: var(--c-primary);
  font-weight: 500;
}
</style>
