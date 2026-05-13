<template>
  <section class="hero">
    <!-- Ambient glow blobs -->
    <div class="hero-glow glow-1"></div>
    <div class="hero-glow glow-2"></div>

    <div class="hero-content">
      <!-- Centered Hero Text -->
      <div class="hero-text-center">
        <h1 class="hero-title animate-in" style="--d:0">
          Explore, Shop, Repeat.
        </h1>
        <p class="hero-subtitle animate-in" style="--d:1">
          OnlineHero is the driving force behind the dreams of emerging<br />
          entrepreneurs and a trusted partner for industry leaders.
        </p>
        <div class="hero-cta animate-in" style="--d:2">
          <button class="btn-primary" @click="$router.push('/products')">
            <span>Start free trial</span>
            <span class="btn-arrow">→</span>
          </button>
        </div>
      </div>

      <!-- Bento Box Grid -->
      <div class="bento-grid">
        <!-- Top Left: Product Table -->
        <div class="bento-card glass-card top-left animate-in" style="--d:3" @mousemove="tiltCard" @mouseleave="resetTilt">
          <div class="table-header">
            <span>Product</span>
            <span>Price</span>
            <span>Inventory</span>
            <span>Status</span>
          </div>
          <div class="table-row" v-for="item in tableProducts" :key="item.id">
            <div class="product-info">
              <div class="icon-box shoe-bg">{{ getCategoryIcon(item.category) }}</div>
              <span class="truncate-name">{{ item.name.split(' ')[0] }} {{ item.name.split(' ')[1] || '' }}</span>
            </div>
            <span>₹{{ item.price.toLocaleString('en-IN') }}</span>
            <span>{{ item.inventoryCount }} in stock</span>
            <span class="status-badge" :class="{'out-of-stock': item.inventoryCount < 10}">Active</span>
          </div>
          <div v-if="tableProducts.length === 0" class="loading-state">Syncing live data...</div>
        </div>

        <!-- Top Center: Nike Image -->
        <div class="bento-card glass-card top-center image-card animate-in" style="--d:4" @click="$router.push('/products')" @mousemove="tiltCard" @mouseleave="resetTilt">
          <img src="https://images.unsplash.com/photo-1542291026-7eec264c27ff?auto=format&fit=crop&q=80&w=600" alt="Nike Zoom Overpower" />
          <div class="card-overlay-bottom">
            <span class="item-name">Nike Zoom Overpower</span>
            <span class="item-price">₹3,599</span>
          </div>
          <div class="paid-badge">✓ Top Seller</div>
        </div>

        <!-- Top Right: Stat -->
        <div class="bento-card glass-card top-right dark-accent animate-in" style="--d:5" @mousemove="tiltCard" @mouseleave="resetTilt">
          <h2 class="huge-stat">{{ liveCustomers }}<span style="font-size: 2rem;">+</span></h2>
          <p class="stat-desc">Live active shoppers</p>
        </div>

        <!-- Middle: Logo Bar -->
        <div class="bento-card glass-card middle-bar animate-in" style="--d:6" @mousemove="tiltCard" @mouseleave="resetTilt">
          <div class="logo-group">
            <div class="circle-icon">🛍️</div>
            <div class="circle-icon">A</div>
            <div class="circle-icon">e</div>
            <div class="circle-icon">W</div>
          </div>
          <p class="collaborate-text">Collaborating with numerous e-commerce platforms.</p>
        </div>

        <!-- Bottom Left: Laptop Image -->
        <div class="bento-card glass-card bottom-left image-card animate-in" style="--d:7" @mousemove="tiltCard" @mouseleave="resetTilt">
          <img src="https://images.unsplash.com/photo-1522071820081-009f0129c71c?auto=format&fit=crop&q=80&w=600" alt="Working" />
        </div>

        <!-- Bottom Center: Sales Chart -->
        <div class="bento-card glass-card bottom-center animate-in" style="--d:8" @mousemove="tiltCard" @mouseleave="resetTilt">
          <div class="chart-header">
            <div class="sales-info">
              <span class="sales-label">Live Platform Sales Volume</span>
              <span class="sales-amount">₹{{ liveSales.toLocaleString('en-IN') }}</span>
              <span class="sales-growth" :class="{'negative': liveGrowth < 0}">{{ liveGrowth > 0 ? '↗' : '↘' }} {{ Math.abs(liveGrowth).toFixed(1) }}% this minute</span>
            </div>
            <button class="btn-ghost-small">View report</button>
          </div>
          <div class="chart-body">
            <div class="y-axis">
              <span>60k</span>
              <span>40k</span>
              <span>20k</span>
            </div>
            <div class="chart-bars">
              <div class="bar-bg"></div>
              <div class="bar-fill" :style="{ height: `${salesBarHeight}%`, transition: 'height 0.5s ease' }"></div>
              <div class="avg-line"><span>Avg {{ Math.round(salesBarHeight) }}%</span></div>
            </div>
          </div>
        </div>

        <!-- Bottom Right: Interactive 3D Model -->
        <div class="bento-card glass-card bottom-right animate-in" style="--d:9; padding:0; overflow:hidden;" @mousemove="tiltCard" @mouseleave="resetTilt">
          <div style="position: absolute; top: 1rem; left: 1.5rem; z-index: 10;">
            <span style="font-weight: 800; font-size: 1.1rem; letter-spacing: -0.02em; color: white;">Interactive 3D</span>
            <p style="font-size: 0.75rem; color: var(--text-muted);">Drag to rotate</p>
          </div>
          <model-viewer 
            src="https://modelviewer.dev/shared-assets/models/Shoe.glb" 
            auto-rotate 
            camera-controls 
            rotation-per-second="30deg"
            shadow-intensity="1"
            interaction-prompt="none"
            style="width: 100%; height: 100%; background: radial-gradient(circle at center, rgba(139,92,246,0.15) 0%, transparent 70%);">
          </model-viewer>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue';

const tableProducts = ref([]);
const featuredProduct = ref(null);

const liveCustomers = ref(142);
const liveSales = ref(18499);
const liveGrowth = ref(12.4);
const salesBarHeight = ref(60);

let syncInterval;

const getCategoryIcon = (category) => {
  const cat = (category || '').toLowerCase();
  if (cat.includes('electronics')) return '💻';
  if (cat.includes('fashion') || cat.includes('clothing')) return '👕';
  if (cat.includes('home')) return '🛋️';
  if (cat.includes('sports')) return '⚽';
  return '📦';
};

const fetchLiveData = async () => {
  try {
    const res = await fetch('http://localhost:5000/api/products?limit=5');
    if (!res.ok) throw new Error('Failed to fetch');
    const data = await res.json();
    
    // Set featured product (e.g., the 4th item)
    if (data.products && data.products.length > 3) {
      featuredProduct.value = data.products[3];
      tableProducts.value = data.products.slice(0, 3);
    } else {
      tableProducts.value = data.products || [];
      featuredProduct.value = data.products?.[0] || null;
    }
  } catch (error) {
    console.error("Failed to fetch live dashboard data", error);
  }
};

// 3D Tilt Effect
const tiltCard = (e) => {
  const card = e.currentTarget;
  const rect = card.getBoundingClientRect();
  const x = e.clientX - rect.left;
  const y = e.clientY - rect.top;
  
  const centerX = rect.width / 2;
  const centerY = rect.height / 2;
  
  const rotateX = ((y - centerY) / centerY) * -8; // Max 8 deg
  const rotateY = ((x - centerX) / centerX) * 8;
  
  card.style.transition = 'transform 0.1s ease-out';
  card.style.transform = `perspective(1000px) rotateX(${rotateX}deg) rotateY(${rotateY}deg) scale3d(1.02, 1.02, 1.02)`;
  card.style.zIndex = 10;
};

const resetTilt = (e) => {
  const card = e.currentTarget;
  card.style.transition = 'transform 0.5s ease';
  card.style.transform = `perspective(1000px) rotateX(0) rotateY(0) scale3d(1, 1, 1)`;
  card.style.zIndex = 1;
};

onMounted(() => {
  // Fetch actual data from backend
  fetchLiveData();
  
  // Simulate live WebSocket data for dashboard metrics
  syncInterval = setInterval(() => {
    // Fluctuate active shoppers
    const change = Math.floor(Math.random() * 5) - 2; // -2 to +2
    liveCustomers.value = Math.max(10, liveCustomers.value + change);
    
    // Simulate incoming sales
    if (Math.random() > 0.6) {
      const newSale = Math.floor(Math.random() * 5000) + 500;
      liveSales.value += newSale;
      
      // Update growth & bar visually
      liveGrowth.value = parseFloat((liveGrowth.value + (Math.random() * 2)).toFixed(1));
      salesBarHeight.value = Math.min(100, salesBarHeight.value + (Math.random() * 5));
    } else {
      // Natural decay in growth to look realistic
      liveGrowth.value = parseFloat((liveGrowth.value - (Math.random() * 1)).toFixed(1));
      if (salesBarHeight.value > 20) {
        salesBarHeight.value -= Math.random() * 2;
      }
    }
  }, 3500);
});

onUnmounted(() => {
  if (syncInterval) clearInterval(syncInterval);
});
</script>

<style scoped>
.hero {
  position: relative;
  padding: var(--space-xl) 0;
  overflow: hidden;
  min-height: 100vh;
}

/* Ambient Glows */
.hero-glow {
  position: absolute;
  border-radius: 50%;
  filter: blur(120px);
  pointer-events: none;
}

.glow-1 {
  width: 600px;
  height: 600px;
  background: rgba(46, 91, 255, 0.15);
  top: -100px;
  left: 50%;
  transform: translateX(-50%);
  animation: float 8s ease-in-out infinite;
}

.glow-2 {
  width: 500px;
  height: 500px;
  background: rgba(139, 92, 246, 0.1);
  bottom: 100px;
  right: 10%;
  animation: float 10s ease-in-out infinite reverse;
}

.hero-content {
  position: relative;
  z-index: 2;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: var(--space-xl);
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 1rem;
}

/* Centered Text */
.hero-text-center {
  text-align: center;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: var(--space-md);
  margin-top: 2rem;
}

.hero-title {
  font-size: clamp(3.5rem, 6vw, 5.5rem);
  font-weight: 800;
  letter-spacing: -0.04em;
  line-height: 1.1;
  color: var(--text-primary);
}

.hero-subtitle {
  font-size: 1.1rem;
  color: var(--text-secondary);
  line-height: 1.6;
}

.hero-cta {
  margin-top: 1rem;
}

.btn-primary {
  display: inline-flex;
  align-items: center;
  gap: 0.75rem;
  padding: 1rem 2rem;
  background: var(--text-primary);
  color: var(--bg-void);
  border-radius: var(--radius-full);
  font-weight: 700;
  font-size: 1rem;
  transition: all 0.3s ease;
  box-shadow: 0 8px 30px rgba(255, 255, 255, 0.1);
}

.btn-primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 12px 40px rgba(255, 255, 255, 0.2);
  background: white;
}

/* Bento Grid */
.bento-grid {
  display: grid;
  grid-template-columns: 1fr 1fr 1fr;
  grid-template-rows: auto auto auto;
  gap: 1.5rem;
  width: 100%;
}

.bento-card {
  padding: 1.5rem;
  overflow: hidden;
  position: relative;
  display: flex;
  flex-direction: column;
  transform-style: preserve-3d;
  will-change: transform;
}

.image-card {
  padding: 0;
}

.image-card img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.6s var(--ease-out);
}

.image-card:hover img {
  transform: scale(1.05);
}

/* Specific Card Layouts */
.top-left {
  gap: 1.2rem;
}

.table-header, .table-row {
  display: grid;
  grid-template-columns: 2fr 1fr 1fr 1fr;
  align-items: center;
  font-size: 0.85rem;
}

.table-header {
  color: var(--text-muted);
  font-weight: 600;
  margin-bottom: 0.5rem;
}

.product-info {
  display: flex;
  align-items: center;
  gap: 0.8rem;
  font-weight: 600;
}

.icon-box {
  width: 36px;
  height: 36px;
  border-radius: var(--radius-md);
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.1rem;
}

.shoe-bg { background: rgba(16, 185, 129, 0.2); }
.cloth-bg { background: rgba(59, 130, 246, 0.2); }
.bag-bg { background: rgba(245, 158, 11, 0.2); }

.status-badge {
  background: rgba(16, 185, 129, 0.2);
  color: #10b981;
  padding: 0.2rem 0.6rem;
  border-radius: var(--radius-full);
  font-size: 0.75rem;
  justify-self: start;
}
.status-badge.out-of-stock {
  background: rgba(245, 158, 11, 0.2);
  color: #f59e0b;
}

.truncate-name {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  max-width: 100px;
}

.top-center {
  height: 300px;
}

.card-overlay-bottom {
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
  padding: 2rem 1.5rem 1.5rem;
  background: linear-gradient(to top, rgba(0,0,0,0.8), transparent);
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.3rem;
}

.item-name {
  color: rgba(255,255,255,0.8);
  font-size: 0.9rem;
}

.item-price {
  color: white;
  font-size: 1.5rem;
  font-weight: 800;
  font-family: var(--font-headline);
}

.paid-badge {
  position: absolute;
  top: 1rem;
  right: 1rem;
  background: rgba(0,0,0,0.5);
  backdrop-filter: blur(8px);
  padding: 0.3rem 0.8rem;
  border-radius: var(--radius-full);
  font-size: 0.75rem;
  font-weight: 600;
}

.top-right {
  justify-content: center;
  align-items: center;
  text-align: center;
  background: rgba(0, 0, 0, 0.4);
  border-color: rgba(255,255,255,0.1);
}

.huge-stat {
  font-size: 4.5rem;
  font-weight: 800;
  line-height: 1;
  background: linear-gradient(135deg, #fff, #aaa);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

.stat-desc {
  color: var(--text-muted);
  font-size: 0.95rem;
  margin-top: 0.5rem;
}

.middle-bar {
  grid-column: 1 / -1;
  flex-direction: row;
  align-items: center;
  gap: 1.5rem;
  padding: 1rem 2rem;
  background: rgba(0, 0, 0, 0.4);
  border-radius: var(--radius-full);
}

.logo-group {
  display: flex;
}

.circle-icon {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background: var(--surface-high);
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: bold;
  border: 2px solid var(--bg-void);
  margin-left: -10px;
}
.circle-icon:first-child { margin-left: 0; }

.collaborate-text {
  color: var(--text-secondary);
  font-size: 0.95rem;
}

.bottom-left, .bottom-right {
  height: 250px;
}

.bottom-center {
  gap: 1.5rem;
}

.chart-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
}

.sales-info {
  display: flex;
  flex-direction: column;
  gap: 0.2rem;
}

.sales-label {
  color: var(--text-muted);
  font-size: 0.85rem;
}

.sales-amount {
  font-size: 1.8rem;
  font-weight: 800;
  font-family: var(--font-headline);
}

.sales-growth {
  font-size: 0.75rem;
  color: var(--success);
  background: rgba(16, 185, 129, 0.1);
  padding: 0.2rem 0.6rem;
  border-radius: var(--radius-full);
  width: fit-content;
  transition: all 0.3s;
}
.sales-growth.negative {
  color: #ef4444;
  background: rgba(239, 68, 68, 0.1);
}

.btn-ghost-small {
  background: transparent;
  border: 1px solid var(--border-glass);
  color: var(--text-primary);
  padding: 0.4rem 0.8rem;
  border-radius: var(--radius-full);
  font-size: 0.75rem;
  transition: all 0.2s;
}
.btn-ghost-small:hover {
  background: var(--bg-glass);
}

.chart-body {
  display: flex;
  gap: 1rem;
  height: 120px;
  align-items: flex-end;
  position: relative;
}

.y-axis {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  height: 100%;
  color: var(--text-muted);
  font-size: 0.75rem;
}

.chart-bars {
  flex-grow: 1;
  position: relative;
  height: 100%;
  display: flex;
  align-items: flex-end;
  justify-content: center;
}

.bar-bg {
  position: absolute;
  bottom: 0;
  width: 40px;
  height: 100%;
  background: rgba(255, 255, 255, 0.05);
  border-radius: var(--radius-md) var(--radius-md) 0 0;
}

.bar-fill {
  width: 40px;
  background: var(--text-primary);
  border-radius: var(--radius-md) var(--radius-md) 0 0;
  z-index: 1;
}

.avg-line {
  position: absolute;
  top: 50%;
  left: 0;
  right: 0;
  border-top: 1px dashed var(--text-muted);
  z-index: 2;
}

.avg-line span {
  position: absolute;
  left: 50%;
  transform: translate(-50%, -50%);
  background: var(--bg-elevated);
  padding: 0.2rem 0.6rem;
  border-radius: var(--radius-full);
  font-size: 0.7rem;
  color: var(--text-primary);
}

/* Staggered animation */
.animate-in {
  opacity: 0;
  transform: translateY(25px);
  animation: fadeInUp 0.7s var(--ease-out) forwards;
  animation-delay: calc(var(--d, 0) * 0.1s);
}

@media (max-width: 1024px) {
  .bento-grid {
    grid-template-columns: 1fr 1fr;
  }
  .middle-bar { grid-column: 1 / -1; }
  .bottom-center { grid-column: 1 / -1; }
}

@media (max-width: 768px) {
  .hero-title { font-size: 3rem; }
  .bento-grid {
    grid-template-columns: 1fr;
  }
  .table-header, .table-row {
    grid-template-columns: 1.5fr 1fr 1fr;
  }
  .table-header span:last-child, .table-row span:last-child {
    display: none;
  }
}
</style>
