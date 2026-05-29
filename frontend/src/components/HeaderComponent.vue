<template>
  <header class="luxe-header">
    <div class="header-inner">
      <router-link v-if="!authStore.isAuthenticated" to="/login" class="login-entry">
        Log in
      </router-link>

      <router-link :to="isAdminSection ? '/admin' : '/'" class="logo">
        <span class="logo-text">{{ isAdminSection ? 'LUXE ADMIN' : 'LUXE' }}</span>
        <span class="logo-dot"></span>
      </router-link>

      <nav class="nav-links">
        <template v-if="isAdminSection">
          <router-link to="/admin" class="nav-item" active-class="active">Dashboard</router-link>
          <router-link to="/admin/bidding" class="nav-item" active-class="active">Bidding Portal</router-link>
          <router-link to="/" class="nav-item storefront-link">Storefront</router-link>
        </template>
        <template v-else>
          <router-link to="/" class="nav-item" active-class="active">Shop</router-link>
          <router-link to="/deals" class="nav-item" active-class="active">Deals</router-link>
          <router-link to="/products" class="nav-item" active-class="active">Discover</router-link>
          <router-link to="/cart" class="nav-item" active-class="active">Cart</router-link>
          <a v-if="authStore.isAdmin" href="http://localhost:5174/admin" class="nav-item admin-link">Admin Portal</a>
        </template>
      </nav>

      <button
        type="button"
        @click="toggleLightDark"
        class="mode-toggle-btn"
        :title="themeStore.currentMood === 'minimal' ? 'Switch to dark mode' : 'Switch to light mode'"
      >
        {{ themeStore.currentMood === 'minimal' ? 'Dark' : 'Light' }}
      </button>

      <div class="theme-selector">
        <select :value="themeStore.currentMood" @change="changeMood" class="mood-select">
          <option value="luxury">Luxury Mood</option>
          <option value="gaming">Gaming Mood</option>
          <option value="minimal">Minimal Mood</option>
          <option value="festival">Festival Mood</option>
        </select>
      </div>

      <form class="search-box" @submit.prevent="doSearch">
        <span class="search-icon">Search</span>
        <input v-model="query" type="text" placeholder="Search premium products..." />
        <button type="button" class="mic-btn" @click="startVoiceSearch" :class="{ listening: isListening }" title="Voice search">
          Voice
        </button>
      </form>

      <div class="header-actions">
        <router-link v-if="!isAdminSection" to="/cart" class="cart-btn" title="Cart">
          <span class="cart-icon">Cart</span>
          <span v-if="cartCount > 0" class="cart-badge">{{ cartCount }}</span>
        </router-link>

        <div v-if="authStore.isAuthenticated" class="user-pill">
          <span class="user-avatar">{{ userInitial }}</span>
          <span class="user-name">{{ authStore.user?.name || 'User' }}</span>
          <button type="button" @click="logout" class="logout-btn">Sign Out</button>
        </div>
      </div>
    </div>
  </header>
</template>

<script setup>
import { ref, computed } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useCartStore } from '../store/cart';
import { useAuthStore } from '../store/auth.js';
import { useThemeStore } from '../store/theme.js';

const cartStore = useCartStore();
const authStore = useAuthStore();
const themeStore = useThemeStore();
const router = useRouter();
const route = useRoute();
const query = ref('');
const isListening = ref(false);

const isAdminSection = computed(() => route.path.startsWith('/admin'));
const cartCount = computed(() => cartStore.itemCount);
const userInitial = computed(() => {
  const name = authStore.user?.name || 'U';
  return name.charAt(0).toUpperCase();
});

const toggleLightDark = () => {
  if (themeStore.currentMood === 'minimal') {
    themeStore.setMood('luxury');
  } else {
    themeStore.setMood('minimal');
  }
};

const doSearch = () => {
  if (query.value.trim()) {
    router.push({ path: '/products', query: { search: query.value } });
  }
};

const changeMood = (event) => {
  const mood = event.target.value;
  themeStore.setMood(mood);

  if (mood === 'luxury') return;

  let targetCategory = 'All';
  if (mood === 'gaming') targetCategory = 'gaming';
  if (mood === 'minimal') targetCategory = 'home';
  if (mood === 'festival') targetCategory = 'fashion';

  router.push({ path: '/products', query: { category: targetCategory } });
};

const startVoiceSearch = () => {
  if (!('webkitSpeechRecognition' in window)) {
    alert('Voice search is not supported in your browser.');
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
    console.error('Speech recognition error', event.error);
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
  background: var(--bg-glass);
  backdrop-filter: blur(24px);
  -webkit-backdrop-filter: blur(24px);
  border-bottom: 1px solid var(--border-glass);
}

.header-inner {
  max-width: var(--max-width);
  margin: 0 auto;
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 0.875rem var(--gutter);
}

.login-entry {
  flex-shrink: 0;
  padding: 0.55rem 1rem;
  border-radius: var(--radius-full);
  background: var(--text-primary);
  color: var(--bg-void);
  border: 1px solid var(--border-glass);
  font-family: var(--font-headline);
  font-size: 0.875rem;
  font-weight: 700;
  transition: all 0.25s ease;
}

.login-entry:hover {
  background: var(--text-accent);
  color: var(--bg-void);
  transform: translateY(-1px);
}

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
  color: var(--text-primary);
}

.logo-dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  background: var(--accent-gradient);
  animation: glow-pulse 3s ease-in-out infinite;
}

.nav-links {
  display: flex;
  gap: 0.25rem;
}

.nav-item {
  font-family: var(--font-headline);
  font-size: 0.875rem;
  font-weight: 500;
  color: var(--text-secondary);
  padding: 0.5rem 0.8rem;
  border-radius: var(--radius-full);
  transition: all 0.25s ease;
}

.nav-item:hover {
  color: var(--text-primary);
  background: var(--bg-card-hover);
}

.nav-item.active {
  color: var(--text-primary);
  background: rgba(212, 175, 55, 0.1);
}

.mode-toggle-btn,
.mood-select,
.cart-btn,
.mic-btn {
  border: 1px solid var(--border-glass);
}

.mode-toggle-btn {
  flex-shrink: 0;
  min-width: 56px;
  height: 40px;
  border-radius: var(--radius-full);
  background: var(--bg-card);
  color: var(--text-primary);
  font-size: 0.75rem;
  font-weight: 700;
  transition: all 0.3s ease;
}

.mode-toggle-btn:hover,
.cart-btn:hover {
  background: rgba(255, 255, 255, 0.1);
  border-color: var(--border-glass-hover);
}

.mood-select {
  background: var(--bg-card);
  color: var(--text-primary);
  padding: 0.4rem 0.8rem;
  border-radius: var(--radius-full);
  font-family: var(--font-headline);
  font-weight: 600;
  font-size: 0.8rem;
  cursor: pointer;
}

.mood-select option {
  background: var(--bg-surface);
  color: var(--text-primary);
}

.search-box {
  flex: 1;
  max-width: 400px;
  min-width: 180px;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  background: var(--bg-card);
  border: 1px solid var(--border-glass);
  border-radius: var(--radius-full);
  padding: 0.45rem 0.65rem 0.45rem 1rem;
  transition: all 0.3s ease;
}

.search-box:focus-within {
  border-color: var(--accent-violet);
  box-shadow: 0 0 0 3px var(--accent-glow);
  background: var(--bg-card-hover);
}

.search-icon {
  font-size: 0.72rem;
  color: var(--text-muted);
  text-transform: uppercase;
}

.search-box input {
  flex: 1;
  min-width: 0;
  border: none;
  background: transparent;
  color: var(--text-primary);
  font-size: 0.875rem;
}

.search-box input::placeholder {
  color: var(--text-muted);
}

.mic-btn {
  flex-shrink: 0;
  padding: 0.35rem 0.55rem;
  border-radius: var(--radius-full);
  background: var(--bg-card);
  color: var(--text-secondary);
  font-size: 0.7rem;
  transition: all 0.3s ease;
}

.mic-btn:hover,
.mic-btn.listening {
  color: var(--text-primary);
  border-color: var(--border-glass-hover);
}

.header-actions {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  flex-shrink: 0;
}

.cart-btn {
  position: relative;
  min-width: 48px;
  height: 40px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: var(--radius-md);
  background: var(--bg-card);
  color: var(--text-primary);
  transition: all 0.25s ease;
}

.cart-icon {
  font-size: 0.75rem;
  font-weight: 700;
}

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
  background: var(--bg-card);
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
  color: #ffffff;
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
  background: var(--bg-card);
  padding: 0.3rem 0.65rem;
  border-radius: var(--radius-full);
  transition: all 0.2s ease;
}

.logout-btn:hover {
  color: var(--error);
  background: rgba(239, 68, 68, 0.1);
}

.storefront-link {
  color: #00f2ff !important;
  font-weight: 600;
}

.admin-link {
  color: #d4af37 !important;
  font-weight: 600;
}

@media (max-width: 980px) {
  .header-inner {
    flex-wrap: wrap;
  }

  .search-box {
    order: 2;
    max-width: none;
    width: 100%;
  }
}

@media (max-width: 640px) {
  .header-inner {
    gap: 0.65rem;
  }

  .nav-links {
    order: 3;
    width: 100%;
    overflow-x: auto;
    padding-bottom: 0.15rem;
  }

  .nav-item {
    white-space: nowrap;
  }

  .theme-selector {
    flex: 1;
  }

  .mood-select {
    width: 100%;
  }

  .user-name {
    display: none;
  }
}
</style>
