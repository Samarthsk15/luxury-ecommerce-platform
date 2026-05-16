<template>
  <div class="success-page">
    <div class="glass-card success-card">
      <div class="icon-container">
        <span class="icon">✅</span>
      </div>
      <h1 class="title">Payment Successful!</h1>
      <p class="subtitle">Thank you for your purchase. Your order has been placed.</p>
      <button class="home-btn" @click="$router.push('/')">Return to Home</button>
    </div>
  </div>
</template>

<script setup>
import { onMounted } from 'vue';
import { useCartStore } from '../store/cart';

const cart = useCartStore();

onMounted(() => {
  cart.clearCart();
});
</script>

<style scoped>
.success-page {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 80vh;
  padding: 2rem;
}

.success-card {
  padding: 3rem;
  border-radius: var(--radius-xl);
  text-align: center;
  max-width: 500px;
  width: 100%;
  background: var(--bg-card);
  border: 1px solid rgba(16, 185, 129, 0.3);
  box-shadow: 0 10px 40px rgba(16, 185, 129, 0.1);
  animation: popIn 0.5s cubic-bezier(0.175, 0.885, 0.32, 1.275);
}

.icon-container {
  width: 80px;
  height: 80px;
  background: rgba(16, 185, 129, 0.1);
  border-radius: 50%;
  display: flex;
  justify-content: center;
  align-items: center;
  margin: 0 auto 1.5rem;
  border: 2px solid #10b981;
}

.icon {
  font-size: 2.5rem;
}

.title {
  font-size: 2rem;
  color: #10b981;
  margin-bottom: 1rem;
  font-weight: 800;
}

.subtitle {
  color: var(--text-secondary);
  font-size: 1.1rem;
  margin-bottom: 2rem;
}

.home-btn {
  background: var(--text-primary);
  color: var(--bg-void);
  border: none;
  padding: 1rem 2rem;
  border-radius: var(--radius-full);
  font-weight: 700;
  font-size: 1rem;
  cursor: pointer;
  transition: all 0.3s;
  width: 100%;
}

.home-btn:hover {
  background: white;
  transform: translateY(-2px);
  box-shadow: 0 5px 15px rgba(255, 255, 255, 0.2);
}

@keyframes popIn {
  from {
    opacity: 0;
    transform: scale(0.9);
  }
  to {
    opacity: 1;
    transform: scale(1);
  }
}
</style>
