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
        class="product-card glass-card"
      >
        <div class="product-img">
          <img :src="product.imageUrl" :alt="product.name" loading="lazy" />
          <span class="product-badge">{{ product.category }}</span>
        </div>
        <div class="product-body">
          <h3 class="product-name">{{ product.name }}</h3>
          <p class="product-desc">{{ (product.description || '').substring(0, 60) }}</p>
          <div class="product-footer">
            <span class="product-price">₹{{ product.price.toLocaleString('en-IN') }}</span>
            <button
              class="add-btn"
              :class="{ added: addedId === product.id }"
              @click="addToCart(product)"
              :disabled="product.inventoryCount === 0"
            >
              {{ product.inventoryCount === 0 ? 'Sold Out' : (addedId === product.id ? '✓ Added' : 'Add') }}
            </button>
          </div>
        </div>
      </article>
    </div>

    <!-- Load More -->
    <div v-if="visibleProducts.length < totalProducts && !loading" class="load-more">
      <button @click="loadMore" class="load-more-btn">Load More Products</button>
    </div>
    <div v-if="loading && visibleProducts.length > 0" class="load-more">
      <div class="spinner small"></div>
    </div>
  </section>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import { useCartStore } from '../store/cart';

const cart = useCartStore();
const products = ref([]);
const loading = ref(true);
const page = ref(1);
const pageSize = 20;
const totalProducts = ref(0);
const addedId = ref(null);

const categories = ['All', 'electronics', 'fashion', 'home', 'beauty', 'toys', 'sports', 'books', 'groceries', 'gaming', 'automotive'];
const activeCategory = ref('All');

const visibleProducts = computed(() => products.value);

const setCategory = (cat) => {
  activeCategory.value = cat;
  fetchProducts(true);
};

const fetchProducts = async (reset = false) => {
  if (reset) { page.value = 1; }
  loading.value = true;
  try {
    const catQuery = activeCategory.value === 'All' ? '' : `&category=${activeCategory.value}`;
    const res = await fetch(`http://localhost:5000/api/products?page=${page.value}&limit=${pageSize}${catQuery}`);
    if (!res.ok) throw new Error('API error');
    const data = await res.json();
    if (reset) {
      products.value = data.products;
    } else {
      products.value = [...products.value, ...data.products];
    }
    totalProducts.value = data.total;
  } catch {
    if (reset) {
      products.value = [
        { id: 1, name: 'Sample Product', category: 'electronics', description: 'Start your backend to see 10,000+ items', price: 999, inventoryCount: 10, imageUrl: 'https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=400&h=300&fit=crop' },
      ];
      totalProducts.value = 1;
    }
  } finally {
    loading.value = false;
  }
};

const addToCart = (product) => {
  if (product.inventoryCount > 0) {
    addedId.value = product.id;
    cart.addItem({ id: product.id, name: product.name, price: product.price, imageUrl: product.imageUrl });
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
</style>
