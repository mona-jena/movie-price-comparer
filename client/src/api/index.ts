import { ApiResponse, MovieComparison, MovieDetails } from "../types";

const BASE_URL = import.meta.env.VITE_EXTERNAL_API_BASE;

export async function fetchMovies(): Promise<ApiResponse<MovieComparison[]>> {
  try {
    const response = await fetch(`${BASE_URL}/movie`);
    return await response.json();
  } catch (err) {
    return {
      data: [],
      errors: [
        {
          service: "frontend",
          title: "Unexpected error",
          statusCode: 500,
          detail: (err as Error).message,
        },
      ],
    };
  }
}

export async function fetchMovieDetails(
  provider: string,
  id: string
): Promise<ApiResponse<MovieDetails | null>> {
  try {
    const response = await fetch(`${BASE_URL}/${provider}/movie/${id}`);
    return await response.json();
  } catch (err) {
    return {
      data: null,
      errors: [
        {
          service: provider,
          title: `Failed to fetch movie details from ${provider}`,
          statusCode: 500,
          detail: (err as Error).message,
        },
      ],
    };
  }
}
