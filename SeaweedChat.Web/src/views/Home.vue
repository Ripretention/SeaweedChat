<template>
  <template v-if="isAuthorized">
    <ChatHub
      v-if="selectedChat == undefined"
      :chats="chats"
      @add-chat="startChatWithUser"
      @select-chat="selectChat"
    />
    <ChatDialog
      v-else
      :id="selectedChat"
      :messages="messages"
      :owner-id="ownerId"
      @load-messages="loadMessages"
      @send-message="sendMessage"
      @back-to-hub="() => store.commit(MutationType.RESET_SELECTED_CHAT)"
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
import { MutationType } from "@/store/modules/messanger";
import ChatHub from "@/components/ChatHub.vue";
import ChatDialog from "@/components/ChatDialog.vue";
import store from "@/store";
import { computed, onMounted } from "vue";
import type { Chat } from "@/types/Chat";

const ownerId = computed(() => store.state.user.id);
const selectedChat = computed(() => store.state.messanger.selectedChat);
const isAuthorized = computed(() => store.getters.isAuthorized);
const chats = computed(() => store.state.messanger.chats);
const messages = computed(() => {
  return selectedChat.value == null
    ? []
    : store.state.messanger.messages[selectedChat.value];
});

onMounted(async () => {
  await store.dispatch("loadChats");
});

async function startChatWithUser(username: string) {
  await store.dispatch("createChatWithUser", username);
}
async function loadMessages(params: { chatId: string; offset: number }) {
  await store.dispatch("loadMessages", params);
}
async function sendMessage(chatId: string, text: string) {
  return store.dispatch("dispatchMessage", {
    chatId,
    text,
  });
}
function selectChat(chat: Chat) {
  console.log(123);
  store.commit(MutationType.SET_SELECTED_CHAT, chat.id);
}
</script>
