import { createApp } from 'vue';
import { createPinia } from 'pinia';
import router from './router';
import App from './App.vue';
import './styles.css';

const app = createApp(App);

// Magnetic Hover Directive
app.directive('magnetic', {
  mounted(el) {
    el.addEventListener('mousemove', (e) => {
      const rect = el.getBoundingClientRect();
      const x = e.clientX - rect.left - rect.width / 2;
      const y = e.clientY - rect.top - rect.height / 2;
      
      el.style.transform = `translate(${x * 0.3}px, ${y * 0.3}px)`;
      el.style.transition = 'transform 0.1s ease-out';
    });
    
    el.addEventListener('mouseleave', () => {
      el.style.transform = 'translate(0, 0)';
      el.style.transition = 'transform 0.5s cubic-bezier(0.175, 0.885, 0.32, 1.275)';
    });
  }
});

// Scroll Reveal Directive (Intersection Observer)
app.directive('scroll-reveal', {
  mounted(el) {
    el.classList.add('reveal-hidden');
    const observer = new IntersectionObserver(
      (entries, observer) => {
        entries.forEach((entry) => {
          if (entry.isIntersecting) {
            el.classList.add('reveal-visible');
            observer.unobserve(el);
          }
        });
      },
      { threshold: 0.1, rootMargin: '0px 0px -50px 0px' }
    );
    observer.observe(el);
  }
});

app.config.errorHandler = (err, instance, info) => {
  const errorDiv = document.createElement('div');
  errorDiv.style.cssText = 'position:fixed;top:0;left:0;right:0;background:red;color:white;padding:20px;z-index:9999;white-space:pre-wrap;font-family:monospace;';
  errorDiv.textContent = `Vue Error: ${err.message}\n\n${err.stack}\n\nInfo: ${info}`;
  document.body.appendChild(errorDiv);
  console.error(err, info);
};

window.addEventListener('error', (event) => {
  const errorDiv = document.createElement('div');
  errorDiv.style.cssText = 'position:fixed;top:0;left:0;right:0;background:darkred;color:white;padding:20px;z-index:9999;white-space:pre-wrap;font-family:monospace;';
  errorDiv.textContent = `Global Error: ${event.message}\n\n${event.error?.stack}`;
  document.body.appendChild(errorDiv);
});

const pinia = createPinia();
app.use(pinia);
app.use(router);

// Initialize auth store and check for existing authentication
import { useAuthStore } from './store/auth.js';
const authStore = useAuthStore();
authStore.checkAuth();

// Clear ephemeral browsing history on full page load/refresh.
// Note: we intentionally do NOT clear `cart_local` so cart persists across refresh/login.
window.addEventListener('load', () => {
  try {
    localStorage.removeItem('recentlyViewed');
    localStorage.removeItem('searchHistory');
    localStorage.removeItem('history');
    sessionStorage.clear();
  } catch (e) {
    console.warn('Failed to clear browsing history keys on load', e);
  }
});

app.mount('#app');
