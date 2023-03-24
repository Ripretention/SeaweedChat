import type { Chat, ChatCreateParams } from "@/types/api/chat";
import axios from "axios";

export async function createChat(
  params: ChatCreateParams
): Promise<Chat | undefined> {
  let response = await axios.put("chats", {
    data: params,
  });

  let location = response.headers?.["location"];
  if (!location) {
    return undefined;
  }

  let chat = (
    await axios.request<Chat>({
      method: "GET",
      baseURL: location,
    })
  ).data;

  return chat;
}
export async function getChats(): Promise<Chat[]> {
  return (await axios.get<{ chats: Chat[] }>("chats"))?.data?.chats ?? [];
}
export async function addMemberByUsername(chat: Chat, username: string) {
  await axios.put(`chats/${chat.id}/members`, {
    baseURL: location,
    data: {
      username,
    },
  });
}
