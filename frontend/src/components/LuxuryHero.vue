<template>
  <section class="luxury-hero">
    <!-- Liquid Glass Ambient Background -->
    <div class="liquid-bg">
      <div class="liquid-blob blob-1"></div>
      <div class="liquid-blob blob-2"></div>
      <div class="liquid-blob blob-3"></div>
    </div>

    <div class="hero-content">
      <div class="hero-text animate-in" style="--d:0">
        <h1 class="hero-title" :key="adStore.rank1Product?.id">
          <span class="title-line">{{ adStore.rank1Product?.product.split(' ')[0] }}</span>
          <span class="title-line italic">{{ adStore.rank1Product?.product.split(' ').slice(1).join(' ') || 'Redefined.' }}</span>
        </h1>
        <p class="hero-subtitle animate-in" style="--d:1" :key="'desc' + adStore.rank1Product?.id">
          {{ adStore.rank1Product?.description }}
        </p>
        <div class="hero-actions animate-in" style="--d:2">
          <button class="btn-luxury" @click="$router.push('/products')">
            <span>Explore Collection</span>
            <div class="btn-glow"></div>
          </button>
          <button class="btn-ghost" @click="scrollToStory">
            <span>Our Heritage</span>
          </button>
        </div>
      </div>
      
      <div class="hero-visual animate-in" style="--d:3">
        <div class="glass-showcase" @mousemove="handleMouseMove" @mouseleave="resetMouse" ref="showcase">
          <div class="glass-reflection"></div>
          <!-- 3D Model or High-End Image -->
          <div class="visual-inner">
             <img :src="adStore.rank1Product?.imageUrl" alt="Premium Promoted Item" class="luxury-item-img" :key="adStore.rank1Product?.id" />
          </div>
          
          <div class="floating-badge badge-1 glass">
            <span class="badge-icon">👑</span>
            <span class="badge-text">Sponsored #1</span>
          </div>
          
          <div class="floating-badge badge-2 glass">
            <span class="badge-text">Limited Edition</span>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup>
import { ref, computed } from 'vue';
import { useAdvertisementStore } from '../store/advertisement';

const adStore = useAdvertisementStore();
const showcase = ref(null);

const handleMouseMove = (e) => {
  if (!showcase.value) return;
  const rect = showcase.value.getBoundingClientRect();
  const x = (e.clientX - rect.left) / rect.width - 0.5;
  const y = (e.clientY - rect.top) / rect.height - 0.5;
  
  showcase.value.style.transform = `perspective(1000px) rotateY(${x * 10}deg) rotateX(${-y * 10}deg) scale3d(1.02, 1.02, 1.02)`;
  
  const reflection = showcase.value.querySelector('.glass-reflection');
  if(reflection) {
    reflection.style.background = `radial-gradient(circle at ${e.clientX - rect.left}px ${e.clientY - rect.top}px, rgba(255,255,255,0.2) 0%, transparent 60%)`;
  }
};

const resetMouse = () => {
  if (!showcase.value) return;
  showcase.value.style.transform = `perspective(1000px) rotateY(0) rotateX(0) scale3d(1, 1, 1)`;
  const reflection = showcase.value.querySelector('.glass-reflection');
  if(reflection) {
    reflection.style.background = 'none';
  }
};

const scrollToStory = () => {
  document.getElementById('brand-story')?.scrollIntoView({ behavior: 'smooth' });
};
</script>

<style scoped>
.luxury-hero {
  position: relative;
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: var(--space-xl) var(--space-md);
  overflow: hidden;
  background: var(--bg-void);
}

.liquid-bg {
  position: absolute;
  inset: 0;
  z-index: 0;
  overflow: hidden;
  filter: blur(80px);
  opacity: 0.6;
}

.liquid-blob {
  position: absolute;
  border-radius: 50%;
  animation: liquidFloat 15s infinite alternate ease-in-out;
}

.blob-1 {
  top: -10%; left: -10%;
  width: 50vw; height: 50vw;
  background: rgba(212, 175, 55, 0.15); /* Gold */
}

.blob-2 {
  bottom: -20%; right: -10%;
  width: 60vw; height: 60vw;
  background: rgba(255, 255, 255, 0.08); /* Silver/White */
  animation-delay: -5s;
}

.blob-3 {
  top: 40%; left: 40%;
  width: 40vw; height: 40vw;
  background: rgba(10, 10, 10, 0.8);
  animation-duration: 20s;
}

@keyframes liquidFloat {
  0% { transform: translate(0, 0) scale(1); }
  50% { transform: translate(5%, 10%) scale(1.1); }
  100% { transform: translate(-5%, -5%) scale(0.9); }
}

.hero-content {
  position: relative;
  z-index: 10;
  max-width: var(--max-width);
  width: 100%;
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: var(--space-xl);
  align-items: center;
}

.hero-text {
  display: flex;
  flex-direction: column;
  gap: var(--space-md);
}

.hero-title {
  font-size: clamp(4rem, 8vw, 7rem);
  line-height: 0.9;
  font-weight: 300;
  letter-spacing: -0.02em;
  text-transform: uppercase;
}

.title-line {
  display: block;
}

.title-line.italic {
  font-style: italic;
  color: var(--accent-violet); /* Gold */
  padding-left: 10%;
}

.hero-subtitle {
  font-size: 1.1rem;
  color: var(--text-secondary);
  max-width: 80%;
  line-height: 1.8;
  font-weight: 300;
}

.hero-actions {
  display: flex;
  gap: var(--space-md);
  margin-top: var(--space-sm);
}

.btn-luxury {
  position: relative;
  padding: 1rem 2.5rem;
  background: transparent;
  color: var(--text-primary);
  border: 1px solid var(--text-primary);
  border-radius: 0;
  text-transform: uppercase;
  letter-spacing: 0.1em;
  font-size: 0.8rem;
  overflow: hidden;
  transition: all 0.4s ease;
}

.btn-luxury:hover {
  background: var(--text-primary);
  color: var(--bg-void);
}

.btn-ghost {
  padding: 1rem 2.5rem;
  background: transparent;
  color: var(--text-secondary);
  border: 1px solid transparent;
  text-transform: uppercase;
  letter-spacing: 0.1em;
  font-size: 0.8rem;
  transition: all 0.4s ease;
}

.btn-ghost:hover {
  color: var(--text-primary);
  border-bottom: 1px solid var(--text-primary);
}

/* Glass Showcase */
.hero-visual {
  perspective: 1200px;
}

.glass-showcase {
  position: relative;
  width: 100%;
  aspect-ratio: 4/5;
  background: rgba(255, 255, 255, 0.02);
  backdrop-filter: blur(20px);
  -webkit-backdrop-filter: blur(20px);
  border: 1px solid rgba(255, 255, 255, 0.05);
  border-radius: 2px;
  overflow: hidden;
  transition: transform 0.1s ease-out;
  transform-style: preserve-3d;
  box-shadow: 0 30px 60px rgba(0,0,0,0.5);
}

.glass-reflection {
  position: absolute;
  inset: 0;
  z-index: 5;
  pointer-events: none;
}

.visual-inner {
  position: absolute;
  inset: 20px;
  background: #000;
  overflow: hidden;
  border-radius: 2px;
}

.luxury-item-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  opacity: 0.8;
  mix-blend-mode: luminosity;
  transition: all 1s ease;
}

.glass-showcase:hover .luxury-item-img {
  transform: scale(1.05);
  opacity: 1;
  mix-blend-mode: normal;
}

.floating-badge {
  position: absolute;
  z-index: 10;
  padding: 0.8rem 1.5rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 0.75rem;
  text-transform: uppercase;
  letter-spacing: 0.1em;
  transform: translateZ(30px);
}

.badge-1 {
  bottom: 10%;
  left: -5%;
}

.badge-2 {
  top: 15%;
  right: -5%;
  background: rgba(212, 175, 55, 0.1) !important;
  border-color: rgba(212, 175, 55, 0.3) !important;
  color: var(--accent-violet);
}

.badge-icon {
  font-size: 1.2rem;
  color: var(--accent-violet);
}

.animate-in {
  opacity: 0;
  transform: translateY(30px);
  animation: fadeInUp 1s cubic-bezier(0.16, 1, 0.3, 1) forwards;
  animation-delay: calc(var(--d) * 0.2s);
}

@media (max-width: 1024px) {
  .hero-content {
    grid-template-columns: 1fr;
    text-align: center;
    gap: var(--space-lg);
  }
  
  .title-line.italic {
    padding-left: 0;
  }
  
  .hero-subtitle {
    margin: 0 auto;
  }
  
  .hero-actions {
    justify-content: center;
  }
  
  .glass-showcase {
    max-width: 500px;
    margin: 0 auto;
  }
  
  .floating-badge {
    left: 10%;
    right: auto;
  }
  .badge-2 {
    right: 10%;
    left: auto;
  }
}
</style>
