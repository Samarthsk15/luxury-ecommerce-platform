import { defineStore } from 'pinia';
import { ref, computed } from 'vue';

export const useAdvertisementStore = defineStore('advertisement', () => {
  const auctionBids = ref([
    { id: 101, product: 'Noise Cancelling Headphones', amount: 1500, rank: 1, status: 'Live (Homepage)', imageUrl: 'https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=800&q=80', description: 'Experience pure audio clarity and deep bass with our flagship noise-cancelling technology.' },
    { id: 102, product: 'Smart Home Hub 2.0', amount: 1000, rank: 2, status: 'Live (Search)', imageUrl: 'https://images.unsplash.com/photo-1558002038-1055907df827?w=800&q=80', description: 'Control your entire home ecosystem from a single, beautiful glass interface.' },
    { id: 103, product: 'Ergonomic Desk Chair', amount: 400, rank: 3, status: 'Banner Only', imageUrl: 'https://images.unsplash.com/photo-1505843490538-5133c6c7d0e1?w=800&q=80', description: 'Premium comfort for long hours of focused work and creativity.' },
    { id: 104, product: 'Mechanical Keyboard v3', amount: 250, rank: 4, status: 'Outbid', imageUrl: 'https://images.unsplash.com/photo-1511467687858-23d96c32e4ae?w=800&q=80', description: 'Tactile typing experience with customizable RGB lighting and premium switches.' }
  ]);

  const rank1Product = computed(() => auctionBids.value.find(b => b.rank === 1));
  const rank2Product = computed(() => auctionBids.value.find(b => b.rank === 2));
  const rank3Product = computed(() => auctionBids.value.find(b => b.rank === 3));

  const simulateNewBid = () => {
    const newAmount = Math.floor(Math.random() * 2000) + 500;
    
    const mockProducts = [
      { product: 'Gaming Monitor 4K', imageUrl: 'https://images.unsplash.com/photo-1527443224154-c4a3942d3acf?w=800&q=80', description: 'Ultra-wide 4K resolution with 144Hz refresh rate for competitive edge.' },
      { product: 'Wireless Mouse Pro', imageUrl: 'https://images.unsplash.com/photo-1527864550417-7fd91fc51a46?w=800&q=80', description: 'Zero latency wireless technology with an ergonomic shape.' },
      { product: 'Leather Wallet', imageUrl: 'https://images.unsplash.com/photo-1627123424574-724758594e93?w=800&q=80', description: 'Hand-stitched Italian leather with RFID protection.' },
      { product: 'Luxury Watch', imageUrl: 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=800&q=80', description: 'Timeless elegance combined with modern precision.' }
    ];
    
    const newProduct = mockProducts[Math.floor(Math.random() * mockProducts.length)];
    
    auctionBids.value.push({
      id: Date.now(),
      product: newProduct.product,
      amount: newAmount,
      rank: 0,
      status: '',
      imageUrl: newProduct.imageUrl,
      description: newProduct.description
    });
    
    auctionBids.value.sort((a, b) => b.amount - a.amount);
    
    auctionBids.value.forEach((bid, index) => {
      bid.rank = index + 1;
      if (bid.rank === 1) bid.status = 'Live (Homepage)';
      else if (bid.rank === 2) bid.status = 'Live (Search)';
      else if (bid.rank === 3) bid.status = 'Banner Only';
      else bid.status = 'Outbid';
    });
    
    if (auctionBids.value.length > 6) {
       auctionBids.value = auctionBids.value.slice(0, 6);
    }
  };

  const placeBid = (productName, amount) => {
    const existingIndex = auctionBids.value.findIndex(b => b.product.toLowerCase() === productName.toLowerCase());
    
    if (existingIndex !== -1) {
      auctionBids.value[existingIndex].amount = amount;
    } else {
      auctionBids.value.push({
        id: Date.now(),
        product: productName,
        amount: amount,
        rank: 0,
        status: '',
        imageUrl: 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=800&q=80',
        description: 'Premium custom bid product.'
      });
    }

    auctionBids.value.sort((a, b) => b.amount - a.amount);
    
    auctionBids.value.forEach((bid, index) => {
      bid.rank = index + 1;
      if (bid.rank === 1) bid.status = 'Live (Homepage)';
      else if (bid.rank === 2) bid.status = 'Live (Search)';
      else if (bid.rank === 3) bid.status = 'Banner Only';
      else bid.status = 'Outbid';
    });
    
    if (auctionBids.value.length > 6) {
       auctionBids.value = auctionBids.value.slice(0, 6);
    }
  };

  return { auctionBids, rank1Product, rank2Product, rank3Product, simulateNewBid, placeBid };
});
