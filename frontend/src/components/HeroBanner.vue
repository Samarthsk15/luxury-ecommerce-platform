<template>
  <section class="hero">
    <!-- Ambient glow blobs -->
    <div class="hero-glow glow-1"></div>
    <div class="hero-glow glow-2"></div>

    <div class="hero-content">
      <!-- Left: Text -->
      <div class="hero-text">
        <div class="hero-badge animate-in" style="--d:0">
          <span class="badge-dot"></span>
          <span>New Collection 2026</span>
        </div>
        <h1 class="hero-title animate-in" style="--d:1">
          New<br/>Arrivals
        </h1>
        <p class="hero-subtitle animate-in" style="--d:2">
          Experience the pinnacle of design and technology with our latest curated selection of high-performance luxury essentials.
        </p>
        <div class="hero-stats animate-in" style="--d:3">
          <div class="stat">
            <span class="stat-number">12,482</span>
            <span class="stat-label">Premium Items</span>
          </div>
          <div class="stat-divider"></div>
          <div class="stat">
            <span class="stat-number">98%</span>
            <span class="stat-label">Satisfaction</span>
          </div>
        </div>
        <div class="hero-cta animate-in" style="--d:4">
          <button class="btn-primary" @click="$router.push('/products')">
            <span>Explore Collection</span>
            <span class="btn-arrow">→</span>
          </button>
          <button class="btn-ghost" @click="$router.push('/deals')">
            View Deals
          </button>
        </div>
      </div>

      <!-- Right: Featured Products -->
      <div class="hero-showcase">
        <div class="showcase-card card-main animate-in" style="--d:2">
          <div class="card-glow"></div>
          <img src="https://images.unsplash.com/photo-1542291026-7eec264c27ff?auto=format&fit=crop&q=80&w=600" alt="Premium Sneaker" />
          <div class="card-overlay">
            <span class="card-tag">Featured</span>
            <span class="card-price">$299.00</span>
          </div>
        </div>
        <div class="showcase-card card-secondary animate-in" style="--d:3">
          <img src="https://images.unsplash.com/photo-1505740420928-5e560c06d30e?auto=format&fit=crop&q=80&w=400" alt="Headphones" />
          <div class="card-overlay">
            <span class="card-price">$549.00</span>
          </div>
        </div>
        <div class="showcase-card card-tertiary animate-in" style="--d:4">
          <img src="https://images.unsplash.com/photo-1523275335684-37898b6baf30?auto=format&fit=crop&q=80&w=400" alt="Watch" />
          <div class="card-overlay">
            <span class="card-price">$890.00</span>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup>
</script>

<style scoped>
.hero {
  position: relative;
  padding: var(--space-xl) 0;
  overflow: hidden;
  min-height: 520px;
}

/* Ambient Glows */
.hero-glow {
  position: absolute;
  border-radius: 50%;
  filter: blur(100px);
  pointer-events: none;
}

.glow-1 {
  width: 500px;
  height: 500px;
  background: rgba(46, 91, 255, 0.12);
  top: -100px;
  right: 10%;
  animation: float 8s ease-in-out infinite;
}

.glow-2 {
  width: 400px;
  height: 400px;
  background: rgba(139, 92, 246, 0.08);
  bottom: -80px;
  left: 5%;
  animation: float 10s ease-in-out infinite reverse;
}

.hero-content {
  position: relative;
  z-index: 2;
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: var(--space-xl);
  align-items: center;
}

/* Hero Text */
.hero-text {
  display: flex;
  flex-direction: column;
  gap: var(--space-md);
}

.hero-badge {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.5rem 1rem;
  background: rgba(46, 91, 255, 0.1);
  border: 1px solid rgba(46, 91, 255, 0.2);
  border-radius: var(--radius-full);
  width: fit-content;
  font-size: 0.8rem;
  font-weight: 600;
  color: var(--text-accent);
  font-family: var(--font-headline);
  letter-spacing: 0.03em;
}

.badge-dot {
  width: 6px;
  height: 6px;
  border-radius: 50%;
  background: var(--accent-blue);
  animation: glow-pulse 2s ease-in-out infinite;
}

.hero-title {
  font-size: clamp(3.5rem, 6vw, 5rem);
  font-weight: 800;
  letter-spacing: -0.04em;
  line-height: 1.05;
  background: linear-gradient(135deg, var(--text-primary) 0%, rgba(184, 195, 255, 0.8) 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.hero-subtitle {
  font-size: 1.05rem;
  color: var(--text-secondary);
  line-height: 1.7;
  max-width: 440px;
}

.hero-stats {
  display: flex;
  align-items: center;
  gap: 1.5rem;
}

.stat {
  display: flex;
  flex-direction: column;
  gap: 0.15rem;
}

.stat-number {
  font-family: var(--font-headline);
  font-size: 1.75rem;
  font-weight: 800;
  color: var(--text-primary);
}

.stat-label {
  font-size: 0.75rem;
  color: var(--text-muted);
  text-transform: uppercase;
  letter-spacing: 0.1em;
  font-weight: 500;
}

.stat-divider {
  width: 1px;
  height: 40px;
  background: var(--border-glass);
}

/* CTA */
.hero-cta {
  display: flex;
  gap: 1rem;
  margin-top: 0.5rem;
}

.btn-primary {
  display: inline-flex;
  align-items: center;
  gap: 0.75rem;
  padding: 0.875rem 1.75rem;
  background: var(--accent-gradient);
  color: white;
  border-radius: var(--radius-full);
  font-weight: 700;
  font-size: 0.9rem;
  letter-spacing: 0.01em;
  transition: all 0.3s ease;
  box-shadow: 0 8px 30px var(--accent-glow);
}

.btn-primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 12px 40px var(--accent-glow), 0 0 60px var(--accent-glow-violet);
}

.btn-arrow {
  transition: transform 0.3s ease;
}

.btn-primary:hover .btn-arrow {
  transform: translateX(4px);
}

.btn-ghost {
  padding: 0.875rem 1.75rem;
  background: transparent;
  color: var(--text-secondary);
  border: 1px solid var(--border-glass);
  border-radius: var(--radius-full);
  font-weight: 600;
  font-size: 0.9rem;
  transition: all 0.3s ease;
}

.btn-ghost:hover {
  color: var(--text-primary);
  border-color: var(--border-glass-hover);
  background: rgba(255, 255, 255, 0.04);
}

/* Showcase Cards */
.hero-showcase {
  display: grid;
  grid-template-columns: 1fr 1fr;
  grid-template-rows: auto auto;
  gap: 1rem;
  position: relative;
}

.showcase-card {
  position: relative;
  border-radius: var(--radius-xl);
  overflow: hidden;
  border: 1px solid var(--border-glass);
  background: var(--bg-card);
  cursor: pointer;
  transition: all 0.5s var(--ease-out);
}

.showcase-card img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.6s var(--ease-out);
}

.showcase-card:hover img {
  transform: scale(1.06);
}

.showcase-card:hover {
  border-color: rgba(46, 91, 255, 0.3);
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.4), 0 0 30px var(--accent-glow);
}

.card-main {
  grid-column: 1 / -1;
  height: 280px;
}

.card-secondary {
  height: 200px;
}

.card-tertiary {
  height: 200px;
}

.card-glow {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: linear-gradient(135deg, rgba(46, 91, 255, 0.15), transparent 60%);
  z-index: 1;
  pointer-events: none;
}

.card-overlay {
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
  padding: 1rem 1.25rem;
  background: linear-gradient(to top, rgba(0,0,0,0.7), transparent);
  display: flex;
  justify-content: space-between;
  align-items: center;
  z-index: 2;
}

.card-tag {
  font-family: var(--font-headline);
  font-size: 0.7rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.1em;
  padding: 0.3rem 0.75rem;
  border-radius: var(--radius-full);
  background: var(--accent-blue);
  color: white;
}

.card-price {
  font-family: var(--font-headline);
  font-size: 1.1rem;
  font-weight: 800;
  color: white;
}

/* Staggered animation */
.animate-in {
  opacity: 0;
  transform: translateY(25px);
  animation: fadeInUp 0.7s var(--ease-out) forwards;
  animation-delay: calc(var(--d, 0) * 0.12s);
}

@media (max-width: 768px) {
  .hero-content {
    grid-template-columns: 1fr;
    gap: var(--space-lg);
  }
  .hero-title {
    font-size: 2.75rem;
  }
  .hero-showcase {
    max-width: 400px;
  }
}
</style>
