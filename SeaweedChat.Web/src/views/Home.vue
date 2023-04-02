<template>
  <template v-if="isAuthorized">
    <ChatHub
      :chats="chats"
      @add-chat="(username) => store.dispatch('createChatWithUser', username)"
      @add-channel="(title) => store.dispatch('createChannel', title)"
      @select-chat="selectChat"
    />
  </template>
  <template v-else>
    <v-alert dark icon="mdi-vuetify" prominent>
      praesent congue erat at massa. nullam vel sem. aliquam lorem ante, dapibus
      in, viverra quis, feugiat a, tellus. proin viverra, ligula sit amet
      ultrices semper, ligula arcu tristique sapien, a accumsan nisi mauris ac
      eros. curabitur at lacus ac velit ornare lobortis.
    </v-alert>
  </template>
</template>

<script setup lang="ts">
import { useRouter } from "vue-router";
import ChatHub from "@/components/ChatHub.vue";
import store from "@/store";
import type { Chat } from "@/types/api/chat";
import { computed, onMounted } from "vue";

const router = useRouter();

const chats = computed(() => store.state.chats.chats);
const isAuthorized = computed(() => store.getters.isAuthorized);

onMounted(async () => {
  if (!isAuthorized.value) {
    return;
  }
  await store.dispatch("loadChats");
});

function selectChat(chat: Chat) {
  router.replace({ name: "chat", params: { id: chat.id } });
}
</script>
