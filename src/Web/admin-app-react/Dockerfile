FROM node:19-alpine3.16 AS base
WORKDIR /app

FROM base AS build
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

FROM base as final
ENV NODE_ENV production
EXPOSE 5000
RUN npm install -g serve
COPY --from=builder /app/build ./build
CMD [ "serve", "build" ]