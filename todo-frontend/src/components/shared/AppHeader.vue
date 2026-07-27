<script setup lang="ts">
import { useAuthStore } from '@/stores/auth'
import { useRouter } from 'vue-router'

const auth = useAuthStore()
const router = useRouter()

function logout() {
  auth.logout()
  router.push('/login')
}
</script>

<template>
  <header class="header">
    <div class="header__inner">
      <RouterLink to="/tasks" class="header__brand">
        <span class="header__brand-icon">✓</span>
        ToDoApp
      </RouterLink>

      <nav class="header__nav">
        <RouterLink to="/tasks" class="header__link">Tasks</RouterLink>
        <RouterLink to="/categories" class="header__link">Categories</RouterLink>
      </nav>

      <div class="header__right">
        <span class="header__username">{{ auth.user?.username }}</span>
        <button class="header__logout" @click="logout">Log out</button>
      </div>
    </div>
  </header>
</template>

<style scoped>
.header {
  position: sticky;
  top: 0;
  z-index: 50;
  background: rgba(255, 255, 255, 0.85);
  backdrop-filter: blur(12px);
  -webkit-backdrop-filter: blur(12px);
  border-bottom: 1px solid var(--c-border);
}

.header__inner {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  max-width: 960px;
  margin: 0 auto;
  padding: 0 1rem;
  height: 56px;
}

.header__brand {
  display: flex;
  align-items: center;
  gap: 0.375rem;
  font-size: 0.9375rem;
  font-weight: 700;
  color: var(--c-text);
  letter-spacing: -0.02em;
  flex-shrink: 0;
}

.header__brand-icon {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 24px;
  height: 24px;
  background: var(--c-primary);
  color: #fff;
  border-radius: 6px;
  font-size: 0.75rem;
  font-weight: 700;
}

.header__nav {
  display: flex;
  align-items: center;
  gap: 0.125rem;
  margin-left: 1rem;
}

.header__link {
  padding: 0.3125rem 0.75rem;
  border-radius: var(--r-sm);
  font-size: 0.875rem;
  font-weight: 500;
  color: var(--c-text-muted);
  transition: color var(--t-fast), background var(--t-fast);
}

.header__link:hover {
  color: var(--c-text);
  background: var(--c-surface-2);
}

.header__link.router-link-active {
  color: var(--c-primary);
  background: var(--c-primary-lt);
}

.header__right {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  margin-left: auto;
}

.header__username {
  font-size: 0.8125rem;
  font-weight: 500;
  color: var(--c-text-muted);
  display: none;
}

.header__logout {
  font-family: var(--font);
  font-size: 0.8125rem;
  font-weight: 500;
  color: var(--c-text-muted);
  background: none;
  border: none;
  cursor: pointer;
  padding: 0.3125rem 0.625rem;
  border-radius: var(--r-sm);
  transition: color var(--t-fast), background var(--t-fast);
}

.header__logout:hover {
  color: var(--c-text);
  background: var(--c-surface-2);
}

@media (min-width: 640px) {
  .header__username {
    display: block;
  }
}
</style>
