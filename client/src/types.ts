export type MovieDetails = {
  id: string;
  title: string;
  year?: string;
  poster?: string;
  price?: number | null;
};

export type MovieVersion = {
  id: string;
  provider: string;
  price?: number | null;
  year?: string;
  poster?: string;
};

export type MovieComparison = {
  title: string;
  versions: MovieVersion[];
  cheapestProvider?: string;
};

export type ApiResponse<T> = {
  data: T;
  errors?: ApiError[];
};

export type ApiError = {
  service: string;
  title: string;
  statusCode: number;
  detail: string;
};
