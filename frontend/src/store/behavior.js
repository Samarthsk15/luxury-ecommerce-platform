import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useBehaviorStore = defineStore('behavior', () => {
  const recentInteractions = ref([]);
  const dominantCategory = ref('premium'); // Default fallback

  const trackInteraction = (category) => {
    if (!category) return;
    recentInteractions.value = [category];
    dominantCategory.value = category;
  };

  return {
    recentInteractions,
    dominantCategory,
    trackInteraction
  };
});
