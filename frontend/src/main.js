import { createApp } from 'vue';
import { createPinia } from 'pinia';
import router from './router';
import App from './App.vue';
import './styles.css';

const app = createApp(App);

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

app.mount('#app');
