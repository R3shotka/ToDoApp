<script setup lang="ts">
import { ref, onMounted } from 'vue'
import type { CategoryDto } from '@/types'
import { useCategories } from '@/composables/useCategories'
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
    const result = await editCategory(editingCategory.value.id, { name })
    ok = result !== null
  } else {
    const result = await addCategory({ name })
    ok = result !== null
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
  <div class="categories-view">
    <div class="page-header">
      <h1>Categories</h1>
      <button class="btn btn--primary" @click="openCreate">+ New Category</button>
    </div>

    <div v-if="formVisible" class="form-panel">
      <h2 class="form-title">{{ editingCategory ? 'Edit Category' : 'New Category' }}</h2>
      <CategoryForm
        :category="editingCategory ?? undefined"
        :saving="saving"
        :server-error="formError"
        @save="handleSave"
        @cancel="closeForm"
      />
    </div>

    <p v-if="error && !formVisible" class="page-error">{{ error }}</p>

    <CategoryList
      :categories="categories"
      :loading="loading"
      @edit="openEdit"
      @delete="handleDelete"
    />
  </div>
</template>

<style scoped>
.categories-view {
  max-width: 600px;
  margin: 0 auto;
  padding: 1rem;
}

.page-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 1.5rem;
}

h1 {
  font-size: 1.5rem;
  margin: 0;
}

.form-panel {
  border: 1px solid var(--color-border, #e2e8f0);
  border-radius: 8px;
  padding: 1rem;
  margin-bottom: 1.5rem;
  background: var(--color-surface, #f7fafc);
}

.form-title {
  font-size: 1rem;
  margin: 0 0 0.75rem;
  color: var(--color-text-muted, #718096);
}

.page-error {
  color: #e53e3e;
  margin-bottom: 1rem;
}

.btn {
  padding: 0.5rem 1.25rem;
  border: none;
  border-radius: 6px;
  font-size: 0.95rem;
  cursor: pointer;
}

.btn--primary {
  background: var(--color-primary, #3b82f6);
  color: #fff;
}

@media (min-width: 768px) {
  .categories-view {
    padding: 2rem 1rem;
  }
}
</style>
