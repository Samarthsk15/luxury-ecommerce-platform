<template>
  <div class="login-page">
    <!-- Left: Showcase -->
    <div class="login-showcase">
      <div class="showcase-glow"></div>
      <div class="showcase-content">
        <h1 class="showcase-logo">LUXE<span class="dot">.</span></h1>
        <p class="showcase-tagline">Premium Shopping<br/>Experience</p>
        <div class="showcase-stats">
          <div class="showcase-stat">
            <span class="ss-num">10K+</span>
            <span class="ss-label">Products</span>
          </div>
          <div class="showcase-stat">
            <span class="ss-num">50K+</span>
            <span class="ss-label">Customers</span>
          </div>
        </div>
      </div>
      <img
        class="showcase-img"
        src="https://images.unsplash.com/photo-1441986300917-64674bd600d8?auto=format&fit=crop&q=80&w=800"
        alt="Premium Shopping"
      />
    </div>

    <!-- Right: Form -->
    <div class="login-form-area">
      <div class="form-card">
        <h2 class="form-title">{{ isRegister ? 'Create Account' : 'Welcome Back' }}</h2>
        <p class="form-sub">{{ isRegister ? 'Join the LUXE experience' : 'Sign in to continue shopping' }}</p>

        <div v-if="error" class="form-error">{{ error }}</div>

        <form @submit.prevent="handleSubmit" class="form-fields">
          <div v-if="isRegister" class="field">
            <label>Full Name</label>
            <input v-model="name" type="text" placeholder="John Doe" required />
          </div>
          <div class="field">
            <label>Email</label>
            <input v-model="email" type="email" placeholder="you@example.com" required />
          </div>
          <div class="field">
            <label>Password</label>
            <input v-model="password" type="password" placeholder="••••••••" required />
          </div>

          <button type="submit" class="submit-btn" :disabled="submitting">
            <span v-if="submitting" class="btn-spinner"></span>
            <span v-else>{{ isRegister ? 'Create Account' : 'Sign In' }}</span>
          </button>
        </form>

        <p class="form-toggle">
          {{ isRegister ? 'Already have an account?' : "Don't have an account?" }}
          <button @click="isRegister = !isRegister" class="toggle-btn">
            {{ isRegister ? 'Sign In' : 'Create Account' }}
          </button>
        </p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useAuthStore } from '../store/auth.js';
import { useRouter } from 'vue-router';

const authStore = useAuthStore();
const router = useRouter();

const isRegister = ref(false);
const name = ref('');
const email = ref('');
const password = ref('');
const error = ref('');
const submitting = ref(false);

const handleSubmit = async () => {
  error.value = '';
  submitting.value = true;

  try {
    let result;
    if (isRegister.value) {
      result = await authStore.register(name.value, email.value, password.value);
    } else {
      result = await authStore.login(email.value, password.value);
    }

    if (result.success) {
      router.push('/');
    } else {
      error.value = result.message || 'Something went wrong';
    }
  } catch (e) {
    error.value = 'Connection error. Please try again.';
  } finally {
    submitting.value = false;
  }
};
</script>

<style scoped>
.login-page {
  display: grid;
  grid-template-columns: 1fr 1fr;
  min-height: 100vh;
}

/* Showcase */
.login-showcase {
  position: relative;
  overflow: hidden;
  display: flex;
  align-items: flex-end;
  padding: var(--space-xl);
}

.showcase-img {
  position: absolute;
  inset: 0;
  width: 100%;
  height: 100%;
  object-fit: cover;
  z-index: 0;
}

.showcase-glow {
  position: absolute;
  inset: 0;
  background: linear-gradient(to top, rgba(10, 10, 15, 0.95) 0%, rgba(10, 10, 15, 0.4) 60%, rgba(46, 91, 255, 0.15) 100%);
  z-index: 1;
}

.showcase-content {
  position: relative;
  z-index: 2;
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.showcase-logo {
  font-family: var(--font-headline);
  font-size: 3rem;
  font-weight: 800;
  letter-spacing: -0.04em;
  color: white;
}

.showcase-logo .dot {
  color: var(--accent-blue);
}

.showcase-tagline {
  font-size: 1.5rem;
  font-weight: 300;
  color: rgba(255, 255, 255, 0.7);
  line-height: 1.4;
}

.showcase-stats {
  display: flex;
  gap: 2rem;
  margin-top: 1rem;
}

.showcase-stat {
  display: flex;
  flex-direction: column;
  gap: 0.15rem;
}

.ss-num {
  font-family: var(--font-headline);
  font-size: 1.5rem;
  font-weight: 800;
  color: white;
}

.ss-label {
  font-size: 0.75rem;
  color: rgba(255, 255, 255, 0.4);
  text-transform: uppercase;
  letter-spacing: 0.1em;
}

/* Form Area */
.login-form-area {
  display: flex;
  align-items: center;
  justify-content: center;
  padding: var(--space-xl);
  background: var(--bg-void);
}

.form-card {
  width: 100%;
  max-width: 400px;
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.form-title {
  font-size: 1.75rem;
  font-weight: 800;
  letter-spacing: -0.03em;
}

.form-sub {
  color: var(--text-secondary);
  font-size: 0.9rem;
  margin-top: -0.75rem;
}

.form-error {
  padding: 0.75rem 1rem;
  background: rgba(239, 68, 68, 0.1);
  border: 1px solid rgba(239, 68, 68, 0.2);
  border-radius: var(--radius-md);
  color: #fca5a5;
  font-size: 0.85rem;
}

.form-fields {
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
}

.field {
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
}

.field label {
  font-family: var(--font-headline);
  font-size: 0.8rem;
  font-weight: 600;
  color: var(--text-secondary);
  text-transform: uppercase;
  letter-spacing: 0.06em;
}

.field input {
  padding: 0.8rem 1rem;
  background: rgba(255, 255, 255, 0.04);
  border: 1px solid var(--border-glass);
  border-radius: var(--radius-md);
  color: var(--text-primary);
  font-size: 0.95rem;
  transition: all 0.25s ease;
}

.field input:focus {
  border-color: var(--accent-blue);
  box-shadow: 0 0 0 3px rgba(46, 91, 255, 0.12);
  background: rgba(255, 255, 255, 0.06);
}

.field input::placeholder {
  color: var(--text-muted);
}

.submit-btn {
  padding: 0.9rem;
  border-radius: var(--radius-md);
  background: var(--accent-gradient);
  color: white;
  font-weight: 700;
  font-size: 0.95rem;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  box-shadow: 0 6px 24px var(--accent-glow);
}

.submit-btn:hover:not(:disabled) {
  transform: translateY(-1px);
  box-shadow: 0 10px 32px var(--accent-glow);
}

.submit-btn:disabled {
  opacity: 0.7;
  cursor: wait;
}

.btn-spinner {
  width: 18px;
  height: 18px;
  border: 2px solid rgba(255, 255, 255, 0.3);
  border-top-color: white;
  border-radius: 50%;
  animation: spin 0.6s linear infinite;
}

.form-toggle {
  text-align: center;
  font-size: 0.85rem;
  color: var(--text-secondary);
}

.toggle-btn {
  background: none;
  color: var(--text-accent);
  font-weight: 600;
  font-size: 0.85rem;
  margin-left: 0.25rem;
  transition: color 0.2s ease;
}

.toggle-btn:hover {
  color: var(--accent-blue);
}

@media (max-width: 768px) {
  .login-page { grid-template-columns: 1fr; }
  .login-showcase { min-height: 300px; }
}
</style>
