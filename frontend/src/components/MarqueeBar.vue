<template>
  <div class="marquee-shell glass">
    <div class="marquee-runner">🔥 MEGA SALE 🔥 <span class="highlight">70% OFF</span> ON ELECTRONICS | FREE SHIPPING | BUY 1 GET 1 | LIMITED STOCK | <span class="highlight">SHOPZONE PRO</span> EXCLUSIVE</div>
  </div>
</template>

<style scoped>
.marquee-shell {
  width: 100%;
  overflow: hidden;
  border-radius: 999px;
  padding: 0.85rem 1rem;
  box-shadow: inset 0 0 0 1px rgba(255,255,255,0.05);
  background: rgba(139, 92, 246, 0.05);
}

.marquee-runner {
  display: inline-block;
  white-space: nowrap;
  animation: slide 25s linear infinite;
  color: var(--text-secondary);
  font-size: 0.95rem;
  letter-spacing: 0.15em;
  font-weight: 500;
}

.highlight {
  color: var(--accent-secondary);
  font-weight: 800;
  text-shadow: 0 0 10px var(--accent-glow);
}

@keyframes slide {
  0% { transform: translateX(100%); }
  100% { transform: translateX(-100%); }
}
</style>
