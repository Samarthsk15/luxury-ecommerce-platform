<template>
  <div class="bidding-portal-container fade-in">
    <!-- Top Glass Header -->
    <div class="portal-header glass">
      <div class="status-badge pulse-container">
        <span class="pulse-dot"></span>
        <span class="status-text">LIVE PLATFORM AUCTION ACTIVE</span>
      </div>
      <h1 class="portal-title font-headline">Luxe Ad Placement Auction</h1>
      <p class="portal-subtitle font-body">Bid for high-visibility spots on the Luxe homepage and search results.</p>
    </div>

    <!-- Main Grid -->
    <div class="portal-grid">
      <!-- Left: Active Leaderboard -->
      <div class="leaderboard-section">
        <div class="section-title-bar">
          <span class="icon">🏆</span>
          <h2 class="font-headline">Live Bid Leaderboard</h2>
        </div>

        <div class="leaderboard-cards">
          <transition-group name="list">
            <div 
              v-for="bid in adStore.auctionBids" 
              :key="bid.id" 
              :class="['bid-card', `rank-${bid.rank}`, { 'outbid-card': bid.status === 'Outbid' }]"
            >
              <!-- Card Background Blur and Border Accent -->
              <div class="rank-badge">#{{ bid.rank }}</div>
              
              <div class="product-thumb">
                <img :src="bid.imageUrl || 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=120&q=80'" :alt="bid.product" />
              </div>

              <div class="bid-info">
                <h3 class="bid-product font-headline">{{ bid.product }}</h3>
                <p class="bid-desc font-body">{{ bid.description || 'Premium listing placement' }}</p>
                <div class="bid-status-pill font-body" :class="bid.status.toLowerCase().replace(/[^a-z0-9]/g, '-')">
                  {{ bid.status }}
                </div>
              </div>

              <div class="bid-financials">
                <span class="currency">₹</span>
                <span class="amount font-headline">{{ bid.amount.toLocaleString() }}</span>
                <span class="label font-body">Current Bid</span>
              </div>
            </div>
          </transition-group>
        </div>
      </div>

      <!-- Right: Bid & Simulate Console -->
      <div class="console-section">
        <!-- Interactive Bid Card -->
        <div class="glass-card form-card">
          <h2 class="font-headline console-card-title">Place Advertisement Bid</h2>
          <p class="form-desc font-body">Select a product and specify your maximum placement bid amount to outbid competitors.</p>

          <form @submit.prevent="submitCustomBid" class="bid-form">
            <div class="input-group">
              <label class="font-body">Select Product Name</label>
              <select v-model="selectedProduct" class="custom-select font-body">
                <option value="" disabled>-- Choose a Product --</option>
                <option v-for="product in sampleProducts" :key="product" :value="product">{{ product }}</option>
                <option value="Custom">Custom Brand Product...</option>
              </select>
            </div>

            <div v-if="selectedProduct === 'Custom'" class="input-group slide-in">
              <label class="font-body">Custom Product Name</label>
              <input 
                v-model="customProductName" 
                type="text" 
                placeholder="Enter premium brand product..." 
                class="custom-input font-body"
                required
              />
            </div>

            <div class="input-group">
              <label class="font-body d-flex justify-between">
                <span>Bid Amount (INR)</span>
                <span class="min-bid font-headline">Min: ₹{{ minBidAmount }}</span>
              </label>
              <div class="bid-amount-input-wrapper">
                <span class="input-prefix font-headline">₹</span>
                <input 
                  v-model.number="bidAmount" 
                  type="number" 
                  class="custom-input amount-input font-headline" 
                  :min="minBidAmount"
                  required
                />
              </div>
              <input 
                v-model.number="bidAmount" 
                type="range" 
                :min="minBidAmount" 
                :max="minBidAmount + 5000" 
                step="50"
                class="custom-range"
              />
            </div>

            <button type="submit" class="submit-btn font-headline">
              Confirm Placement Bid
            </button>
          </form>
        </div>

        <!-- Simulation Card -->
        <div class="glass-card simulation-card">
          <div class="sim-header">
            <span class="icon">⚡</span>
            <h2 class="font-headline">Automated Live Simulator</h2>
          </div>
          <p class="form-desc font-body">Simulate high-velocity active market competition to test outbid scenarios and dynamic UI ranking reorders.</p>
          
          <div class="activity-log font-body" ref="logContainer">
            <div v-for="(log, idx) in activityLogs" :key="idx" class="log-entry" :class="log.type">
              <span class="log-time">{{ log.time }}</span>
              <span class="log-text">{{ log.message }}</span>
            </div>
            <div v-if="activityLogs.length === 0" class="log-placeholder">
              Ready for real-time market simulation...
            </div>
          </div>

          <div class="sim-controls">
            <button @click="triggerMarketSimulation" class="sim-btn font-headline">
              Trigger Competitor Bid
            </button>
            <button @click="toggleAutoSimulation" :class="['sim-btn', 'auto-btn', { 'active-sim': isAutoSimulating }]" class="font-headline">
              {{ isAutoSimulating ? 'Stop Auto Bid' : 'Auto Bid (1.5s)' }}
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Notification Toast -->
    <transition name="toast">
      <div v-if="toastMessage" class="toast-notification font-body" :class="toastType">
        <span class="toast-icon">{{ toastType === 'success' ? '🎉' : '⚡' }}</span>
        <span class="toast-text">{{ toastMessage }}</span>
      </div>
    </transition>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, onUnmounted, nextTick } from 'vue';
import { useAdvertisementStore } from '../store/advertisement';

const adStore = useAdvertisementStore();

// Form Data
const selectedProduct = ref('');
const customProductName = ref('');
const bidAmount = ref(1500);

const sampleProducts = [
  'Noise Cancelling Headphones',
  'Smart Home Hub 2.0',
  'Ergonomic Desk Chair',
  'Mechanical Keyboard v3',
  'Leather Wallet',
  'Luxury Watch'
];

// Activity Logs
const activityLogs = ref([]);
const logContainer = ref(null);
const isAutoSimulating = ref(false);
let autoSimInterval = null;

// Toast Notifications
const toastMessage = ref('');
const toastType = ref('success');
let toastTimeout = null;

// Dynamic Minimum Bid Amount
const minBidAmount = computed(() => {
  if (adStore.auctionBids.length === 0) return 100;
  // Make the minimum bid amount higher than the lowest bid, or standard starting point
  const amounts = adStore.auctionBids.map(b => b.amount);
  return Math.min(...amounts) + 50;
});

// Watch minBidAmount to update the bid input dynamically
onMounted(() => {
  bidAmount.value = minBidAmount.value + 200;
});

const showToast = (message, type = 'success') => {
  toastMessage.value = message;
  toastType.value = type;
  if (toastTimeout) clearTimeout(toastTimeout);
  toastTimeout = setTimeout(() => {
    toastMessage.value = '';
  }, 4000);
};

const addActivityLog = (message, type = 'info') => {
  const now = new Date();
  const timeStr = now.toTimeString().split(' ')[0];
  activityLogs.value.push({ time: timeStr, message, type });
  
  if (activityLogs.value.length > 20) {
    activityLogs.value.shift();
  }

  nextTick(() => {
    if (logContainer.value) {
      logContainer.value.scrollTop = logContainer.value.scrollHeight;
    }
  });
};

const submitCustomBid = () => {
  const name = selectedProduct.value === 'Custom' ? customProductName.value : selectedProduct.value;
  if (!name) {
    showToast('Please select or enter a valid product name.', 'error');
    return;
  }

  if (bidAmount.value < minBidAmount.value) {
    showToast(`Bid must be at least ₹${minBidAmount.value.toLocaleString()} to place on the board.`, 'error');
    return;
  }

  adStore.placeBid(name, bidAmount.value);
  showToast(`Successfully placed bid of ₹${bidAmount.value.toLocaleString()} for ${name}!`, 'success');
  addActivityLog(`You placed a bid of ₹${bidAmount.value.toLocaleString()} on "${name}"`, 'success');

  // Reset form partials
  if (selectedProduct.value === 'Custom') {
    customProductName.value = '';
  }
  selectedProduct.value = '';
  bidAmount.value = minBidAmount.value + 150;
};

const triggerMarketSimulation = () => {
  const oldTopProduct = adStore.rank1Product?.product || 'None';
  const oldTopBid = adStore.rank1Product?.amount || 0;

  adStore.simulateNewBid();
  
  const newTopProduct = adStore.rank1Product?.product;
  const newTopBid = adStore.rank1Product?.amount;

  addActivityLog(`Competitor submitted a new live advertisement bid!`, 'info');

  if (newTopProduct !== oldTopProduct && newTopBid > oldTopBid) {
    addActivityLog(`New Leader: "${newTopProduct}" takes #1 Rank with ₹${newTopBid.toLocaleString()}!`, 'warning');
    showToast(`New high bid of ₹${newTopBid.toLocaleString()} for ${newTopProduct}!`, 'warning');
  } else {
    showToast(`Competitor placed bid on the board!`, 'info');
  }
};

const toggleAutoSimulation = () => {
  isAutoSimulating.value = !isAutoSimulating.value;
  if (isAutoSimulating.value) {
    addActivityLog('Real-time competitor bidding engine started.', 'success');
    autoSimInterval = setInterval(() => {
      triggerMarketSimulation();
    }, 1500);
  } else {
    addActivityLog('Real-time competitor bidding engine stopped.', 'info');
    clearInterval(autoSimInterval);
  }
};

onUnmounted(() => {
  if (autoSimInterval) clearInterval(autoSimInterval);
});
</script>

<style scoped>
.bidding-portal-container {
  max-width: var(--max-width);
  margin: 0 auto;
  padding: 4rem var(--gutter) var(--space-lg);
  color: var(--text-primary);
}

/* Header */
.portal-header {
  border-radius: var(--radius-xl);
  padding: 2.5rem;
  text-align: center;
  margin-bottom: 3rem;
  border: 1px solid var(--border-glass);
  box-shadow: 0 10px 40px rgba(0, 0, 0, 0.2);
}

.status-badge {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  background: rgba(16, 185, 129, 0.1);
  border: 1px solid rgba(16, 185, 129, 0.3);
  padding: 0.4rem 1rem;
  border-radius: var(--radius-full);
  margin-bottom: 1.25rem;
}

.pulse-dot {
  width: 8px;
  height: 8px;
  background: var(--success);
  border-radius: 50%;
  box-shadow: 0 0 12px var(--success);
  animation: dot-pulse 1.8s infinite;
}

.status-text {
  font-size: 0.75rem;
  font-weight: 700;
  letter-spacing: 0.1em;
  color: var(--success);
}

.portal-title {
  font-size: 3rem;
  font-weight: 800;
  background: linear-gradient(135deg, #ffffff 30%, #d4af37 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  margin-bottom: 0.5rem;
  letter-spacing: -0.02em;
}

.portal-subtitle {
  color: var(--text-secondary);
  font-size: 1.1rem;
  max-width: 600px;
  margin: 0 auto;
}

/* Grid Layout */
.portal-grid {
  display: grid;
  grid-template-columns: 1.2fr 0.8fr;
  gap: 2.5rem;
  align-items: start;
}

@media (max-width: 968px) {
  .portal-grid {
    grid-template-columns: 1fr;
  }
}

/* Leaderboard */
.section-title-bar {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  margin-bottom: 1.5rem;
}

.section-title-bar h2 {
  font-size: 1.8rem;
  font-weight: 600;
  color: var(--text-accent);
}

.section-title-bar .icon {
  font-size: 1.8rem;
}

.leaderboard-cards {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

/* Bid Card styling */
.bid-card {
  position: relative;
  display: flex;
  align-items: center;
  gap: 1.5rem;
  padding: 1.5rem;
  background: var(--bg-card);
  backdrop-filter: blur(20px);
  border: 1px solid var(--border-glass);
  border-radius: var(--radius-xl);
  transition: all 0.4s var(--ease-out);
  overflow: hidden;
}

.bid-card::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  width: 4px;
  height: 100%;
  background: transparent;
  transition: all 0.3s;
}

.bid-card:hover {
  background: var(--bg-card-hover);
  transform: translateX(8px);
}

/* Custom styles based on rank */
.rank-1 {
  border-color: rgba(212, 175, 55, 0.4);
  box-shadow: 0 0 30px rgba(212, 175, 55, 0.1);
  background: linear-gradient(135deg, rgba(212, 175, 55, 0.05) 0%, rgba(255,255,255,0.02) 100%);
}
.rank-1::before {
  background: linear-gradient(to bottom, #ffe670, #d4af37);
}
.rank-1 .rank-badge {
  background: #d4af37;
  color: var(--text-primary);
  font-weight: 800;
  box-shadow: 0 0 10px rgba(212, 175, 55, 0.4);
}

.rank-2 {
  border-color: rgba(0, 242, 255, 0.3);
  box-shadow: 0 0 25px rgba(0, 242, 255, 0.08);
}
.rank-2::before {
  background: #00f2ff;
}
.rank-2 .rank-badge {
  background: #00f2ff;
  color: var(--text-primary);
  font-weight: 800;
}

.rank-3 {
  border-color: rgba(255, 255, 255, 0.15);
}
.rank-3::before {
  background: #aaa;
}

.outbid-card {
  opacity: 0.6;
  border-color: rgba(239, 68, 68, 0.15);
}
.outbid-card::before {
  background: var(--error);
}

.rank-badge {
  position: absolute;
  top: 1rem;
  left: 1rem;
  width: 26px;
  height: 26px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 0.75rem;
  font-weight: 700;
  background: rgba(255, 255, 255, 0.1);
  color: var(--text-primary);
}

.product-thumb {
  width: 80px;
  height: 80px;
  border-radius: var(--radius-lg);
  overflow: hidden;
  border: 1px solid var(--border-glass);
  flex-shrink: 0;
  margin-left: 1.5rem; /* Allow space for floating rank badge */
}

.product-thumb img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.bid-info {
  flex: 1;
}

.bid-product {
  font-size: 1.35rem;
  font-weight: 600;
  margin-bottom: 0.25rem;
}

.bid-desc {
  font-size: 0.85rem;
  color: var(--text-secondary);
  margin-bottom: 0.5rem;
  display: -webkit-box;
  -webkit-line-clamp: 1;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.bid-status-pill {
  display: inline-block;
  font-size: 0.7rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.05em;
  padding: 0.2rem 0.6rem;
  border-radius: var(--radius-full);
}

.bid-status-pill.live-homepage {
  background: rgba(212, 175, 55, 0.15);
  color: #facc15;
  border: 1px solid rgba(212, 175, 55, 0.3);
}

.bid-status-pill.live-search {
  background: rgba(0, 242, 255, 0.15);
  color: #00f2ff;
  border: 1px solid rgba(0, 242, 255, 0.3);
}

.bid-status-pill.banner-only {
  background: rgba(255, 255, 255, 0.08);
  color: var(--text-secondary);
  border: 1px solid rgba(255, 255, 255, 0.15);
}

.bid-status-pill.outbid {
  background: rgba(239, 68, 68, 0.15);
  color: var(--error);
  border: 1px solid rgba(239, 68, 68, 0.3);
}

.bid-financials {
  text-align: right;
  display: flex;
  flex-direction: column;
}

.bid-financials .currency {
  font-size: 1rem;
  font-weight: 600;
  color: var(--success);
}

.bid-financials .amount {
  font-size: 2.2rem;
  font-weight: 800;
  line-height: 1;
  color: var(--success);
  text-shadow: 0 0 10px rgba(16, 185, 129, 0.2);
}

.bid-financials .label {
  font-size: 0.75rem;
  color: var(--text-muted);
  text-transform: uppercase;
  letter-spacing: 0.05em;
  margin-top: 0.25rem;
}

/* Console Section */
.console-section {
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

.form-card {
  padding: 2.5rem;
  border-radius: var(--radius-xl);
  border: 1px solid var(--border-glass);
}

.console-card-title {
  font-size: 1.8rem;
  font-weight: 700;
  margin-bottom: 0.5rem;
}

.form-desc {
  font-size: 0.9rem;
  color: var(--text-secondary);
  line-height: 1.4;
  margin-bottom: 2rem;
}

.bid-form {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.input-group {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.input-group label {
  font-size: 0.85rem;
  font-weight: 600;
  color: var(--text-secondary);
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

.min-bid {
  color: #ff9d00;
  font-weight: 700;
}

.custom-select, .custom-input {
  width: 100%;
  background: var(--bg-card);
  border: 1px solid var(--border-glass);
  color: var(--text-primary);
  padding: 0.875rem 1.25rem;
  border-radius: var(--radius-md);
  font-size: 0.95rem;
  transition: all 0.3s;
  outline: none;
}

.custom-select:focus, .custom-input:focus {
  border-color: var(--text-accent);
  background: var(--bg-card-hover);
  box-shadow: 0 0 15px var(--accent-glow-violet);
}

.custom-select option {
  background: var(--bg-card);
  color: var(--text-primary);
}

.bid-amount-input-wrapper {
  position: relative;
  display: flex;
  align-items: center;
}

.input-prefix {
  position: absolute;
  left: 1.25rem;
  font-size: 1.6rem;
  font-weight: 700;
  color: var(--success);
}

.amount-input {
  padding-left: 2.5rem;
  font-size: 1.8rem;
  font-weight: 800;
  color: var(--success);
}

.custom-range {
  margin-top: 0.5rem;
  -webkit-appearance: none;
  width: 100%;
  height: 6px;
  border-radius: 3px;
  background: rgba(255,255,255,0.1);
  outline: none;
}

.custom-range::-webkit-slider-thumb {
  -webkit-appearance: none;
  appearance: none;
  width: 18px;
  height: 18px;
  border-radius: 50%;
  background: var(--text-accent);
  cursor: pointer;
  box-shadow: 0 0 10px rgba(212, 175, 55, 0.8);
  transition: transform 0.1s;
}

.custom-range::-webkit-slider-thumb:hover {
  transform: scale(1.2);
}

.submit-btn {
  width: 100%;
  padding: 1.1rem;
  background: var(--accent-gradient);
  color: #000;
  font-weight: 700;
  font-size: 1.1rem;
  letter-spacing: 0.02em;
  border-radius: var(--radius-md);
  box-shadow: 0 4px 20px var(--accent-glow-violet);
  transition: all 0.3s;
}

.submit-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 30px rgba(212, 175, 55, 0.4);
}

/* Simulation Console Card */
.simulation-card {
  padding: 2.5rem;
  border-radius: var(--radius-xl);
  border: 1px solid var(--border-glass);
}

.sim-header {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  margin-bottom: 0.5rem;
}

.sim-header h2 {
  font-size: 1.6rem;
  font-weight: 700;
  color: var(--accent-blue);
  text-shadow: 0 0 10px rgba(17, 24, 39, 0.08);
}

.sim-header .icon {
  font-size: 1.6rem;
}

.activity-log {
  height: 150px;
  background: var(--bg-card);
  border: 1px solid var(--border-glass);
  border-radius: var(--radius-md);
  padding: 1rem;
  overflow-y: auto;
  font-family: monospace;
  font-size: 0.8rem;
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
  margin-bottom: 1.5rem;
  scroll-behavior: smooth;
}

.log-entry {
  display: flex;
  gap: 0.75rem;
  line-height: 1.4;
}

.log-time {
  color: var(--text-muted);
  flex-shrink: 0;
}

.log-entry.success { color: var(--success); }
.log-entry.warning { color: #ffe670; }
.log-entry.error { color: var(--error); }
.log-entry.info { color: #00f2ff; }

.log-placeholder {
  color: var(--text-muted);
  text-align: center;
  margin: auto;
  font-style: italic;
}

.sim-controls {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
}

.sim-btn {
  padding: 0.85rem;
  background: rgba(255, 255, 255, 0.04);
  border: 1px solid var(--border-glass);
  color: var(--text-primary);
  border-radius: var(--radius-md);
  font-weight: 600;
  transition: all 0.3s;
}

.sim-btn:hover {
  background: rgba(0, 242, 255, 0.1);
  border-color: rgba(0, 242, 255, 0.4);
  color: #00f2ff;
  transform: translateY(-1px);
}

.auto-btn.active-sim {
  background: rgba(239, 68, 68, 0.1);
  border-color: rgba(239, 68, 68, 0.4);
  color: var(--error);
  animation: emergency-glow 1.5s infinite alternate;
}

/* Animations */
@keyframes dot-pulse {
  0% { transform: scale(0.9); box-shadow: 0 0 0 0 rgba(16, 185, 129, 0.7); }
  70% { transform: scale(1.1); box-shadow: 0 0 0 8px rgba(16, 185, 129, 0); }
  100% { transform: scale(0.9); box-shadow: 0 0 0 0 rgba(16, 185, 129, 0); }
}

@keyframes emergency-glow {
  from { box-shadow: 0 0 4px rgba(239, 68, 68, 0.2); }
  to { box-shadow: 0 0 15px rgba(239, 68, 68, 0.5); }
}

/* Vue List Transitions */
.list-enter-active, .list-leave-active {
  transition: all 0.6s ease;
}
.list-enter-from, .list-leave-to {
  opacity: 0;
  transform: scale(0.9) translateY(20px);
}
.list-move {
  transition: transform 0.6s ease;
}

/* Toast styling */
.toast-notification {
  position: fixed;
  bottom: 2rem;
  right: 2rem;
  display: flex;
  align-items: center;
  gap: 0.75rem;
  background: rgba(10, 10, 15, 0.95);
  border: 1px solid var(--border-glass);
  box-shadow: 0 10px 40px rgba(0, 0, 0, 0.5);
  padding: 1rem 1.75rem;
  border-radius: var(--radius-lg);
  z-index: 1000;
  backdrop-filter: blur(20px);
}

.toast-notification.success {
  border-color: rgba(16, 185, 129, 0.4);
  box-shadow: 0 0 20px rgba(16, 185, 129, 0.15);
}

.toast-notification.warning {
  border-color: rgba(212, 175, 55, 0.4);
  box-shadow: 0 0 20px rgba(212, 175, 55, 0.15);
}

.toast-notification.error {
  border-color: rgba(239, 68, 68, 0.4);
  box-shadow: 0 0 20px rgba(239, 68, 68, 0.15);
}

.toast-enter-active, .toast-leave-active {
  transition: all 0.4s cubic-bezier(0.16, 1, 0.3, 1);
}
.toast-enter-from {
  opacity: 0;
  transform: translateY(30px) scale(0.9);
}
.toast-leave-to {
  opacity: 0;
  transform: translateY(10px) scale(0.9);
}

.d-flex { display: flex; }
.justify-between { justify-content: space-between; }
.slide-in {
  animation: slideIn 0.3s ease-out forwards;
}

@keyframes slideIn {
  from { opacity: 0; transform: translateY(-10px); }
  to { opacity: 1; transform: translateY(0); }
}
</style>
