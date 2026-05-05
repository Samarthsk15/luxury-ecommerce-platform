<template>
  <footer class="luxe-footer">
    <div class="footer-inner">
      <div class="footer-brand">
        <span class="footer-logo">LUXE<span class="dot">.</span></span>
        <p class="footer-tagline">Premium shopping, redefined.</p>
      </div>
      <div class="footer-links">
        <div class="link-group">
          <h4>Shop</h4>
          <router-link to="/products">All Products</router-link>
          <router-link to="/deals">Deals</router-link>
          <router-link to="/">Categories</router-link>
        </div>
        <div class="link-group">
          <h4>Account</h4>
          <router-link to="/cart">Cart</router-link>
          <router-link to="/login">Login</router-link>
        </div>
        <div class="link-group">
          <h4>Support</h4>
          <a href="#">Help Center</a>
          <a href="#">Contact Us</a>
          <a href="#">Returns</a>
        </div>
      </div>
    </div>
    <div class="footer-bottom">
      <span>© 2026 LUXE. All rights reserved.</span>
    </div>
  </footer>
</template>

<style scoped>
.luxe-footer {
  margin-top: var(--space-xl);
  border-top: 1px solid var(--border-glass);
  background: rgba(10, 10, 15, 0.6);
  backdrop-filter: blur(12px);
}
.footer-inner {
  max-width: var(--max-width);
  margin: 0 auto;
  padding: var(--space-xl) var(--gutter);
  display: flex;
  justify-content: space-between;
  gap: var(--space-xl);
}
.footer-logo {
  font-family: var(--font-headline);
  font-size: 1.5rem;
  font-weight: 800;
  color: var(--text-primary);
}
.footer-logo .dot { color: var(--accent-blue); }
.footer-tagline {
  color: var(--text-muted);
  font-size: 0.85rem;
  margin-top: 0.5rem;
}
.footer-links {
  display: flex;
  gap: var(--space-xl);
}
.link-group {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}
.link-group h4 {
  font-family: var(--font-headline);
  font-size: 0.8rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.08em;
  color: var(--text-secondary);
  margin-bottom: 0.25rem;
}
.link-group a {
  font-size: 0.85rem;
  color: var(--text-muted);
  transition: color 0.2s ease;
}
.link-group a:hover { color: var(--text-primary); }
.footer-bottom {
  max-width: var(--max-width);
  margin: 0 auto;
  padding: var(--space-sm) var(--gutter);
  border-top: 1px solid var(--border-glass);
  text-align: center;
  font-size: 0.75rem;
  color: var(--text-muted);
}
</style>
