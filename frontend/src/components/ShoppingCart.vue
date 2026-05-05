<template>
  <section class="cart-shell">
    <h2>YOUR CART</h2>
    <div v-if="!items.length" class="empty-cart">Your cart is empty. Add more neon madness!</div>
    <transition-group name="cart-item" tag="div" class="cart-items" v-else>
      <div v-for="item in items" :key="item.product.id" class="cart-item">
        <div>
          <div class="cart-name">{{ item.product.name }}</div>
          <div class="cart-price">{{ item.quantity }} × ₹{{ item.product.price.toLocaleString('en-IN') }}</div>
        </div>
        <div class="cart-controls">
          <button @click="updateQuantity(item.product.id, -1)">-</button>
          <button @click="updateQuantity(item.product.id, 1)">+</button>
          <button @click="removeItem(item.product.id)">Remove</button>
        </div>
      </div>
    </transition-group>
    <div v-if="items.length" class="cart-summary">
      <div class="cart-total">Total: ₹{{ totalAmount.toLocaleString('en-IN') }}</div>
      <button @click="placeOrder" class="order-btn">Place Order</button>
    </div>
  </section>
</template>

<script setup>
import { useCartStore } from '../store/cart';
const cart = useCartStore();

const items = cart.items;
const totalAmount = cart.totalAmount;

const updateQuantity = (id, amount) => cart.updateQuantity(id, amount);
const removeItem = (id) => cart.removeItem(id);
const placeOrder = () => {
  if (!items.length) {
    alert('No items in cart yet. Add something loud!');
    return;
  }
  alert('Order placed on SHOPZONE! Your cart has been cleared.');
  cart.clearCart();
};
</script>

<style scoped>
.cart-shell {
  width: min(980px, 100%);
  padding: 2rem;
  background: #ffffff;
  border-radius: 32px;
  box-shadow: 0 30px 80px rgba(15, 23, 42, 0.08);
  color: #0f172a;
}

.cart-shell h2 {
  margin: 0 0 1.5rem;
  font-size: 2.5rem;
}

.empty-cart {
  background: #f8fafc;
  padding: 1.25rem;
  border-radius: 20px;
  color: #475569;
}

.cart-items {
  display: grid;
  gap: 1rem;
}

.cart-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.1rem 1.25rem;
  background: #f8fafc;
  border-radius: 24px;
}

.cart-item-enter-from,
.cart-item-leave-to {
  opacity: 0;
  transform: translateX(-16px);
}

.cart-item-enter-to,
.cart-item-leave-from {
  opacity: 1;
  transform: translateX(0);
}

.cart-item-enter-active,
.cart-item-leave-active {
  transition: all 0.25s ease;
}

.cart-item-move {
  transition: transform 0.25s ease;
}

.cart-summary {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1rem;
  margin-top: 1rem;
  flex-wrap: wrap;
}

.cart-name {
  font-weight: 700;
  font-size: 1.1rem;
}

.cart-price {
  color: #475569;
}

.cart-controls button {
  margin-left: 0.5rem;
  border: none;
  border-radius: 999px;
  background: #2563eb;
  color: #ffffff;
  padding: 0.55rem 0.85rem;
  cursor: pointer;
}

.cart-total {
  font-size: 1.5rem;
  font-weight: 700;
  background: #f8fafc;
  color: #0f172a;
  padding: 1rem;
  border-radius: 24px;
}

.order-btn {
  width: 100%;
  padding: 1rem;
  background: #2563eb;
  color: #fff;
  border: none;
  border-radius: 999px;
  cursor: pointer;
}
</style>
