<template>
  <section class="deals-section">
    <div class="section-header">
      <div>
        <h2 class="section-title">Today's Best Deals</h2>
        <p class="section-sub">Handpicked offers refreshed daily</p>
      </div>
      <router-link to="/deals" class="see-all">See All Deals →</router-link>
    </div>
    <div class="deals-grid">
      <article v-for="deal in deals" :key="deal.id" class="deal-card glass-card">
        <div class="deal-image">
          <img :src="deal.image" :alt="deal.name" loading="lazy" />
          <span class="deal-badge">-{{ deal.discount }}%</span>
        </div>
        <div class="deal-body">
          <span class="deal-category">{{ deal.category }}</span>
          <h3 class="deal-name">{{ deal.name }}</h3>
          <div class="deal-pricing">
            <span class="deal-price">₹{{ deal.salePrice.toLocaleString() }}</span>
            <span class="deal-original">₹{{ deal.originalPrice.toLocaleString() }}</span>
          </div>
          <button class="deal-btn" @click="addToCart(deal)">Add to Cart</button>
        </div>
      </article>
    </div>
  </section>
</template>

<script setup>
import { useCartStore } from '../store/cart';

const cart = useCartStore();

const deals = [
  { id: 101, name: 'Velocity Elite X', category: 'Audio', discount: 35, salePrice: 8499, originalPrice: 12999, image: 'https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=400&h=300&fit=crop' },
  { id: 102, name: 'Aura Sonic Gen-3', category: 'Wearables', discount: 25, salePrice: 14999, originalPrice: 19999, image: 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=400&h=300&fit=crop' },
  { id: 103, name: 'Lumina Prime 85mm', category: 'Photography', discount: 40, salePrice: 42999, originalPrice: 71999, image: 'https://images.unsplash.com/photo-1617005082133-548c4dd27f35?w=400&h=300&fit=crop' },
  { id: 104, name: 'Nexus Lounge Chair', category: 'Furniture', discount: 20, salePrice: 24999, originalPrice: 31499, image: 'https://images.unsplash.com/photo-1555041469-a586c61ea9bc?w=400&h=300&fit=crop' },
];

const addToCart = (deal) => {
  cart.addItem({ id: deal.id, name: deal.name, price: deal.salePrice, imageUrl: deal.image });
};
</script>

<style scoped>
.deals-section {
  display: flex;
  flex-direction: column;
  gap: var(--space-lg);
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
}

.section-title {
  font-size: 2rem;
  font-weight: 800;
  letter-spacing: -0.03em;
  margin-bottom: 0.25rem;
}

.section-sub {
  color: var(--text-secondary);
  font-size: 0.9rem;
}

.see-all {
  font-family: var(--font-headline);
  font-size: 0.85rem;
  font-weight: 600;
  color: var(--text-accent);
  transition: color 0.2s ease;
}

.see-all:hover {
  color: var(--accent-blue);
}

.deals-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 1.25rem;
}

.deal-card {
  border-radius: var(--radius-xl);
  overflow: hidden;
  display: flex;
  flex-direction: column;
  transition: all 0.4s var(--ease-out);
}

.deal-card:hover {
  transform: translateY(-8px);
}

.deal-image {
  position: relative;
  height: 200px;
  overflow: hidden;
  background: var(--bg-elevated);
}

.deal-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.5s var(--ease-out);
}

.deal-card:hover .deal-image img {
  transform: scale(1.08);
}

.deal-badge {
  position: absolute;
  top: 12px;
  left: 12px;
  padding: 0.3rem 0.7rem;
  background: var(--error);
  color: white;
  font-family: var(--font-headline);
  font-size: 0.75rem;
  font-weight: 700;
  border-radius: var(--radius-full);
}

.deal-body {
  padding: 1.25rem;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  flex: 1;
}

.deal-category {
  font-size: 0.7rem;
  text-transform: uppercase;
  letter-spacing: 0.1em;
  color: var(--text-accent);
  font-weight: 600;
}

.deal-name {
  font-size: 1rem;
  font-weight: 700;
  letter-spacing: -0.01em;
}

.deal-pricing {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  margin-top: auto;
}

.deal-price {
  font-family: var(--font-headline);
  font-size: 1.25rem;
  font-weight: 800;
  color: var(--text-primary);
}

.deal-original {
  font-size: 0.85rem;
  color: var(--text-muted);
  text-decoration: line-through;
}

.deal-btn {
  margin-top: 0.75rem;
  padding: 0.7rem;
  border-radius: var(--radius-md);
  background: rgba(46, 91, 255, 0.1);
  border: 1px solid rgba(46, 91, 255, 0.2);
  color: var(--text-accent);
  font-weight: 600;
  font-size: 0.85rem;
  transition: all 0.3s ease;
}

.deal-btn:hover {
  background: var(--accent-blue);
  color: white;
  border-color: var(--accent-blue);
  box-shadow: 0 8px 24px var(--accent-glow);
}

@media (max-width: 1024px) {
  .deals-grid { grid-template-columns: repeat(2, 1fr); }
}
@media (max-width: 640px) {
  .deals-grid { grid-template-columns: 1fr; }
}
</style>
