# Use an official Node.js image as the base image
FROM node:20-alpine

# Set working directory inside the container
WORKDIR /app

# Copy package.json and package-lock.json files
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application source code
COPY . .

# Expose port 3000
EXPOSE 3000

# Start the React development server
CMD ["npm", "start"]


# Step 1: Build the React app
# FROM node:20-alpine AS build
# WORKDIR /app

# # Install dependencies
# COPY package*.json ./
# RUN npm install

# # Copy the remaining files and build the app
# COPY . ./
# RUN npm run build

# # Step 2: Serve the built app with Nginx
# FROM nginx:alpine
# COPY --from=build /app/build /usr/share/nginx/html

# # Expose port 3000 to the outside world
# EXPOSE 3000

# # Start Nginx when the container starts
# CMD ["nginx", "-g", "daemon off;"]