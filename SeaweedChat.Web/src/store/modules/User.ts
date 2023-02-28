import ApiClient from "@/network";
import type { Module, ActionTree, MutationTree, GetterTree } from "vuex";

const api = new ApiClient();

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
  async login({ commit }, data: { email: string; password: string }) {
    let { id } = (
      await api.request<{
        id: string;
      }>(`accounts/${data.email}`, {
        method: "GET",
      })
    ).data;
    commit(MutationType.SET_ID, id);

    let { sessionToken } = (
      await api.request<{ sessionToken: string }>(`accounts/${id}/sessions`, {
        method: "PUT",
        data,
      })
    ).data;
    commit(MutationType.SET_TOKEN, sessionToken);
  },
};

const namespaced: boolean = false;
export const User: Module<UserState, any> = {
  namespaced,
  state,
};
