# Stori - Client application (NextJS Frontend)

This is a [Next.js 15](https://nextjs.org) application bootstrapped with [`create-next-app`](https://nextjs.org/docs/app/api-reference/cli/create-next-app). This project has [Docker](https://www.docker.com/) support and it is the easiest method of setting up and running both the Development and Production enviroments. Instructions for manual deployment is also included and the details are available below.

## Getting started

### Docker setup (Recommended)

Download, install and start [Docker Desktop](https://www.docker.com/).

Copy and modify (if necessary) the environment variables file.

```bash
cp .env.example .env.development.local
```

> Note: When running this project under Docker, make sure to match the `BACKEND_HOSTNAME` enviroment variable to the right backend Docker hostname. This is set to the development hostname by default, and in case you are running the client manually, change its value to `localhost` (or whichever hostname you are using) in order to connect to the server!

After that, bring up the container by running:

```bash
docker compose --profile dev up -d
```

To stop the container and delete the allocated resources, run:

```bash
docker compose --profile dev down
```

The client application will be launched in Development mode and can be viewed at http://localhost:8080 in your browser.

#### Production environment

To run the container in the production environment, copy the `.env.example` once again, but rename the destination to `.env.production.local`.

```bash
cp .env.example .env.production.local
```

> Note: Remember to change the `BACKEND_HOSTNAME` environment variable value to the production backend Docker hostname, assuming it will also be ran in production, the default hostname is: `stori_backend_prod`

Now, to bring up the container:

```bash
docker compose --profile prod up -d
```

To stop the container and delete the allocated resources, run:

```bash
docker compose --profile prod down
```

The client application will be launched in Production mode and can be viewed at http://localhost:8080 in your browser.

### <a name="manual-setup"></a>Manual setup

Install the latest [NodeJS LTS](https://nodejs.org/en).

Then, download and install the needed dependencies with:

```bash
npm install
```

Now, copy and modify the environment variables file:

```bash
cp .env.example .env.development.local
```

> Note: Remember to change the `BACKEND_HOSTNAME` environment variable value to `localhost` (or whichever hostname your backend is running on), since launching the project manually won't use the Docker networking name resolution.

And run the development server with:

```bash
npm run dev
```

The client application will be launched in Development mode and can be viewed at http://localhost:3000 in your browser.

#### Production environment

To run the container in the production environment, copy the `.env.example` once again, but rename the destination to `.env.production.local`.

```bash
cp .env.example .env.production.local
```

> Note: Remember to change the `BACKEND_HOSTNAME` environment variable value to `localhost` (or whichever hostname your backend is running on), since launching the project manually won't use the Docker networking name resolution.

Next, build the project using:

```bash
npm run build
```

And finally, to bring up the server:

```bash
node .next/standalone/server.js
```

The client application will be launched in Production mode and can be viewed at http://localhost:3000 in your browser.

## Testing

### Docker setup

To run tests inside Docker, no environment variable file needs to be set, just run:

```bash
docker compose --profile test up
```

### Manual setup

Follow the same steps as in the [manual setup](#manual-setup), installing Node and the project dependencies, then run:

```bash
npm run test
```

## Generate Schema Definitions

If the backend API was modified and a new client schema needs to be generated, there is a script available that automatically fetches the new data from Swagger and creates the updated version.

I haven't setup a Docker Compose profile for it, so in order to run, follow the [manual setup](#manual-setup) instructions installing Node and the project dependencies and after that, run:

```bash
npm run generateApiSchema
```

An updated typing definition file will be created and saved at `src/typings/api.d.ts`.
