import axios from "axios";
import type { AuthResult, SignUpParams, AuthParams } from "@/types/api/auth";

export async function signUp(params: SignUpParams): Promise<AuthResult> {
  let { id } = (await axios.put<{ id: string }>("accounts", params)).data;

  let sessionToken = await getSessionToken(id, {
    email: params.email,
    password: params.password,
  });

  return {
    id,
    sessionToken,
  };
}
export async function signIn(
  email: string,
  password: string
): Promise<AuthResult> {
  let { id } = (
    await axios.get<{
      id: string;
    }>(`accounts/${email}`)
  ).data;

  let sessionToken = await getSessionToken(id, {
    email,
    password,
  });

  return {
    id,
    sessionToken,
  };
}

async function getSessionToken(accountId: string, params: AuthParams) {
  let { sessionToken } = (
    await axios.put<{ sessionToken: string }>(
      `accounts/${accountId}/sessions`,
      params
    )
  ).data;

  return sessionToken;
}
