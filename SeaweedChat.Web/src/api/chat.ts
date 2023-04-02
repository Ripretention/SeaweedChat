import type { Chat, ChatCreateParams } from "@/types/api/chat";
import axios from "axios";

export async function createChat(
  params: ChatCreateParams
): Promise<Chat | undefined> {
  let response = await axios.put("chats", params);

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
export async function deleteChat(chat: Chat) {
  await axios.delete(`chats/${chat.id}`);
}
export async function getChats(): Promise<Chat[]> {
  return (await axios.get<{ chats: Chat[] }>("chats"))?.data?.chats ?? [];
}
export async function getChat(id: string) {
  return (await axios.get<{ chat: Chat }>(`chats/${id}`))?.data?.chat;
}
export async function addMemberByUsername(chat: Chat, username: string) {
  await axios.put(`chats/${chat.id}/members`, {
    username,
  });
}
