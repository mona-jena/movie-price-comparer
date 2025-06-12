import React from "react";
import "./../styles/App.css";
import { ApiError, MovieComparison } from "../types";

type MovieTableProps = {
  movieList: MovieComparison[];
  errors: ApiError[];
  loading: boolean;
};

const MovieTable: React.FC<MovieTableProps> = ({
  movieList,
  errors,
  loading,
}) => {
  if (loading) return <div className="loading">Loading movies...</div>;

  const failedProviders = new Set(errors.map((e) => e.service));

  if (movieList.length === 0) {
    return <p className="error">No movies found. Check back in later.</p>;
  }

  const providers = Array.from(
    new Set(movieList.flatMap((movie) => movie.versions.map((v) => v.provider)))
  );

  return (
    <div className="table-wrapper">
      <table className="movie-table">
        <thead>
          <tr>
            <th className="sticky-col">Title</th>
            <th className="sticky-col">Year</th>
            {providers.map((p) => (
              <th key={p}>{p}</th>
            ))}
            <th className="sticky-col cheapest-header">Cheapest</th>
          </tr>
        </thead>
        <tbody>
          {movieList.map((movie) => {
            const poster =
              movie.versions.find((v) => v.poster)?.poster ?? "/theaters.svg";
            const year = movie.versions.find((v) => v.year)?.year ?? "—";

            return (
              <tr key={movie.title}>
                <td className="sticky-col">
                  <div className="title-cell">
                    <img
                      src={poster}
                      alt={`${movie.title} poster`}
                      className="poster-img"
                      onError={(e) => {
                        if (e.currentTarget.src !== "/theaters.svg") {
                          e.currentTarget.src = "/theaters.svg";
                        }
                      }}
                    />
                    <span className="movie-title">{movie.title}</span>
                  </div>
                </td>
                <td className="sticky-col">{year}</td>
                {providers.map((p) => {
                  const version = movie.versions.find((v) => v.provider === p);
                  const failed = failedProviders.has(p);
                  return (
                    <td key={p}>
                      {failed || !version?.price
                        ? "Unavailable"
                        : `$${version.price.toLocaleString(undefined, {
                            minimumFractionDigits: 2,
                            maximumFractionDigits: 2,
                          })}`}
                    </td>
                  );
                })}
                <td className="sticky-col cheapest-col">
                  {movie.cheapestProvider ?? "Unavailable"}
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
};

export default MovieTable;
