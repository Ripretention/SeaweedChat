import {
  deleteMessage,
  editMessage,
  getMessages,
  sendMessage,
} from "@/api/message";
import type { Chat, ChatCreateParams } from "@/types/api/chat";
import type {
  Message,
  MessageCreateParams,
  MessageDeleteParams,
  MessageEditParams,
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
  APPEND_MESSAGES = "APPEND_MESSAGES",
  EDIT_MESSAGE = "EDIT_MESSAGE",
  DELETE_MESSAGE = "DELETE_MESSAGE",
}
export const mutations: MutationTree<MessagesState> = {
  [MutationType.ADD_MESSAGE](
    state,
    params: { chatId: string; message: Message }
  ) {
    let { chatId, message } = params;

    if (!state.messages[chatId]) {
      state.messages[chatId] = [];
    }
    if (message != null) {
      state.messages[chatId].unshift(message);
    }
  },
  [MutationType.APPEND_MESSAGES](
    state,
    params: { chatId: string; messages: Message[] }
  ) {
    let { chatId, messages } = params;

    state.messages[chatId] = (state.messages?.[chatId] ?? []).concat(messages);
  },
  [MutationType.EDIT_MESSAGE](
    state,
    params: { id: string; chatId: string; text: string }
  ) {
    let msg = state.messages?.[params.chatId]?.find?.(
      (msg) => msg.id === params.id
    );
    if (msg) {
      msg.text = params.text;
    }
  },
  [MutationType.DELETE_MESSAGE](state, params: { id: string; chatId: string }) {
    let chatMessages = state.messages?.[params.chatId];
    let msgIndex = chatMessages.findIndex((msg) => msg.id === params.id) ?? -1;
    if (msgIndex !== -1) {
      chatMessages.splice(msgIndex, 1);
    }
  },
};
export const actions: ActionTree<MessagesState, any> = {
  async loadMessages({ commit }, params: MessagesGetParams) {
    let messages = await getMessages(params);

    commit(MutationType.APPEND_MESSAGES, {
      chatId: params.chat.id,
      messages,
    });

    return true;
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
  async editMessage({ commit }, params: MessageEditParams) {
    let status = await editMessage(params);
    if (status) {
      commit(MutationType.EDIT_MESSAGE, {
        id: params.id,
        chatId: params.chatId,
        text: params.text,
      });
    }
    return status;
  },
  async deleteMessage({ commit }, params: MessageDeleteParams) {
    let status = await deleteMessage(params);
    if (status) {
      commit(MutationType.DELETE_MESSAGE, {
        id: params.id,
        chatId: params.chatId,
      });
    }

    return status;
  },
};

const namespaced: boolean = false;
export const Messanger: Module<MessagesState, any> = {
  namespaced,
  state,
  mutations,
};
