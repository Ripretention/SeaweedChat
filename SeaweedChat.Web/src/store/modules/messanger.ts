import ApiClient from "@/network";
import type { Chat, Message } from "@/types/Chat";
import type { Module, ActionTree, MutationTree, GetterTree } from "vuex";

const api = new ApiClient();

export interface MessangerState {
  selectedChat?: string;
  chats: Chat[];
  messages: Record<string, Message[]>;
}
export const state: MessangerState = {
  chats: [],
  messages: {},
};
export enum MutationType {
  ADD_CHAT = "ADD_CHAT",
  ADD_MESSAGE = "ADD_MESSAGE",
  SET_SELECTED_CHAT = "SET_SELECTED_CHAT",
  RESET_SELECTED_CHAT = "RESET_SELECTED_CHAT",
}
export const mutations: MutationTree<MessangerState> = {
  [MutationType.ADD_CHAT](state, chat: Chat) {
    if (state.chats.some((c) => c.id == chat.id)) return;

    state.chats.push(chat);
    state.messages[chat.id] = [];
  },
  [MutationType.ADD_MESSAGE](state, msg: { chatId: string; body: Message }) {
    if (state.messages[msg.chatId]) state.messages[msg.chatId] = [];
    if (msg.body == null) return;

    state.messages[msg.chatId].push(msg.body);
  },
  [MutationType.SET_SELECTED_CHAT](state, id: string) {
    if (id != undefined && id.trim() != "") state.selectedChat = id;
  },
  [MutationType.RESET_SELECTED_CHAT](state, id: string) {
    state.selectedChat = undefined;
  },
};
export const actions: ActionTree<MessangerState, any> = {
  async createChatWithUser(
    { commit },
    username: string
  ): Promise<Chat | undefined> {
    let { location } = await api.request("chats", {
      method: "PUT",
      data: {
        title: "Our chat",
        type: 0,
      },
    });

    if (location == null) return undefined;

    let response = await api.request("members", {
      method: "PUT",
      baseURL: location,
      data: {
        username,
      },
    });

    let chat = (
      await api.request<Chat>("", {
        method: "GET",
        baseURL: location,
      })
    )?.data;

    commit(MutationType.ADD_CHAT, chat);
    return chat;
  },
  async loadMessages(
    { commit },
    params: {
      chatId: string;
      offset: number;
    }
  ) {
    let { messages = [] } = (
      await api.request<{ messages: Message[] }>(
        `chats/${params.chatId}/messages`,
        {
          method: "GET",
          params: {
            offset: params.offset,
          },
        }
      )
    ).data;

    for (let msg of messages)
      commit(MutationType.ADD_MESSAGE, {
        chatId: params.chatId,
        body: msg,
      });
  },
  async dispatchMessage(
    { commit },
    params: {
      chatId: string;
      text: string;
    }
  ) {
    let { location } = await api.request(`chats/${params.chatId}/messages`, {
      method: "PUT",
      data: {
        text: params.text,
      },
    });

    if (location == null) return undefined;

    let { messageBody: message } = (
      await api.request<{
        messageBody: Message;
      }>("", { baseURL: location, method: "GET" })
    ).data;

    commit(MutationType.ADD_MESSAGE, {
      chatId: params.chatId,
      body: message,
    });
    return message;
  },
  async loadChats({ commit, state }): Promise<Chat[]> {
    let { chats = [] } = (
      await api.request<{
        chats: Chat[];
      }>("chats", {
        method: "GET",
      })
    )?.data;

    for (let chat of chats) commit(MutationType.ADD_CHAT, chat);

    return state.chats;
  },
};

const namespaced: boolean = false;
export const Messanger: Module<MessangerState, any> = {
  namespaced,
  state,
  mutations,
};
