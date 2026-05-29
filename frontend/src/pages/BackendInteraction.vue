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
  background: var(--bg-card);
  border: 1px solid var(--border-glass);
  padding: 1rem;
  border-radius: 16px;
}
.lane {
  position: relative;
  min-height: 100px;
  padding: 1rem 1.5rem 1rem 1rem;
  border-radius: 16px;
  display: flex;
  align-items: center;
  justify-content: center;
  text-align: center;
  color: var(--text-primary);
  background: var(--bg-surface);
  border: 1px solid var(--border-glass);
  box-shadow: 0 10px 25px rgba(0,0,0,0.08);
}
.lane:not(:last-child)::after {
  content: '';
  position: absolute;
  top: 50%;
  right: -0.75rem;
  width: 1.5rem;
  height: 2px;
  background: var(--text-secondary);
  transform: translateY(-50%);
}
.lane:not(:last-child)::before {
  content: '';
  position: absolute;
  top: 50%;
  right: -0.35rem;
  width: 0;
  height: 0;
  border-top: 5px solid transparent;
  border-bottom: 5px solid transparent;
  border-left: 8px solid var(--text-secondary);
  transform: translateY(-50%);
}
.api { background: rgba(255, 238, 212, 0.92); }
.services { background: rgba(223, 233, 255, 0.92); }
.repo { background: rgba(213, 240, 213, 0.95); }
.orm { background: rgba(255, 241, 246, 0.95); }
.db { background: rgba(232, 244, 255, 0.95); }
.flow {
  grid-column: 1 / -1;
  display: flex;
  flex-wrap: wrap;
  gap: 0.75rem;
  margin-top: 1rem;
  padding: 1rem;
  background: var(--bg-card);
  border: 1px solid var(--border-glass);
  border-radius: 16px;
}
.flow div {
  position: relative;
  flex: 1 1 calc(16.66% - 0.75rem);
  min-width: 140px;
  padding: 0.8rem;
  border-radius: 12px;
  background: var(--surface-high);
  color: var(--text-primary);
  font-size: 0.95rem;
  text-align: center;
}
.flow div:not(:last-child)::after {
  content: '→';
  position: absolute;
  right: -1.1rem;
  top: 50%;
  transform: translateY(-50%);
  color: var(--text-secondary);
  font-size: 1rem;
}
.details { margin-top: 1.2rem; color: var(--text-secondary); }
.details ol {
  margin: 1rem 0 0 1.2rem;
}
.details li { margin-bottom: 0.75rem; }
.details a { color: var(--accent-blue); }
</style>
