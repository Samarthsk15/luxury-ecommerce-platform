<template>
  <section class="ai-recommendations">
    <div class="ai-header">
      <span class="ai-icon">✨</span>
      <h2>AI Personalized For You</h2>
      <p>Based on your browsing history and predicted preferences</p>
    </div>
    
    <div class="recommendations-container">
      <div v-for="item in recommended" :key="item.id" class="ai-card glass-card">
        <div class="ai-img">
          <img :src="item.imageUrl" :alt="item.name" />
          <span class="match-badge">{{ item.match }}% Match</span>
        </div>
        <div class="ai-info">
          <h3>{{ item.name }}</h3>
          <p class="price">₹{{ item.price.toLocaleString('en-IN') }}</p>
          <button @click="addToCart(item)" class="add-btn" v-magnetic>Quick Add</button>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useCartStore } from '../store/cart';

const cart = useCartStore();
const recommended = ref([]);

onMounted(() => {
  // Simulating an AI recommendation engine fetching personalized data
  setTimeout(() => {
    recommended.value = [
      { id: 991, name: 'Smart Home Hub 2.0', price: 4999, match: 98, imageUrl: 'https://images.unsplash.com/photo-1558089687-f282ffcbc126?w=400&h=300&fit=crop' },
      { id: 992, name: 'Noise Cancelling Headphones', price: 12999, match: 94, imageUrl: 'https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=400&h=300&fit=crop' },
      { id: 993, name: 'Ergonomic Desk Chair', price: 8500, match: 89, imageUrl: 'https://images.unsplash.com/photo-1505843490538-5133c6c7d0e1?w=400&h=300&fit=crop' },
    ];
  }, 1000);
});

const addToCart = (item) => {
  cart.addItem(item);
  alert(`Added ${item.name} to cart!`);
};
</script>

<style scoped>
.ai-recommendations {
  padding: 2rem 0;
}
.ai-header {
  margin-bottom: 2rem;
  text-align: left;
}
.ai-icon {
  font-size: 2rem;
  display: block;
  margin-bottom: 0.5rem;
  animation: float 3s ease-in-out infinite;
}
.ai-header h2 {
  font-size: 2rem;
  background: linear-gradient(135deg, #a855f7 0%, #3b82f6 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  font-weight: 800;
}
.ai-header p {
  color: var(--text-muted);
  font-size: 0.95rem;
}
.recommendations-container {
  display: flex;
  gap: 1.5rem;
  overflow-x: auto;
  padding-bottom: 1rem;
  scrollbar-width: thin;
}
.ai-card {
  min-width: 260px;
  border-radius: var(--radius-xl);
  overflow: hidden;
  border: 1px solid rgba(168, 85, 247, 0.3);
  background: rgba(168, 85, 247, 0.05);
  transition: transform 0.3s ease;
}
.ai-card:hover {
  transform: translateY(-5px);
  border-color: rgba(168, 85, 247, 0.6);
  box-shadow: 0 10px 30px rgba(168, 85, 247, 0.15);
}
.ai-img {
  position: relative;
  height: 160px;
}
.ai-img img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}
.match-badge {
  position: absolute;
  top: 10px;
  right: 10px;
  background: linear-gradient(135deg, #10b981 0%, #059669 100%);
  color: white;
  padding: 0.2rem 0.6rem;
  border-radius: var(--radius-full);
  font-size: 0.7rem;
  font-weight: 700;
  box-shadow: 0 4px 10px rgba(16, 185, 129, 0.3);
}
.ai-info {
  padding: 1.2rem;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}
.ai-info h3 {
  font-size: 1rem;
  font-weight: 600;
  color: white;
}
.price {
  font-size: 1.1rem;
  font-weight: 700;
  color: var(--text-secondary);
}
.add-btn {
  margin-top: 0.5rem;
  padding: 0.6rem;
  border-radius: var(--radius-md);
  background: rgba(168, 85, 247, 0.2);
  color: #c084fc;
  border: 1px solid rgba(168, 85, 247, 0.3);
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s;
}
.add-btn:hover {
  background: #a855f7;
  color: white;
}
@keyframes float {
  0%, 100% { transform: translateY(0); }
  50% { transform: translateY(-8px); }
}
</style>
