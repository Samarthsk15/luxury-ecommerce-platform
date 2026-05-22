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

      <!-- Live Auction System Summary -->
      <section class="glass-card module-section bidding-summary-section">
        <div class="module-header">
          <span class="icon">⚖️</span>
          <h2>Real-Time Ad Placements</h2>
        </div>
        <p class="ad-disclaimer" style="margin-top:0; margin-bottom:1.5rem; border-color:var(--warning);">
          <em>Manage premium brand ad placements, homepage real estate, and sponsored search auction systems.</em>
        </p>

        <div class="premium-link-card">
          <div class="glow-container">
            <span class="live-dot-glow"></span>
            <span class="active-badge-text">LIVE AUCTION SUMMARY</span>
          </div>
          <div class="quick-stats-row">
            <div class="quick-stat">
              <span class="lbl font-body">Top Spot Bid</span>
              <span class="val font-headline">₹{{ adStore.rank1Product ? adStore.rank1Product.amount.toLocaleString() : '1,500' }}</span>
            </div>
            <div class="quick-stat">
              <span class="lbl font-body">Active Bidders</span>
              <span class="val font-headline">{{ adStore.auctionBids.length }}</span>
            </div>
          </div>
          
          <router-link to="/admin/bidding" class="btn-launch-portal font-headline">
            Launch Bidding Console ↗
          </router-link>
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

/* Bidding Portal Summary Card */
.premium-link-card {
  background: rgba(255, 255, 255, 0.02);
  border: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: var(--radius-lg);
  padding: 1.5rem;
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
  position: relative;
  overflow: hidden;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.2);
  transition: all 0.3s;
}

.premium-link-card:hover {
  background: rgba(255, 255, 255, 0.04);
  border-color: rgba(212, 175, 55, 0.3);
  box-shadow: 0 15px 40px rgba(212, 175, 55, 0.08);
}

.glow-container {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.live-dot-glow {
  width: 8px;
  height: 8px;
  background: #facc15;
  border-radius: 50%;
  box-shadow: 0 0 10px #facc15;
  animation: dot-pulse 1.8s infinite;
}

.active-badge-text {
  font-size: 0.7rem;
  font-weight: 700;
  color: #facc15;
  letter-spacing: 0.1em;
}

.quick-stats-row {
  display: flex;
  gap: 2rem;
}

.quick-stat {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.quick-stat .lbl {
  font-size: 0.8rem;
  color: var(--text-muted);
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

.quick-stat .val {
  font-size: 1.8rem;
  font-weight: 800;
  color: var(--success);
}

.btn-launch-portal {
  display: block;
  width: 100%;
  text-align: center;
  padding: 0.8rem;
  background: linear-gradient(135deg, #ffffff, #d4af37);
  color: #000;
  font-weight: 700;
  border-radius: var(--radius-md);
  box-shadow: 0 4px 15px rgba(212, 175, 55, 0.15);
  transition: all 0.3s;
}

.btn-launch-portal:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 25px rgba(212, 175, 55, 0.3);
}

@keyframes dot-pulse {
  0% { transform: scale(0.9); box-shadow: 0 0 0 0 rgba(250, 204, 21, 0.7); }
  70% { transform: scale(1.1); box-shadow: 0 0 0 8px rgba(250, 204, 21, 0); }
  100% { transform: scale(0.9); box-shadow: 0 0 0 0 rgba(250, 204, 21, 0); }
}

.fade-in {
  animation: fadeIn 0.8s ease-out forwards;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(10px); }
  to { opacity: 1; transform: translateY(0); }
}
</style>
