# --- Build stage ---
FROM node:22-alpine AS build
WORKDIR /src

# Restore first (separate layer so `npm ci` is cached unless package*.json changes)
COPY package*.json ./
RUN npm ci

# Which Angular build configuration to bake in. "production" (the default) points
# API_BASE_URL at localhost:8080 — correct for running the full stack locally via
# docker compose. Override with --build-arg NG_CONFIGURATION=hetzner to bake in the
# Hetzner preview server's IP instead (see docker-compose.hetzner.yaml).
ARG NG_CONFIGURATION=production

# Copy the rest of the source and build (outputs both the browser bundle and the
# Node SSR server bundle)
COPY . .
RUN npx ng build --configuration "$NG_CONFIGURATION"

# --- Runtime stage ---
FROM node:22-alpine AS final
WORKDIR /app

ENV NODE_ENV=production
ENV PORT=4000
EXPOSE 4000

# Full node_modules copied over rather than reinstalled, so whatever the SSR bundle
# actually needs at runtime (express, etc.) is guaranteed present without guessing.
COPY --from=build /src/node_modules ./node_modules
COPY --from=build /src/dist ./dist
COPY --from=build /src/package.json ./package.json

CMD ["node", "dist/icof-web/server/server.mjs"]
