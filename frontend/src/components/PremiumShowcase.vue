<template>
  <section class="premium-showcase">
    <div class="showcase-header" v-scroll-reveal>
      <h2 class="section-title">The <span class="italic-gold">Collection</span></h2>
      <button class="btn-text" @click="$router.push('/products')">View Full Catalog <span class="arrow">→</span></button>
    </div>

    <div class="horizontal-track-container" ref="trackContainer">
      <div class="horizontal-track">
        <div 
          class="showcase-card glass-card" 
          v-for="(item, index) in showcaseItems" 
          :key="index"
          v-scroll-reveal
        >
          <div class="card-image">
            <img :src="item.image" :alt="item.name" />
            <div class="overlay-gradient"></div>
          </div>
          <div class="card-content">
            <div class="item-meta">
              <span class="item-category">{{ item.category }}</span>
              <span class="item-price">₹{{ item.price.toLocaleString('en-IN') }}</span>
            </div>
            <h3 class="item-name">{{ item.name }}</h3>
            <button class="btn-icon">
              <span class="plus-icon">+</span>
            </button>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup>
import { ref } from 'vue';

const showcaseItems = ref([
  {
    name: "Obsidian Chronograph",
    category: "Timepieces",
    price: 145000,
    image: "https://images.unsplash.com/photo-1523170335258-f5ed11844a49?auto=format&fit=crop&q=80&w=600"
  },
  {
    name: "Aura Silk Scarf",
    category: "Accessories",
    price: 12500,
    image: "https://images.unsplash.com/photo-1584916201218-f4242ceb4809?auto=format&fit=crop&q=80&w=600"
  },
  {
    name: "Midnight Leather Tote",
    category: "Leather Goods",
    price: 48000,
    image: "https://images.unsplash.com/photo-1590874103328-eac38a683ce7?auto=format&fit=crop&q=80&w=600"
  },
  {
    name: "Gold-Plated Aviators",
    category: "Eyewear",
    price: 22000,
    image: "https://images.unsplash.com/photo-1511499767150-a48a237f0083?auto=format&fit=crop&q=80&w=600"
  }
]);
</script>

<style scoped>
.premium-showcase {
  padding: var(--space-xl) 0;
  background: var(--bg-void);
  overflow: hidden;
}

.showcase-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
  padding: 0 var(--space-md);
  max-width: var(--max-width);
  margin: 0 auto var(--space-lg);
}

.section-title {
  font-size: clamp(2.5rem, 4vw, 3.5rem);
  font-weight: 300;
  text-transform: uppercase;
  letter-spacing: -0.02em;
}

.italic-gold {
  font-style: italic;
  color: var(--accent-violet);
}

.btn-text {
  background: transparent;
  color: var(--text-secondary);
  text-transform: uppercase;
  letter-spacing: 0.1em;
  font-size: 0.8rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  transition: color 0.3s;
}

.btn-text:hover {
  color: var(--text-primary);
}

.btn-text .arrow {
  transition: transform 0.3s;
}

.btn-text:hover .arrow {
  transform: translateX(5px);
}

/* Horizontal Scrolling Track */
.horizontal-track-container {
  width: 100%;
  padding: 0 var(--space-md);
  overflow-x: auto;
  scrollbar-width: none; /* Firefox */
  -ms-overflow-style: none;  /* IE and Edge */
}

.horizontal-track-container::-webkit-scrollbar {
  display: none;
}

.horizontal-track {
  display: flex;
  gap: var(--space-md);
  padding-bottom: var(--space-md); /* Space for shadow */
  width: max-content;
}

/* Showcase Card */
.showcase-card {
  width: 350px;
  height: 500px;
  display: flex;
  flex-direction: column;
  position: relative;
  overflow: hidden;
  border-radius: 2px; /* Sharp edges for luxury */
  cursor: pointer;
}

.card-image {
  position: absolute;
  inset: 0;
  z-index: 1;
}

.card-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.8s var(--ease-out);
  filter: grayscale(20%) contrast(110%);
}

.showcase-card:hover .card-image img {
  transform: scale(1.05);
  filter: grayscale(0%) contrast(110%);
}

.overlay-gradient {
  position: absolute;
  inset: 0;
  background: linear-gradient(to top, rgba(5,5,5,0.9) 0%, rgba(5,5,5,0.2) 50%, transparent 100%);
  z-index: 2;
}

.card-content {
  position: relative;
  z-index: 10;
  margin-top: auto;
  padding: 2rem 1.5rem;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.item-meta {
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-family: var(--font-body);
  font-size: 0.75rem;
  text-transform: uppercase;
  letter-spacing: 0.1em;
  color: var(--text-secondary);
}

.item-price {
  color: var(--accent-violet);
}

.item-name {
  font-size: 1.5rem;
  font-weight: 400;
  line-height: 1.2;
}

.btn-icon {
  position: absolute;
  top: 1.5rem;
  right: 1.5rem;
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.1);
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.2);
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--text-primary);
  font-size: 1.2rem;
  transition: all 0.3s ease;
  z-index: 10;
}

.showcase-card:hover .btn-icon {
  background: var(--text-primary);
  color: var(--bg-void);
  transform: rotate(90deg);
}

@media (max-width: 768px) {
  .showcase-header {
    flex-direction: column;
    align-items: flex-start;
    gap: 1rem;
  }
  .showcase-card {
    width: 280px;
    height: 400px;
  }
}
</style>
