import axios from "axios";
import { ApiResponse } from "./ApiResponse";

export default class ApiClient {
  public baseUrl = "http://localhost:5000/api/v1";
  public headers = () => ({
    Authorization: localStorage.getItem("token"),
  });
  public async request<T>(
    method: string,
    request: Parameters<(typeof axios)["request"]>[0]
  ) {
    request.headers = { ...request.headers, ...this.headers() };
    request.baseURL = (request.baseURL || this.baseUrl) + "/" + method;
    let response = await axios.request(request);
    return new ApiResponse<T>(response.status, response.data);
  }
}
