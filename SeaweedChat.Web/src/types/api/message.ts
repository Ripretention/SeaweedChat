import type { Chat } from "./chat";

export interface MessageCreateParams {
  chat: Chat;
  text: string;
}
export interface MessagesGetParams {
  chat: Chat;
  offset?: number;
}
export interface Message {
  id: string;
  text: string;
  ownerId: string;
  ownerUsername: string;
  createdAt: Date;
  editAt: Date;
}
