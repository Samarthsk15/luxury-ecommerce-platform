<template>
  <div class="admin-orders fade-in">
    <div class="orders-header">
      <div class="header-content">
        <h1 class="glow-text">📦 Orders & Tracking</h1>
        <p class="subtitle">Complete Order Management & Customer Tracking System</p>
      </div>
      <div class="header-actions">
        <button @click="refreshOrders" class="btn-refresh">🔄 Refresh</button>
        <router-link to="/admin" class="btn-back">← Back to Dashboard</router-link>
      </div>
    </div>

    <!-- Summary Cards -->
    <div class="summary-grid">
      <div class="summary-card glass-card">
        <div class="card-icon">📊</div>
        <div class="card-content">
          <span class="card-label">Total Orders</span>
          <span class="card-value">{{ orders.length }}</span>
        </div>
      </div>
      <div class="summary-card glass-card">
        <div class="card-icon">💰</div>
        <div class="card-content">
          <span class="card-label">Total Revenue</span>
          <span class="card-value">₹{{ totalRevenue.toLocaleString() }}</span>
        </div>
      </div>
      <div class="summary-card glass-card">
        <div class="card-icon">👥</div>
        <div class="card-content">
          <span class="card-label">Customers</span>
          <span class="card-value">{{ uniqueCustomers }}</span>
        </div>
      </div>
      <div class="summary-card glass-card">
        <div class="card-icon">✅</div>
        <div class="card-content">
          <span class="card-label">Completed</span>
          <span class="card-value">{{ completedOrders }}</span>
        </div>
      </div>
    </div>

    <!-- Orders Table -->
    <section class="orders-section glass-card">
      <div class="section-header">
        <h2>All Orders</h2>
        <span class="order-count">{{ orders.length }} {{ orders.length === 1 ? 'order' : 'orders' }}</span>
      </div>

      <div v-if="loading" class="loading-state">
        <div class="spinner"></div>
        <p>Loading orders...</p>
      </div>

      <div v-else-if="orders.length === 0" class="empty-state">
        <div class="empty-icon">📭</div>
        <h3>No Orders Yet</h3>
        <p>Orders will appear here once customers place their first purchase.</p>
        <router-link to="/" class="btn-create-demo">View Storefront</router-link>
      </div>

      <div v-else class="table-wrapper">
        <table class="orders-table">
          <thead>
            <tr>
              <th>Order ID</th>
              <th>Customer Email</th>
              <th>Order Date</th>
              <th>Status</th>
              <th>Items</th>
              <th>Total Amount</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="order in orders" :key="order.id" class="order-row" :class="`status-${order.status.toLowerCase()}`">
              <td class="order-id">#{{ order.id }}</td>
              <td class="customer-email">{{ order.customerEmail }}</td>
              <td class="order-date">{{ formatDate(order.createdAt) }}</td>
              <td class="order-status">
                <span class="status-badge" :class="`badge-${order.status.toLowerCase()}`">
                  {{ order.status }}
                </span>
              </td>
              <td class="items-count">{{ order.itemCount }} items</td>
              <td class="order-amount">₹{{ parseFloat(order.totalAmount).toLocaleString() }}</td>
              <td class="actions">
                <button @click="viewOrderDetails(order)" class="btn-view-details">View</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </section>

    <!-- Order Details Modal -->
    <div v-if="selectedOrder" class="modal-overlay" @click="selectedOrder = null">
      <div class="modal-content" @click.stop>
        <div class="modal-header">
          <h2>Order #{{ selectedOrder.id }} Details</h2>
          <button @click="selectedOrder = null" class="btn-close">✕</button>
        </div>

        <div class="modal-body">
          <div class="detail-section">
            <h3>Customer Information</h3>
            <div class="detail-grid">
              <div class="detail-item">
                <span class="detail-label">Email:</span>
                <span class="detail-value">{{ selectedOrder.customerEmail }}</span>
              </div>
              <div class="detail-item">
                <span class="detail-label">Phone:</span>
                <span class="detail-value">{{ selectedOrder.customerPhone || 'N/A' }}</span>
              </div>
              <div class="detail-item">
                <span class="detail-label">Order Date:</span>
                <span class="detail-value">{{ formatDateTime(selectedOrder.createdAt) }}</span>
              </div>
              <div class="detail-item">
                <span class="detail-label">Status:</span>
                <span class="detail-value">
                  <span class="status-badge" :class="`badge-${selectedOrder.status.toLowerCase()}`">
                    {{ selectedOrder.status }}
                  </span>
                </span>
              </div>
            </div>
          </div>

          <div class="detail-section">
            <h3>Delivery Address</h3>
            <div class="address-box">
              {{ selectedOrder.deliveryAddress || 'Address not provided' }}
            </div>
          </div>

          <div class="detail-section">
            <h3>Order Items</h3>
            <div class="items-table">
              <div v-for="item in selectedOrder.items" :key="item.id" class="item-row">
                <div class="item-name">{{ item.productName }}</div>
                <div class="item-qty">Qty: {{ item.quantity }}</div>
                <div class="item-price">₹{{ parseFloat(item.unitPrice).toLocaleString() }}</div>
              </div>
              <div class="items-total">
                <span>Total:</span>
                <span>₹{{ parseFloat(selectedOrder.totalAmount).toLocaleString() }}</span>
              </div>
            </div>
          </div>
        </div>

        <div class="modal-footer">
          <button @click="selectedOrder = null" class="btn-close-modal">Close</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();
const orders = ref([]);
const loading = ref(true);
const selectedOrder = ref(null);

const API_BASE = import.meta.env.VITE_API_BASE || 'http://localhost:5000';

onMounted(async () => {
  await fetchOrders();
});

const fetchOrders = async () => {
  loading.value = true;
  try {
    const response = await fetch(`${API_BASE}/api/orders`, {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      }
    });

    if (response.ok) {
      const data = await response.json();
      // Transform data for display, extracting product names from items
      orders.value = data.map(order => ({
        id: order.id,
        customerEmail: order.user?.email || order.email || 'Unknown',
        customerPhone: order.user?.phone || order.phone || '',
        createdAt: order.createdAt,
        status: order.status || 'Pending',
        totalAmount: order.totalAmount,
        deliveryAddress: order.deliveryAddress || '',
        itemCount: order.items?.length || 0,
        items: (order.items || []).map(item => ({
          id: item.id,
          productName: item.product?.name || `Product #${item.productId}`,
          quantity: item.quantity,
          unitPrice: item.unitPrice,
          lineTotal: item.quantity * item.unitPrice
        }))
      }));
      console.log('Orders fetched successfully:', orders.value);
    } else if (response.status === 401) {
      console.error('Unauthorized - Invalid token');
    } else {
      console.error('Failed to fetch orders:', response.status);
    }
  } catch (error) {
    console.error('Error fetching orders:', error);
  } finally {
    loading.value = false;
  }
};

const refreshOrders = async () => {
  await fetchOrders();
};

const viewOrderDetails = (order) => {
  selectedOrder.value = order;
};

const formatDate = (dateString) => {
  const date = new Date(dateString);
  return date.toLocaleDateString('en-IN', {
    day: '2-digit',
    month: 'short',
    year: 'numeric'
  });
};

const formatDateTime = (dateString) => {
  const date = new Date(dateString);
  return date.toLocaleDateString('en-IN', {
    day: '2-digit',
    month: 'short',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  });
};

const totalRevenue = computed(() => {
  return orders.value.reduce((sum, order) => sum + parseFloat(order.totalAmount || 0), 0);
});

const uniqueCustomers = computed(() => {
  const emails = new Set(orders.value.map(o => o.customerEmail));
  return emails.size;
});

const completedOrders = computed(() => {
  return orders.value.filter(o => o.status.toLowerCase() === 'completed').length;
});
</script>

<style>
.admin-orders {
  padding: 3rem 2rem;
  max-width: 1400px;
  margin: 0 auto;
  color: var(--text-primary);
  background: var(--bg-surface);
  min-height: 100vh;
}

.admin-orders .header-content .subtitle,
.admin-orders .order-count,
.admin-orders .empty-state p,
.admin-orders .orders-table td,
.admin-orders .detail-grid .detail-label {
  color: var(--text-muted);
}

.admin-orders .btn-refresh,
.admin-orders .btn-back,
.admin-orders .btn-close-modal,
.admin-orders .btn-view-details {
  border: 1px solid var(--border-glass);
  background: rgba(88, 28, 135, 0.18);
  color: var(--text-primary);
}

[data-theme="minimal"] .admin-orders .btn-refresh,
[data-theme="minimal"] .admin-orders .btn-back,
[data-theme="minimal"] .admin-orders .btn-close-modal,
[data-theme="minimal"] .admin-orders .btn-view-details {
  background: rgba(99, 102, 241, 0.1);
  border-color: rgba(99, 102, 241, 0.2);
}

.admin-orders .summary-card,
.admin-orders .orders-section,
.admin-orders .modal-content {
  background: var(--bg-card);
  border-color: var(--border-glass);
}

[data-theme="minimal"] .admin-orders .summary-card,
[data-theme="minimal"] .admin-orders .orders-section,
[data-theme="minimal"] .admin-orders .modal-content {
  background: rgba(255, 255, 255, 0.9);
  border-color: rgba(99, 102, 241, 0.3);
}


.orders-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 3rem;
  flex-wrap: wrap;
  gap: 2rem;
}

.header-content h1 {
  font-size: 2.5rem;
  font-family: 'Cormorant', serif;
  background: linear-gradient(135deg, #a855f7 0%, #3b82f6 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  font-weight: 700;
  margin: 0;
}

[data-theme="minimal"] .header-content h1 {
  background: linear-gradient(135deg, #6366f1 0%, #3b82f6 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

.header-content .subtitle {
  color: var(--color-text-muted);
  font-size: 1rem;
  margin-top: 0.5rem;
}

.header-actions {
  display: flex;
  gap: 1rem;
  flex-wrap: wrap;
}

.btn-refresh,
.btn-back,
.btn-close-modal,
.btn-view-details {
  padding: 0.75rem 1.5rem;
  border: 1px solid var(--color-border);
  background: rgba(88, 28, 135, 0.3);
  color: var(--color-text);
  border-radius: 0.5rem;
  cursor: pointer;
  font-size: 0.9rem;
  transition: all 0.3s ease;
}

[data-theme="minimal"] .btn-refresh,
[data-theme="minimal"] .btn-back,
[data-theme="minimal"] .btn-close-modal,
[data-theme="minimal"] .btn-view-details {
  background: rgba(99, 102, 241, 0.1);
  border-color: rgba(99, 102, 241, 0.3);
}

.btn-refresh:hover,
.btn-back:hover,
.btn-close-modal:hover,
.btn-view-details:hover {
  background: rgba(168, 85, 247, 0.5);
  border-color: #a855f7;
}

[data-theme="minimal"] .btn-refresh:hover,
[data-theme="minimal"] .btn-back:hover,
[data-theme="minimal"] .btn-close-modal:hover,
[data-theme="minimal"] .btn-view-details:hover {
  background: rgba(99, 102, 241, 0.3);
  border-color: #6366f1;
}

.btn-back {
  text-decoration: none;
  display: inline-flex;
  align-items: center;
}

/* Summary Grid */
.summary-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1.5rem;
  margin-bottom: 2rem;
}

.summary-card {
  display: flex;
  align-items: center;
  gap: 1.5rem;
  padding: 1.5rem;
  border-radius: 1rem;
  border: 1px solid var(--color-border);
  background: var(--color-card);
  backdrop-filter: blur(16px);
  -webkit-backdrop-filter: blur(16px);
  transition: all 0.3s ease;
}

[data-theme="minimal"] .summary-card {
  background: rgba(99, 102, 241, 0.05);
  border-color: rgba(99, 102, 241, 0.2);
}

.summary-card:hover {
  border-color: var(--color-accent);
  background: var(--color-card);
  transform: translateY(-2px);
}

.card-icon {
  font-size: 2rem;
}

.card-label {
  display: block;
  font-size: 0.85rem;
  color: var(--color-text-muted);
  margin-bottom: 0.5rem;
}

.card-value {
  display: block;
  font-size: 1.75rem;
  font-weight: 600;
  background: linear-gradient(135deg, #a855f7, #3b82f6);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

[data-theme="minimal"] .card-value {
  background: linear-gradient(135deg, #6366f1, #3b82f6);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

/* Orders Section */
.orders-section {
  border-radius: 1rem;
  border: 1px solid var(--color-border);
  background: var(--color-card);
  backdrop-filter: blur(16px);
  -webkit-backdrop-filter: blur(16px);
  padding: 2rem;
}

[data-theme="minimal"] .orders-section {
  background: rgba(99, 102, 241, 0.03);
  border-color: rgba(99, 102, 241, 0.2);
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
  padding-bottom: 1rem;
  border-bottom: 1px solid var(--color-border);
}

.section-header h2 {
  font-size: 1.5rem;
  margin: 0;
  color: var(--color-text);
}

.order-count {
  color: var(--color-text-muted);
  font-size: 0.9rem;
}

/* Loading & Empty States */
.loading-state {
  text-align: center;
  padding: 3rem;
  color: var(--text-muted, #888);
}

.spinner {
  width: 40px;
  height: 40px;
  border: 3px solid rgba(255, 255, 255, 0.1);
  border-top-color: #a855f7;
  border-radius: 50%;
  margin: 0 auto 1rem;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

.empty-state {
  text-align: center;
  padding: 3rem;
}

.empty-icon {
  font-size: 3rem;
  margin-bottom: 1rem;
}

.empty-state h3 {
  font-size: 1.5rem;
  margin-bottom: 0.5rem;
}

.empty-state p {
  color: var(--color-text-muted);
  margin-bottom: 1.5rem;
}

.btn-create-demo {
  padding: 0.75rem 1.5rem;
  background: linear-gradient(135deg, #a855f7, #3b82f6);
  color: #fff;
  border: none;
  border-radius: 0.5rem;
  cursor: pointer;
  text-decoration: none;
  font-weight: 500;
  transition: all 0.3s ease;
}

[data-theme="minimal"] .btn-create-demo {
  background: linear-gradient(135deg, #6366f1, #3b82f6);
}

.btn-create-demo:hover {
  transform: scale(1.05);
}

/* Orders Table */
.table-wrapper {
  overflow-x: auto;
}

.orders-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 0.95rem;
}

.orders-table thead {
  background: rgba(88, 28, 135, 0.1);
  border-bottom: 2px solid var(--color-border);
}

[data-theme="minimal"] .orders-table thead {
  background: rgba(99, 102, 241, 0.1);
}

.orders-table th {
  padding: 1rem;
  text-align: left;
  font-weight: 600;
  color: #a855f7;
}

[data-theme="minimal"] .orders-table th {
  color: #6366f1;
}

.order-row {
  border-bottom: 1px solid var(--color-border);
  transition: all 0.3s ease;
}

.order-row:hover {
  background: rgba(88, 28, 135, 0.1);
}

[data-theme="minimal"] .order-row:hover {
  background: rgba(99, 102, 241, 0.1);
}

.orders-table td {
  padding: 1rem;
  color: var(--color-text);
}

.order-id {
  font-weight: 600;
  color: #a855f7;
}

[data-theme="minimal"] .order-id {
  color: #6366f1;
}

.order-date {
  color: var(--color-text-muted);
  font-size: 0.9rem;
}

.status-badge {
  padding: 0.4rem 0.8rem;
  border-radius: 0.25rem;
  font-weight: 500;
  font-size: 0.85rem;
}

.badge-pending {
  background: rgba(255, 193, 7, 0.2);
  color: #ffc107;
}

.badge-completed {
  background: rgba(76, 175, 80, 0.2);
  color: #4caf50;
}

.badge-cancelled {
  background: rgba(244, 67, 54, 0.2);
  color: #f44336;
}

.badge-shipped {
  background: rgba(33, 150, 243, 0.2);
  color: #2196f3;
}

.order-amount {
  font-weight: 600;
  color: #4caf50;
}

.actions {
  text-align: center;
}

/* Modal */
.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.8);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  backdrop-filter: blur(4px);
}

[data-theme="minimal"] .modal-overlay {
  background: rgba(0, 0, 0, 0.5);
}

.modal-content {
  background: linear-gradient(135deg, rgba(30, 30, 40, 0.95), rgba(50, 20, 70, 0.8));
  border: 1px solid rgba(255, 255, 255, 0.2);
  border-radius: 1rem;
  max-width: 600px;
  width: 90%;
  max-height: 80vh;
  overflow-y: auto;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.5);
}

[data-theme="minimal"] .modal-content {
  background: linear-gradient(135deg, rgba(255, 255, 255, 0.98), rgba(245, 245, 250, 0.95));
  border-color: rgba(99, 102, 241, 0.3);
  color: #1a1a1a;
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.5rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
  color: var(--color-text);
}

[data-theme="minimal"] .modal-header {
  border-bottom-color: rgba(99, 102, 241, 0.2);
}

.modal-header h2 {
  margin: 0;
  font-size: 1.5rem;
}

.btn-close {
  background: none;
  border: none;
  color: var(--color-text);
  font-size: 1.5rem;
  cursor: pointer;
  padding: 0;
  width: 30px;
  height: 30px;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: color 0.3s ease;
}

.btn-close:hover {
  color: #a855f7;
}

.modal-body {
  padding: 1.5rem;
}

.detail-section {
  margin-bottom: 1.5rem;
}

.detail-section h3 {
  margin: 0 0 1rem 0;
  color: #a855f7;
  font-size: 1.1rem;
}

.detail-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 1rem;
}

.detail-item {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.detail-label {
  font-size: 0.85rem;
  color: var(--text-muted, #888);
}

.detail-value {
  font-weight: 500;
}

.address-box {
  background: rgba(88, 28, 135, 0.1);
  padding: 1rem;
  border-radius: 0.5rem;
  border-left: 3px solid #a855f7;
  line-height: 1.6;
}

.items-table {
  background: rgba(88, 28, 135, 0.05);
  border-radius: 0.5rem;
  overflow: hidden;
}

.item-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.75rem 1rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.05);
}

.item-row:last-of-type {
  border-bottom: none;
}

.item-name {
  flex: 1;
  font-weight: 500;
}

.item-qty,
.item-price {
  color: var(--text-muted, #888);
  font-size: 0.9rem;
  min-width: 80px;
  text-align: right;
}

.item-price {
  color: #4caf50;
  font-weight: 600;
}

.items-total {
  display: flex;
  justify-content: space-between;
  padding: 1rem;
  background: rgba(88, 28, 135, 0.2);
  font-weight: 600;
  border-top: 1px solid rgba(255, 255, 255, 0.1);
}

.items-total span:last-child {
  color: #4caf50;
}

.modal-footer {
  padding: 1rem 1.5rem;
  border-top: 1px solid rgba(255, 255, 255, 0.1);
  text-align: right;
}

/* Responsive */
@media (max-width: 768px) {
  .orders-header {
    flex-direction: column;
    align-items: flex-start;
  }

  .header-content h1 {
    font-size: 1.75rem;
  }

  .detail-grid {
    grid-template-columns: 1fr;
  }

  .orders-table {
    font-size: 0.85rem;
  }

  .orders-table th,
  .orders-table td {
    padding: 0.75rem 0.5rem;
  }
}
</style>
