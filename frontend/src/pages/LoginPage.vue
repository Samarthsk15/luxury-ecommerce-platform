<template>
  <div class="login-page">
    <div class="login-card">
      <div class="glow-edge"></div>
      
      <div class="card-content">
        <h1 class="title">{{ titleText }}</h1>
        <p class="subtitle">{{ subtitleText }}</p>

        <div v-if="error" class="error-msg">{{ error }}</div>

        <!-- Verification Step -->
        <form v-if="isVerification" @submit.prevent="handleVerification" class="form-wrapper">
          <div class="input-group">
            <span class="input-icon">🔑</span>
            <input v-model="verificationCode" type="text" placeholder="Enter verification code" required />
          </div>
          <button type="submit" class="submit-btn" :disabled="submitting">
            <span v-if="submitting" class="spinner"></span>
            <span v-else>Verify Account</span>
          </button>
        </form>

        <!-- Login / Register Step -->
        <form v-else @submit.prevent="handleSubmit" class="form-wrapper animate-in">
          <div v-if="isRegister && !isAdminLogin" class="input-group">
            <span class="input-icon">👤</span>
            <input v-model="name" type="text" placeholder="Enter your full name" required />
          </div>

          <div v-if="isRegister && !isAdminLogin" class="input-group">
            <span class="input-icon">📱</span>
            <input v-model="phone" type="tel" placeholder="Enter your phone number (10 digits)" required />
          </div>

          <div v-if="isRegister && !isAdminLogin" class="input-group">
            <span class="input-icon">🏠</span>
            <input v-model="address" type="text" placeholder="Enter your full shipping address" required />
          </div>

          <div class="input-group">
            <span class="input-icon">✉️</span>
            <input v-model="email" type="email" placeholder="Enter your email address" required />
          </div>

          <div class="input-group">
            <span class="input-icon">🔒</span>
            <input v-model="password" type="password" placeholder="Enter your password" required />
            <span class="input-icon-right">🔵</span>
          </div>

          <button type="submit" class="submit-btn" :disabled="submitting" :style="isAdminLogin ? 'background: rgba(212, 175, 55, 0.15); border: 1px solid rgba(212, 175, 55, 0.5); color: #d4af37;' : ''">
            <span v-if="submitting" class="spinner"></span>
            <span v-else>{{ isAdminLogin ? 'Secure Admin Login' : (isRegister ? 'Sign up' : 'Log in') }}</span>
          </button>

          <!-- Standard Social Login (Quick Admin switch button completely removed!) -->
          <div v-if="!isAdminLogin" class="social-login">
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
          <template v-else-if="isAdminLogin && !isAdminPort">
            Not an administrator?
            <a href="#" @click.prevent="toggleAdminMode">Return to user login</a>
          </template>
          <template v-else-if="!isAdminPort">
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
import { useRouter } from 'vue-router';

const authStore = useAuthStore();
const router = useRouter();

const isRegister = ref(false);
const isVerification = ref(false);

const isAdminPort = computed(() => typeof window !== 'undefined' && window.location.port === '5174');
// Automatically set to admin login if running on port 5174
const isAdminLogin = ref(typeof window !== 'undefined' && window.location.port === '5174');

const name = ref('');
const email = ref('');
const phone = ref('');
const address = ref('');
const password = ref('');
const verificationCode = ref('');
const error = ref('');
const submitting = ref(false);

const titleText = computed(() => {
  if (isVerification.value) return 'Check your email';
  if (isAdminLogin.value) return 'Admin Portal';
  return isRegister.value ? 'Sign up' : 'Log in';
});

const subtitleText = computed(() => {
  if (isVerification.value) {
    return `We sent a verification link to ${email.value}. Please enter the code below to verify your account.`;
  }
  if (isAdminLogin.value) {
    return 'Secure access for authorized administrators only.';
  }
  if (isRegister.value) {
    return 'Create your account and seamlessly start managing your projects, ideas, and progress.';
  }
  return 'Log in to your account and seamlessly continue managing your projects, ideas, and progress just where you left off.';
});

const toggleMode = () => {
  isRegister.value = !isRegister.value;
  error.value = '';
};

const toggleAdminMode = () => {
  isAdminLogin.value = !isAdminLogin.value;
  isRegister.value = false;
  error.value = '';
  email.value = '';
  password.value = '';
  phone.value = '';
  address.value = '';
};

const handleGoogleOAuth = () => {
  if (window.google) {
    const client = window.google.accounts.oauth2.initTokenClient({
      client_id: '552180619464-lmc0nh71d180akcll4tvg9c6pgrbq3pt.apps.googleusercontent.com',
      scope: 'email profile',
      callback: (response) => {
        if (response.access_token) {
          authStore.googleLogin(response.access_token);
          router.push('/');
        }
      },
    });
    client.requestAccessToken();
  } else {
    // If the script is missing, simulate a successful Google Login for testing
    authStore.googleLogin('simulated-token');
    router.push('/');
  }
};

const handleSubmit = async () => {
  error.value = '';

  if (isRegister.value && !isAdminLogin.value) {
    if (name.value.trim().length < 3) {
      error.value = 'Name must be at least 3 characters.';
      return;
    }
    
    // Phone validation: must contain exactly 10 digits
    const digitsOnly = phone.value.replace(/\D/g, '');
    if (digitsOnly.length !== 10) {
      error.value = 'Please enter a valid 10-digit phone number.';
      return;
    }
    
    // Address validation: minimum length
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
    if (isAdminLogin.value) {
      // Simulate Admin Verification for Presentation
      if (email.value.includes('admin') && password.value === 'admin123') {
        await new Promise(res => setTimeout(res, 800));
        authStore.demoAdminLogin();
        router.push('/admin');
      } else {
        await new Promise(res => setTimeout(res, 800));
        error.value = 'Invalid administrator credentials. (Hint: admin@luxe.com / admin123)';
      }
    } else if (isRegister.value) {
      // Simulate sending verification email
      await new Promise(res => setTimeout(res, 1000));
      isVerification.value = true;
    } else {
      const result = await authStore.login(email.value, password.value);
      if (result.success) {
        router.push('/');
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
    // Simulate verifying code and registering user
    await new Promise(res => setTimeout(res, 1500));
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
  alert("Verification code resent to " + email.value);
};
</script>

<style scoped>
.login-page {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: var(--bg-void);
  padding: 1rem;
  transition: background 0.3s ease;
}

.login-card {
  position: relative;
  width: 100%;
  max-width: 500px;
  background: var(--bg-surface);
  backdrop-filter: blur(24px);
  -webkit-backdrop-filter: blur(24px);
  border-radius: 24px;
  overflow: hidden;
  box-shadow: 0 30px 60px rgba(0, 0, 0, 0.15);
  border: 1px solid var(--border-glass);
  transition: all 0.3s ease;
}

.glow-edge {
  position: absolute;
  top: 0;
  left: 0;
  bottom: 0;
  width: 1px;
  background: linear-gradient(to bottom, transparent, var(--text-accent), transparent);
  box-shadow: 0 0 20px 2px var(--accent-glow);
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
  color: var(--text-primary);
  margin-bottom: 1rem;
  font-family: var(--font-headline);
}

.subtitle {
  font-size: 0.9rem;
  color: var(--text-secondary);
  line-height: 1.6;
  margin-bottom: 2.5rem;
  max-width: 90%;
}

.error-msg {
  width: 100%;
  padding: 0.8rem;
  background: rgba(239, 68, 68, 0.1);
  border: 1px solid rgba(239, 68, 68, 0.2);
  border-radius: 12px;
  color: #fca5a5;
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
  left: 1.2rem;
  font-size: 1.1rem;
  opacity: 0.5;
  pointer-events: none;
  color: var(--text-primary);
}

.input-icon-right {
  position: absolute;
  right: 1.2rem;
  font-size: 0.8rem;
  pointer-events: none;
  color: var(--text-muted);
}

.input-group input {
  width: 100%;
  padding: 1.1rem 1.2rem 1.1rem 3rem;
  background: rgba(255, 255, 255, 0.02);
  border: 1px solid var(--border-glass);
  border-radius: 16px;
  color: var(--text-primary);
  font-size: 0.95rem;
  transition: all 0.3s ease;
  outline: none;
}

.input-group input::placeholder {
  color: var(--text-muted);
}

.input-group input:focus {
  border-color: var(--text-accent);
  background: rgba(255, 255, 255, 0.04);
  box-shadow: inset 0 0 0 1px var(--border-glass-hover);
}

.submit-btn {
  width: 100%;
  padding: 1.1rem;
  margin-top: 0.5rem;
  background: var(--text-primary);
  color: var(--bg-void);
  border: 1px solid var(--border-glass);
  border-radius: 16px;
  font-weight: 600;
  font-size: 1rem;
  transition: all 0.3s ease;
  display: flex;
  justify-content: center;
  align-items: center;
  cursor: pointer;
}

.submit-btn:hover:not(:disabled) {
  background: var(--text-secondary);
  color: var(--bg-void);
  transform: translateY(-1px);
}

.submit-btn:disabled {
  opacity: 0.5;
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
  background: transparent;
  border: 1px solid var(--border-glass);
  border-radius: 16px;
  color: var(--text-secondary);
  font-size: 0.85rem;
  font-weight: 500;
  transition: all 0.2s ease;
  cursor: pointer;
}

.social-btn:hover {
  background: rgba(255, 255, 255, 0.05);
  color: var(--text-primary);
  border-color: var(--border-glass-hover);
}

.s-icon {
  font-weight: bold;
  font-family: serif;
}

.footer-toggle {
  margin-top: 2.5rem;
  font-size: 0.85rem;
  color: var(--text-muted);
}

.footer-toggle a {
  color: var(--text-accent);
  margin-left: 0.3rem;
  font-weight: 500;
  transition: color 0.2s;
}

.footer-toggle a:hover {
  color: var(--text-primary);
  text-decoration: underline;
}

.spinner {
  width: 20px;
  height: 20px;
  border: 2px solid rgba(255, 255, 255, 0.3);
  border-top-color: var(--text-primary);
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

@media (max-width: 480px) {
  .card-content {
    padding: 2.5rem 1.5rem;
  }
  .social-login {
    flex-direction: column;
  }
}
</style>
