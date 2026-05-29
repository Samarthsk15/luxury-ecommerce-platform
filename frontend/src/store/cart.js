import { defineStore } from 'pinia';

const CART_KEY = 'cart_local';

export const useCartStore = defineStore('cart', {
  state: () => ({
    items: JSON.parse(localStorage.getItem(CART_KEY) || '[]'),
  }),
  getters: {
    itemCount: (state) => state.items.reduce((count, item) => count + item.quantity, 0),
    totalAmount: (state) => state.items.reduce((total, item) => total + item.quantity * (item.product?.price || 0), 0),
  },
  actions: {
    _saveLocal() {
      try {
        localStorage.setItem(CART_KEY, JSON.stringify(this.items));
      } catch (e) {
        console.error('Failed to save cart to localStorage', e);
      }
    },
    addItem(product) {
      const existing = this.items.find((item) => item.product.id === product.id);
      if (existing) {
        existing.quantity += 1;
      } else {
        this.items.push({ product, quantity: 1 });
      }
      this._saveLocal();
    },
    removeItem(productId) {
      this.items = this.items.filter((item) => item.product.id !== productId);
      this._saveLocal();
    },
    updateQuantity(productId, amount) {
      const item = this.items.find((entry) => entry.product.id === productId);
      if (!item) return;
      item.quantity += amount;
      if (item.quantity < 1) {
        this.removeItem(productId);
        return;
      }
      this._saveLocal();
    },
    clearLocal() {
      this.items = [];
      try {
        localStorage.removeItem(CART_KEY);
      } catch (e) {
        console.error('Failed to remove cart from localStorage', e);
      }
    },
    async mergeAfterLogin(token) {
      if (!this.items || this.items.length === 0) return;
      try {
        // Attempt to merge local cart into server-side cart if API exists.
        await fetch('/api/cart/merge', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            ...(token ? { Authorization: `Bearer ${token}` } : {}),
          },
          body: JSON.stringify({ items: this.items }),
        });
        // On success, clear local copy (server becomes source of truth).
        this.clearLocal();
      } catch (e) {
        console.warn('Cart merge failed; keeping local cart', e);
      }
    },
  },
});
