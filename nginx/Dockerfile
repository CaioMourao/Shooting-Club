FROM node:18-alpine AS frontend-builder
WORKDIR /app

COPY ./src/Frontend/package*.json ./
RUN npm install

COPY ./src/Frontend/ .

RUN npm run build

FROM nginx:alpine

COPY ./nginx/nginx.conf /etc/nginx/conf.d/default.conf

COPY --from=frontend-builder /app/dist /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]