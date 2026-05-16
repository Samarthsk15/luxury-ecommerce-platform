<template>
  <section class="cart-shell glass-card">
    <h2 class="cart-title">YOUR CART</h2>
    <div v-if="!items.length" class="empty-cart glass">
      <span class="empty-icon">🛍️</span>
      <p>Your cart is empty. Add more premium essentials!</p>
      <button class="btn-ghost-small" @click="$router.push('/products')" v-magnetic>Browse Catalog</button>
    </div>
    
    <transition-group name="cart-item" tag="div" class="cart-items" v-else>
      <div v-for="item in items" :key="item.product.id" class="cart-item glass">
        <div class="cart-item-info">
          <img v-if="item.product.imageUrl" :src="item.product.imageUrl" :alt="item.product.name" class="cart-img" />
          <div class="cart-details">
            <div class="cart-name">{{ item.product.name }}</div>
            <div class="cart-price">{{ item.quantity }} × ₹{{ item.product.price.toLocaleString('en-IN') }}</div>
          </div>
        </div>
        
        <div class="cart-controls">
          <button class="control-btn" @click="updateQuantity(item.product.id, -1)" v-magnetic>−</button>
          <span class="quantity-display">{{ item.quantity }}</span>
          <button class="control-btn" @click="updateQuantity(item.product.id, 1)" v-magnetic>+</button>
          <button class="remove-btn" @click="removeItem(item.product.id)" v-magnetic>Remove</button>
        </div>
      </div>
    </transition-group>
    
    <div v-if="items.length" class="cart-summary">
      <div class="cart-total glass">
        <span class="total-label">Total Amount:</span>
        <span class="total-amount">₹{{ totalAmount.toLocaleString('en-IN') }}</span>
      </div>
      <button @click="placeOrder" class="order-btn" v-magnetic>
        <span>Proceed to Checkout</span>
        <span class="arrow">→</span>
      </button>
    </div>
  </section>
</template>

<script setup>
import { useCartStore } from '../store/cart';
import { useRouter } from 'vue-router';

const cart = useCartStore();
const router = useRouter();

const items = cart.items;
const totalAmount = cart.totalAmount;

const updateQuantity = (id, amount) => cart.updateQuantity(id, amount);
const removeItem = (id) => cart.removeItem(id);
const placeOrder = () => {
  if (!items.length) {
    alert('No items in cart yet.');
    return;
  }
  router.push('/checkout');
};
</script>

<style scoped>
.cart-shell {
  width: 100%;
  max-width: 900px;
  margin: 0 auto;
  padding: 2.5rem;
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

.cart-title {
  font-size: 2rem;
  font-weight: 800;
  letter-spacing: -0.03em;
  color: var(--text-primary);
  margin-bottom: 0.5rem;
}

/* Empty State */
.empty-cart {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 1.5rem;
  padding: 4rem 2rem;
  border-radius: var(--radius-xl);
  text-align: center;
}

.empty-icon {
  font-size: 3rem;
  filter: grayscale(0.5);
  opacity: 0.5;
}

.empty-cart p {
  color: var(--text-secondary);
  font-size: 1.1rem;
}

.btn-ghost-small {
  padding: 0.6rem 1.2rem;
  border-radius: var(--radius-full);
  background: transparent;
  color: var(--text-primary);
  border: 1px solid var(--border-glass);
  cursor: pointer;
  font-size: 0.9rem;
  font-weight: 600;
  transition: all 0.3s;
}

.btn-ghost-small:hover {
  background: rgba(255, 255, 255, 0.1);
}

/* Cart Items Grid */
.cart-items {
  display: flex;
  flex-direction: column;
  gap: 1.2rem;
}

.cart-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.2rem;
  border-radius: var(--radius-lg);
  background: var(--bg-card);
  transition: all 0.3s ease;
}

.cart-item-info {
  display: flex;
  align-items: center;
  gap: 1.2rem;
}

.cart-img {
  width: 64px;
  height: 64px;
  object-fit: cover;
  border-radius: var(--radius-md);
  border: 1px solid var(--border-glass);
}

.cart-details {
  display: flex;
  flex-direction: column;
  gap: 0.3rem;
}

.cart-name {
  font-weight: 700;
  font-size: 1.1rem;
  color: var(--text-primary);
}

.cart-price {
  color: var(--text-secondary);
  font-size: 0.95rem;
  font-family: var(--font-headline);
}

.cart-controls {
  display: flex;
  align-items: center;
  gap: 0.8rem;
  background: rgba(0, 0, 0, 0.2);
  padding: 0.4rem;
  border-radius: var(--radius-full);
}

.control-btn {
  width: 32px;
  height: 32px;
  border-radius: 50%;
  border: none;
  background: rgba(255, 255, 255, 0.05);
  color: var(--text-primary);
  font-size: 1.2rem;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: all 0.2s;
}

.control-btn:hover {
  background: rgba(255, 255, 255, 0.15);
}

.quantity-display {
  font-weight: 700;
  min-width: 20px;
  text-align: center;
}

.remove-btn {
  margin-left: 0.5rem;
  padding: 0.5rem 1rem;
  border: none;
  border-radius: var(--radius-full);
  background: rgba(239, 68, 68, 0.15);
  color: #fca5a5;
  font-weight: 600;
  font-size: 0.85rem;
  cursor: pointer;
  transition: all 0.2s;
}

.remove-btn:hover {
  background: rgba(239, 68, 68, 0.3);
  color: #fff;
}

/* Animations: Fade + Shrink */
.cart-item-enter-from,
.cart-item-leave-to {
  opacity: 0;
  transform: scale(0.85) translateY(10px);
}

.cart-item-enter-to,
.cart-item-leave-from {
  opacity: 1;
  transform: scale(1) translateY(0);
}

.cart-item-enter-active,
.cart-item-leave-active {
  transition: all 0.4s var(--ease-spring);
}

.cart-item-leave-active {
  position: absolute; /* Ensures smooth list transition for remaining items */
  width: calc(100% - 4rem); /* account for padding */
}

.cart-item-move {
  transition: transform 0.4s var(--ease-spring);
}

/* Summary Section */
.cart-summary {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 1.5rem;
  padding-top: 2rem;
  border-top: 1px dashed var(--border-glass);
}

.cart-total {
  display: flex;
  flex-direction: column;
  gap: 0.3rem;
  padding: 1rem 1.5rem;
  border-radius: var(--radius-lg);
}

.total-label {
  font-size: 0.85rem;
  color: var(--text-muted);
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

.total-amount {
  font-size: 1.8rem;
  font-weight: 800;
  font-family: var(--font-headline);
  color: var(--text-primary);
}

.order-btn {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 1.2rem 2.5rem;
  background: var(--text-primary);
  color: var(--bg-void);
  border: none;
  border-radius: var(--radius-full);
  font-size: 1.1rem;
  font-weight: 700;
  cursor: pointer;
  transition: all 0.3s;
  box-shadow: 0 10px 30px rgba(255, 255, 255, 0.1);
}

.order-btn:hover {
  transform: translateY(-2px);
  background: #fff;
  box-shadow: 0 15px 40px rgba(255, 255, 255, 0.2);
}

.arrow {
  transition: transform 0.3s;
}

.order-btn:hover .arrow {
  transform: translateX(4px);
}

@media (max-width: 640px) {
  .cart-item {
    flex-direction: column;
    align-items: flex-start;
    gap: 1.5rem;
  }
  .cart-controls {
    width: 100%;
    justify-content: space-between;
  }
  .cart-summary {
    flex-direction: column;
    gap: 1.5rem;
  }
  .order-btn {
    width: 100%;
    justify-content: center;
  }
}
</style>
