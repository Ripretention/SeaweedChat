import { getMessages, sendMessage } from "@/api/message";
import type { Chat, ChatCreateParams } from "@/types/api/chat";
import type {
  Message,
  MessageCreateParams,
  MessagesGetParams,
} from "@/types/api/message";
import type { Module, ActionTree, MutationTree, GetterTree } from "vuex";

export interface MessagesState {
  messages: {
    [chatId: string]: Message[];
  };
}
export const state: MessagesState = {
  messages: {},
};
export enum MutationType {
  ADD_MESSAGE = "ADD_MESSAGE",
}
export const mutations: MutationTree<MessagesState> = {
  [MutationType.ADD_MESSAGE](
    state,
    params: { chatId: string; message: Message }
  ) {
    let { chatId, message } = params;

    if (state.messages[chatId]) {
      state.messages[chatId] = [];
    }
    if (message != null) {
      state.messages[chatId].push(message);
    }
  },
};
export const actions: ActionTree<MessagesState, any> = {
  async loadMessages({ commit }, params: MessagesGetParams) {
    let messages = await getMessages(params);

    for (let message of messages) {
      commit(MutationType.ADD_MESSAGE, {
        chatId: params.chat.id,
        message,
      });
    }
  },
  async sendMessage({ commit }, params: MessageCreateParams) {
    let message = await sendMessage(params);
    if (!message) {
      return;
    }

    commit(MutationType.ADD_MESSAGE, {
      chatId: params.chat.id,
      message,
    });
    return message;
  },
};

const namespaced: boolean = false;
export const Messanger: Module<MessagesState, any> = {
  namespaced,
  state,
  mutations,
};
