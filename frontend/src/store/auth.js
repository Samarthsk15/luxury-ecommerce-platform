import { defineStore } from 'pinia';
import { ref, computed } from 'vue';

export const useAuthStore = defineStore('auth', () => {
  const user = ref(null);
  const token = ref(localStorage.getItem('token') || null);

  const isAuthenticated = computed(() => !!token.value);

  const login = async (email, password) => {
    try {
      const response = await fetch('http://localhost:5000/api/auth/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email, password }),
      });

      const data = await response.json().catch(() => null);
      if (!response.ok) {
        const message = data?.message || 'Login failed';
        return { success: false, message };
      }

      user.value = {
        id: data.id,
        name: data.name,
        email: data.email,
        role: data.role,
      };
      token.value = data.token;
      localStorage.setItem('token', data.token);
      return { success: true };
    } catch (error) {
      console.error('Login error:', error);
      return { success: false, message: 'Unable to connect to the server.' };
    }
  };

  const register = async (name, email, password) => {
    try {
      const response = await fetch('http://localhost:5000/api/auth/register', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ name, email, password }),
      });

      const data = await response.json().catch(() => null);
      if (!response.ok) {
        const message = data?.message || 'Registration failed';
        return { success: false, message };
      }

      user.value = {
        id: data.id,
        name: data.name,
        email: data.email,
        role: data.role,
      };
      token.value = data.token;
      localStorage.setItem('token', data.token);
      return { success: true };
    } catch (error) {
      console.error('Registration error:', error);
      return { success: false, message: 'Unable to connect to the server.' };
    }
  };

  const logout = () => {
    user.value = null;
    token.value = null;
    localStorage.removeItem('token');
  };

  const checkAuth = () => {
    const storedToken = localStorage.getItem('token');
    if (storedToken) {
      token.value = storedToken;
      // You might want to validate the token with the server here
    }
  };

  return {
    user,
    token,
    isAuthenticated,
    login,
    register,
    logout,
    checkAuth,
  };
});