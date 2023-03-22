<template>
  <ChatContainter>
    <template #toolbar>
      <v-toolbar color="blue">
        <v-btn icon @click="emit('backToHub')">
          <v-icon>mdi-arrow-left</v-icon>
        </v-btn>
        <v-toolbar-title>{{ props.title }}</v-toolbar-title>
      </v-toolbar>
    </template>

    <template #content>
      <div ref="messages">
        <div class="d-flex flex-column">
          <ChatMessage
            v-for="msg in props.messages"
            :key="msg.id"
            :author="msg.ownerUsername"
            :text="msg.text"
            :direction="msg.ownerId == props.ownerId ? 'from' : 'to'"
            :date="msg.editAt ?? msg.createdAt"
          />
        </div>

        <div
          class="d-flex flex-row ma-0 pa-0"
          style="position: sticky; bottom: 0"
        >
          <v-text-field
            outlined
            v-model="currentMessage"
            color="blue"
            placeholder="Write a message"
            @keypress.enter="sendMessage"
            append-inner-icon="mdi-send"
            hide-details
            bg-color="grey-lighten-2"
          ></v-text-field>
        </div>
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
import type { Message } from "@/types/Chat";

const currentMessage = ref<string | null>(null);
const messagesForm = ref<HTMLDivElement | null>(null);

const props = withDefaults(
  defineProps<{
    id?: string;
    ownerId?: string;
    messages: Message[];
    title: string;
  }>(),
  {
    messages: () => [],
    title: "Chat",
  }
);
const emit = defineEmits<{
  (
    e: "loadMessages",
    params: {
      chatId: string;
      offset: number;
    }
  ): Promise<void>;
  (e: "sendMessage", chatId: string, text: string): Promise<void>;
  (e: "backToHub"): void;
}>();

onMounted(async () => {
  if (props.id != null)
    await emit("loadMessages", {
      chatId: props.id,
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
    props.id == null
  )
    return;

  await emit("sendMessage", props.id, currentMessage.value);
  currentMessage.value = "";
}
</script>
