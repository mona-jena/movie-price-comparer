import { useEffect, useState } from "react";
import { fetchMovies } from "../api";
import type { MovieComparison, ApiError } from "../types";

export function useMovieData() {
  const [moviePrices, setMoviePrices] = useState<MovieComparison[]>([]);
  const [errors, setErrors] = useState<ApiError[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const loadData = async () => {
      setLoading(true);
      setErrors([]);

      try {
        const response = await fetchMovies();
        setMoviePrices(response.data ?? []);
        setErrors(response.errors ?? []);
      } catch (err) {
        setMoviePrices([]);
        setErrors([
          {
            service: "frontend",
            title: "Unexpected error calling backend",
            statusCode: 500,
            detail: (err as Error).message,
          },
        ]);
      }

      setLoading(false);
    };

    loadData();
  }, []);

  return { moviePrices, errors, loading };
}
