<template>
  <div class="cart-page">
    <h1 class="page-title">Your Cart</h1>
    <p class="page-sub">{{ cartStore.itemCount }} item{{ cartStore.itemCount !== 1 ? 's' : '' }} in your bag</p>

    <div v-if="cartStore.items.length === 0" class="empty-cart">
      <div class="empty-icon">🛒</div>
      <h2>Your cart is empty</h2>
      <p>Looks like you haven't added anything yet.</p>
      <router-link to="/" class="shop-btn">Start Shopping →</router-link>
    </div>

    <div v-else class="cart-layout">
      <!-- Items List -->
      <div class="cart-items">
        <div v-for="item in cartStore.items" :key="item.product.id" class="cart-item glass-card">
          <div class="item-img">
            <img :src="item.product.imageUrl" :alt="item.product.name" />
          </div>
          <div class="item-details">
            <h3 class="item-name">{{ item.product.name }}</h3>
            <span class="item-price">₹{{ item.product.price.toLocaleString('en-IN') }}</span>
          </div>
          <div class="item-qty">
            <button @click="cartStore.updateQuantity(item.product.id, -1)" class="qty-btn">−</button>
            <span class="qty-num">{{ item.quantity }}</span>
            <button @click="cartStore.updateQuantity(item.product.id, 1)" class="qty-btn">+</button>
          </div>
          <div class="item-subtotal">
            ₹{{ (item.product.price * item.quantity).toLocaleString('en-IN') }}
          </div>
          <button @click="cartStore.removeItem(item.product.id)" class="remove-btn">✕</button>
        </div>
      </div>

      <!-- Order Summary -->
      <aside class="order-summary glass-card">
        <h3 class="summary-title">Order Summary</h3>
        <div class="summary-rows">
          <div class="summary-row">
            <span>Subtotal</span>
            <span>₹{{ cartStore.totalAmount.toLocaleString('en-IN') }}</span>
          </div>
          <div class="summary-row">
            <span>Shipping</span>
            <span class="free-tag">FREE</span>
          </div>
          <div class="summary-row">
            <span>Tax (18%)</span>
            <span>₹{{ Math.round(cartStore.totalAmount * 0.18).toLocaleString('en-IN') }}</span>
          </div>
        </div>
        <div class="summary-divider"></div>
        <div class="summary-total">
          <span>Total</span>
          <span class="total-price">₹{{ Math.round(cartStore.totalAmount * 1.18).toLocaleString('en-IN') }}</span>
        </div>
        <button class="checkout-btn">Proceed to Checkout</button>
        <router-link to="/" class="continue-link">← Continue Shopping</router-link>
      </aside>
    </div>
  </div>
</template>

<script setup>
import { useCartStore } from '../store/cart';
const cartStore = useCartStore();
</script>

<style scoped>
.cart-page {
  display: flex;
  flex-direction: column;
  gap: var(--space-md);
}

.page-title {
  font-size: 2.25rem;
  font-weight: 800;
  letter-spacing: -0.03em;
}

.page-sub {
  color: var(--text-secondary);
  font-size: 0.9rem;
  margin-top: -0.5rem;
}

/* Empty State */
.empty-cart {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1rem;
  padding: 5rem 2rem;
  text-align: center;
}

.empty-icon { font-size: 4rem; opacity: 0.3; }

.empty-cart h2 {
  font-size: 1.5rem;
  font-weight: 700;
}

.empty-cart p {
  color: var(--text-secondary);
}

.shop-btn {
  margin-top: 0.5rem;
  padding: 0.8rem 1.75rem;
  background: var(--accent-gradient);
  color: white;
  border-radius: var(--radius-full);
  font-weight: 700;
  font-size: 0.9rem;
  transition: all 0.3s ease;
  box-shadow: 0 6px 24px var(--accent-glow);
}

.shop-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 10px 32px var(--accent-glow);
}

/* Layout */
.cart-layout {
  display: grid;
  grid-template-columns: 1fr 360px;
  gap: var(--space-lg);
  align-items: flex-start;
  margin-top: var(--space-sm);
}

/* Cart Items */
.cart-items {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.cart-item {
  display: grid;
  grid-template-columns: 80px 1fr auto auto auto;
  align-items: center;
  gap: 1.25rem;
  padding: 1.25rem;
  border-radius: var(--radius-lg);
}

.item-img {
  width: 80px;
  height: 80px;
  border-radius: var(--radius-md);
  overflow: hidden;
  background: var(--bg-elevated);
}

.item-img img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.item-details {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.item-name {
  font-size: 0.95rem;
  font-weight: 700;
}

.item-price {
  font-size: 0.8rem;
  color: var(--text-secondary);
}

/* Quantity */
.item-qty {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.qty-btn {
  width: 32px;
  height: 32px;
  border-radius: var(--radius-sm);
  background: rgba(255, 255, 255, 0.06);
  border: 1px solid var(--border-glass);
  color: var(--text-primary);
  font-size: 1rem;
  font-weight: 600;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.2s ease;
}

.qty-btn:hover {
  background: rgba(255, 255, 255, 0.1);
  border-color: var(--border-glass-hover);
}

.qty-num {
  font-family: var(--font-headline);
  font-weight: 700;
  font-size: 0.9rem;
  min-width: 24px;
  text-align: center;
}

.item-subtotal {
  font-family: var(--font-headline);
  font-weight: 800;
  font-size: 1rem;
  min-width: 90px;
  text-align: right;
}

.remove-btn {
  width: 32px;
  height: 32px;
  border-radius: var(--radius-sm);
  background: rgba(239, 68, 68, 0.08);
  color: var(--error);
  font-size: 0.8rem;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.2s ease;
}

.remove-btn:hover {
  background: rgba(239, 68, 68, 0.2);
}

/* Summary */
.order-summary {
  padding: 1.75rem;
  border-radius: var(--radius-xl);
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
  position: sticky;
  top: 100px;
}

.summary-title {
  font-size: 1.1rem;
  font-weight: 800;
}

.summary-rows {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.summary-row {
  display: flex;
  justify-content: space-between;
  font-size: 0.9rem;
  color: var(--text-secondary);
}

.free-tag {
  color: var(--success);
  font-weight: 600;
}

.summary-divider {
  height: 1px;
  background: var(--border-glass);
}

.summary-total {
  display: flex;
  justify-content: space-between;
  font-family: var(--font-headline);
  font-weight: 800;
  font-size: 1.15rem;
}

.total-price {
  font-size: 1.3rem;
}

.checkout-btn {
  padding: 0.9rem;
  border-radius: var(--radius-md);
  background: var(--accent-gradient);
  color: white;
  font-weight: 700;
  font-size: 0.95rem;
  transition: all 0.3s ease;
  box-shadow: 0 6px 24px var(--accent-glow);
}

.checkout-btn:hover {
  transform: translateY(-1px);
  box-shadow: 0 10px 32px var(--accent-glow);
}

.continue-link {
  text-align: center;
  font-size: 0.85rem;
  color: var(--text-secondary);
  transition: color 0.2s ease;
}

.continue-link:hover {
  color: var(--text-accent);
}

@media (max-width: 900px) {
  .cart-layout { grid-template-columns: 1fr; }
  .cart-item { grid-template-columns: 60px 1fr auto auto; gap: 0.75rem; }
  .item-subtotal { display: none; }
}
</style>
