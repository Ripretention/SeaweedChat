import * as user from "./modules/user";
import * as chats from "./modules/chats";
import * as messages from "./modules/messages";
import { createStore } from "vuex";

export interface StoreState {
  user: user.UserState;
  chats: chats.ChatsState;
  messages: messages.MessagesState;
}
export default createStore<StoreState>({
  modules: {
    user,
    chats,
    messages,
  },
  mutations: {},
  actions: {},
});
