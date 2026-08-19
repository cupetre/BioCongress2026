# --- Build stage ---
FROM node:22-alpine AS build
WORKDIR /src

# Restore first (separate layer so `npm ci` is cached unless package*.json changes)
COPY package*.json ./
RUN npm ci

# Copy the rest of the source and build (default `ng build` config = production,
# outputs both the browser bundle and the Node SSR server bundle)
COPY . .
RUN npm run build

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
