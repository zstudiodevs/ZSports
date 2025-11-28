# Etapa 1: Build Angular
FROM node:20-alpine AS build

WORKDIR /app

COPY angular/zsport-ui/package*.json ./
RUN npm install

COPY angular/zsport-ui/ ./
RUN npm run build -- --configuration docker-dev

ARG DIST_DIR=dist/zsport-ui/browser

# Etapa 2: Nginx
FROM nginx:1.27-alpine

RUN rm /etc/nginx/conf.d/default.conf
COPY docker/nginx.conf /etc/nginx/conf.d/default.conf

COPY --from=build /app/${DIST_DIR} /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]