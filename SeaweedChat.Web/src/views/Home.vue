<template>
  <template v-if="isAuthorized">
    <ChatHub :chats="previewMessage" />
  </template>
  <template v-else>
    <v-alert dark icon="mdi-vuetify" border="left" prominent>
      praesent congue erat at massa. nullam vel sem. aliquam lorem ante, dapibus
      in, viverra quis, feugiat a, tellus. proin viverra, ligula sit amet
      ultrices semper, ligula arcu tristique sapien, a accumsan nisi mauris ac
      eros. curabitur at lacus ac velit ornare lobortis.
    </v-alert>
  </template>
</template>

<script setup lang="ts">
import ChatHub from "@/components/ChatHub.vue";
import store from "@/store";
import { computed, reactive, ref } from "vue";
const isAuthorized = computed(() => store.getters.isAuthorized);
import { getRandomElement } from "../utils";
import type { ChatPreview } from "@/types/Chat";

const dialog = ref(false);
const previewMessage = reactive<ChatPreview[]>([]);
const names = [
  "John",
  "Loid",
  "Alex",
  "Darwin",
  "Olesia",
  "Viktoria",
  "Jack",
  "Helga",
];
const families = ["Wolter", "Cenry", "McDred", "Softest", "Ivanov"];
const date = new Date();

for (let i = 0; i < 15; i++) {
  previewMessage.push({
    date,
    author: getRandomElement(names) + " " + getRandomElement(families),
    text: `hello, ${getRandomElement(names)}`,
  });
}
</script>
