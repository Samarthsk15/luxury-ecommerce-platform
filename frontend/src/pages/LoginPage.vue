<template>
  <div class="login-page">
    <a :href="isAdminHost ? 'http://localhost:5173/' : '/'" class="home-link">Back to store</a>

    <div class="login-card">
      <div class="glow-edge"></div>

      <div class="card-content">
        <h1 class="title">{{ titleText }}</h1>
        <p class="subtitle">{{ subtitleText }}</p>

        <div v-if="error" class="error-msg">{{ error }}</div>

        <form v-if="isVerification" @submit.prevent="handleVerification" class="form-wrapper">
          <div class="input-group">
            <span class="input-icon">Code</span>
            <input v-model="verificationCode" type="text" placeholder="Enter verification code" required />
          </div>
          <button type="submit" class="submit-btn" :disabled="submitting">
            <span v-if="submitting" class="spinner"></span>
            <span v-else>Verify Account</span>
          </button>
        </form>

        <form v-else @submit.prevent="handleSubmit" class="form-wrapper animate-in">
          <div v-if="isRegister && !isAdminHost" class="input-group">
            <span class="input-icon">Name</span>
            <input v-model="name" type="text" placeholder="Enter your full name" required />
          </div>

          <div v-if="isRegister && !isAdminHost" class="input-group">
            <span class="input-icon">Phone</span>
            <input v-model="phone" type="tel" placeholder="Enter your phone number (10 digits)" required />
          </div>

          <div v-if="isRegister && !isAdminHost" class="input-group">
            <span class="input-icon">Ship</span>
            <input v-model="address" type="text" placeholder="Enter your full shipping address" required />
          </div>

          <div class="input-group">
            <span class="input-icon">Email</span>
            <input v-model="email" type="email" placeholder="Enter your email address" required />
          </div>

          <div class="input-group">
            <span class="input-icon">Pass</span>
            <input v-model="password" type="password" placeholder="Enter your password" required />
          </div>

          <button type="submit" class="submit-btn" :disabled="submitting">
            <span v-if="submitting" class="spinner"></span>
            <span v-else>{{ isRegister ? 'Sign up' : 'Log in' }}</span>
          </button>

          <div v-if="!isAdminHost" class="social-login">
            <button type="button" class="social-btn">
              <span class="s-icon">f</span> Facebook
            </button>
            <button type="button" class="social-btn" @click="handleGoogleOAuth">
              <span class="s-icon">G</span> Google
            </button>
          </div>
        </form>

        <div class="footer-toggle">
          <template v-if="isVerification">
            Didn't receive the email? <a href="#" @click.prevent="resendCode">Click to resend</a>
          </template>
          <template v-else-if="!isAdminHost">
            {{ isRegister ? 'Already have an account?' : "Didn't have an account?" }}
            <a href="#" @click.prevent="toggleMode">{{ isRegister ? 'Log in' : 'Sign up' }}</a>
          </template>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue';
import { useAuthStore } from '../store/auth.js';
import { useRoute, useRouter } from 'vue-router';

const authStore = useAuthStore();
const router = useRouter();
const route = useRoute();

const isRegister = ref(false);
const isVerification = ref(false);
const name = ref('');
const email = ref('');
const phone = ref('');
const address = ref('');
const password = ref('');
const verificationCode = ref('');
const error = ref('');
const submitting = ref(false);
const isAdminHost = computed(() => typeof window !== 'undefined' && window.location.port === '5174');
const redirectTarget = computed(() => {
  const redirect = route.query.redirect;
  if (typeof redirect === 'string' && redirect.startsWith('/')) return redirect;
  return isAdminHost.value ? '/admin' : '/';
});

const titleText = computed(() => {
  if (isVerification.value) return 'Check your email';
  if (isAdminHost.value) return 'Admin Log in';
  return isRegister.value ? 'Sign up' : 'Log in';
});

const subtitleText = computed(() => {
  if (isVerification.value) {
    return `We sent a verification link to ${email.value}. Please enter the code below to verify your account.`;
  }
  if (isAdminHost.value) {
    return 'Secure access for authorized administrators.';
  }
  if (isRegister.value) {
    return 'Create your account and continue shopping with your saved details.';
  }
  return 'Log in to your customer account and continue shopping.';
});

const toggleMode = () => {
  if (isAdminHost.value) return;
  isRegister.value = !isRegister.value;
  error.value = '';
};

const handleGoogleOAuth = () => {
  if (window.google) {
    const client = window.google.accounts.oauth2.initTokenClient({
      client_id: '552180619464-lmc0nh71d180akcll4tvg9c6pgrbq3pt.apps.googleusercontent.com',
      scope: 'email profile',
      callback: (response) => {
        if (response.access_token) {
          authStore.googleLogin(response.access_token);
          router.push(redirectTarget.value);
        }
      },
    });
    client.requestAccessToken();
  } else {
    authStore.googleLogin('simulated-token');
    router.push(redirectTarget.value);
  }
};

const handleSubmit = async () => {
  error.value = '';

  if (isRegister.value && !isAdminHost.value) {
    if (name.value.trim().length < 3) {
      error.value = 'Name must be at least 3 characters.';
      return;
    }

    const digitsOnly = phone.value.replace(/\D/g, '');
    if (digitsOnly.length !== 10) {
      error.value = 'Please enter a valid 10-digit phone number.';
      return;
    }

    if (address.value.trim().length < 10) {
      error.value = 'Please enter a complete shipping address (minimum 10 characters).';
      return;
    }
  }

  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  if (!emailRegex.test(email.value)) {
    error.value = 'Please enter a valid email address.';
    return;
  }

  if (password.value.length < 6) {
    error.value = 'Password must be at least 6 characters.';
    return;
  }

  submitting.value = true;

  try {
    if (isRegister.value && !isAdminHost.value) {
      await new Promise((resolve) => setTimeout(resolve, 1000));
      isVerification.value = true;
    } else {
      const result = await authStore.login(email.value, password.value);
      if (result.success) {
        router.push(redirectTarget.value);
      } else {
        error.value = result.message || 'Invalid credentials';
      }
    }
  } catch (e) {
    error.value = 'Connection error. Please try again.';
  } finally {
    submitting.value = false;
  }
};

const handleVerification = async () => {
  error.value = '';
  submitting.value = true;

  try {
    await new Promise((resolve) => setTimeout(resolve, 1500));
    const result = await authStore.register(name.value, email.value, password.value, phone.value, address.value);

    if (result.success) {
      router.push('/');
    } else {
      error.value = result.message || 'Verification failed';
    }
  } catch (e) {
    error.value = 'Connection error. Please try again.';
  } finally {
    submitting.value = false;
  }
};

const resendCode = () => {
  alert('Verification code resent to ' + email.value);
};
</script>

<style scoped>
.login-page {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background:
    radial-gradient(circle at top left, rgba(212, 175, 55, 0.16), transparent 34rem),
    linear-gradient(135deg, #fffaf0 0%, #f8fafc 52%, #eef2ff 100%);
  padding: 1rem;
  position: relative;
}

.home-link {
  position: absolute;
  top: 1.25rem;
  left: 1.25rem;
  padding: 0.65rem 1rem;
  border-radius: 999px;
  background: #ffffff;
  color: #111827;
  border: 1px solid rgba(148, 163, 184, 0.35);
  box-shadow: 0 12px 28px rgba(15, 23, 42, 0.08);
  font-size: 0.85rem;
  font-weight: 700;
}

.login-card {
  position: relative;
  width: min(500px, 100%);
  background: rgba(255, 255, 255, 0.94);
  backdrop-filter: blur(18px);
  -webkit-backdrop-filter: blur(18px);
  border-radius: 24px;
  overflow: hidden;
  box-shadow: 0 30px 70px rgba(15, 23, 42, 0.14);
  border: 1px solid rgba(148, 163, 184, 0.28);
}

.glow-edge {
  position: absolute;
  top: 0;
  left: 0;
  bottom: 0;
  width: 2px;
  background: linear-gradient(to bottom, transparent, #d4af37, transparent);
  box-shadow: 0 0 20px 2px rgba(212, 175, 55, 0.2);
}

.card-content {
  padding: 3.5rem 3rem;
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
}

.title {
  font-size: 2.2rem;
  font-weight: 600;
  color: #111827;
  margin-bottom: 1rem;
  font-family: var(--font-headline);
}

.subtitle {
  font-size: 0.9rem;
  color: #475569;
  line-height: 1.6;
  margin-bottom: 2.5rem;
  max-width: 90%;
}

.error-msg {
  width: 100%;
  padding: 0.8rem;
  background: #fef2f2;
  border: 1px solid #fecaca;
  border-radius: 12px;
  color: #b91c1c;
  font-size: 0.85rem;
  margin-bottom: 1.5rem;
}

.form-wrapper {
  width: 100%;
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
}

.input-group {
  position: relative;
  display: flex;
  align-items: center;
}

.input-icon {
  position: absolute;
  left: 1rem;
  width: 3rem;
  color: #64748b;
  font-size: 0.72rem;
  font-weight: 700;
  text-align: left;
  pointer-events: none;
  text-transform: uppercase;
}

.input-group input {
  width: 100%;
  padding: 1.1rem 1.2rem 1.1rem 4.5rem;
  background: #ffffff;
  border: 1px solid #dbe3ef;
  border-radius: 16px;
  color: #111827;
  font-size: 0.95rem;
  transition: all 0.3s ease;
}

.input-group input::placeholder {
  color: #94a3b8;
}

.input-group input:focus {
  border-color: #d4af37;
  box-shadow: 0 0 0 4px rgba(212, 175, 55, 0.12);
}

.submit-btn {
  width: 100%;
  padding: 1.1rem;
  margin-top: 0.5rem;
  background: #111827;
  color: #ffffff;
  border: 1px solid #111827;
  border-radius: 16px;
  font-weight: 700;
  font-size: 1rem;
  transition: all 0.3s ease;
  display: flex;
  justify-content: center;
  align-items: center;
}

.submit-btn:hover:not(:disabled) {
  background: #d4af37;
  border-color: #d4af37;
  color: #111827;
  transform: translateY(-1px);
}

.submit-btn:disabled {
  opacity: 0.55;
  cursor: wait;
}

.social-login {
  display: flex;
  gap: 0.8rem;
  margin-top: 1rem;
  width: 100%;
}

.social-btn {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  padding: 0.9rem;
  background: #ffffff;
  border: 1px solid #dbe3ef;
  border-radius: 16px;
  color: #475569;
  font-size: 0.85rem;
  font-weight: 600;
  transition: all 0.2s ease;
}

.social-btn:hover {
  background: #f8fafc;
  color: #111827;
  border-color: #d4af37;
}

.s-icon {
  font-weight: 800;
  font-family: serif;
}

.footer-toggle {
  margin-top: 2.5rem;
  font-size: 0.85rem;
  color: #64748b;
}

.footer-toggle a {
  color: #9f7a14;
  margin-left: 0.3rem;
  font-weight: 700;
  transition: color 0.2s;
}

.footer-toggle a:hover {
  color: #111827;
  text-decoration: underline;
}

.spinner {
  width: 20px;
  height: 20px;
  border: 2px solid rgba(255, 255, 255, 0.35);
  border-top-color: #ffffff;
  border-radius: 50%;
  animation: spin 0.6s linear infinite;
}

.animate-in {
  animation: fadeIn 0.4s ease-out forwards;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(10px); }
  to { opacity: 1; transform: translateY(0); }
}

@keyframes spin {
  from { transform: rotate(0deg); }
  to { transform: rotate(360deg); }
}

@media (max-width: 520px) {
  .login-page {
    align-items: flex-start;
    padding-top: 5rem;
  }

  .card-content {
    padding: 2.5rem 1.5rem;
  }

  .social-login {
    flex-direction: column;
  }
}
</style>
