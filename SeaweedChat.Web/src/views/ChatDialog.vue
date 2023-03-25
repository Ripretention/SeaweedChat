<template>
  <ChatContainter v-if="chat != null">
    <template #toolbar>
      <v-toolbar color="blue" @click="() => router.push({ path: '/' })">
        <v-btn icon>
          <v-icon>mdi-arrow-left</v-icon>
        </v-btn>
        <v-toolbar-title>{{ chat.title }}</v-toolbar-title>
      </v-toolbar>
    </template>

    <template #content>
      <div
        ref="messagesForm"
        class="d-flex flex-column-reverse"
        style="min-height: calc(100% - 56px)"
      >
        <ChatMessage
          v-for="msg in messages"
          :key="msg.id"
          :author="msg.ownerUsername"
          :text="msg.text"
          :direction="msg.ownerId == ownerId ? 'from' : 'to'"
          :date="msg.editAt?.toString() ?? msg.createdAt.toString()"
        />
      </div>

      <div style="position: sticky; bottom: 0">
        <v-text-field
          autofocus
          outlined
          v-model="currentMessage"
          color="blue"
          placeholder="Write a message..."
          @keypress.enter="sendMessage"
          append-inner-icon="mdi-send"
          hide-details
          bg-color="grey-lighten-2"
        ></v-text-field>
      </div>
    </template>
  </ChatContainter>
</template>

<script setup lang="ts">
import store from "@/store";
import { useRoute, useRouter } from "vue-router";
import ChatMessage from "@/components/ChatMessage.vue";
import ChatContainter from "@/components/ChatContainer.vue";
import { ref, onMounted, watch, nextTick, computed } from "vue";
import type { Chat } from "@/types/api/chat";

const router = useRouter();
const routeIdParam = useRoute().params["id"];
const chatId =
  typeof routeIdParam === "string" ? routeIdParam : routeIdParam[0];

const chat = ref<Chat | undefined>();
const ownerId = computed(() => store.state.user.id);
const messages = computed(() => store.state.messages.messages?.[chatId] ?? []);

const currentMessage = ref<string | null>(null);
const messagesForm = ref<HTMLDivElement>();

onMounted(async () => {
  chat.value = await store.dispatch("getChat", chatId);
  if (chat.value != null && messages.value.length == 0) {
    await store.dispatch("loadMessages", {
      chat: chat.value,
      offset: 0,
    });
  }

  await nextTick();
  scrollToBottom();
});
function scrollToBottom() {
  let element = messagesForm?.value?.parentElement;
  if (element) {
    element.scrollTop = element.scrollHeight;
  }
}

let lastMessageId = "";
watch(messages.value, async () => {
  console.log(lastMessageId);
  if (lastMessageId != messages.value?.[0]?.id) {
    lastMessageId = messages.value?.[0]?.id;
    await nextTick();
    scrollToBottom();
  }
});

async function sendMessage() {
  if (
    currentMessage.value == null ||
    currentMessage.value.trim() == "" ||
    chat.value == null
  )
    return;

  await store.dispatch("sendMessage", {
    chat: chat.value,
    text: currentMessage.value,
  });
  currentMessage.value = "";
}
</script>
