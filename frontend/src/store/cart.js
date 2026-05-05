import { defineStore } from 'pinia';

export const useCartStore = defineStore('cart', {
  state: () => ({
    items: [],
  }),
  getters: {
    itemCount: (state) => state.items.reduce((count, item) => count + item.quantity, 0),
    totalAmount: (state) => state.items.reduce((total, item) => total + item.quantity * item.product.price, 0),
  },
  actions: {
    addItem(product) {
      const existing = this.items.find((item) => item.product.id === product.id);
      if (existing) {
        existing.quantity += 1;
      } else {
        this.items.push({ product, quantity: 1 });
      }
    },
    removeItem(productId) {
      this.items = this.items.filter((item) => item.product.id !== productId);
    },
    updateQuantity(productId, amount) {
      const item = this.items.find((entry) => entry.product.id === productId);
      if (!item) return;
      item.quantity += amount;
      if (item.quantity < 1) {
        this.removeItem(productId);
      }
    },
    clearCart() {
      this.items = [];
    },
  },
});
