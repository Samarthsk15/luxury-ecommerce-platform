<template>
  <section class="categories-section">
    <div class="section-header">
      <h2 class="section-title">Shop by Category</h2>
      <p class="section-sub">Curated collections for every lifestyle</p>
    </div>
    <div class="cat-grid">
      <router-link
        v-for="cat in categories"
        :key="cat.slug"
        :to="`/products?category=${cat.slug}`"
        class="cat-tile glass-card"
        @click="trackClick(cat.slug)"
      >
        <span class="cat-icon">{{ cat.icon }}</span>
        <span class="cat-name">{{ cat.name }}</span>
        <span class="cat-count">{{ cat.count }}+ items</span>
      </router-link>
    </div>
  </section>
</template>

<script setup>
import { useBehaviorStore } from '../store/behavior';

const behaviorStore = useBehaviorStore();

const categories = [
  { name: 'Electronics', slug: 'electronics', icon: '🔌', count: '1,200' },
  { name: 'Fashion', slug: 'fashion', icon: '👗', count: '2,400' },
  { name: 'Home & Living', slug: 'home', icon: '🏠', count: '980' },
  { name: 'Beauty', slug: 'beauty', icon: '✨', count: '1,500' },
  { name: 'Sports', slug: 'sports', icon: '⚽', count: '870' },
  { name: 'Books', slug: 'books', icon: '📚', count: '1,100' },
  { name: 'Gaming', slug: 'gaming', icon: '🎮', count: '650' },
  { name: 'Automotive', slug: 'automotive', icon: '🚗', count: '420' },
  { name: 'Groceries', slug: 'groceries', icon: '🛒', count: '3,200' },
  { name: 'Toys', slug: 'toys', icon: '🧸', count: '780' },
];

const trackClick = (slug) => {
  // Translate standard categories into our targeted recommendation buckets
  let targetBucket = 'premium'; // Default
  if (slug === 'gaming') targetBucket = 'gaming';
  if (slug === 'electronics') targetBucket = 'audio';
  
  behaviorStore.trackInteraction(targetBucket);
};
</script>

<style scoped>
.categories-section {
  display: flex;
  flex-direction: column;
  gap: var(--space-lg);
}

.section-header {
  text-align: center;
}

.section-title {
  font-size: 2rem;
  font-weight: 800;
  letter-spacing: -0.03em;
  margin-bottom: 0.35rem;
}

.section-sub {
  color: var(--text-secondary);
  font-size: 0.95rem;
}

.cat-grid {
  display: grid;
  grid-template-columns: repeat(5, 1fr);
  gap: 1rem;
}

.cat-tile {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.75rem;
  padding: 1.75rem 1rem;
  border-radius: var(--radius-lg);
  text-decoration: none;
  text-align: center;
  cursor: pointer;
  transition: all 0.4s var(--ease-out);
}

.cat-tile:hover {
  transform: translateY(-6px);
  background: var(--bg-card-hover);
  border-color: var(--border-accent);
  box-shadow: 0 16px 48px rgba(0, 0, 0, 0.3), 0 0 20px var(--accent-glow);
}

.cat-icon {
  font-size: 2rem;
  display: flex;
  align-items: center;
  justify-content: center;
  width: 56px;
  height: 56px;
  border-radius: var(--radius-md);
  background: rgba(46, 91, 255, 0.08);
  border: 1px solid rgba(46, 91, 255, 0.12);
  transition: all 0.3s ease;
}

.cat-tile:hover .cat-icon {
  background: rgba(46, 91, 255, 0.15);
  border-color: rgba(46, 91, 255, 0.3);
  transform: scale(1.08);
}

.cat-name {
  font-family: var(--font-headline);
  font-weight: 700;
  font-size: 0.85rem;
  color: var(--text-primary);
}

.cat-count {
  font-size: 0.7rem;
  color: var(--text-muted);
  font-weight: 500;
}

@media (max-width: 768px) {
  .cat-grid {
    grid-template-columns: repeat(3, 1fr);
  }
}
</style>
