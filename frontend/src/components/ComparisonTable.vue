<template>
  <section class="comparison-shell">
    <h2>PRODUCT COMPARISON</h2>
    <table>
      <thead>
        <tr>
          <th>Feature</th>
          <th>Product A</th>
          <th>Product B</th>
          <th>Product C</th>
          <th>Product D</th>
          <th>Product E</th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td>Name</td>
          <td>Basmati Rice</td>
          <td>Saffron</td>
          <td>Silk Saree</td>
          <td>Cow Ghee</td>
          <td>Darjeeling Tea</td>
        </tr>
        <tr>
          <td>Price</td>
          <td>₹180</td>
          <td>₹2500</td>
          <td>₹8500</td>
          <td>₹450</td>
          <td>₹280</td>
        </tr>
        <tr>
          <td>Rating</td>
          <td>4.8</td>
          <td>4.9</td>
          <td>4.7</td>
          <td>4.6</td>
          <td>4.8</td>
        </tr>
        <tr>
          <td>Discount</td>
          <td>18%</td>
          <td>17%</td>
          <td>11%</td>
          <td>18%</td>
          <td>20%</td>
        </tr>
        <tr>
          <td>Shipping</td>
          <td>Free</td>
          <td>Free</td>
          <td>₹99</td>
          <td>Free</td>
          <td>Free</td>
        </tr>
      </tbody>
    </table>
  </section>
</template>

<script setup>
</script>

<style scoped>
.comparison-shell {
  width: min(1000px, 100%);
  padding: 2rem;
  background: #ffffff;
  border-radius: 32px;
  box-shadow: 0 30px 80px rgba(15, 23, 42, 0.08);
  color: #0f172a;
}

.comparison-shell h2 {
  font-size: 2rem;
  margin-bottom: 1.5rem;
}

table {
  width: 100%;
  border-collapse: separate;
  border-spacing: 0;
  background: #f8fafc;
  border-radius: 24px;
  overflow: hidden;
}

th, td {
  padding: 1rem 1.15rem;
  text-align: center;
  color: #475569;
}

th {
  background: #e2e8f0;
  color: #0f172a;
  font-weight: 700;
}

tbody tr {
  border-bottom: 1px solid #e2e8f0;
}

tbody tr:last-child {
  border-bottom: none;
}
</style>
