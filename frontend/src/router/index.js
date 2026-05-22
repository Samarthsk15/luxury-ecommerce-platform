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
  { path: '/', name: 'Home', component: HomePage, meta: { requiresAuth: true } },
  { path: '/cart', name: 'Cart', component: CartPage, meta: { requiresAuth: true } },
  { path: '/checkout', name: 'Checkout', component: () => import('../pages/CheckoutPage.vue'), meta: { requiresAuth: true } },
  { path: '/checkout-success', name: 'CheckoutSuccess', component: SuccessPage, meta: { requiresAuth: true } },
  { path: '/deals', name: 'Deals', component: OffersPage, meta: { requiresAuth: true } },
  { path: '/products', name: 'Products', component: ProductsPage, meta: { requiresAuth: true } },
  { path: '/admin', name: 'Admin', component: () => import('../pages/AdminDashboard.vue'), meta: { requiresAuth: true, requiresAdmin: true } },
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
  const isPortAdmin = typeof window !== 'undefined' && window.location.port === '5174';

  if (isPortAdmin && to.path === '/') {
    next('/admin');
    return;
  }

  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    next('/login');
  } else if (to.meta.requiresAdmin && !authStore.isAdmin) {
    alert("Access Denied: Administrator privileges required.");
    next('/');
  } else {
    next();
  }
});

export default router;
