<template>
  <v-app>
    <TheHeader
      :isAuthorized="isAuthorized"
      :username="username"
      @logout="logout"
    />
    <v-main>
      <router-view></router-view>
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
import router from "@/router";
import store from "@/store";
import TheHeader from "@/components/TheHeader.vue";
import { computed, onMounted } from "vue";
import { MutationType } from "./store/modules/user";

const isAuthorized = computed(() => store.getters.isAuthorized);
const username = computed(() => store.state.user.username);

onMounted(async () => {
  if (username.value == null && isAuthorized.value) {
    await store.dispatch("loadUser");
  }
});

function logout() {
  store.commit(MutationType.RESET);
  router.push("/login");
}
</script>
