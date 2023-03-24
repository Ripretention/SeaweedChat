import { createApp } from "vue";
import { defineAPIUrl, defineTokenSource } from "./api/api";
import App from "./App.vue";
import router from "./router";
import store from "./store";

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
app.use(router).use(vuetify).use(store).mount("#app");
