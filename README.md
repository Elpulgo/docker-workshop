# A practical docker workshop

This repository is designed as a hands-on tutorial for developers to learn how to containerize a full-stack .NET application using Docker and Docker Compose. The workshop challenges you to discuss topics and common scenarios and pitfalls when using Docker and Docker Compose. 

The repository is designed in such a way that participants don't need to look at or modify the actual code in the .NET application, focusing on the objective of running it in Docker instead.

This was built and designed by me on a workshop I held at one of my assignments.

## 🧰 Workshop Overview

Objective: Learn to containerize and orchestrate a .NET Web API, a frontend application served by Nginx, and a SQL Server database using Docker Compose and make them communicate together.

## Key Components:

    Backend API (GandalfApi): A .NET Web API project.

    Frontend UI (Legolas.UI): A frontend application, built with vanilla Javascript and served via Nginx.

    Shared Library (Saruman.Core): A shared .NET library used across the application.

    SQL Server Database: Used for data persistence.

    Docker Compose Files: docker-compose.yml: Defines the services and their configurations.

## 📁 Repository Structure

    GandalfApi/: Contains the backend API source code.
      - Dockerfile: An empty Dockerfile where the participants need to containerize GandalfApi

    Legolas.UI/: Contains the frontend application source code.
      - Dockerfile: An empty Dockerfile where the participants need to containerize Legolas.UI
      - dist/: The source code for the frontend application
      - default.conf.template: an nginx configuration which should be used to serve Legolas.UI

    Saruman.Core/: Contains shared code or models used by both frontend and backend.

    docker-compose.yml: Empty Docker Compose configuration file where the participants should tie together the containers
    

## 🎯 Learning Outcomes

By completing this workshop, you will:

    - Understand how to containerize a .NET Web API and a frontend application.

    - Learn to configure Nginx to serve a frontend application.

    - Set up and connect a SQL Server database within a Docker environment.

    - Use Docker Compose to orchestrate multi-container applications.

    - Manage inter-service communication and environment configurations.

    - Learn basic pitfalls/configuration scenarios which requires familiarity with docker cli to solve.

## 🧪 Solution Branch

The feature/solution branch provides a suggested completed version of the workshop. This branch includes fully configured services and Docker Compose files, serving as a reference if you encounter issues during the workshop. The solution has a prefix of *Lab.

This workshop is ideal for developers looking to gain practical experience with Docker in a .NET ecosystem, providing a comprehensive introduction to containerizing and orchestrating full-stack applications.