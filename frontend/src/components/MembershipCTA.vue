<template>
  <section class="membership-cta">
    <div class="cta-background">
      <div class="glow-orb"></div>
    </div>
    
    <div class="cta-content" v-scroll-reveal>
      <span class="eyebrow">Exclusive Access</span>
      <h2 class="cta-title">Join The <span class="italic-gold">Inner Circle</span></h2>
      <p class="cta-desc">
        Unlock a world of privileges. Gain early access to limited editions, bespoke services, and invitations to private viewing events.
      </p>
      
      <form @submit.prevent="handleSubscribe" class="subscribe-form">
        <div class="input-group">
          <input type="email" placeholder="Enter your email address" required v-model="email" />
          <button type="submit" class="btn-submit">Request Invitation</button>
        </div>
      </form>
    </div>
  </section>
</template>

<script setup>
import { ref } from 'vue';

const email = ref('');

const handleSubscribe = () => {
  if (email.value) {
    alert(`Invitation requested for ${email.value}. We will contact you shortly.`);
    email.value = '';
  }
};
</script>

<style scoped>
.membership-cta {
  position: relative;
  padding: 8rem 1rem;
  display: flex;
  justify-content: center;
  align-items: center;
  background: var(--bg-void);
  overflow: hidden;
  border-top: 1px solid rgba(255,255,255,0.05);
  border-bottom: 1px solid rgba(255,255,255,0.05);
}

.cta-background {
  position: absolute;
  inset: 0;
  z-index: 0;
  display: flex;
  justify-content: center;
  align-items: center;
}

.glow-orb {
  width: 50vw;
  height: 50vw;
  max-width: 600px;
  max-height: 600px;
  background: radial-gradient(circle, rgba(212, 175, 55, 0.1) 0%, transparent 70%);
  border-radius: 50%;
  filter: blur(40px);
}

.cta-content {
  position: relative;
  z-index: 10;
  text-align: center;
  max-width: 600px;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1.5rem;
}

.eyebrow {
  font-family: var(--font-body);
  text-transform: uppercase;
  letter-spacing: 0.2em;
  font-size: 0.75rem;
  color: var(--text-secondary);
}

.cta-title {
  font-size: clamp(2.5rem, 5vw, 4rem);
  font-weight: 300;
  text-transform: uppercase;
  letter-spacing: -0.02em;
  line-height: 1.1;
}

.italic-gold {
  font-style: italic;
  color: var(--accent-violet);
}

.cta-desc {
  color: var(--text-secondary);
  font-weight: 300;
  line-height: 1.6;
  margin-bottom: 1.5rem;
}

.subscribe-form {
  width: 100%;
}

.input-group {
  display: flex;
  background: rgba(255,255,255,0.03);
  border: 1px solid rgba(255,255,255,0.1);
  padding: 0.5rem;
  border-radius: 2px;
  transition: border-color 0.3s;
}

.input-group:focus-within {
  border-color: rgba(212, 175, 55, 0.5);
}

input {
  flex-grow: 1;
  background: transparent;
  border: none;
  color: var(--text-primary);
  padding: 0.8rem 1rem;
  font-size: 0.9rem;
}

input::placeholder {
  color: rgba(255,255,255,0.3);
  text-transform: uppercase;
  letter-spacing: 0.05em;
  font-size: 0.8rem;
}

.btn-submit {
  background: var(--text-primary);
  color: var(--bg-void);
  padding: 0.8rem 1.5rem;
  text-transform: uppercase;
  letter-spacing: 0.1em;
  font-size: 0.8rem;
  transition: all 0.3s;
}

.btn-submit:hover {
  background: var(--accent-violet);
  color: var(--bg-void);
}

@media (max-width: 600px) {
  .input-group {
    flex-direction: column;
    background: transparent;
    border: none;
    padding: 0;
    gap: 1rem;
  }
  
  input {
    background: rgba(255,255,255,0.03);
    border: 1px solid rgba(255,255,255,0.1);
  }
  
  .btn-submit {
    width: 100%;
  }
}
</style>
