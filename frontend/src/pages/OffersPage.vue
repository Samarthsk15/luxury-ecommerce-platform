<template>
  <div class="deals-page">
    <!-- Flash Sale Banner -->
    <section class="flash-banner">
      <div class="flash-glow-left"></div>
      <div class="flash-glow-right"></div>
      <div class="flash-content">
        <span class="flash-pill">⚡ FLASH SALE</span>
        <h1 class="flash-heading">Mega Savings Event</h1>
        <p class="flash-desc">Up to 70% off on premium electronics, fashion, and more</p>
        <div class="flash-timer">
          <div class="t-unit">
            <span class="t-num">{{ hours }}</span>
            <span class="t-lbl">HRS</span>
          </div>
          <span class="t-sep">:</span>
          <div class="t-unit">
            <span class="t-num">{{ minutes }}</span>
            <span class="t-lbl">MIN</span>
          </div>
          <span class="t-sep">:</span>
          <div class="t-unit">
            <span class="t-num">{{ seconds }}</span>
            <span class="t-lbl">SEC</span>
          </div>
        </div>
      </div>
    </section>

    <!-- Best Deals -->
    <section class="deals-section">
      <h2 class="section-title">Today's Best Deals</h2>
      <div class="deal-grid">
        <article v-for="deal in deals" :key="deal.id" class="deal-item glass-card">
          <div class="deal-img">
            <img :src="deal.image" :alt="deal.name" loading="lazy" />
            <span class="off-badge">-{{ deal.off }}%</span>
          </div>
          <div class="deal-body">
            <span class="deal-cat">{{ deal.category }}</span>
            <h3>{{ deal.name }}</h3>
            <div class="deal-prices">
              <span class="now">₹{{ deal.sale.toLocaleString() }}</span>
              <span class="was">₹{{ deal.original.toLocaleString() }}</span>
            </div>
            <button class="deal-add" @click="addToCart(deal)">Add to Cart</button>
          </div>
        </article>
      </div>
    </section>

    <!-- Clearance -->
    <section class="deals-section">
      <h2 class="section-title">Clearance Picks</h2>
      <div class="clearance-row">
        <div v-for="item in clearance" :key="item.id" class="clearance-card glass-card">
          <img :src="item.image" :alt="item.name" loading="lazy" />
          <div class="cl-info">
            <span class="cl-name">{{ item.name }}</span>
            <span class="cl-price">₹{{ item.price.toLocaleString() }}</span>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue';
import { useCartStore } from '../store/cart';

const cart = useCartStore();

const hours = ref('06');
const minutes = ref('45');
const seconds = ref('30');
let interval;

const tick = () => {
  let s = parseInt(seconds.value), m = parseInt(minutes.value), h = parseInt(hours.value);
  s--;
  if (s < 0) { s = 59; m--; }
  if (m < 0) { m = 59; h--; }
  if (h < 0) { h = 23; m = 59; s = 59; }
  hours.value = String(h).padStart(2, '0');
  minutes.value = String(m).padStart(2, '0');
  seconds.value = String(s).padStart(2, '0');
};

onMounted(() => { interval = setInterval(tick, 1000); });
onUnmounted(() => { clearInterval(interval); });

const deals = [
  { id: 301, name: 'Sony WH-1000XM5', category: 'Audio', off: 40, sale: 17999, original: 29999, image: 'https://images.unsplash.com/photo-1583394838336-acd977736f90?w=400&h=300&fit=crop' },
  { id: 302, name: 'Apple Watch Ultra', category: 'Wearables', off: 25, sale: 59999, original: 79999, image: 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=400&h=300&fit=crop' },
  { id: 303, name: 'Canon EOS R6 II', category: 'Photography', off: 30, sale: 139999, original: 199999, image: 'https://images.unsplash.com/photo-1617005082133-548c4dd27f35?w=400&h=300&fit=crop' },
  { id: 304, name: 'Herman Miller Aeron', category: 'Furniture', off: 35, sale: 84999, original: 129999, image: 'https://images.unsplash.com/photo-1555041469-a586c61ea9bc?w=400&h=300&fit=crop' },
  { id: 305, name: 'MacBook Pro M4', category: 'Laptops', off: 15, sale: 169999, original: 199999, image: 'https://images.unsplash.com/photo-1517336714731-489689fd1ca8?w=400&h=300&fit=crop' },
  { id: 306, name: 'Dyson V15 Detect', category: 'Home', off: 20, sale: 44999, original: 56999, image: 'https://images.unsplash.com/photo-1558618666-fcd25c85f82e?w=400&h=300&fit=crop' },
];

const clearance = [
  { id: 401, name: 'Wireless Mouse', price: 799, image: 'https://images.unsplash.com/photo-1527864550417-7fd91fc51a46?w=300&h=200&fit=crop' },
  { id: 402, name: 'USB-C Hub', price: 1299, image: 'https://images.unsplash.com/photo-1625842268584-8f3296236761?w=300&h=200&fit=crop' },
  { id: 403, name: 'Phone Stand', price: 499, image: 'https://images.unsplash.com/photo-1586953208448-b95a79798f07?w=300&h=200&fit=crop' },
  { id: 404, name: 'Desk Lamp LED', price: 1899, image: 'https://images.unsplash.com/photo-1507473885765-e6ed057ab6fe?w=300&h=200&fit=crop' },
  { id: 405, name: 'Keyboard Pad', price: 599, image: 'https://images.unsplash.com/photo-1587829741301-dc798b83add3?w=300&h=200&fit=crop' },
];

const addToCart = (deal) => {
  cart.addItem({ id: deal.id, name: deal.name, price: deal.sale, imageUrl: deal.image });
};
</script>

<style scoped>
.deals-page {
  display: flex;
  flex-direction: column;
  gap: var(--space-xl);
}

/* Flash Banner */
.flash-banner {
  position: relative;
  padding: var(--space-xl);
  border-radius: var(--radius-xl);
  background: linear-gradient(135deg, #0f1a3a 0%, #1a0a2e 60%, #0f1a3a 100%);
  border: 1px solid rgba(139, 92, 246, 0.2);
  overflow: hidden;
  text-align: center;
}

.flash-glow-left, .flash-glow-right {
  position: absolute;
  width: 400px;
  height: 400px;
  border-radius: 50%;
  filter: blur(100px);
  pointer-events: none;
}

.flash-glow-left {
  top: -150px;
  left: -100px;
  background: rgba(46, 91, 255, 0.2);
}

.flash-glow-right {
  bottom: -150px;
  right: -100px;
  background: rgba(139, 92, 246, 0.15);
}

.flash-content {
  position: relative;
  z-index: 2;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1rem;
}

.flash-pill {
  padding: 0.5rem 1.25rem;
  background: rgba(139, 92, 246, 0.2);
  border: 1px solid rgba(139, 92, 246, 0.35);
  border-radius: var(--radius-full);
  color: #d0bcff;
  font-family: var(--font-headline);
  font-size: 0.85rem;
  font-weight: 700;
  letter-spacing: 0.1em;
}

.flash-heading {
  font-size: 2.75rem;
  font-weight: 800;
  letter-spacing: -0.04em;
  background: linear-gradient(135deg, #fff, #d0bcff);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.flash-desc {
  color: rgba(255, 255, 255, 0.5);
  font-size: 1rem;
  max-width: 500px;
}

.flash-timer {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  margin-top: 0.5rem;
}

.t-unit {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.2rem;
  min-width: 60px;
  padding: 0.75rem;
  background: rgba(255, 255, 255, 0.06);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: var(--radius-md);
}

.t-num {
  font-family: var(--font-headline);
  font-size: 1.5rem;
  font-weight: 800;
  color: white;
}

.t-lbl {
  font-size: 0.6rem;
  text-transform: uppercase;
  letter-spacing: 0.1em;
  color: rgba(255, 255, 255, 0.35);
}

.t-sep {
  font-size: 1.25rem;
  font-weight: 800;
  color: rgba(255, 255, 255, 0.25);
}

/* Deals Grid */
.section-title {
  font-size: 1.75rem;
  font-weight: 800;
  letter-spacing: -0.03em;
  margin-bottom: var(--space-md);
}

.deal-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 1.25rem;
}

.deal-item {
  border-radius: var(--radius-xl);
  overflow: hidden;
  transition: all 0.4s var(--ease-out);
}

.deal-item:hover {
  transform: translateY(-6px);
}

.deal-img {
  position: relative;
  height: 200px;
  overflow: hidden;
  background: var(--bg-elevated);
}

.deal-img img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.5s var(--ease-out);
}

.deal-item:hover .deal-img img { transform: scale(1.06); }

.off-badge {
  position: absolute;
  top: 12px;
  left: 12px;
  padding: 0.3rem 0.65rem;
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
  gap: 0.4rem;
}

.deal-cat {
  font-size: 0.7rem;
  text-transform: uppercase;
  letter-spacing: 0.1em;
  color: var(--text-accent);
  font-weight: 600;
}

.deal-body h3 {
  font-size: 1rem;
  font-weight: 700;
}

.deal-prices {
  display: flex;
  gap: 0.75rem;
  align-items: center;
  margin-top: 0.25rem;
}

.now {
  font-family: var(--font-headline);
  font-size: 1.2rem;
  font-weight: 800;
}

.was {
  font-size: 0.85rem;
  color: var(--text-muted);
  text-decoration: line-through;
}

.deal-add {
  margin-top: 0.75rem;
  padding: 0.65rem;
  border-radius: var(--radius-md);
  background: rgba(46, 91, 255, 0.1);
  border: 1px solid rgba(46, 91, 255, 0.2);
  color: var(--text-accent);
  font-weight: 600;
  font-size: 0.85rem;
  transition: all 0.3s ease;
}

.deal-add:hover {
  background: var(--accent-blue);
  color: white;
  border-color: var(--accent-blue);
}

/* Clearance */
.clearance-row {
  display: grid;
  grid-template-columns: repeat(5, 1fr);
  gap: 1rem;
}

.clearance-card {
  border-radius: var(--radius-lg);
  overflow: hidden;
  transition: all 0.3s var(--ease-out);
  cursor: pointer;
}

.clearance-card:hover {
  transform: translateY(-4px);
}

.clearance-card img {
  width: 100%;
  height: 120px;
  object-fit: cover;
}

.cl-info {
  padding: 0.75rem;
  display: flex;
  flex-direction: column;
  gap: 0.2rem;
}

.cl-name {
  font-size: 0.8rem;
  font-weight: 600;
  color: var(--text-primary);
}

.cl-price {
  font-family: var(--font-headline);
  font-weight: 800;
  font-size: 0.9rem;
  color: var(--text-accent);
}

@media (max-width: 1024px) {
  .deal-grid { grid-template-columns: repeat(2, 1fr); }
  .clearance-row { grid-template-columns: repeat(3, 1fr); }
}

@media (max-width: 640px) {
  .deal-grid { grid-template-columns: 1fr; }
  .clearance-row { grid-template-columns: repeat(2, 1fr); }
}
</style>
