<template>
  <div class="home-page luxury-theme">
    <WelcomeModal />
    <LuxuryHero />
    <BrandStory />
    <PremiumShowcase />
    <MembershipCTA />
  </div>
</template>

<script setup>
import WelcomeModal from '../components/WelcomeModal.vue';
import LuxuryHero from '../components/LuxuryHero.vue';
import BrandStory from '../components/BrandStory.vue';
import PremiumShowcase from '../components/PremiumShowcase.vue';
import MembershipCTA from '../components/MembershipCTA.vue';
</script>

<style scoped>
.home-page {
  display: flex;
  flex-direction: column;
  background: var(--bg-void);
}

.luxury-theme {
  --space-xl: 6rem;
}
</style>
