<template>
  <header class="luxe-header">
    <div class="header-inner">
      <!-- Logo -->
      <router-link to="/" class="logo">
        <span class="logo-text">LUXE</span>
        <span class="logo-dot"></span>
      </router-link>

      <!-- Nav Links -->
      <nav class="nav-links">
        <router-link to="/" class="nav-item" active-class="active">Shop</router-link>
        <router-link to="/deals" class="nav-item" active-class="active">Deals</router-link>
        <router-link to="/products" class="nav-item" active-class="active">Discover</router-link>
        <router-link to="/cart" class="nav-item" active-class="active">Cart</router-link>
        <router-link v-if="authStore.isAdmin" to="/admin" class="nav-item" active-class="active">Admin</router-link>
      </nav>

      <!-- Theme Selector -->
      <div class="theme-selector">
        <select :value="themeStore.currentMood" @change="changeMood" class="mood-select">
          <option value="luxury">Luxury Mood</option>
          <option value="gaming">Gaming Mood</option>
          <option value="minimal">Minimal Mood</option>
          <option value="festival">Festival Mood</option>
        </select>
      </div>

      <!-- Search -->
      <form class="search-box" @submit.prevent="doSearch">
        <span class="search-icon">🔍</span>
        <input v-model="query" type="text" placeholder="Search premium products..." />
        <button type="button" class="mic-btn" @click="startVoiceSearch" :class="{ listening: isListening }" title="Voice Search">🎙️</button>
      </form>

      <!-- Right Actions -->
      <div class="header-actions">
        <router-link to="/cart" class="cart-btn">
          <span class="cart-icon">🛒</span>
          <span v-if="cartCount > 0" class="cart-badge">{{ cartCount }}</span>
        </router-link>

        <div v-if="authStore.isAuthenticated" class="user-pill">
          <span class="user-avatar">{{ userInitial }}</span>
          <span class="user-name">{{ authStore.user?.name || 'User' }}</span>
          <button @click="logout" class="logout-btn">Sign Out</button>
        </div>
      </div>
    </div>
  </header>
</template>

<script setup>
import { ref, computed } from 'vue';
import { useCartStore } from '../store/cart';
import { useAuthStore } from '../store/auth.js';
import { useThemeStore } from '../store/theme.js';
import { useRouter } from 'vue-router';

const cartStore = useCartStore();
const authStore = useAuthStore();
const themeStore = useThemeStore();
const router = useRouter();
const query = ref('');

const cartCount = computed(() => cartStore.itemCount);
const userInitial = computed(() => {
  const name = authStore.user?.name || 'U';
  return name.charAt(0).toUpperCase();
});

const doSearch = () => {
  if (query.value.trim()) {
    router.push({ path: '/products', query: { search: query.value } });
  }
};

const changeMood = (event) => {
  const mood = event.target.value;
  themeStore.setMood(mood);
  
  if (mood === 'luxury') return; // Luxury is just the baseline theme, don't force a category filter
  
  let targetCategory = 'All';
  if (mood === 'gaming') targetCategory = 'gaming';
  if (mood === 'minimal') targetCategory = 'home';
  if (mood === 'festival') targetCategory = 'fashion';
  
  // Navigate directly to the products page filtered by this theme's vibe
  router.push({ path: '/products', query: { category: targetCategory } });
};

const isListening = ref(false);
const startVoiceSearch = () => {
  if (!('webkitSpeechRecognition' in window)) {
    alert("Voice search is not supported in your browser.");
    return;
  }
  const recognition = new window.webkitSpeechRecognition();
  recognition.continuous = false;
  recognition.interimResults = false;
  recognition.lang = 'en-US';

  recognition.onstart = () => {
    isListening.value = true;
  };
  recognition.onresult = (event) => {
    const transcript = event.results[0][0].transcript;
    query.value = transcript;
    doSearch();
  };
  recognition.onerror = (event) => {
    console.error("Speech recognition error", event.error);
    isListening.value = false;
  };
  recognition.onend = () => {
    isListening.value = false;
  };
  recognition.start();
};

const logout = () => {
  authStore.logout();
  router.push('/login');
};
</script>

<style scoped>
.luxe-header {
  position: sticky;
  top: 0;
  z-index: 100;
  background: rgba(10, 10, 15, 0.8);
  backdrop-filter: blur(24px);
  -webkit-backdrop-filter: blur(24px);
  border-bottom: 1px solid var(--border-glass);
}

.header-inner {
  max-width: var(--max-width);
  margin: 0 auto;
  display: flex;
  align-items: center;
  gap: 2rem;
  padding: 0.875rem var(--gutter);
}

/* Logo */
.logo {
  display: flex;
  align-items: center;
  gap: 0.375rem;
  text-decoration: none;
  flex-shrink: 0;
}

.logo-text {
  font-family: var(--font-headline);
  font-size: 1.5rem;
  font-weight: 800;
  letter-spacing: -0.03em;
  color: var(--text-primary);
}

.logo-dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  background: var(--accent-gradient);
  animation: glow-pulse 3s ease-in-out infinite;
}

/* Nav */
.nav-links {
  display: flex;
  gap: 0.25rem;
}

.nav-item {
  font-family: var(--font-headline);
  font-size: 0.875rem;
  font-weight: 500;
  color: var(--text-secondary);
  padding: 0.5rem 1rem;
  border-radius: var(--radius-full);
  transition: all 0.25s ease;
  letter-spacing: 0.01em;
}

.nav-item:hover {
  color: var(--text-primary);
  background: rgba(255, 255, 255, 0.05);
}

.nav-item.active {
  color: var(--text-primary);
  background: rgba(212, 175, 55, 0.1);
}

/* Theme Selector */
.mood-select {
  background: rgba(255, 255, 255, 0.05);
  color: var(--text-primary);
  border: 1px solid var(--border-glass);
  padding: 0.4rem 0.8rem;
  border-radius: var(--radius-full);
  font-family: var(--font-headline);
  font-weight: 600;
  font-size: 0.8rem;
  cursor: pointer;
  outline: none;
  transition: all 0.3s;
}

.mood-select:hover {
  background: rgba(255, 255, 255, 0.1);
  border-color: var(--border-glass-hover);
}

.mood-select option {
  background: #111;
  color: white;
}

/* Search */
.search-box {
  flex: 1;
  max-width: 400px;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid var(--border-glass);
  border-radius: var(--radius-full);
  padding: 0.5rem 1rem;
  transition: all 0.3s ease;
}

.search-box:focus-within {
  border-color: var(--accent-violet);
  box-shadow: 0 0 0 3px rgba(212, 175, 55, 0.1);
  background: rgba(255, 255, 255, 0.07);
}

.search-icon {
  font-size: 0.875rem;
  opacity: 0.5;
}

.search-box input {
  flex: 1;
  border: none;
  background: transparent;
  color: var(--text-primary);
  font-size: 0.875rem;
}

.search-box input::placeholder {
  color: var(--text-muted);
}

.mic-btn {
  background: transparent;
  border: none;
  font-size: 1.1rem;
  cursor: pointer;
  opacity: 0.6;
  transition: all 0.3s ease;
  padding: 0 0.5rem;
}

.mic-btn:hover {
  opacity: 1;
  transform: scale(1.1);
}

.mic-btn.listening {
  opacity: 1;
  animation: mic-pulse 1.5s infinite;
}

@keyframes mic-pulse {
  0% { transform: scale(1); filter: drop-shadow(0 0 0 rgba(239, 68, 68, 0)); }
  50% { transform: scale(1.2); filter: drop-shadow(0 0 8px rgba(239, 68, 68, 0.8)); }
  100% { transform: scale(1); filter: drop-shadow(0 0 0 rgba(239, 68, 68, 0)); }
}

/* Actions */
.header-actions {
  display: flex;
  align-items: center;
  gap: 1rem;
  flex-shrink: 0;
}

.cart-btn {
  position: relative;
  width: 40px;
  height: 40px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: var(--radius-md);
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid var(--border-glass);
  transition: all 0.25s ease;
}

.cart-btn:hover {
  background: rgba(255, 255, 255, 0.1);
  border-color: var(--border-glass-hover);
}

.cart-icon { font-size: 1.1rem; }

.cart-badge {
  position: absolute;
  top: -4px;
  right: -4px;
  min-width: 18px;
  height: 18px;
  display: flex;
  align-items: center;
  justify-content: center;
  background: var(--accent-violet);
  color: var(--bg-void);
  font-size: 0.65rem;
  font-weight: 700;
  border-radius: var(--radius-full);
  padding: 0 4px;
}

.user-pill {
  display: flex;
  align-items: center;
  gap: 0.625rem;
  padding: 0.375rem 0.5rem 0.375rem 0.375rem;
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid var(--border-glass);
  border-radius: var(--radius-full);
}

.user-avatar {
  width: 28px;
  height: 28px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
  background: var(--accent-gradient);
  color: white;
  font-size: 0.75rem;
  font-weight: 700;
  font-family: var(--font-headline);
}

.user-name {
  font-size: 0.8rem;
  font-weight: 500;
  color: var(--text-secondary);
}

.logout-btn {
  font-size: 0.75rem;
  font-weight: 600;
  color: var(--text-muted);
  background: rgba(255, 255, 255, 0.06);
  padding: 0.3rem 0.65rem;
  border-radius: var(--radius-full);
  transition: all 0.2s ease;
}

.logout-btn:hover {
  color: var(--error);
  background: rgba(239, 68, 68, 0.1);
}
</style>
