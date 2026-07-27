<script setup lang="ts">
import { ref, onMounted } from 'vue'
import type { CategoryDto } from '@/types'
import { useCategories } from '@/composables/useCategories'
import AppLayout from '@/components/shared/AppLayout.vue'
import CategoryForm from '@/components/categories/CategoryForm.vue'
import CategoryList from '@/components/categories/CategoryList.vue'

const { categories, loading, error, fetchAll, addCategory, editCategory, deleteCategory } =
  useCategories()

const editingCategory = ref<CategoryDto | null>(null)
const formVisible = ref(false)
const saving = ref(false)
const formError = ref<string | null>(null)

onMounted(fetchAll)

function openCreate() {
  editingCategory.value = null
  formError.value = null
  formVisible.value = true
}

function openEdit(category: CategoryDto) {
  editingCategory.value = category
  formError.value = null
  formVisible.value = true
}

function closeForm() {
  formVisible.value = false
  editingCategory.value = null
  formError.value = null
}

async function handleSave(name: string) {
  saving.value = true
  formError.value = null
  let ok: boolean
  if (editingCategory.value) {
    ok = (await editCategory(editingCategory.value.id, { name })) !== null
  } else {
    ok = (await addCategory({ name })) !== null
  }
  saving.value = false
  if (ok) {
    closeForm()
  } else {
    formError.value = error.value
  }
}

async function handleDelete(id: number) {
  if (!window.confirm('Delete this category?')) return
  await deleteCategory(id)
}
</script>

<template>
  <AppLayout>
    <div class="categories-view">
      <div class="categories-view__header">
        <div>
          <h1 class="categories-view__title">Categories</h1>
          <p class="categories-view__subtitle" v-if="!loading">
            {{ categories.length }} categor{{ categories.length !== 1 ? 'ies' : 'y' }}
          </p>
        </div>
        <button class="btn btn--primary" @click="openCreate">
          <span class="btn-icon">+</span> New Category
        </button>
      </div>

      <div v-if="formVisible" class="form-panel">
        <h2 class="form-panel__title">
          {{ editingCategory ? 'Edit category' : 'New category' }}
        </h2>
        <CategoryForm
          :category="editingCategory ?? undefined"
          :saving="saving"
          :server-error="formError"
          @save="handleSave"
          @cancel="closeForm"
        />
      </div>

      <div v-if="error && !formVisible" class="error-block">{{ error }}</div>

      <CategoryList
        :categories="categories"
        :loading="loading"
        @edit="openEdit"
        @delete="handleDelete"
      />
    </div>
  </AppLayout>
</template>

<style scoped>
.categories-view {
  max-width: 600px;
  margin: 0 auto;
}

.categories-view__header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 1rem;
  margin-bottom: 1.5rem;
}

.categories-view__title {
  font-size: 1.5rem;
  font-weight: 700;
  color: var(--c-text);
  letter-spacing: -0.025em;
  line-height: 1.2;
}

.categories-view__subtitle {
  font-size: 0.8125rem;
  color: var(--c-text-muted);
  margin-top: 0.125rem;
}

.btn-icon {
  font-size: 1.125rem;
  line-height: 1;
}

.form-panel {
  background: var(--c-surface);
  border: 1px solid var(--c-border);
  border-radius: var(--r-md);
  padding: 1.25rem;
  margin-bottom: 1.5rem;
  box-shadow: var(--shadow-sm);
}

.form-panel__title {
  font-size: 0.875rem;
  font-weight: 600;
  color: var(--c-text-2);
  margin-bottom: 1rem;
  letter-spacing: -0.01em;
}

.error-block {
  margin-bottom: 1rem;
}
</style>
