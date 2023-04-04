import store from "@/store";
import { createRouter, createWebHistory } from "vue-router";
import Home from "@/views/Home.vue";
import Login from "@/views/Login.vue";
import Register from "@/views/Register.vue";

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: "/",
      name: "home",
      component: Home,
    },
    {
      path: "/register",
      name: "register",
      component: Register,
    },
    {
      path: "/login",
      name: "login",
      component: Login,
    },
    {
      path: "/chats/:id",
      name: "chat",
      component: () => import("@/views/ChatDialog.vue"),
      meta: {
        requiresAuth: true,
      },
    },
  ],
});

router.beforeEach((to) => {
  if ((to.meta?.requiresAuth ?? false) && !store.getters.isAuthorized) {
    return { name: "login", query: { redirect: to.fullPath } };
  }
});

export default router;
declare module "vue-router" {
  interface RouteMeta {
    requiresAuth?: boolean;
  }
}
