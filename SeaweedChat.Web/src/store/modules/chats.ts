import { addMemberByUsername, createChat, getChats } from "@/api/chat";
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
  SET_SELECTED_CHAT = "SET_SELECTED_CHAT",
  RESET_SELECTED_CHAT = "RESET_SELECTED_CHAT",
}
export const mutations: MutationTree<ChatsState> = {
  [MutationType.ADD_CHAT](state, chat: Chat) {
    if (state.chats.some((c) => c.id == chat.id)) {
      return;
    }

    state.chats.push(chat);
  },
  [MutationType.SET_SELECTED_CHAT](state, id: string) {
    if (id != undefined && id.trim() != "") {
      state.currentSelectedChatId = id;
    }
  },
  [MutationType.RESET_SELECTED_CHAT](state) {
    state.currentSelectedChatId = undefined;
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
  async loadChats({ commit, state }): Promise<Chat[]> {
    let chats = await getChats();
    for (let chat of chats) {
      commit(MutationType.ADD_CHAT, chat);
    }

    return state.chats;
  },
};

const namespaced: boolean = false;
export const Chats: Module<ChatsState, any> = {
  namespaced,
  state,
  mutations,
};
