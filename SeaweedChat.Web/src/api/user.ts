import axios from "axios";
import type { User } from "../types/api/user";

export async function getCurrentUser(): Promise<User> {
  return (await axios.get<{ account: User }>("accounts"))?.data?.account;
}
