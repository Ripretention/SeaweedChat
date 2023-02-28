<template>
  <v-app>
    <TheHeader :isAuthorized="isAuthorized" @logout="logout" />
    <v-main>
      <router-view></router-view>
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
import router from "@/router";
import store from "@/store";
import TheHeader from "@/components/TheHeader.vue";
import { computed } from "vue";
import { MutationType } from "./store/modules/User";
const isAuthorized = computed(() => store.getters.isAuthorized);

function logout() {
  store.commit(MutationType.RESET);
  router.push("/login");
}
</script>
