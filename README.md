# Movie Price Comparison App

This web app compares prices for the same movies across multiple providers and highlights the cheapest option. It handles flaky APIs and aims to demonstrate clear separation of concerns, graceful fallback logic with a clean UI.

## Features

- Compares prices from **Cinemaworld** and **Filmworld**
- Shows **cheapest provider**
- Handles **intermittent API failures**
- Simple, responsive **table UI** with support for multiple providers
- **Proxy server** used to bypass CORS and protect tokens
- Includes loading indicators and error messaging

## Assumptions

- Movies are matched **by title** between providers.
- A movie is only shown if it's available from **all working providers**.
- If one provider fails, the app continues with the available data.
- Poster and year are optional and included only if available.
- This solution should be able to run locally and doesn't need to be setup to be deployed at this point.

## Running Locally

1. **Install dependencies**

```bash
npm install
```

2. Add a .env file (at the root of this project) with the below content inside. Make sure to update the API token value

```env
API_TOKEN={your-api-token-here}
EXTERNAL_API_BASE=http://webjetapitest.azurewebsites.net/api
```

3. Start the proxy server

```bash
npm run proxy
```

4. Start the frontend dev server

```bash
npm run dev
```

This will launch the app on:

- Frontend: http://localhost:5173
- Proxy server: http://localhost:5001

## Design

- The proxy server (proxy-server.ts) attaches the API token and handles CORS.
- The app avoids making requests in child components and keeps data logic inside the hook.
- If neither provider responds, a user friendly message is shown.
- The table UI shows title, poster, year, individual prices, and the cheapest provider.
- Movie poster and year are optional fields and are included if available from detail API responses.

### Why a Proxy Server Was Needed

The Webjet API does not support CORS (Cross-Origin Resource Sharing), which prevents frontend applications from making direct API requests in the browser due to security restrictions. I discovered this limitation by trying a directly make a call to the API from the React frontend, which resulted in a CORS policy error in the browser console.

To solve this:

- A lightweight proxy server was created using Express to make server side requests.
- This server attaches the token, forwards the request to Webjet API, and sends back the response, bypassing the browser's CORS restriction.
- This keeps the token hidden from the client, therefore safely keeps the token a secret.
- Allows future enhancements like logging or caching without frontend changes.

**Alternatives considered**

- **Backend service** - If expanding this service, I would have definetly added a backend service for clearer seperation of concerns and to add flexibility for authentication, logging and caching. But as this was a simple challenge, a simple proxy was sufficient and avoided an overengineered solution.
- **Serverless function** - This is great for simple deployment with built in routing, but this challenge required it to only run locally and didn't need to be deployed.

### Types Summary

- _MovieFromProvider_: A movie version from a provider, including id, price, etc.
- _MovieEntry_: A movie title and its versions across providers.
- _MoviePrice_: Merged movie info used to render the table, including the cheapest provider and any metadata (like poster and year).

## Future Improvements

- **Search & Filter**: Allow users to filter by genre, year, or search by title.
- **Caching**: Cache provider responses to reduce API calls and improve speed.
- **Pagination**: Handle longer movie lists using pagination or lazy loading.
- **Unit Testing**: Add tests for hook, table rendering, and error handling logic.
- **Error Reporting**: Log API failures for debugging.
- **Accessibility**: Improve focus states and screen reader support.
