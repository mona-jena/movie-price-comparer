# Movie Price Comparison App

This web app compares prices for the same movies across multiple providers and highlights the cheapest option. It handles flaky APIs and aims to demonstrate clear separation of concerns, graceful fallback logic with a clean UI.

## Features

- Compares prices from **Cinemaworld** and **Filmworld**
- Shows **cheapest provider**
- Handles **intermittent API failures**
- Simple, responsive **table UI** with support for multiple providers
- **API** to fetch list of movies with cheapest provider
- Includes loading indicators and error messaging

## Assumptions

- Movies are matched by movie Id between providers.
- A movie is only shown if it's available from atleast one working provider.
- If one provider fails, the app continues with the available data.
- Poster and year are optional and included only if available. It's assumed that both the Webjet endpoints always return atleast Id and Title if they do return anything at all.
- This solution should be able to run locally and doesn't need to be setup to be deployed at this point.

## Running Locally

1. Add a .env file inside the client folder with the below content inside.

```env
VITE_EXTERNAL_API_BASE=http://localhost:5086/api
```

2. Update the token in client/appsettings.json inside External Api > Webjet

```
"Token": "{ENTER-TOKEN}"
```

3. Install dependencies from /client directory

```bash
cd client
npm install
```

4. Start the client from /client directory

```bash
cd client
npm run dev
```

5. Start the server from /server directory

```bash
cd server
dotnet run
```

This will launch the app on:

- Frontend: http://localhost:5173

You can also view the Swagger doc on:

- Swagger: http://localhost:5086/swagger/index.html

## Design

- The server attaches the API token and handles CORS.
- The server handles the logic to remove missing data and perform price comparison to find cheapest provider.
- The client is only responsible for making request to the api and displaying the results with a clean UX.
- If neither provider responds, a user friendly message is shown.
- The table UI shows title, poster, year, individual prices, and the cheapest provider.
- API supports partial failure and provides both data and structured error.
- Movie poster and year are optional fields and are included if available from detail API responses.
- Included API to fetch Movie details even though it's not called for this problem. But it's a natural extension and good to have if we decide to add filtering logic in the future. It also keeps the logic of getting movie details testable, ensuring provider logic is consistently exposed and ready for future extension.

### Data Model Design

- _MovieComparison_: Info about each movie including pricing info per provider and the cheapest provider for that movie.
- _MovieVersion_: Info about a movie including provider info.
- _MovieListResponse_: List of movie info.
- _MovieSummary_: Basic info about a movie returned from the Webjet /movie endpoint.
- \_MovieDetails: Detailed info about a movie returned from the Webjet /movie/{ID} endpoint. This was done to keep both endpoint results separate, in case in the future
- _WebjetMovieResponse_: Data as it comes straight from external API.

Provider-level models reflect the raw Webjet API shape, while domain models are shaped for our business logic. This separation avoids leaking external contracts into the rest of the app and enables easier mapping, testing, and evolution. I did consider adding another DTO layer that could be mapped at controller level. But since there wasn't much transformation logic, it didn't warrant a DTO model at this point. In the future if the way this data needs to be strucutred becomes more complex, then I would add it.

## Future Improvements

- **Autogenerate Typescript client**: from backend using NSwag to ensure alignment between business model and API contracts
- **Search & Filter**: Allow users to filter by genre, year, or search by title.
- **Caching**: Cache provider responses to reduce API calls and improve speed.
- **Pagination**: Handle longer movie lists using pagination or lazy loading.
- **Unit Testing**: Add tests for hook, table rendering, and error handling logic.
- **Error Reporting**: Log API failures for debugging
- **Accessibility**: Improve focus states and screen reader support.
