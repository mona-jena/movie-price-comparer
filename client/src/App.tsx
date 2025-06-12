import React from "react";
import Header from "./components/Header";
import MovieTable from "./components/MovieTable";
import { useMovieData } from "./hooks/useMovieData";

function App() {
  const { moviePrices, loading, errors } = useMovieData();

  return (
    <div className="app-container">
      <Header />
      <MovieTable movieList={moviePrices} loading={loading} errors={errors} />
    </div>
  );
}

export default App;
