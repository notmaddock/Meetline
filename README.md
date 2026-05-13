# Meetline

![Build and test](https://github.com/SowTag/Meetline/actions/workflows/dotnet.yml/badge.svg)
![Frontend build](https://github.com/SowTag/Meetline/actions/workflows/frontend.yml/badge.svg)
![Docker](https://github.com/SowTag/Meetline/actions/workflows/docker-publish.yml/badge.svg)

Meetline is an EdTech-oriented online chat and video conferencing platform.

**WARNING:** This is an unfinished project. Even this README is. **Everything** is subject to change

### Architecture

Meetline is built on top of ASP.NET and React. It uses .NET Aspire for infrastructure orchestration locally and 
(in the future) deployments and a TanStack Router-based frontend SPA.

The backend's architecture is a modular monolith that could allow separating modules into separate services in the future.
All modules have their own database within a single Postgres container, so that they're built with decoupling in mind.


### Running locally

You can run Meetline locally just by installing the [.NET SDK 10+](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) 
and the [Aspire CLI](https://aspire.dev/). Run `aspire run` and all containers will spin up automatically. 
Migrations may throw an exception the first time the volumes are created because Entity Framework Core won't find the migrations 
table. This is expected.

### Deploying

Deployment is not available yet. In the future I'll set up deployment with Aspire (`aspire publish` and `aspire deploy`)
but for now Meetline is in deep alpha.
