<template>
  <div class="admin-dashboard fade-in">
    <div class="dashboard-header">
      <h1 class="glow-text">Smart Advertisement & Product Promotion Dashboard</h1>
      <p class="subtitle">Product Promotion & Analytics Module</p>
    </div>

    <div class="dashboard-grid">
      <!-- Analytics Module -->
      <section class="glass-card module-section">
        <div class="module-header">
          <span class="icon">📊</span>
          <h2>Product Popularity Tracking</h2>
        </div>
        <div class="stats-container">
          <div class="stat-box">
            <span class="stat-label">Top Premium Item</span>
            <span class="stat-value">Smart Home Hub 2.0</span>
            <span class="trend positive">+12% Views</span>
          </div>
          <div class="stat-box">
            <span class="stat-label">Highest Conversion</span>
            <span class="stat-value">Noise Cancelling Headphones</span>
            <span class="trend positive">+8% Sales</span>
          </div>
        </div>
      </section>

      <!-- Advertisement Dashboard -->
      <section class="glass-card module-section">
        <div class="module-header">
          <span class="icon">🚀</span>
          <h2>Smart Advertisement Dashboard</h2>
        </div>
        <div class="ad-controls">
          <button class="btn-luxury">Launch Flash Sale</button>
          <button class="btn-luxury">Manage Sponsored Products</button>
          <button class="btn-luxury">Premium Promotions</button>
          <button class="btn-luxury">Membership Campaigns</button>
        </div>
        <div class="ad-disclaimer">
          <p>
            <em>This module helps administrators manage product promotions, sponsored products, flash sales, and analyze product popularity through an interactive dashboard.</em>
          </p>
          <p style="margin-top: 0.5rem; border-top: 1px solid rgba(255,255,255,0.1); padding-top: 0.5rem;">
            <small>The module is designed with AI-inspired recommendation concepts and can be extended in future with machine learning-based product recommendations.</small>
          </p>
        </div>
      </section>

      <!-- Live Auction System -->
      <section class="glass-card module-section">
        <div class="module-header">
          <span class="icon">⚖️</span>
          <h2>Advertisement Auction System</h2>
        </div>
        <p class="ad-disclaimer" style="margin-top:0; margin-bottom:1.5rem; border-color:var(--warning);">
          <em>Dynamic advertisement marketplace. Sellers bid for priority visibility on the homepage.</em>
        </p>

        <div class="auction-board">
          <div class="auction-row header-row">
            <span>Rank</span>
            <span>Product</span>
            <span>Bid Amount</span>
            <span>Status</span>
          </div>
          <div v-for="bid in adStore.auctionBids" :key="bid.id" class="auction-row animate-row">
            <span :class="['rank', `rank-${bid.rank}`]">#{{ bid.rank }}</span>
            <span class="product-name">{{ bid.product }}</span>
            <span class="bid-amt">₹{{ bid.amount.toLocaleString() }}</span>
            <span :class="['status', bid.status === 'Outbid' ? 'outbid' : 'live']">{{ bid.status }}</span>
          </div>
        </div>
        
        <div class="ad-controls" style="margin-top:1.5rem;">
          <button class="btn-luxury" @click="simulateNewBid">Simulate New Bid</button>
          <button class="btn-luxury">Manage Auction Rules</button>
        </div>
      </section>
    </div>
  </div>
</template>

<script setup>
import { onMounted } from 'vue';
import { useAdvertisementStore } from '../store/advertisement';

const adStore = useAdvertisementStore();

const simulateNewBid = () => {
  adStore.simulateNewBid();
};

onMounted(async () => {
  try {
    console.log("Dashboard mounted, API integration ready.");
  } catch (err) {
    console.error("Error loading dashboard data", err);
  }
});
</script>

<style scoped>
.admin-dashboard {
  padding: 4rem 2rem;
  max-width: 1200px;
  margin: 0 auto;
  color: #fff;
}

.dashboard-header {
  text-align: center;
  margin-bottom: 3rem;
}

.glow-text {
  font-size: 2.5rem;
  font-family: 'Cormorant', serif;
  background: linear-gradient(135deg, #a855f7 0%, #3b82f6 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  font-weight: 700;
  margin-bottom: 0.5rem;
}

.subtitle {
  color: var(--text-muted);
  font-size: 1.1rem;
}

.dashboard-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(400px, 1fr));
  gap: 2rem;
}

.module-section {
  padding: 2rem;
  border-radius: var(--radius-xl);
  border: 1px solid rgba(255, 255, 255, 0.1);
  background: rgba(20, 20, 20, 0.6);
  backdrop-filter: blur(16px);
  -webkit-backdrop-filter: blur(16px);
}

.module-header {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-bottom: 1.5rem;
}

.module-header h2 {
  font-size: 1.5rem;
  font-weight: 600;
}

.icon {
  font-size: 2rem;
}

.stats-container {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.stat-box {
  background: rgba(255, 255, 255, 0.05);
  padding: 1.5rem;
  border-radius: var(--radius-lg);
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.stat-label {
  font-size: 0.9rem;
  color: var(--text-muted);
}

.stat-value {
  font-size: 1.25rem;
  font-weight: 700;
  color: #fff;
}

.trend.positive {
  color: #10b981;
  font-size: 0.85rem;
  font-weight: 600;
}

.ad-controls {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
  margin-bottom: 1.5rem;
}

.btn-luxury {
  padding: 0.8rem;
  background: rgba(168, 85, 247, 0.1);
  border: 1px solid rgba(168, 85, 247, 0.3);
  color: white;
  border-radius: var(--radius-md);
  cursor: pointer;
  transition: all 0.3s;
  font-weight: 500;
}

.btn-luxury:hover {
  background: rgba(168, 85, 247, 0.3);
  transform: translateY(-2px);
  box-shadow: 0 4px 15px rgba(168, 85, 247, 0.2);
}

.ad-disclaimer {
  font-size: 0.85rem;
  color: var(--text-muted);
  border-left: 3px solid #3b82f6;
  padding-left: 1rem;
  margin-top: 1.5rem;
  line-height: 1.4;
}

/* Auction Board */
.auction-board {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  background: rgba(0,0,0,0.3);
  border-radius: var(--radius-md);
  padding: 0.5rem;
  border: 1px solid rgba(255,255,255,0.05);
}

.auction-row {
  display: grid;
  grid-template-columns: 0.5fr 2fr 1fr 1fr;
  padding: 0.8rem 1rem;
  border-radius: var(--radius-sm);
  background: rgba(255,255,255,0.03);
  font-size: 0.85rem;
  align-items: center;
}

.header-row {
  background: transparent;
  color: var(--text-muted);
  font-weight: 600;
  text-transform: uppercase;
  font-size: 0.7rem;
  letter-spacing: 0.05em;
}

.rank { font-weight: 800; color: var(--text-muted); }
.rank-1 { color: #facc15; font-size: 1rem; text-shadow: 0 0 10px rgba(250, 204, 21, 0.5); }
.rank-2 { color: #94a3b8; }
.rank-3 { color: #b45309; }

.product-name { font-weight: 500; color: #fff; }
.bid-amt { font-family: var(--font-headline); font-weight: 700; color: #10b981; }

.status {
  padding: 0.2rem 0.5rem;
  border-radius: var(--radius-full);
  font-size: 0.7rem;
  font-weight: 600;
  text-align: center;
}
.status.live { background: rgba(16, 185, 129, 0.15); color: #10b981; border: 1px solid rgba(16, 185, 129, 0.3); }
.status.outbid { background: rgba(239, 68, 68, 0.15); color: #ef4444; border: 1px solid rgba(239, 68, 68, 0.3); }

.fade-in {
  animation: fadeIn 0.8s ease-out forwards;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(10px); }
  to { opacity: 1; transform: translateY(0); }
}
</style>
