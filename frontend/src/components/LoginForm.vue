<template>
  <section class="login-shell">
    <form @submit.prevent="submitLogin" class="login-form">
      <h2>LOGIN TO SHOPZONE</h2>
      <label>
        Email
        <input v-model="email" type="email" placeholder="you@example.com" required />
      </label>
      <label>
        Password
        <input v-model="password" type="password" placeholder="Enter password" required />
      </label>
      <button type="submit" :disabled="loading">
        {{ loading ? 'Logging in...' : 'Login' }}
      </button>
      <p v-if="error" class="error">{{ error }}</p>
      <p class="hint">Default admin: admin@shop.local / Admin@123</p>
      <p class="register-link">
        New user? <button type="button" @click="showRegister = !showRegister" class="link-button">
          {{ showRegister ? 'Back to Login' : 'Register Here' }}
        </button>
      </p>
    </form>

    <form v-if="showRegister" @submit.prevent="submitRegister" class="login-form register-form">
      <h2>REGISTER FOR SHOPZONE</h2>
      <label>
        Name
        <input v-model="registerName" type="text" placeholder="Your full name" required />
      </label>
      <label>
        Email
        <input v-model="registerEmail" type="email" placeholder="you@example.com" required />
      </label>
      <label>
        Password
        <input v-model="registerPassword" type="password" placeholder="Enter password" required />
      </label>
      <button type="submit" :disabled="loading">
        {{ loading ? 'Registering...' : 'Register' }}
      </button>
      <p v-if="error" class="error">{{ error }}</p>
    </form>
  </section>
</template>

<script setup>
import { ref } from 'vue';
import { useAuthStore } from '../store/auth.js';
import { useRouter } from 'vue-router';

const authStore = useAuthStore();
const router = useRouter();

const email = ref('');
const password = ref('');
const registerName = ref('');
const registerEmail = ref('');
const registerPassword = ref('');
const loading = ref(false);
const error = ref('');
const showRegister = ref(false);

const submitLogin = async () => {
  loading.value = true;
  error.value = '';

  const result = await authStore.login(email.value, password.value);

  if (result.success) {
    router.push('/');
  } else {
    error.value = result.message || 'Invalid email or password';
  }

  loading.value = false;
};

const submitRegister = async () => {
  loading.value = true;
  error.value = '';

  const result = await authStore.register(registerName.value, registerEmail.value, registerPassword.value);

  if (result.success) {
    router.push('/');
  } else {
    error.value = result.message || 'Registration failed. Email might already be registered.';
  }

  loading.value = false;
};
</script>

<style scoped>
.login-shell {
  padding: 2rem;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 2rem;
}

.login-form {
  width: min(460px, 100%);
  padding: 2rem;
  border-radius: 32px;
  background: #ffffff;
  box-shadow: 0 30px 80px rgba(15, 23, 42, 0.08);
}

.register-form {
  margin-top: 1rem;
}

.login-form h2 {
  margin-bottom: 1.25rem;
  font-size: 2rem;
  color: #0f172a;
}

label {
  display: grid;
  gap: 0.55rem;
  margin-bottom: 1.25rem;
  color: #475569;
  font-weight: 500;
}

input {
  width: 100%;
  padding: 1rem 1.1rem;
  border-radius: 18px;
  border: 1px solid #cbd5e1;
  outline: none;
  background: #f8fafc;
  color: #0f172a;
}

button {
  width: 100%;
  margin-top: 0.75rem;
  border: none;
  border-radius: 999px;
  background: #2563eb;
  color: #fff;
  padding: 1rem;
  font-weight: 700;
  cursor: pointer;
}

button:disabled {
  background: #94a3b8;
  cursor: not-allowed;
}

.error {
  color: #dc2626;
  margin-top: 1rem;
  text-align: center;
}

.hint {
  margin-top: 1rem;
  color: #64748b;
  text-align: center;
}

.register-link {
  margin-top: 1rem;
  text-align: center;
  color: #64748b;
}

.link-button {
  background: none;
  border: none;
  color: #2563eb;
  cursor: pointer;
  text-decoration: underline;
  font-size: inherit;
  padding: 0;
}
</style>
