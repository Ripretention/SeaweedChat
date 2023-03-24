<template>
  <template v-if="isAuthorized">
    <ChatHub
      v-if="selectedChatId == undefined"
      :chats="chats"
      @add-chat="startChatWithUser"
      @select-chat="selectChat"
    />
    <ChatDialog
      v-else
      :chat="selectedChat"
      :messages="messages"
      :owner-id="ownerId"
      @load-messages="(params) => store.dispatch('loadMessages', params)"
      @send-message="(params) => store.dispatch('sendMessage', params)"
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
import { MutationType } from "@/store/modules/chats";
import ChatHub from "@/components/ChatHub.vue";
import ChatDialog from "@/components/ChatDialog.vue";
import store from "@/store";
import type { Chat } from "@/types/api/chat";
import { computed, onMounted } from "vue";

const ownerId = computed(() => store.state.user.id);
const selectedChatId = computed(() => store.state.chats.currentSelectedChatId);
const selectedChat = computed(() =>
  store.state.chats.chats.find((chat) => chat.id == selectedChatId.value)
);
const isAuthorized = computed(() => store.getters.isAuthorized);
const chats = computed(() => store.state.chats.chats);
const messages = computed(() => {
  return selectedChatId.value == null
    ? []
    : store.state.messages.messages[selectedChatId.value];
});

onMounted(async () => {
  await store.dispatch("loadChats");
});

async function startChatWithUser(username: string) {
  await store.dispatch("createChatWithUser", username);
}
async function loadMessages(params: { chat: Chat; offset: number }) {
  await store.dispatch("loadMessages", params);
}
async function sendMessage(chatId: string, text: string) {
  return store.dispatch("sendMessage", {
    chatId,
    text,
  });
}
function selectChat(chat: Chat) {
  console.log(123);
  store.commit(MutationType.SET_SELECTED_CHAT, chat.id);
}
</script>
