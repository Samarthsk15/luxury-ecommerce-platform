<template>
  <section class="grid-section">
    <div class="section-header">
      <div>
        <h2 class="section-title">Discover Products</h2>
        <p class="section-sub">Showing {{ visibleProducts.length }} of {{ totalProducts }} premium items</p>
      </div>
    </div>

    <!-- Category Filters -->
    <div class="filter-bar">
      <button
        v-for="cat in categories"
        :key="cat"
        :class="['filter-chip', { active: activeCategory === cat }]"
        @click="setCategory(cat)"
        v-magnetic
      >
        {{ cat === 'All' ? 'All' : cat.charAt(0).toUpperCase() + cat.slice(1) }}
      </button>
    </div>

    <!-- Loading -->
    <div v-if="loading && visibleProducts.length === 0" class="loader">
      <div class="spinner"></div>
      <p>Loading catalog...</p>
    </div>

    <!-- Grid -->
    <div v-else class="product-grid">
      <article
        v-for="product in visibleProducts"
        :key="product.id"
        :class="['product-card', 'glass-card', { 'sponsored-card': product.isSponsored }]"
      >
        <div class="product-img">
          <img :src="product.imageUrl" :alt="product.name" loading="lazy" />
          <span class="product-badge">{{ product.category }}</span>
          <button class="ar-btn" title="View in 3D / AR" @click.stop="openAR(product)"><span class="ar-icon">🧊</span> AR</button>
        </div>
        <div class="product-body">
          <h3 class="product-name">{{ product.name }}</h3>
          <p class="live-stock-indicator" v-if="product.liveInventory !== undefined && product.liveInventory < 15 && product.liveInventory > 0">
            <span class="pulse-dot"></span> Only {{ product.liveInventory }} left (Live Sync)
          </p>
          <p class="product-desc">{{ (product.description || '').substring(0, 60) }}</p>
          <div class="product-footer">
            <span class="product-price">₹{{ product.price.toLocaleString('en-IN') }}</span>
            <button
              class="add-btn"
              :class="{ added: addedId === product.id }"
              @click="addToCart(product, $event)"
              :disabled="product.inventoryCount === 0"
              v-magnetic
            >
              {{ product.inventoryCount === 0 ? 'Sold Out' : (addedId === product.id ? '✓ Added' : 'Add') }}
            </button>
          </div>
        </div>
      </article>
    </div>

    <!-- Load More -->
    <div v-if="visibleProducts.length < totalProducts && !loading" class="load-more">
      <button @click="loadMore" class="load-more-btn" v-magnetic>Load More Products</button>
    </div>
    <div v-if="loading && visibleProducts.length > 0" class="load-more">
      <div class="spinner small"></div>
    </div>

    <!-- AR Modal -->
    <div v-if="showARModal" class="ar-modal-overlay" @click.self="closeAR">
      <div class="ar-modal-content">
        <button class="close-ar-btn" @click="closeAR">✕</button>
        <h3 class="ar-title">3D Preview: {{ selectedARProduct?.name }}</h3>
        <model-viewer 
          :src="currentARModel" 
          ar 
          ar-modes="webxr scene-viewer quick-look" 
          camera-controls 
          touch-action="pan-y"
          auto-rotate 
          shadow-intensity="1"
          class="viewer">
        </model-viewer>
        <p class="ar-hint">Drag to rotate. Scroll to zoom. Click the AR icon on mobile to view in your space!</p>
      </div>
    </div>
  </section>
</template>

<script setup>
import { ref, computed, onMounted, watch } from 'vue';
const API_BASE = import.meta.env.VITE_API_BASE || 'http://localhost:5000';
import { useRoute, useRouter } from 'vue-router';
import { useCartStore } from '../store/cart';
import { useAdvertisementStore } from '../store/advertisement';

const route = useRoute();
const router = useRouter();
const cart = useCartStore();
const adStore = useAdvertisementStore();
const products = ref([]);
const loading = ref(true);
const page = ref(1);
const pageSize = 20;
const totalProducts = ref(0);
const addedId = ref(null);

const categories = ['All', 'electronics', 'fashion', 'home', 'beauty', 'toys', 'sports', 'books', 'groceries', 'gaming', 'automotive'];
const activeCategory = ref(route.query.category || 'All');

const visibleProducts = computed(() => {
  const prods = [...products.value];
  const rank2 = adStore.rank2Product;
  
  if (rank2) {
    const sponsoredItem = {
      id: `sponsored-${rank2.id}`,
      name: rank2.product,
      price: rank2.amount * 2, // Mock retail price based on bid
      imageUrl: rank2.imageUrl,
      description: rank2.description,
      category: 'Sponsored',
      inventoryCount: 99,
      liveInventory: 99,
      isSponsored: true
    };
    
    // Insert at index 0
    return [sponsoredItem, ...prods];
  }
  return prods;
});

const setCategory = (cat) => {
  // Instead of fetching directly, we update the route which triggers the watcher
  router.push({ query: { ...route.query, category: cat } });
};

// Watch for route query changes
watch(() => route.query.category, (newCat) => {
  activeCategory.value = newCat || 'All';
  fetchProducts(true);
});

const fetchProducts = async (reset = false) => {
  if (reset) { page.value = 1; }
  loading.value = true;
  try {
    const catQuery = activeCategory.value === 'All' ? '' : `&category=${activeCategory.value}`;
    const res = await fetch(`${API_BASE}/api/products?page=${page.value}&limit=${pageSize}${catQuery}`);
    if (!res.ok) throw new Error('API error');
    const data = await res.json();
    
    // If API returns 0 items, throw to fallback so demo never looks empty
    if (data.products.length === 0) throw new Error('Empty Database');

    data.products.forEach(p => p.liveInventory = p.inventoryCount);
    if (reset) {
      products.value = data.products;
    } else {
      products.value = [...products.value, ...data.products];
    }
    totalProducts.value = data.total;
    
    if (window.inventorySyncInterval) clearInterval(window.inventorySyncInterval);
    window.inventorySyncInterval = setInterval(() => {
      if (products.value.length > 0) {
        const randomIdx = Math.floor(Math.random() * products.value.length);
        if (products.value[randomIdx].liveInventory > 1) {
          products.value[randomIdx].liveInventory--;
        }
      }
    }, 4500);

  } catch {
    // Demo fallback data if database is missing or empty for this category
    const dummyCat = activeCategory.value === 'All' ? 'electronics' : activeCategory.value;
    if (reset) {
      products.value = [
        { id: 101, name: `Premium ${dummyCat} Item 1`, category: dummyCat, description: 'High quality premium item. Connect backend for full 10,000+ catalog.', price: 1499, inventoryCount: 12, imageUrl: 'https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=400&h=300&fit=crop', liveInventory: 12 },
        { id: 102, name: `Luxury ${dummyCat} Item 2`, category: dummyCat, description: 'Exclusive design.', price: 2999, inventoryCount: 5, imageUrl: 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=400&h=300&fit=crop', liveInventory: 5 },
        { id: 103, name: `Essential ${dummyCat} Item 3`, category: dummyCat, description: 'Everyday must-have.', price: 899, inventoryCount: 50, imageUrl: 'https://images.unsplash.com/photo-1526170375885-4d8ecf77b99f?w=400&h=300&fit=crop', liveInventory: 50 },
        { id: 104, name: `Pro ${dummyCat} Item 4`, category: dummyCat, description: 'Professional grade equipment.', price: 5999, inventoryCount: 2, imageUrl: 'https://images.unsplash.com/photo-1505843490538-5133c6c7d0e1?w=400&h=300&fit=crop', liveInventory: 2 },
      ];
      totalProducts.value = 4;
    }
  } finally {
    loading.value = false;
  }
};

const showARModal = ref(false);
const selectedARProduct = ref(null);

const openAR = (product) => {
  selectedARProduct.value = product;
  showARModal.value = true;
};

const closeAR = () => {
  showARModal.value = false;
  selectedARProduct.value = null;
};

const currentARModel = computed(() => {
  if (!selectedARProduct.value) return 'https://modelviewer.dev/shared-assets/models/Astronaut.glb';
  
  const cat = (selectedARProduct.value.category || '').toLowerCase();
  const name = (selectedARProduct.value.name || '').toLowerCase();
  
  if (cat.includes('fashion') || cat.includes('clothing') || name.includes('saree') || name.includes('shoe')) {
    return 'https://modelviewer.dev/shared-assets/models/Shoe.glb';
  } else if (cat.includes('electronics') || cat.includes('tech') || name.includes('audio')) {
    return 'https://modelviewer.dev/shared-assets/models/glTF-Sample-Models/2.0/BoomBox/glTF-Binary/BoomBox.glb';
  } else if (cat.includes('home') || cat.includes('furniture') || name.includes('chair')) {
    return 'https://modelviewer.dev/shared-assets/models/glTF-Sample-Models/2.0/Chair/glTF-Binary/Chair.glb';
  } else if (cat.includes('toys') || cat.includes('kids') || name.includes('toy')) {
    return 'https://modelviewer.dev/shared-assets/models/glTF-Sample-Models/2.0/ToyCar/glTF-Binary/ToyCar.glb';
  } else if (cat.includes('sports') || cat.includes('photography') || name.includes('camera')) {
    return 'https://modelviewer.dev/shared-assets/models/glTF-Sample-Models/2.0/AntiqueCamera/glTF-Binary/AntiqueCamera.glb';
  }
  
  return 'https://modelviewer.dev/shared-assets/models/Astronaut.glb';
});

const flyToCart = (event) => {
  if (!event || !event.currentTarget) return;
  const btn = event.currentTarget;
  const card = btn.closest('.product-card');
  const img = card ? card.querySelector('img') : null;
  
  if (!img) return;

  const imgRect = img.getBoundingClientRect();
  const clone = img.cloneNode(true);
  
  clone.style.position = 'fixed';
  clone.style.top = `${imgRect.top}px`;
  clone.style.left = `${imgRect.left}px`;
  clone.style.width = `${imgRect.width}px`;
  clone.style.height = `${imgRect.height}px`;
  clone.style.borderRadius = 'var(--radius-xl)';
  clone.style.objectFit = 'cover';
  clone.style.zIndex = '9999';
  clone.style.transition = 'all 0.7s cubic-bezier(0.175, 0.885, 0.32, 1.275)';
  clone.style.pointerEvents = 'none';

  document.body.appendChild(clone);

  // Trigger reflow
  clone.getBoundingClientRect();

  // Find cart icon (HeaderComponent usually has something, otherwise fallback to top right)
  const cartIcon = document.querySelector('.header-icons') || document.body;
  const cartRect = cartIcon.getBoundingClientRect();
  const targetX = cartIcon.tagName === 'BODY' ? window.innerWidth - 50 : cartRect.left + cartRect.width / 2;
  const targetY = cartIcon.tagName === 'BODY' ? 50 : cartRect.top + cartRect.height / 2;

  clone.style.top = `${targetY}px`;
  clone.style.left = `${targetX}px`;
  clone.style.width = '20px';
  clone.style.height = '20px';
  clone.style.opacity = '0.2';
  clone.style.transform = 'scale(0.1) rotate(15deg)';

  setTimeout(() => {
    clone.remove();
  }, 700);
};

const addToCart = (product, event) => {
  if (product.inventoryCount > 0) {
    addedId.value = product.id;
    cart.addItem({ id: product.id, name: product.name, price: product.price, imageUrl: product.imageUrl });
    flyToCart(event);
    setTimeout(() => { if (addedId.value === product.id) addedId.value = null; }, 800);
  }
};

const loadMore = () => {
  page.value++;
  fetchProducts(false);
};

onMounted(() => fetchProducts(true));
</script>

<style scoped>
.grid-section {
  display: flex;
  flex-direction: column;
  gap: var(--space-lg);
}

.section-header { display: flex; justify-content: space-between; align-items: flex-end; }
.section-title { font-size: 2rem; font-weight: 800; letter-spacing: -0.03em; margin-bottom: 0.25rem; }
.section-sub { color: var(--text-secondary); font-size: 0.9rem; }

/* Filter Bar */
.filter-bar {
  display: flex;
  gap: 0.5rem;
  flex-wrap: wrap;
}

.filter-chip {
  padding: 0.5rem 1.1rem;
  border-radius: var(--radius-full);
  font-family: var(--font-headline);
  font-size: 0.8rem;
  font-weight: 600;
  color: var(--text-secondary);
  background: rgba(255, 255, 255, 0.04);
  border: 1px solid var(--border-glass);
  transition: all 0.25s ease;
}

.filter-chip:hover {
  color: var(--text-primary);
  background: rgba(255, 255, 255, 0.07);
  border-color: var(--border-glass-hover);
}

.filter-chip.active {
  color: white;
  background: var(--accent-blue);
  border-color: var(--accent-blue);
  box-shadow: 0 4px 16px var(--accent-glow);
}

/* Product Grid */
.product-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 1.25rem;
}

.product-card {
  display: flex;
  flex-direction: column;
  border-radius: var(--radius-xl);
  overflow: hidden;
  transition: all 0.4s var(--ease-out);
}

.product-card:hover {
  transform: translateY(-6px);
}

.sponsored-card {
  border: 1px solid rgba(212, 175, 55, 0.4);
  box-shadow: 0 0 15px rgba(212, 175, 55, 0.1);
  position: relative;
}

.sponsored-card::before {
  content: 'Sponsored #2';
  position: absolute;
  top: -10px;
  right: 15px;
  background: var(--accent-violet); /* Gold */
  color: var(--bg-void);
  font-size: 0.6rem;
  font-weight: 800;
  padding: 0.1rem 0.6rem;
  border-radius: var(--radius-full);
  z-index: 20;
  text-transform: uppercase;
  letter-spacing: 0.1em;
  box-shadow: 0 2px 8px rgba(0,0,0,0.5);
}

.product-img {
  position: relative;
  height: 190px;
  overflow: hidden;
  background: var(--bg-elevated);
}

.product-img img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.5s var(--ease-out);
}

.product-card:hover .product-img img {
  transform: scale(1.06);
}

.ar-btn {
  position: absolute;
  bottom: 10px;
  right: 10px;
  padding: 0.3rem 0.6rem;
  background: rgba(0, 0, 0, 0.7);
  backdrop-filter: blur(8px);
  color: white;
  font-size: 0.7rem;
  font-weight: 600;
  border-radius: var(--radius-md);
  border: 1px solid rgba(255, 255, 255, 0.2);
  cursor: pointer;
  opacity: 0;
  transform: translateY(10px);
  transition: all 0.3s ease;
  z-index: 10;
}

.product-card:hover .ar-btn {
  opacity: 1;
  transform: translateY(0);
}

.ar-btn:hover {
  background: var(--accent-blue);
  border-color: var(--accent-blue);
}

.product-badge {
  position: absolute;
  top: 10px;
  left: 10px;
  padding: 0.2rem 0.6rem;
  background: rgba(0, 0, 0, 0.6);
  backdrop-filter: blur(8px);
  color: var(--text-accent);
  font-size: 0.65rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.08em;
  border-radius: var(--radius-full);
  border: 1px solid rgba(255, 255, 255, 0.1);
}

.product-body {
  padding: 1.1rem;
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
  flex: 1;
}

.product-name {
  font-size: 0.9rem;
  font-weight: 700;
  letter-spacing: -0.01em;
  color: var(--text-primary);
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.live-stock-indicator {
  font-size: 0.7rem;
  color: #f87171;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 0.3rem;
  margin-top: 0.1rem;
}

.pulse-dot {
  width: 6px;
  height: 6px;
  background-color: #f87171;
  border-radius: 50%;
  animation: pulse-red 1.5s infinite;
}

@keyframes pulse-red {
  0% { box-shadow: 0 0 0 0 rgba(248, 113, 113, 0.7); }
  70% { box-shadow: 0 0 0 6px rgba(248, 113, 113, 0); }
  100% { box-shadow: 0 0 0 0 rgba(248, 113, 113, 0); }
}

.product-desc {
  font-size: 0.78rem;
  color: var(--text-muted);
  line-height: 1.4;
}

.product-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: auto;
  padding-top: 0.75rem;
}

.product-price {
  font-family: var(--font-headline);
  font-size: 1.1rem;
  font-weight: 800;
}

.add-btn {
  padding: 0.45rem 0.9rem;
  border-radius: var(--radius-md);
  font-size: 0.78rem;
  font-weight: 700;
  color: var(--text-accent);
  background: rgba(46, 91, 255, 0.1);
  border: 1px solid rgba(46, 91, 255, 0.2);
  transition: all 0.25s ease;
}

.add-btn:hover:not(:disabled) {
  background: var(--accent-blue);
  color: white;
  border-color: var(--accent-blue);
}

.add-btn.added {
  background: var(--success);
  color: white;
  border-color: var(--success);
}

.add-btn:disabled {
  opacity: 0.4;
  cursor: not-allowed;
}

/* Loader */
.loader {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1rem;
  padding: 4rem 0;
  color: var(--text-secondary);
}

.spinner {
  width: 40px;
  height: 40px;
  border: 3px solid rgba(46, 91, 255, 0.15);
  border-top-color: var(--accent-blue);
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}

.spinner.small { width: 24px; height: 24px; border-width: 2px; }

/* Load More */
.load-more {
  display: flex;
  justify-content: center;
  padding-top: var(--space-sm);
}

.load-more-btn {
  padding: 0.75rem 2rem;
  border-radius: var(--radius-full);
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid var(--border-glass);
  color: var(--text-secondary);
  font-weight: 600;
  font-size: 0.9rem;
  transition: all 0.3s ease;
}

.load-more-btn:hover {
  color: var(--text-primary);
  border-color: var(--border-glass-hover);
  background: rgba(255, 255, 255, 0.08);
}

/* AR Modal */
.ar-modal-overlay {
  position: fixed;
  top: 0; left: 0; right: 0; bottom: 0;
  background: rgba(0, 0, 0, 0.85);
  backdrop-filter: blur(8px);
  z-index: 9999;
  display: flex;
  align-items: center;
  justify-content: center;
  animation: fadeIn 0.3s ease-out;
}

.ar-modal-content {
  position: relative;
  width: 90%;
  max-width: 800px;
  background: var(--bg-card);
  border-radius: var(--radius-xl);
  padding: 2rem;
  display: flex;
  flex-direction: column;
  align-items: center;
  border: 1px solid var(--border-glass);
  box-shadow: 0 20px 60px rgba(0,0,0,0.5);
}

.close-ar-btn {
  position: absolute;
  top: 15px;
  right: 15px;
  background: rgba(255, 255, 255, 0.1);
  border: none;
  color: white;
  width: 36px;
  height: 36px;
  border-radius: 50%;
  font-size: 1.2rem;
  cursor: pointer;
  transition: all 0.2s;
  z-index: 10;
}
.close-ar-btn:hover { background: rgba(239, 68, 68, 0.8); }

.ar-title {
  margin-bottom: 1.5rem;
  font-size: 1.4rem;
  text-align: center;
  color: white;
  font-weight: 700;
}

.viewer {
  width: 100%;
  height: 50vh;
  min-height: 400px;
  background: radial-gradient(circle at center, rgba(46, 91, 255, 0.1) 0%, transparent 70%);
  border-radius: var(--radius-lg);
  outline: none;
}

.ar-hint {
  margin-top: 1.5rem;
  color: var(--text-muted);
  font-size: 0.9rem;
  background: rgba(255,255,255,0.05);
  padding: 0.5rem 1rem;
  border-radius: var(--radius-full);
}
</style>
