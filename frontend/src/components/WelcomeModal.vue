<template>
  <div v-if="renderComponent" class="welcome-overlay" :class="{ 'fade-out': !show }">
    <div class="welcome-content" :class="{ 'scale-up': animate }">
      <div class="logo-box">
        <span class="logo-icon">LUXE</span>
      </div>
      <p class="tagline">Elegance Redefined</p>
      <div class="loading-bar-container">
        <div class="loading-bar"></div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';

const show = ref(true);
const animate = ref(false);
const renderComponent = ref(true);

onMounted(() => {
  if (sessionStorage.getItem('welcomeShown')) {
    renderComponent.value = false;
    return;
  }
  
  sessionStorage.setItem('welcomeShown', 'true');

  setTimeout(() => {
    animate.value = true;
  }, 100);

  setTimeout(() => {
    show.value = false;
  }, 2200);

  setTimeout(() => {
    renderComponent.value = false;
  }, 3000);
});
</script>

<style scoped>
.welcome-overlay {
  position: fixed;
  top: 0; left: 0; right: 0; bottom: 0;
  background: var(--bg-void);
  z-index: 99999;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: opacity 0.8s cubic-bezier(0.16, 1, 0.3, 1), visibility 0.8s;
}

.welcome-overlay.fade-out {
  opacity: 0;
  visibility: hidden;
  pointer-events: none;
}

.welcome-content {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1.5rem;
  opacity: 0;
  transform: translateY(30px) scale(0.9);
  transition: all 1s cubic-bezier(0.16, 1, 0.3, 1);
}

.welcome-content.scale-up {
  opacity: 1;
  transform: translateY(0) scale(1);
}

.logo-box {
  display: flex;
  align-items: center;
  justify-content: center;
  animation: float 3s ease-in-out infinite;
}

.logo-icon {
  font-family: var(--font-headline);
  font-size: 3.5rem;
  font-weight: 700;
  color: var(--text-primary);
  letter-spacing: 0.1em;
}

.tagline {
  color: var(--accent-violet);
  font-family: var(--font-body);
  font-size: 0.9rem;
  letter-spacing: 0.3em;
  text-transform: uppercase;
  font-weight: 300;
}

.loading-bar-container {
  width: 150px;
  height: 2px;
  background: rgba(255, 255, 255, 0.1);
  margin-top: 1rem;
  overflow: hidden;
}

.loading-bar {
  height: 100%;
  width: 0%;
  background: var(--accent-gradient);
  animation: fillBar 1.8s cubic-bezier(0.16, 1, 0.3, 1) forwards;
}

@keyframes fillBar {
  0% { width: 0%; }
  100% { width: 100%; }
}

@keyframes float {
  0%, 100% { transform: translateY(0); }
  50% { transform: translateY(-10px); }
}
</style>
