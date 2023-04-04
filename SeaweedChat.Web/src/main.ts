import { createApp } from "vue";
import {
  defineAPIUrl,
  defineTokenSource,
  isUnauthorizedError,
} from "./api/api";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import { MutationType } from "./store/modules/user";

import "vuetify/styles";
import { createVuetify } from "vuetify";
import * as components from "vuetify/components";
import * as directives from "vuetify/directives";
import "@mdi/font/css/materialdesignicons.css";

const vuetify = createVuetify({
  components,
  directives,
});

defineAPIUrl("http://localhost:5000/api/v1");
defineTokenSource(() => localStorage.getItem("account-token"));

const app = createApp(App);
app.config.errorHandler = (error) => {
  console.log(error);
  if (isUnauthorizedError(error)) {
    store.commit(MutationType.RESET);
    router.push({ name: "login", query: { redirect: window.location.href } });
  }
};
app.use(router).use(vuetify).use(store).mount("#app");
