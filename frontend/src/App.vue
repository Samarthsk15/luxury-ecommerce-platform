<template>
  <div id="app-root">
    <HeaderComponent v-if="authStore.isAuthenticated && route?.name !== 'Login'" />
    <main :class="['page-view', { 'full-bleed': route?.name === 'Login' }]">
      <router-view v-slot="{ Component }">
        <transition name="page" mode="out-in">
          <component :is="Component" :key="route?.fullPath" />
        </transition>
      </router-view>
    </main>
    <FooterComponent v-if="authStore.isAuthenticated && route?.name !== 'Login'" />
  </div>
</template>

<script setup>
import { useRoute } from 'vue-router';
import HeaderComponent from './components/HeaderComponent.vue';
import FooterComponent from './components/FooterComponent.vue';
import { useAuthStore } from './store/auth.js';

const authStore = useAuthStore();
const route = useRoute();
</script>

<style>
#app-root {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  position: relative;
  z-index: 1;
}

.page-view {
  flex: 1;
  width: 100%;
  max-width: var(--max-width);
  margin: 0 auto;
  padding: var(--space-lg) var(--gutter);
}

.page-view.full-bleed {
  max-width: 100%;
  padding: 0;
}

/* Page transitions */
.page-enter-active,
.page-leave-active {
  transition: opacity 0.4s ease, transform 0.4s var(--ease-out);
}

.page-enter-from {
  opacity: 0;
  transform: translateY(16px);
}

.page-leave-to {
  opacity: 0;
  transform: translateY(-8px);
}
</style>
