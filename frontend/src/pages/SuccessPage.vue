<template>
  <div class="success-page">
    <transition name="toast-slide">
      <div v-if="toastVisible" class="toast-notification success">
        <div class="toast-icon">🎉</div>
        <div class="toast-text">
          <strong>{{ toastTitle }}</strong>
          <p>{{ toastMessage }}</p>
        </div>
      </div>
    </transition>

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
import { onMounted, ref } from 'vue';
import { useCartStore } from '../store/cart';

const cart = useCartStore();
const toastVisible = ref(false);
const toastTitle = ref('Order Confirmed');
const toastMessage = ref('Your luxury order has been processed successfully. Sit back and relax while we prepare it for shipping.');
let toastTimeout = null;

const showToast = () => {
  toastVisible.value = true;
  if (toastTimeout) {
    clearTimeout(toastTimeout);
  }
  toastTimeout = setTimeout(() => {
    toastVisible.value = false;
  }, 4500);
};

onMounted(() => {
  cart.clearCart();
  showToast();
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

.toast-notification {
  position: fixed;
  top: 1.5rem;
  right: 1.5rem;
  z-index: 9999;
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 1rem 1.4rem;
  border-radius: 1.25rem;
  min-width: 320px;
  background: rgba(15, 23, 42, 0.98);
  color: #ffffff;
  box-shadow: 0 24px 80px rgba(0, 0, 0, 0.25);
  border: 1px solid rgba(255, 255, 255, 0.08);
  backdrop-filter: blur(18px);
}

.toast-icon {
  font-size: 1.6rem;
  width: 2.2rem;
  height: 2.2rem;
  display: grid;
  place-items: center;
}

.toast-text strong {
  display: block;
  font-size: 1rem;
  margin-bottom: 0.2rem;
}

.toast-text p {
  margin: 0;
  line-height: 1.4;
  font-size: 0.95rem;
  opacity: 0.9;
}

.toast-slide-enter-active,
.toast-slide-leave-active {
  transition: all 0.25s ease;
}

.toast-slide-enter-from,
.toast-slide-leave-to {
  opacity: 0;
  transform: translateY(-1rem) scale(0.96);
}

.toast-slide-enter-to,
.toast-slide-leave-from {
  opacity: 1;
  transform: translateY(0);
}
</style>
