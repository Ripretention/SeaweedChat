import axios from "axios";
import type { AxiosRequestConfig } from "axios";
import { ApiResponse } from "./ApiResponse";

export default class ApiClient {
  public baseUrl = "http://localhost:5000/api/v1";
  public headers = () => ({
    Authorization: "Bearer " + localStorage.getItem("account-token"),
  });
  public async request<T>(
    method: string,
    request: AxiosRequestConfig<Record<string, any>>
  ) {
    request.headers = { ...request.headers, ...this.headers() };
    request.baseURL = (request.baseURL || this.baseUrl) + "/" + method;
    let response = await axios(request);

    if (typeof response.data === "object") {
      for (let key in response.data) {
        if (key[0] == key[0].toUpperCase()) {
          response.data[key[0] + key.substring(1)] = response.data[key];
          delete response.data[key];
        }
      }
    }

    return new ApiResponse<T>(
      response.status,
      response.data,
      response.headers["location"]
    );
  }
}
