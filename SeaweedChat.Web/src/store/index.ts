import * as user from "./modules/user";
import * as messanger from "./modules/messanger";
import { createStore } from "vuex";

export interface StoreState {
  user: user.UserState;
  messanger: messanger.MessangerState;
}
export default createStore<StoreState>({
  modules: {
    user,
    messanger,
  },
  mutations: {},
  actions: {},
});
