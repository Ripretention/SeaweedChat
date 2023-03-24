<template>
  <ChatContainter>
    <template #toolbar>
      <v-toolbar color="blue">
        <v-btn icon @click="emit('backToHub')">
          <v-icon>mdi-arrow-left</v-icon>
        </v-btn>
        <v-toolbar-title>{{ props.chat.title }}</v-toolbar-title>
      </v-toolbar>
    </template>

    <template #content>
      <div class="d-flex flex-column" style="min-height: calc(100% - 56px)">
        <ChatMessage
          v-for="msg in props.messages"
          :key="msg.id"
          :author="msg.ownerUsername"
          :text="msg.text"
          :direction="msg.ownerId == props.ownerId ? 'from' : 'to'"
          :date="msg.editAt ?? msg.createdAt"
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
import ChatMessage from "./ChatMessage.vue";
import ChatContainter from "./ChatContainer.vue";
import {
  ref,
  defineProps,
  withDefaults,
  defineEmits,
  onMounted,
  watch,
} from "vue";
import type { Chat } from "@/types/api/chat";
import type { Message, MessagesGetParams } from "@/types/api/message";

const currentMessage = ref<string | null>(null);
const messagesForm = ref<HTMLDivElement | null>(null);

const props = withDefaults(
  defineProps<{
    chat: Chat;
    ownerId?: string;
    messages?: Message[];
  }>(),
  {
    messages: () => [],
    title: "Chat",
  }
);
const emit = defineEmits<{
  (e: "loadMessages", params: MessagesGetParams): Promise<void>;
  (e: "sendMessage", params: Record<string, any>): Promise<void>;
  (e: "backToHub"): void;
}>();

onMounted(async () => {
  if (props.chat != null)
    await emit("loadMessages", {
      chat: props.chat,
      offset: 0,
    });
});
watch(props.messages, async () => {
  messagesForm?.value?.scrollIntoView(false);
});
async function sendMessage() {
  if (
    currentMessage.value == null ||
    currentMessage.value.trim() == "" ||
    props.chat == null
  )
    return;

  await emit("sendMessage", {
    chat: props.chat,
    text: currentMessage.value,
  });
  currentMessage.value = "";
}
</script>
