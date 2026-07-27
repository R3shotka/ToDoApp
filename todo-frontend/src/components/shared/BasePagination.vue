<script setup lang="ts">
defineProps<{
  page: number
  totalPages: number
}>()

const emit = defineEmits<{
  change: [page: number]
}>()
</script>

<template>
  <div class="pagination" v-if="totalPages > 1">
    <button :disabled="page <= 1" @click="emit('change', page - 1)" class="pagination__btn">
      Prev
    </button>
    <span class="pagination__info">Page {{ page }} of {{ totalPages }}</span>
    <button
      :disabled="page >= totalPages"
      @click="emit('change', page + 1)"
      class="pagination__btn"
    >
      Next
    </button>
  </div>
</template>

<style scoped>
.pagination {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 1rem;
  margin-top: 1.5rem;
}

.pagination__btn {
  padding: 0.4rem 1rem;
  border: 1px solid #ccc;
  border-radius: 4px;
  background: #fff;
  cursor: pointer;
  font-size: 0.9rem;
}

.pagination__btn:disabled {
  opacity: 0.4;
  cursor: default;
}

.pagination__info {
  font-size: 0.9rem;
  color: #555;
}
</style>
