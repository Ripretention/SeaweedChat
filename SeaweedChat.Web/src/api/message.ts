import type {
  Message,
  MessageCreateParams,
  MessageDeleteParams,
  MessageEditParams,
  MessagesGetParams,
} from "@/types/api/message";
import axios from "axios";

export async function sendMessage(params: MessageCreateParams) {
  let response = await axios.put(`chats/${params.chat.id}/messages`, {
    text: params.text,
  });

  let location = response.headers?.["location"];
  if (!location) {
    return undefined;
  }

  let { messageBody } = (
    await axios.request<{
      messageBody: Message;
    }>({ baseURL: location, method: "GET" })
  ).data;

  return messageBody;
}
export async function getMessages(params: MessagesGetParams) {
  return (
    (
      await axios.get<{ messages: Message[] }>(
        `chats/${params.chat.id}/messages`,
        {
          params: {
            offset: params.offset,
          },
        }
      )
    ).data?.messages ?? []
  );
}
export async function deleteMessage(params: MessageDeleteParams) {
  let response = await axios.delete(
    `chats/${params.chatId}/messages/${params.id}`
  );
  return response.status === 200;
}
export async function editMessage(params: MessageEditParams) {
  let response = await axios.post(
    `chats/${params.chatId}/messages/${params.id}`,
    {
      text: params.text,
    }
  );
  return response.status === 200;
}
