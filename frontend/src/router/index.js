import { createRouter, createWebHistory } from 'vue-router';
import HomePage from '../pages/HomePage.vue';
import LoginPage from '../pages/LoginPage.vue';
import CartPage from '../pages/CartPage.vue';
import OffersPage from '../pages/OffersPage.vue';
import ProductsPage from '../pages/ProductsPage.vue';
import { useAuthStore } from '../store/auth.js';

const routes = [
  { path: '/login', name: 'Login', component: LoginPage },
  { path: '/', name: 'Home', component: HomePage, meta: { requiresAuth: true } },
  { path: '/cart', name: 'Cart', component: CartPage, meta: { requiresAuth: true } },
  { path: '/deals', name: 'Deals', component: OffersPage, meta: { requiresAuth: true } },
  { path: '/products', name: 'Products', component: ProductsPage, meta: { requiresAuth: true } },
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

  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    next('/login');
  } else {
    next();
  }
});

export default router;
