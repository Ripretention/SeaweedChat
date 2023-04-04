import axios, { AxiosError } from "axios";

export function isUnauthorizedError(error: any) {
  return error instanceof AxiosError && error.response?.status === 401;
}
export function defineAPIUrl(url: string) {
  axios.defaults.baseURL = url;
}
export function defineTokenSource(source: () => string | null) {
  axios.interceptors.request.use(
    (reqParams) => {
      let token = source();

      if (token) {
        reqParams.headers["Authorization"] = `Bearer ${token}`;
      }

      return reqParams;
    },
    (error) => Promise.reject(error)
  );
}
