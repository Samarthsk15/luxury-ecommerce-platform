<template>
  <section class="brand-story" id="brand-story">
    <div class="story-container">
      <div class="story-header" v-scroll-reveal>
        <span class="eyebrow">The Heritage</span>
        <h2 class="section-title">A Legacy of <br/><span class="italic-gold">Excellence</span></h2>
      </div>
      
      <div class="story-content">
        <div class="story-text" v-scroll-reveal>
          <p class="lead">
            We believe that true luxury lies in the details. Every piece we offer is a testament to uncompromising quality and timeless design.
          </p>
          <p>
            Founded on the principles of exclusivity and craftsmanship, LuxeShop curates only the finest items from master artisans around the globe. Our collection is not just about ownership; it's about experiencing artistry in your daily life.
          </p>
        </div>
        
        <div class="values-grid">
          <div class="value-item" v-scroll-reveal>
            <div class="value-number">01</div>
            <h3 class="value-title">Craftsmanship</h3>
            <p class="value-desc">Meticulously crafted by master artisans using traditional techniques passed down through generations.</p>
          </div>
          <div class="value-item" v-scroll-reveal>
            <div class="value-number">02</div>
            <h3 class="value-title">Exclusivity</h3>
            <p class="value-desc">Limited production runs ensure that your piece remains as unique as your personal taste.</p>
          </div>
          <div class="value-item" v-scroll-reveal>
            <div class="value-number">03</div>
            <h3 class="value-title">Sustainability</h3>
            <p class="value-desc">Ethically sourced materials and practices that respect both the environment and the creators.</p>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup>
// v-scroll-reveal is registered globally in main.js
</script>

<style scoped>
.brand-story {
  padding: var(--space-xl) var(--space-md);
  background: var(--bg-surface);
  position: relative;
  overflow: hidden;
}

/* Subtle background line */
.brand-story::before {
  content: '';
  position: absolute;
  top: 0;
  left: 50%;
  width: 1px;
  height: 100px;
  background: linear-gradient(to bottom, var(--accent-violet), transparent);
}

.story-container {
  max-width: 1000px;
  margin: 0 auto;
  display: flex;
  flex-direction: column;
  gap: var(--space-xl);
}

.story-header {
  text-align: center;
  margin-bottom: var(--space-md);
}

.eyebrow {
  font-family: var(--font-body);
  text-transform: uppercase;
  letter-spacing: 0.2em;
  font-size: 0.75rem;
  color: var(--text-secondary);
  display: block;
  margin-bottom: 1rem;
}

.section-title {
  font-size: clamp(3rem, 5vw, 4.5rem);
  font-weight: 300;
  line-height: 1.1;
  text-transform: uppercase;
  letter-spacing: -0.02em;
}

.italic-gold {
  font-style: italic;
  color: var(--accent-violet);
}

.story-content {
  display: flex;
  flex-direction: column;
  gap: var(--space-xl);
}

.story-text {
  max-width: 700px;
  margin: 0 auto;
  text-align: center;
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.lead {
  font-size: 1.5rem;
  font-family: var(--font-headline);
  color: var(--text-primary);
  line-height: 1.5;
}

.story-text p:not(.lead) {
  color: var(--text-secondary);
  font-weight: 300;
}

.values-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: var(--space-lg);
  margin-top: var(--space-md);
  padding-top: var(--space-lg);
  border-top: 1px solid rgba(255,255,255,0.05);
}

.value-item {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.value-number {
  font-family: var(--font-headline);
  font-size: 2.5rem;
  color: rgba(212, 175, 55, 0.2); /* Faded gold */
  font-weight: 300;
  line-height: 1;
}

.value-title {
  font-size: 1.2rem;
  text-transform: uppercase;
  letter-spacing: 0.1em;
  font-weight: 400;
}

.value-desc {
  font-size: 0.9rem;
  color: var(--text-secondary);
  line-height: 1.6;
  font-weight: 300;
}

@media (max-width: 768px) {
  .values-grid {
    grid-template-columns: 1fr;
    gap: var(--space-xl);
  }
  
  .value-item {
    align-items: center;
    text-align: center;
  }
}
</style>
