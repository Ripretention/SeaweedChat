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
            :key="msg.date"
            :author="msg.author"
            :text="msg.text"
            :direction="msg.direction"
            :date="msg.date"
          />
        </div>

        <div
          class="d-flex flex-row ma-0 pa-0"
          style="position: sticky; bottom: 0"
        >
          <v-text-field
            outlined
            v-model="msg"
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

const messages = ref<HTMLDivElement | null>(null);
const msg = ref<string | null>(null);
const props = withDefaults(
  defineProps<{
    title: string;
    messages: Message[];
  }>(),
  {
    title: "Chat",
    messages: () => [],
  }
);
const emit = defineEmits<{
  (e: "sendMessage", text: string): Promise<any>;
  (e: "backToHub"): any;
}>();

onMounted(() => {
  messages?.value?.scrollIntoView(false);
});
watch(props.messages, async () => {
  messages?.value?.scrollIntoView(false);
});
async function sendMessage() {
  if (msg.value == null || msg.value.trim() == "") return;

  await emit("sendMessage", msg.value);
  msg.value = "";
}
</script>
