<template>
  <div class="home-page">
    <HeroBanner />
    <CategorySection />
    <DealsOfTheDay />
    <FlashSale />
    <ProductGrid />
  </div>
</template>

<script setup>
import HeroBanner from '../components/HeroBanner.vue';
import CategorySection from '../components/CategorySection.vue';
import DealsOfTheDay from '../components/DealsOfTheDay.vue';
import FlashSale from '../components/FlashSale.vue';
import ProductGrid from '../components/ProductGrid.vue';
</script>

<style scoped>
.home-page {
  display: flex;
  flex-direction: column;
  gap: var(--space-xl);
}
</style>
