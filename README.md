<p align="center">
  <img src="./client/public/assets/icons/BookLogo.svg" height="128">
</p>
<h1 align="center">
  Stori Bookstore
</h1>

Stori is a platform for book lovers to discover, track, and share their reading journey with a community of like-minded people. With over 35.000 titles, Stori offers something for everyone, from casual readers to avid bookworms. Take a deep dive and find your next read here!

This web application is a personal portfolio project built using [Next.js 15](https://nextjs.org/), [ASP.NET Core 9](https://dotnet.microsoft.com/en-us/apps/aspnet), [SQL Server](https://www.microsoft.com/en/sql-server) and a third party dataset from [goodreads](https://www.goodreads.com/).

## Features

- Explore a huge selection of books, from best reviewed to all time classics.
- Comment, rate and add your favorites to a personalized readlist/wishlist.
- Filter books by rating/date/title/category/genre/awards/settings and even characters!
- Share your profile and discover other recommendations from within the community.

## Technologies

- [Backends for frontend (BFF) architecture](https://samnewman.io/patterns/architectural/bff/).
- [Next.js 15](https://nextjs.org/) with App Router, React Server Components and Server Actions.
- [React v19](https://react.dev/) with [Typescript](https://www.typescriptlang.org/)
- State management using [Zustand](https://zustand-demo.pmnd.rs/).
- [Tailwind CSS](https://tailwindcss.com/).
- MVC on [ASP.NET Core 9 Web API](https://dotnet.microsoft.com/en-us/apps/aspnet), [Entity Framework Core](https://learn.microsoft.com/en-us/ef/) and [SQL Server](https://www.microsoft.com/en/sql-server).
- [Docker](https://www.docker.com/) integration.
- Designed on [Figma](https://www.figma.com/).
- Assets from [unDraw](https://undraw.co/) and [MingCute](https://www.mingcute.com/).

## Preview

🎨 Figma mockup available at: https://figma.fun/LeWgQL

## Build it yourself

The recommended way of running this project is by installing [Docker Desktop](https://www.docker.com/). With Docker up and running, follow these steps:

### Client application (NextJS Frontend)

Copy and modify (if necessary) the environment variables file.

```bash
cp client/.env.example client/.env.development.local
```

After that, bring up the client container by running:

```bash
docker compose -f client/docker-compose.yml --profile dev up -d
```

### Server application (ASP.Net Core Backend + SQL Server):

For the server container, there is no environment file to be set, you can just run it with:

```bash
docker compose -f server/docker-compose.yml --profile dev up -d
```

> Migrations will be applied automatically when the server container is ran in development mode.

With both compose projects launched, you can view the application at:

NextJS Frontend: http://localhost:8080

ASP.Net Core Backend API endpoints: http://localhost:8081/api/v1/

Swagger endpoint (Development mode only): http://localhost:8081

Detailed information for each project can be viewed at their own subdirectory.

Have a good adventure! ⛵☀️

## License

This project is licensed under the MIT License — see the [LICENSE.md](LICENSE.md) file for details.
