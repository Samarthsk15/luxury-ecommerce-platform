<template>
  <div class="floating-product-container">
    <div class="floating-orb glow-cyan"></div>
    <div class="floating-orb glow-violet"></div>
    
    <div class="product-wrapper">
      <div class="glass-pedestal"></div>
      <!-- We can use a model-viewer or a high-quality transparent PNG for the floating effect -->
      <img src="https://images.unsplash.com/photo-1523275335684-37898b6baf30?auto=format&fit=crop&q=80&w=400&rect=0,100,400,300" 
           alt="Premium Product" 
           class="floating-image" />
      
      <div class="product-info-tag glass-tag">
        <span class="tag-title">Pro Series Watch</span>
        <span class="tag-price">₹12,499</span>
      </div>
    </div>
  </div>
</template>

<style scoped>
.floating-product-container {
  position: absolute;
  top: 15%;
  right: 5%;
  width: 350px;
  height: 400px;
  pointer-events: none; /* Let clicks pass through to the background */
  z-index: 10;
  display: flex;
  align-items: center;
  justify-content: center;
  perspective: 1000px;
}

.floating-orb {
  position: absolute;
  width: 200px;
  height: 200px;
  border-radius: 50%;
  filter: blur(60px);
  opacity: 0.4;
  animation: orbit 10s linear infinite;
}

.glow-cyan {
  background: #2E5BFF;
  top: 0;
  left: 0;
  animation-delay: -5s;
}

.glow-violet {
  background: #8B5CF6;
  bottom: 0;
  right: 0;
}

.product-wrapper {
  position: relative;
  display: flex;
  flex-direction: column;
  align-items: center;
  animation: float-product 6s ease-in-out infinite;
  transform-style: preserve-3d;
}

.floating-image {
  width: 280px;
  height: auto;
  object-fit: cover;
  border-radius: 24px;
  filter: drop-shadow(0 30px 30px rgba(0,0,0,0.5));
  mask-image: radial-gradient(circle, black 80%, transparent 100%);
  -webkit-mask-image: radial-gradient(circle, black 80%, transparent 100%);
  animation: tilt-spin 15s linear infinite;
}

.glass-pedestal {
  position: absolute;
  bottom: -20px;
  width: 150px;
  height: 20px;
  background: radial-gradient(ellipse at center, rgba(46, 91, 255, 0.4) 0%, transparent 70%);
  filter: blur(5px);
  border-radius: 50%;
  animation: shadow-pulse 6s ease-in-out infinite;
}

.glass-tag {
  position: absolute;
  bottom: -40px;
  right: -20px;
  background: rgba(20, 20, 22, 0.7);
  backdrop-filter: blur(12px);
  -webkit-backdrop-filter: blur(12px);
  border: 1px solid rgba(255, 255, 255, 0.1);
  padding: 0.8rem 1.5rem;
  border-radius: 16px;
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  box-shadow: 0 10px 30px rgba(0,0,0,0.3);
  animation: float-tag 7s ease-in-out infinite reverse;
}

.tag-title {
  color: rgba(255, 255, 255, 0.8);
  font-size: 0.85rem;
  font-family: 'Inter', sans-serif;
  letter-spacing: 0.05em;
}

.tag-price {
  color: white;
  font-weight: 800;
  font-size: 1.2rem;
  font-family: 'Space Grotesk', sans-serif;
  background: linear-gradient(135deg, #2E5BFF, #8B5CF6);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

@keyframes float-product {
  0%, 100% { transform: translateY(0) rotateX(5deg) rotateY(-5deg); }
  50% { transform: translateY(-30px) rotateX(-5deg) rotateY(5deg); }
}

@keyframes float-tag {
  0%, 100% { transform: translateY(0) translateZ(30px); }
  50% { transform: translateY(15px) translateZ(50px); }
}

@keyframes shadow-pulse {
  0%, 100% { transform: scale(1); opacity: 0.8; }
  50% { transform: scale(0.7); opacity: 0.3; }
}

@keyframes orbit {
  0% { transform: rotate(0deg) translateX(50px) rotate(0deg); }
  100% { transform: rotate(360deg) translateX(50px) rotate(-360deg); }
}

@keyframes tilt-spin {
  0% { transform: rotateY(0deg) rotateX(0deg); }
  25% { transform: rotateY(5deg) rotateX(5deg); }
  50% { transform: rotateY(0deg) rotateX(0deg); }
  75% { transform: rotateY(-5deg) rotateX(-5deg); }
  100% { transform: rotateY(0deg) rotateX(0deg); }
}

@media (max-width: 1024px) {
  .floating-product-container {
    display: none; /* Hide on smaller screens to prevent overlapping content */
  }
}
</style>
