import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useThemeStore = defineStore('theme', () => {
  const currentMood = ref('luxury'); 
  
  const setMood = (mood) => {
    currentMood.value = mood;
    document.documentElement.setAttribute('data-theme', mood);
  };
  
  if (typeof document !== 'undefined') {
    document.documentElement.setAttribute('data-theme', currentMood.value);
  }
  
  return { currentMood, setMood };
});
