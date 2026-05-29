import { createRouter, createWebHistory } from 'vue-router';
import HomePage from '../pages/HomePage.vue';
import LoginPage from '../pages/LoginPage.vue';
import CartPage from '../pages/CartPage.vue';
import OffersPage from '../pages/OffersPage.vue';
import ProductsPage from '../pages/ProductsPage.vue';
import SuccessPage from '../pages/SuccessPage.vue';
import { useAuthStore } from '../store/auth.js';

const routes = [
  { path: '/login', name: 'Login', component: LoginPage },
  { path: '/', name: 'Home', component: HomePage },
  { path: '/cart', name: 'Cart', component: CartPage },
  { path: '/checkout', name: 'Checkout', component: () => import('../pages/CheckoutPage.vue'), meta: { requiresAuth: true } },
  { path: '/checkout-success', name: 'CheckoutSuccess', component: SuccessPage, meta: { requiresAuth: true } },
  { path: '/deals', name: 'Deals', component: OffersPage },
  { path: '/products', name: 'Products', component: ProductsPage },
  { path: '/admin', name: 'Admin', component: () => import('../pages/AdminDashboard.vue'), meta: { requiresAuth: true, requiresAdmin: true } },
  { path: '/admin/orders', name: 'AdminOrders', component: () => import('../pages/AdminOrders.vue'), meta: { requiresAuth: true, requiresAdmin: true } },
  { path: '/admin/bidding', name: 'AdminBidding', component: () => import('../pages/BiddingPortal.vue'), meta: { requiresAuth: true, requiresAdmin: true } },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
  scrollBehavior() {
    return { top: 0 };
  },
});

router.beforeEach((to, from, next) => {
  const authStore = useAuthStore();
  const isAdminHost = typeof window !== 'undefined' && window.location.port === '5174';

  if (isAdminHost && to.path === '/') {
    next('/admin');
    return;
  }

  if (!isAdminHost && to.path.startsWith('/admin')) {
    next('/');
    return;
  }

  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    next({ path: '/login', query: { redirect: to.fullPath } });
  } else if (to.meta.requiresAdmin && !authStore.isAdmin) {
    alert("Access Denied: Administrator privileges required.");
    next({ path: '/login', query: { redirect: to.fullPath } });
  } else {
    next();
  }
});

export default router;
