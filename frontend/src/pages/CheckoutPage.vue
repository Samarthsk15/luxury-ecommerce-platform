<template>
  <div class="checkout-page">
    <transition name="toast-slide">
      <div v-if="toastVisible" class="toast-notification" :class="toastType">
        <div class="toast-icon">{{ toastType === 'success' ? '🎉' : '⚠️' }}</div>
        <div class="toast-text">
          <strong>{{ toastTitle }}</strong>
          <p>{{ toastMessage }}</p>
        </div>
      </div>
    </transition>

    <div class="checkout-container">
      
      <!-- Left Column: Checkout Steps -->
      <div class="checkout-steps">
        <h1 class="page-title">Checkout</h1>
        
        <!-- Step 1: Shipping Address -->
        <div class="step-card glass-card" :class="{ 'step-active': currentStep === 1, 'step-completed': currentStep > 1 }">
          <div class="step-header">
            <div class="step-number">1</div>
            <h2>Delivery Address</h2>
            <button v-if="currentStep === 1" @click="smartAutoFill" class="ai-autofill-btn" title="AI Auto-Fill">✨ Smart Fill</button>
            <button v-if="currentStep > 1" @click="currentStep = 1" class="edit-btn">Change</button>
          </div>
          <div class="step-content" v-if="currentStep === 1">
            <div class="form-grid">
              <div class="input-group">
                <input v-model="address.fullName" type="text" placeholder="Full Name" required />
              </div>
              <div class="input-group">
                <input v-model="address.phone" type="text" placeholder="Phone Number" required />
              </div>
              <div class="input-group full-width">
                <input v-model="address.street" type="text" placeholder="Street Address, Area, Landmark" required />
              </div>
              <div class="input-group">
                <input v-model="address.city" type="text" placeholder="City" required />
              </div>
              <div class="input-group">
                <input v-model="address.pincode" type="text" placeholder="Pincode" required />
              </div>
              <div v-if="addressError" class="address-error" style="color: #ef4444; font-size: 0.9rem; grid-column: 1 / -1; text-align: center;">{{ addressError }}</div>
            </div>
            <button class="step-btn" @click="handleAddressSubmit" v-magnetic>Deliver to this address</button>
          </div>
          <div class="step-summary" v-else>
            <p><strong>{{ address.fullName || 'John Doe' }}</strong>, {{ address.phone || '+91 9876543210' }}</p>
            <p>{{ address.street || '123 Luxury Avenue' }}, {{ address.city || 'Tech City' }}, {{ address.pincode || '560001' }}</p>
          </div>
        </div>

        <!-- Step 2: Payment Method (Amazon/Flipkart Accordion Style) -->
        <div class="step-card glass-card" :class="{ 'step-active': currentStep === 2, 'step-disabled': currentStep < 2 }">
          <div class="step-header">
            <div class="step-number">2</div>
            <h2>Select a Payment Method</h2>
          </div>
          <div class="step-content" v-if="currentStep === 2">
            <div class="payment-accordion">
              
              <!-- UPI -->
              <div class="payment-option" :class="{ 'selected': selectedPayment === 'upi' }" @click="selectedPayment = 'upi'">
                <div class="option-header">
                  <div class="radio-circle"></div>
                  <span class="option-label">UPI / Wallets (via Stripe)</span>
                  <span class="option-icons">⚡</span>
                </div>
                <div class="option-details" v-if="selectedPayment === 'upi'" @click.stop>
                  <p class="detail-label">Please enter your UPI ID</p>
                  <div class="input-action-row">
                    <input type="text" placeholder="Ex: yourname@bank" class="form-input" />
                    <button class="verify-btn">Verify</button>
                  </div>
                </div>
              </div>

              <!-- Credit / Debit Card -->
              <div class="payment-option" :class="{ 'selected': selectedPayment === 'card' }" @click="selectedPayment = 'card'">
                <div class="option-header">
                  <div class="radio-circle"></div>
                  <span class="option-label">Credit / Debit Card (via Stripe)</span>
                  <span class="option-icons">💳</span>
                </div>
                <div class="option-details" v-if="selectedPayment === 'card'" @click.stop>
                  <div class="card-form">
                    <input type="text" placeholder="Card Number" class="form-input full-w" />
                    <div class="split-inputs">
                      <input type="text" placeholder="Valid Thru (MM/YY)" class="form-input" />
                      <input type="password" placeholder="CVV" class="form-input" />
                    </div>
                  </div>
                </div>
              </div>

              <!-- Net Banking -->
              <div class="payment-option" :class="{ 'selected': selectedPayment === 'netbanking' }" @click="selectedPayment = 'netbanking'">
                <div class="option-header">
                  <div class="radio-circle"></div>
                  <span class="option-label">Net Banking</span>
                  <span class="option-icons">🏦</span>
                </div>
                <div class="option-details" v-if="selectedPayment === 'netbanking'" @click.stop>
                  <select class="form-input full-w bank-select">
                    <option value="" disabled selected>Choose an Option</option>
                    <option value="hdfc">HDFC Bank</option>
                    <option value="icici">ICICI Bank</option>
                    <option value="sbi">State Bank of India</option>
                    <option value="axis">Axis Bank</option>
                  </select>
                </div>
              </div>

              <!-- Cash on Delivery -->
              <div class="payment-option" :class="{ 'selected': selectedPayment === 'cod' }" @click="selectedPayment = 'cod'">
                <div class="option-header">
                  <div class="radio-circle"></div>
                  <span class="option-label">Cash on Delivery</span>
                  <span class="option-icons">📦</span>
                </div>
                <div class="option-details" v-if="selectedPayment === 'cod'" @click.stop>
                  <div class="cod-box">
                    <p>Pay digitally with SMS link or cash at your doorstep.</p>
                  </div>
                </div>
              </div>

            </div>
          </div>
        </div>
      </div>

      <!-- Right Column: Order Summary matching the image -->
      <div class="checkout-sidebar">
        <div class="order-summary-card">
          <h2 class="summary-title">Order Summary</h2>
          
          <div class="summary-rows">
            <div class="summary-row">
              <span class="label">Subtotal</span>
              <span class="value">₹{{ subtotal.toLocaleString('en-IN') }}</span>
            </div>
            <div class="summary-row">
              <span class="label">Shipping</span>
              <span class="value success">FREE</span>
            </div>
            <div class="summary-row">
              <span class="label">Tax (18%)</span>
              <span class="value">₹{{ tax.toLocaleString('en-IN') }}</span>
            </div>
          </div>
          
          <div class="summary-divider"></div>
          
          <div class="summary-row total-row">
            <span class="label">Total</span>
            <span class="value">₹{{ finalTotal.toLocaleString('en-IN') }}</span>
          </div>
          
          <button 
            class="place-order-btn" 
            :disabled="placingOrder"
            @click="handleRightButtonClick"
            v-magnetic
          >
            <span v-if="placingOrder" class="spinner"></span>
            <span v-else>{{ currentStep === 2 ? 'Place Order & Pay' : 'Proceed to Payment' }}</span>
          </button>
          
          <button class="continue-shopping-btn" @click="$router.push('/products')">
            ← Continue Shopping
          </button>
        </div>
      </div>

    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue';
import { useRouter } from 'vue-router';
const API_BASE = import.meta.env.VITE_API_BASE || 'http://localhost:5000';
import { useCartStore } from '../store/cart';
import { useAuthStore } from '../store/auth';

const router = useRouter();
const cart = useCartStore();
const auth = useAuthStore();

const currentStep = ref(1);
const placingOrder = ref(false);
const selectedPayment = ref('upi'); // Default selection
const toastVisible = ref(false);
const toastMessage = ref('');
const toastTitle = ref('');
const toastType = ref('success');
let toastTimeout = null;

const address = ref({
  fullName: '',
  phone: '',
  street: '',
  city: '',
  pincode: ''
});
const addressError = ref('');

const decodeEmailName = (email) => {
  if (!email || !email.includes('@')) return '';
  const localPart = email.split('@')[0].split('+')[0];
  const normalized = localPart
    .replace(/[_.-]+/g, ' ')
    .replace(/([a-z])([A-Z])/g, '$1 $2')
    .replace(/[0-9]+/g, ' ')
    .trim();
  const words = normalized
    .split(/\s+/)
    .filter(Boolean)
    .map((word) => word.charAt(0).toUpperCase() + word.slice(1).toLowerCase());
  return words.join(' ');
};

const smartAutoFill = () => {
  const emailName = auth.user?.email ? decodeEmailName(auth.user.email) : '';
  const userName = auth.user?.name || auth.user?.username || emailName || 'Valued Customer';
  address.value.fullName = userName;
  address.value.phone = auth.user?.phone || '9876543210';
  addressError.value = '';

  if (!navigator.geolocation) {
    address.value.street = '123 Smart Ave, Tech Park';
    address.value.city = 'Bengaluru';
    address.value.pincode = '560001';
    return;
  }

  navigator.geolocation.getCurrentPosition(async (position) => {
    try {
      const { latitude, longitude } = position.coords;
      const res = await fetch(`https://nominatim.openstreetmap.org/reverse?format=json&lat=${latitude}&lon=${longitude}`);
      const data = await res.json();

      if (data && data.address) {
        address.value.street = data.address.road || data.address.neighbourhood || data.address.suburb || 'Live Location Lane';
        address.value.city = data.address.city || data.address.state_district || data.address.town || data.address.state || 'Unknown City';
        address.value.pincode = (data.address.postcode || '000000').substring(0, 6);

        if (data.address.country_code && data.address.country_code.toLowerCase() !== 'in') {
          address.value.city = `${address.value.city} (Detected)`;
        }
      }
    } catch (e) {
      address.value.city = 'Mumbai';
      address.value.street = 'Bandra West (Live Fallback)';
      address.value.pincode = '400050';
    }
  }, () => {
    address.value.city = 'New Delhi';
    address.value.street = 'Connaught Place (Predicted)';
    address.value.pincode = '110001';
  }, { enableHighAccuracy: true, timeout: 15000, maximumAge: 60000 });
};

const validateAddress = () => {
  addressError.value = '';
  if (!address.value.fullName.trim() || !address.value.phone.trim() || !address.value.street.trim() || !address.value.city.trim() || !address.value.pincode.trim()) {
    addressError.value = 'Please fill out all address fields.';
    return false;
  }
  if (!/^\d{10}$/.test(address.value.phone.trim())) {
    addressError.value = 'Please enter a valid 10-digit phone number.';
    return false;
  }
  if (!/^\d{6}$/.test(address.value.pincode.trim())) {
    addressError.value = 'Please enter a valid 6-digit pincode.';
    return false;
  }
  return true;
};

const handleAddressSubmit = () => {
  if (validateAddress()) {
    currentStep.value = 2;
  }
};

const subtotal = computed(() => cart.totalAmount);
const tax = computed(() => Math.round(cart.totalAmount * 0.18));
const finalTotal = computed(() => subtotal.value + tax.value);

const showToast = (message, type = 'success', title = 'Success') => {
  toastTitle.value = title;
  toastMessage.value = message;
  toastType.value = type;
  toastVisible.value = true;

  if (toastTimeout) {
    clearTimeout(toastTimeout);
  }
  toastTimeout = setTimeout(() => {
    toastVisible.value = false;
  }, 4200);
};

const handleRightButtonClick = () => {
  if (currentStep.value === 1) {
    handleAddressSubmit();
  } else {
    placeOrder();
  }
};

const placeOrder = async () => {
  placingOrder.value = true;

  if (!cart.items?.length) {
    showToast('Your cart is empty. Add at least one item before placing an order.', 'error', 'Cart Empty');
    placingOrder.value = false;
    return;
  }

  const orderPayload = {
    items: cart.items.map(item => ({
      productId: item.product?.id,
      quantity: item.quantity,
    }))
  };

  try {
    const response = await fetch(`${API_BASE}/api/orders`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(orderPayload),
    });

    if (!response.ok) {
      const errorData = await response.json().catch(() => null);
      const message = errorData?.message || 'Unable to place order at this time.';
      showToast(message, 'error', 'Order Failed');
      placingOrder.value = false;
      return;
    }

    const createdOrder = await response.json();

    if (selectedPayment.value === 'cod') {
      showToast('Your order has been placed successfully. Delivery partner will reach out soon.', 'success', 'Order Confirmed');
      cart.clearLocal();
      setTimeout(() => {
        router.push('/');
      }, 1600);
    } else {
      // For payment methods other than COD, continue with checkout flow.
      const paymentResponse = await fetch(`${API_BASE}/api/payment/create-checkout-session`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          amount: finalTotal.value,
          returnUrl: window.location.origin + '/checkout-success',
          orderId: createdOrder.id,
        })
      });

      if (paymentResponse.ok) {
        const data = await paymentResponse.json();
        window.location.href = data.url;
      } else {
        showToast('Payment initialization failed. Please try again.', 'error', 'Payment Error');
        placingOrder.value = false;
      }
    }
  } catch (error) {
    console.error('Checkout order error:', error);
    showToast('An error occurred while placing your order.', 'error', 'Order Error');
    placingOrder.value = false;
  }
};
</script>

<style scoped>
.checkout-page {
  padding: 2rem 0;
  min-height: 80vh;
}

.checkout-container {
  display: grid;
  grid-template-columns: 1fr 380px;
  gap: 2.5rem;
  align-items: start;
}

.page-title {
  font-size: 2.2rem;
  font-weight: 800;
  margin-bottom: 2rem;
  color: var(--text-primary);
  letter-spacing: -0.03em;
}

.toast-notification {
  position: fixed;
  top: 1.5rem;
  right: 1.5rem;
  z-index: 9999;
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 1rem 1.4rem;
  border-radius: 1.25rem;
  min-width: 320px;
  background: rgba(15, 23, 42, 0.98);
  color: #ffffff;
  box-shadow: 0 24px 80px rgba(0, 0, 0, 0.25);
  border: 1px solid rgba(255, 255, 255, 0.08);
  backdrop-filter: blur(18px);
}

.toast-notification.success {
  border-color: rgba(16, 185, 129, 0.7);
  background: linear-gradient(135deg, rgba(16, 185, 129, 0.18), rgba(15, 23, 42, 0.95));
}

.toast-notification.error {
  border-color: rgba(248, 113, 113, 0.7);
  background: linear-gradient(135deg, rgba(248, 113, 113, 0.18), rgba(15, 23, 42, 0.95));
}

.toast-icon {
  font-size: 1.6rem;
  width: 2.2rem;
  height: 2.2rem;
  display: grid;
  place-items: center;
}

.toast-text strong {
  display: block;
  font-size: 1rem;
  margin-bottom: 0.2rem;
}

.toast-text p {
  margin: 0;
  line-height: 1.4;
  font-size: 0.95rem;
  opacity: 0.92;
}

.toast-slide-enter-active,
.toast-slide-leave-active {
  transition: all 0.25s ease;
}

.toast-slide-enter-from,
.toast-slide-leave-to {
  opacity: 0;
  transform: translateY(-1rem) scale(0.96);
}

.toast-slide-enter-to,
.toast-slide-leave-from {
  opacity: 1;
  transform: translateY(0);
}

.step-card {
  border-radius: var(--radius-xl);
  padding: 2rem;
  transition: all 0.3s var(--ease-out);
  border: 1px solid rgba(255,255,255,0.05);
}

.step-active {
  background: var(--bg-card);
  border-color: rgba(46, 91, 255, 0.3);
  box-shadow: 0 10px 40px rgba(0,0,0,0.2);
}

.step-completed {
  opacity: 0.8;
}

.step-disabled {
  opacity: 0.4;
  pointer-events: none;
}

.step-header {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.step-number {
  width: 32px;
  height: 32px;
  background: var(--accent-blue);
  color: white;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 700;
  font-size: 1.1rem;
}

.step-completed .step-number {
  background: var(--success);
}

.step-header h2 {
  font-size: 1.4rem;
  font-weight: 700;
  color: var(--text-primary);
  flex: 1;
}

.edit-btn {
  background: transparent;
  border: none;
  color: var(--accent-blue);
  font-weight: 600;
  cursor: pointer;
  padding: 0.5rem;
}

.edit-btn:hover {
  text-decoration: underline;
}

.ai-autofill-btn {
  background: rgba(168, 85, 247, 0.15);
  border: 1px solid rgba(168, 85, 247, 0.4);
  color: #c084fc;
  font-weight: 700;
  font-size: 0.8rem;
  border-radius: var(--radius-full);
  padding: 0.4rem 0.8rem;
  cursor: pointer;
  transition: all 0.3s;
  display: flex;
  align-items: center;
  gap: 0.3rem;
}

.ai-autofill-btn:hover {
  background: #a855f7;
  color: white;
  box-shadow: 0 0 10px rgba(168, 85, 247, 0.5);
}

.step-content {
  margin-top: 2rem;
  padding-left: 3rem;
  animation: slideDown 0.4s var(--ease-out);
}

.step-summary {
  margin-top: 1rem;
  padding-left: 3rem;
  color: var(--text-secondary);
  font-size: 0.95rem;
  line-height: 1.5;
}

/* Form Grid */
.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1.2rem;
  margin-bottom: 1.5rem;
}

.full-width {
  grid-column: 1 / -1;
}

.input-group input, .form-input {
  width: 100%;
  padding: 1rem 1.2rem;
  background: rgba(0, 0, 0, 0.2);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: var(--radius-lg);
  color: white;
  font-size: 0.95rem;
  transition: all 0.3s ease;
}

.input-group input:focus, .form-input:focus {
  border-color: var(--accent-blue);
  background: rgba(46, 91, 255, 0.05);
}

.step-btn {
  background: var(--text-primary);
  color: var(--bg-void);
  border: none;
  padding: 1rem 2rem;
  border-radius: var(--radius-full);
  font-weight: 700;
  font-size: 1rem;
  cursor: pointer;
  transition: all 0.3s;
}

.step-btn:hover {
  background: white;
  transform: translateY(-2px);
}

/* Payment Accordion (Amazon Style) */
.payment-accordion {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.payment-option {
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: var(--radius-lg);
  background: rgba(255, 255, 255, 0.02);
  overflow: hidden;
  transition: all 0.3s var(--ease-out);
}

.payment-option.selected {
  border-color: rgba(46, 91, 255, 0.5);
  background: rgba(46, 91, 255, 0.05);
  box-shadow: 0 4px 20px rgba(0,0,0,0.2);
}

.option-header {
  display: flex;
  align-items: center;
  padding: 1.25rem 1.5rem;
  cursor: pointer;
}

.radio-circle {
  width: 20px;
  height: 20px;
  border: 2px solid rgba(255, 255, 255, 0.3);
  border-radius: 50%;
  margin-right: 1.2rem;
  position: relative;
  transition: all 0.2s;
}

.payment-option.selected .radio-circle {
  border-color: var(--accent-blue);
}

.payment-option.selected .radio-circle::after {
  content: '';
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 10px;
  height: 10px;
  background: var(--accent-blue);
  border-radius: 50%;
}

.option-label {
  flex: 1;
  font-weight: 600;
  font-size: 1.05rem;
  color: var(--text-primary);
}

.option-icons {
  font-size: 1.2rem;
  opacity: 0.7;
}

.option-details {
  padding: 0 1.5rem 1.5rem 3.5rem;
  animation: slideDown 0.3s var(--ease-out);
}

.detail-label {
  font-size: 0.9rem;
  color: var(--text-muted);
  margin-bottom: 0.5rem;
}

.input-action-row {
  display: flex;
  gap: 1rem;
}

.verify-btn {
  background: rgba(255,255,255,0.1);
  color: white;
  border: none;
  border-radius: var(--radius-md);
  padding: 0 1.5rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
}

.verify-btn:hover {
  background: var(--accent-blue);
}

.full-w {
  width: 100%;
}

.split-inputs {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
  margin-top: 1rem;
}

.bank-select option {
  background: var(--bg-surface);
  color: white;
}

.cod-box p {
  font-size: 0.95rem;
  color: var(--text-secondary);
  background: rgba(255,255,255,0.05);
  padding: 1rem;
  border-radius: var(--radius-md);
}

/* Right Sidebar: Order Summary matching reference image */
.order-summary-card {
  background: #151518; /* Dark elegant background */
  border: 1px solid rgba(255, 255, 255, 0.05);
  border-radius: 20px;
  padding: 2rem;
  position: sticky;
  top: 2rem;
  box-shadow: 0 20px 40px rgba(0,0,0,0.4);
}

.summary-title {
  font-size: 1.3rem;
  font-weight: 700;
  color: white;
  margin-bottom: 1.5rem;
}

.summary-rows {
  display: flex;
  flex-direction: column;
  gap: 1.2rem;
}

.summary-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.summary-row .label {
  color: rgba(255, 255, 255, 0.6);
  font-size: 0.95rem;
}

.summary-row .value {
  color: rgba(255, 255, 255, 0.8);
  font-weight: 500;
  font-size: 0.95rem;
}

.summary-row .value.success {
  color: #10b981; /* Green color for FREE */
  font-weight: 700;
}

.summary-divider {
  height: 1px;
  background: rgba(255, 255, 255, 0.1);
  margin: 1.5rem 0;
}

.total-row .label {
  color: white;
  font-size: 1.2rem;
  font-weight: 700;
}

.total-row .value {
  color: white;
  font-size: 1.4rem;
  font-weight: 800;
}

/* Beautiful Gradient Button */
.place-order-btn {
  width: 100%;
  margin-top: 2rem;
  padding: 1.1rem;
  background: linear-gradient(135deg, #4f46e5 0%, #7c3aed 100%);
  color: white;
  border: none;
  border-radius: 12px;
  font-size: 1rem;
  font-weight: 700;
  cursor: pointer;
  transition: all 0.3s ease;
  box-shadow: 0 8px 20px rgba(99, 102, 241, 0.3);
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 52px;
}

.place-order-btn:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 12px 25px rgba(99, 102, 241, 0.4);
  background: linear-gradient(135deg, #4338ca 0%, #6d28d9 100%);
}

.place-order-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
  transform: none;
  box-shadow: none;
}

.continue-shopping-btn {
  width: 100%;
  margin-top: 1.2rem;
  background: transparent;
  border: none;
  color: rgba(255, 255, 255, 0.5);
  font-size: 0.9rem;
  font-weight: 500;
  cursor: pointer;
  transition: color 0.2s;
}

.continue-shopping-btn:hover {
  color: rgba(255, 255, 255, 0.9);
}

.spinner {
  width: 20px;
  height: 20px;
  border: 2px solid rgba(255, 255, 255, 0.3);
  border-top-color: white;
  border-radius: 50%;
  animation: spin 0.6s linear infinite;
}

@keyframes slideDown {
  from { opacity: 0; transform: translateY(-10px); }
  to { opacity: 1; transform: translateY(0); }
}

@media (max-width: 900px) {
  .checkout-container {
    grid-template-columns: 1fr;
  }
  .order-summary-card {
    position: relative;
    top: 0;
  }
}

@media (max-width: 600px) {
  .form-grid {
    grid-template-columns: 1fr;
  }
  .step-content {
    padding-left: 0;
  }
}
</style>
