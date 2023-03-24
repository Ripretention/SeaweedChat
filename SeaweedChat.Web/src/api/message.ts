import type {
  Message,
  MessageCreateParams,
  MessagesGetParams,
} from "@/types/api/message";
import axios from "axios";

export async function sendMessage(params: MessageCreateParams) {
  let response = await axios.put(`chats/${params.chat.id}/messages`, {
    data: {
      text: params.text,
    },
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
