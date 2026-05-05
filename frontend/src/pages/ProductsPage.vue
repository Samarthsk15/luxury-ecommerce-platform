<template>
  <div class="products-page">
    <div class="page-header">
      <h1 class="page-title">All Products</h1>
      <p class="page-sub">Browse our entire collection of 10,000+ premium items</p>
    </div>
    <ProductGrid />
  </div>
</template>

<script setup>
import ProductGrid from '../components/ProductGrid.vue';
</script>

<style scoped>
.products-page {
  display: flex;
  flex-direction: column;
  gap: var(--space-lg);
}
.page-header {
  padding-bottom: var(--space-sm);
  border-bottom: 1px solid var(--border-glass);
}
.page-title {
  font-size: 2.25rem;
  font-weight: 800;
  letter-spacing: -0.03em;
  margin-bottom: 0.25rem;
}
.page-sub {
  color: var(--text-secondary);
  font-size: 0.9rem;
}
</style>
