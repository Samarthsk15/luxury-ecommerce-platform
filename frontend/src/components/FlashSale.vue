<template>
  <section class="flash-section">
    <div class="flash-glow"></div>
    <div class="flash-inner">
      <div class="flash-header">
        <div class="flash-badge">⚡ FLASH SALE</div>
        <h2 class="flash-title">Limited Time Offers</h2>
        <p class="flash-sub">Grab these deals before they vanish</p>
      </div>
      <div class="flash-timer">
        <div class="timer-unit">
          <span class="timer-num">{{ hours }}</span>
          <span class="timer-label">Hours</span>
        </div>
        <span class="timer-sep">:</span>
        <div class="timer-unit">
          <span class="timer-num">{{ minutes }}</span>
          <span class="timer-label">Min</span>
        </div>
        <span class="timer-sep">:</span>
        <div class="timer-unit">
          <span class="timer-num">{{ seconds }}</span>
          <span class="timer-label">Sec</span>
        </div>
      </div>
      <div class="flash-products">
        <div v-for="item in flashItems" :key="item.id" class="flash-card">
          <div class="flash-img">
            <img :src="item.image" :alt="item.name" loading="lazy" />
          </div>
          <div class="flash-info">
            <span class="flash-name">{{ item.name }}</span>
            <div class="flash-pricing">
              <span class="flash-price">₹{{ item.price.toLocaleString() }}</span>
              <span class="flash-off">{{ item.off }}% OFF</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue';

const hours = ref('04');
const minutes = ref('32');
const seconds = ref('18');
let interval;

const tick = () => {
  let s = parseInt(seconds.value);
  let m = parseInt(minutes.value);
  let h = parseInt(hours.value);
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

const flashItems = [
  { id: 201, name: 'Pro Wireless Buds', price: 2999, off: 50, image: 'https://images.unsplash.com/photo-1590658268037-6bf12f032f55?w=300&h=300&fit=crop' },
  { id: 202, name: 'Smart Fitness Band', price: 1499, off: 60, image: 'https://images.unsplash.com/photo-1575311373937-040b8e1fd5b6?w=300&h=300&fit=crop' },
  { id: 203, name: 'Portable Charger 20K', price: 999, off: 45, image: 'https://images.unsplash.com/photo-1609091839311-d5365f9ff1c5?w=300&h=300&fit=crop' },
  { id: 204, name: 'Noise Cancelling ANC', price: 5999, off: 35, image: 'https://images.unsplash.com/photo-1583394838336-acd977736f90?w=300&h=300&fit=crop' },
];
</script>

<style scoped>
.flash-section {
  position: relative;
  border-radius: var(--radius-xl);
  overflow: hidden;
  background: linear-gradient(135deg, #0f1a3a 0%, #1a0a2e 100%);
  border: 1px solid rgba(139, 92, 246, 0.2);
}

.flash-glow {
  position: absolute;
  top: -50%;
  right: -30%;
  width: 60%;
  height: 150%;
  background: radial-gradient(circle, rgba(139, 92, 246, 0.15) 0%, transparent 60%);
  pointer-events: none;
}

.flash-inner {
  position: relative;
  z-index: 2;
  padding: var(--space-xl);
  display: flex;
  flex-direction: column;
  gap: var(--space-lg);
  align-items: center;
  text-align: center;
}

.flash-badge {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.5rem 1.25rem;
  background: rgba(139, 92, 246, 0.2);
  border: 1px solid rgba(139, 92, 246, 0.35);
  border-radius: var(--radius-full);
  color: #d0bcff;
  font-family: var(--font-headline);
  font-size: 0.8rem;
  font-weight: 700;
  letter-spacing: 0.12em;
  text-transform: uppercase;
}

.flash-title {
  font-size: 2.25rem;
  font-weight: 800;
  letter-spacing: -0.03em;
  margin-top: 0.75rem;
  background: linear-gradient(135deg, #fff, #d0bcff);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.flash-sub {
  color: rgba(255, 255, 255, 0.5);
  font-size: 0.95rem;
}

/* Timer */
.flash-timer {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.timer-unit {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.25rem;
  min-width: 64px;
  padding: 0.75rem;
  background: rgba(255, 255, 255, 0.06);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: var(--radius-md);
}

.timer-num {
  font-family: var(--font-headline);
  font-size: 1.75rem;
  font-weight: 800;
  color: white;
}

.timer-label {
  font-size: 0.65rem;
  text-transform: uppercase;
  letter-spacing: 0.1em;
  color: rgba(255, 255, 255, 0.4);
  font-weight: 500;
}

.timer-sep {
  font-size: 1.5rem;
  font-weight: 800;
  color: rgba(255, 255, 255, 0.3);
}

/* Products */
.flash-products {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 1rem;
  width: 100%;
}

.flash-card {
  display: flex;
  flex-direction: column;
  background: rgba(255, 255, 255, 0.04);
  border: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: var(--radius-lg);
  overflow: hidden;
  transition: all 0.4s var(--ease-out);
  cursor: pointer;
}

.flash-card:hover {
  border-color: rgba(139, 92, 246, 0.4);
  transform: translateY(-6px);
  box-shadow: 0 16px 40px rgba(0, 0, 0, 0.4), 0 0 24px var(--accent-glow-violet);
}

.flash-img {
  height: 160px;
  overflow: hidden;
  background: rgba(0, 0, 0, 0.2);
}

.flash-img img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.5s var(--ease-out);
}

.flash-card:hover .flash-img img {
  transform: scale(1.08);
}

.flash-info {
  padding: 1rem;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.flash-name {
  font-family: var(--font-headline);
  font-weight: 600;
  font-size: 0.9rem;
  color: rgba(255, 255, 255, 0.85);
}

.flash-pricing {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.flash-price {
  font-family: var(--font-headline);
  font-weight: 800;
  font-size: 1.1rem;
  color: white;
}

.flash-off {
  font-size: 0.7rem;
  font-weight: 700;
  padding: 0.2rem 0.5rem;
  border-radius: var(--radius-full);
  background: rgba(16, 185, 129, 0.15);
  color: #34d399;
}

@media (max-width: 768px) {
  .flash-products { grid-template-columns: repeat(2, 1fr); }
}
</style>
