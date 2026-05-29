<template>
  <div class="page backend-interaction">
    <h1>Backend API — Database Interaction</h1>
    <div class="diagram static-diagram">
      <div class="lane client">Client</div>
      <div class="lane api">Backend API<br />(Controllers)</div>
      <div class="lane services">Services</div>
      <div class="lane repo">Repositories</div>
      <div class="lane orm">DbContext / ORM</div>
      <div class="lane db">Database</div>
      <div class="flow">
        <div>HTTP request</div>
        <div>Validate/auth</div>
        <div>Business logic</div>
        <div>Build query</div>
        <div>SQL execute</div>
        <div>Return rows</div>
      </div>
    </div>
    <div class="details">
      <p>This page shows the interaction between the client, backend API, services, repository layer, ORM, and database.</p>
      <ol>
        <li><strong>Client</strong> sends an HTTP request to the backend API.</li>
        <li><strong>Backend API</strong> validates authentication and forwards the request to service logic.</li>
        <li><strong>Services</strong> execute business rules and call repositories.</li>
        <li><strong>Repositories</strong> build ORM/database queries.</li>
        <li><strong>ORM / DbContext</strong> executes SQL against the database.</li>
        <li><strong>Database</strong> returns rows/results back up the chain.</li>
      </ol>
      <p>Mermaid source: <a href="/docs/backend-db-interaction.mmd" target="_blank">docs/backend-db-interaction.mmd</a></p>
    </div>
  </div>
</template>

<script>
export default {
  name: 'BackendInteraction'
};
</script>

<style scoped>
.backend-interaction { padding: 1.5rem; }
.static-diagram {
  display: grid;
  grid-template-columns: repeat(6, minmax(120px, 1fr));
  gap: 1rem;
  margin-top: 1rem;
  background: rgba(255,255,255,0.08);
  border: 1px solid rgba(255,255,255,0.15);
  padding: 1rem;
  border-radius: 16px;
}
.lane {
  min-height: 100px;
  padding: 1rem;
  border-radius: 16px;
  display: flex;
  align-items: center;
  justify-content: center;
  text-align: center;
  color: #111;
  background: #f9f9fb;
  box-shadow: 0 10px 25px rgba(0,0,0,0.06);
}
.api { background: #ffeed4; }
.services { background: #dfe9ff; }
.repo { background: #d5f0d5; }
.orm { background: #fff1f6; }
.db { background: #e8f4ff; }
.flow {
  grid-column: 1 / -1;
  display: flex;
  flex-wrap: wrap;
  gap: 0.75rem;
  margin-top: 1rem;
  padding: 1rem;
  background: rgba(255,255,255,0.95);
  border-radius: 16px;
}
.flow div {
  flex: 1 1 calc(16.66% - 0.75rem);
  min-width: 140px;
  padding: 0.8rem;
  border-radius: 12px;
  background: #232323;
  color: #fff;
  font-size: 0.95rem;
  text-align: center;
}
.details { margin-top: 1.2rem; color: #eee; }
.details ol {
  margin: 1rem 0 0 1.2rem;
}
.details li { margin-bottom: 0.75rem; }
.details a { color: #a0d8ff; }
</style>

<style scoped>
.backend-interaction { padding: 1rem; }
.diagram { margin-top: 1rem; border: 1px solid #eee; padding: 1rem; background: #fff; }
</style>
