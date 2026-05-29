<template>
  <div class="page backend-interaction">
    <h1>Backend API — Database Interaction</h1>
    <div ref="diagram" class="diagram">Loading diagram…</div>
    <p>Mermaid source: <a href="/docs/backend-db-interaction.mmd" target="_blank">docs/backend-db-interaction.mmd</a></p>
  </div>
</template>

<script>
export default {
  name: 'BackendInteraction',
  data() {
    return {
      diagram: `sequenceDiagram\n    participant Client\n    participant API as Backend API (Controllers)\n    participant Svc as Services\n    participant Repo as Repositories\n    participant ORM as DbContext / ORM\n    participant DB as Database\n\n    Client->>API: HTTP request (REST/GraphQL)\n    API->>Svc: validate + auth\n    Svc->>Repo: business logic -> data request\n    Repo->>ORM: build query / ORM call\n    ORM->>DB: SQL executed\n    DB-->>ORM: rows / result\n    ORM-->>Repo: mapped entities\n    Repo-->>Svc: domain objects\n    Svc-->>API: response DTO\n    API-->>Client: HTTP response (JSON)`
    };
  },
  mounted() {
    const render = () => {
      const container = this.$refs.diagram;
      if (window.mermaid && window.mermaid.mermaidAPI) {
        try {
          window.mermaid.initialize({ startOnLoad: false });
          window.mermaid.mermaidAPI.render('mermaidDiagram', this.diagram, (svgCode) => {
            container.innerHTML = svgCode;
          });
        } catch (e) {
          container.textContent = 'Failed to render diagram: ' + e;
        }
      } else {
        setTimeout(render, 150);
      }
    };

    if (!window._mermaidLoaded) {
      const s = document.createElement('script');
      s.src = 'https://cdn.jsdelivr.net/npm/mermaid@10/dist/mermaid.min.js';
      s.onload = () => {
        window._mermaidLoaded = true;
        render();
      };
      s.onerror = () => {
        this.$refs.diagram.textContent = 'Unable to load Mermaid library.';
      };
      document.head.appendChild(s);
    } else {
      render();
    }
  }
};
</script>

<style scoped>
.backend-interaction { padding: 1rem; }
.diagram { margin-top: 1rem; border: 1px solid #eee; padding: 1rem; background: #fff; }
</style>
