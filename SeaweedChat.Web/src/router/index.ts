import { createRouter, createWebHistory } from "vue-router";

export default createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: "/",
      component: () => import("@/views/Home.vue"),
    },
    {
      path: "/register",
      component: () => import("@/views/Register.vue"),
    },
    {
      path: "/login",
      component: () => import("@/views/Login.vue"),
    },
    {
      path: "/chats/:id",
      name: "chat",
      component: () => import("@/views/ChatDialog.vue"),
    },
  ],
});
