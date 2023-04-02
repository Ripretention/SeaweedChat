import {
  addMemberByUsername,
  createChat,
  deleteChat,
  getChat,
  getChats,
} from "@/api/chat";
import { ChatType, type Chat } from "@/types/api/chat";
import type { Module, ActionTree, MutationTree, GetterTree } from "vuex";

export interface ChatsState {
  chats: Chat[];
  currentSelectedChatId?: string;
}

export const state: ChatsState = {
  chats: [],
};
export enum MutationType {
  ADD_CHAT = "ADD_CHAT",
  APPEND_CHATS = "APPEND_CHATS",
  REMOVE_CHAT = "REMOVE_CHAT",
  SET_SELECTED_CHAT = "SET_SELECTED_CHAT",
  RESET_SELECTED_CHAT = "RESET_SELECTED_CHAT",
}
export const mutations: MutationTree<ChatsState> = {
  [MutationType.ADD_CHAT](state, chat: Chat) {
    if (state.chats.some((c) => c.id == chat.id)) {
      return;
    }

    state.chats.unshift(chat);
  },
  [MutationType.APPEND_CHATS](state, chats: Chat[]) {
    for (let chat of chats) {
      if (state.chats.some((c) => c.id == chat.id)) {
        return;
      }

      state.chats.push(chat);
    }
  },
  [MutationType.SET_SELECTED_CHAT](state, id: string) {
    if (id != undefined && id.trim() != "") {
      state.currentSelectedChatId = id;
    }
  },
  [MutationType.RESET_SELECTED_CHAT](state) {
    state.currentSelectedChatId = undefined;
  },
  [MutationType.REMOVE_CHAT](state, chat: Chat) {
    let index = state.chats.findIndex((c) => c.id == chat.id);
    if (index == -1) {
      return;
    }

    state.chats.splice(index, 1);
  },
};
export const actions: ActionTree<ChatsState, any> = {
  async createChatWithUser(
    { commit },
    username: string
  ): Promise<Chat | undefined> {
    let chat = await createChat({
      title: `Chat with ${username}`,
      type: ChatType.Chat,
    });
    if (!chat) {
      return undefined;
    }

    commit(MutationType.ADD_CHAT, chat);
    await addMemberByUsername(chat, username);

    return chat;
  },
  async createChannel({ commit }, channelTitle) {
    let channel = await createChat({
      title: channelTitle,
      type: ChatType.Channel,
    });

    if (!channel) {
      return undefined;
    }

    commit(MutationType.ADD_CHAT, channel);
    return channel;
  },
  async loadChats({ commit, state }): Promise<Chat[]> {
    let chats = await getChats();
    commit(MutationType.APPEND_CHATS, chats);

    return state.chats;
  },
  async getChat({ commit, state }, id: string) {
    let chat = state.chats.find((c) => c.id == id);
    if (!chat) {
      chat = await getChat(id);
      if (chat) {
        commit(MutationType.ADD_CHAT, chat);
      }
    }

    return chat;
  },
  async removeChat({ commit }, chat: Chat) {
    await deleteChat(chat);
    commit(MutationType.REMOVE_CHAT, chat);
  },
};

const namespaced: boolean = false;
export const Chats: Module<ChatsState, any> = {
  namespaced,
  state,
  mutations,
};
