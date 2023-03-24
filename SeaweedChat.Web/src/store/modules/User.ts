import { signIn, signUp } from "@/api/auth";
import type { AuthParams, AuthResult, SignUpParams } from "@/types/api/auth";
import type { Module, ActionTree, MutationTree, GetterTree } from "vuex";

export interface UserState {
  id?: string;
  token?: string;
  username?: string;
}
export const state: UserState = {
  id: localStorage.getItem("account-id") ?? undefined,
  token: localStorage.getItem("account-token") ?? undefined,
};
export enum MutationType {
  RESET = "RESET",
  SET_TOKEN = "SET_TOKEN",
  SET_ID = "SET_ID",
  SET_USERNAME = "SET_USERNAME",
}
export const mutations: MutationTree<UserState> = {
  [MutationType.RESET](state) {
    state.id = undefined;
    state.token = undefined;
    localStorage.removeItem("account-token");
    localStorage.removeItem("account-id");
  },
  [MutationType.SET_TOKEN](state, token: string) {
    state.token = token;
    localStorage.setItem("account-token", token);
  },
  [MutationType.SET_ID](state, id: string) {
    state.id = id;
    localStorage.setItem("account-id", id);
  },
  [MutationType.SET_USERNAME](state, username: string) {
    state.username = username;
  },
};
export const getters: GetterTree<UserState, any> = {
  isAuthorized(state) {
    return state.id != undefined && state.token != undefined;
  },
};
export const actions: ActionTree<UserState, any> = {
  async register({ dispatch }, params: SignUpParams) {
    dispatch("auth", await signUp(params));
  },
  async login({ dispatch }, params: AuthParams) {
    dispatch("auth", await signIn(params.email, params.password));
  },
  async auth({ commit }, authData: AuthResult) {
    commit(MutationType.SET_ID, authData.id);
    commit(MutationType.SET_TOKEN, authData.sessionToken);
  },
};

const namespaced: boolean = false;
export const User: Module<UserState, any> = {
  namespaced,
  state,
};
