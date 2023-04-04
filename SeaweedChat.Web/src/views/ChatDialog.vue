<template>
  <ChatContainter v-if="chat != null">
    <template #toolbar>
      <v-toolbar color="blue" v-if="selectedMessages.length > 0">
        <v-btn
          class="me-2"
          prepend-icon="mdi-delete"
          variant="tonal"
          @click="deleteMessages"
        >
          Delete
          <template #append>
            {{ selectedMessages.length }}
          </template>
        </v-btn>
        <v-btn
          v-if="selectedMessages.length === 1"
          prepend-icon="mdi-pencil"
          variant="tonal"
          @click="startEditingMessage(selectedMessages[0])"
          >Edit</v-btn
        >
        <v-spacer></v-spacer>
        <v-btn class="text-none" @click="resetMessageActions">Close</v-btn>
      </v-toolbar>

      <v-toolbar color="blue" v-else>
        <v-btn icon link to="/">
          <v-icon icon="mdi-arrow-left"></v-icon>
        </v-btn>
        <v-toolbar-title>{{ chat.title }}</v-toolbar-title>

        <v-spacer></v-spacer>

        <v-menu bottom>
          <template v-slot:activator="{ props }">
            <v-btn icon v-bind="props">
              <v-icon icon="mdi-dots-vertical"></v-icon>
            </v-btn>
          </template>

          <v-list>
            <v-list-item link @click="deleteChat">
              <template v-slot:prepend>
                <v-icon icon="mdi-delete" color="error"></v-icon>
              </template>
              <v-list-item-title>Delete</v-list-item-title>
            </v-list-item>
          </v-list>
        </v-menu>
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
          :class="{
            'bg-blue-lighten-4': selectedMessages.some(
              (msgId) => msgId == msg.id
            ),
          }"
          @dblclick="selectMessage(msg.id)"
          @click="
            () => {
              if (selectedMessages.length > 0) selectMessage(msg.id);
            }
          "
        />
      </div>

      <div style="position: sticky; bottom: 0">
        <v-text-field
          v-if="currentEditingMessage != null"
          autofocus
          :loading="isLoading"
          outlined
          color="yellow"
          v-model="currentEditingMessage.text"
          @keyup.esc="resetMessageActions"
          @keypress.enter="editMessage"
          append-inner-icon="mdi-pencil"
          hide-details
          bg-color="grey-lighten-2"
        ></v-text-field>
        <v-text-field
          v-else
          autofocus
          :loading="isLoading"
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
import router from "@/router";
import { useRoute } from "vue-router";
import ChatMessage from "@/components/ChatMessage.vue";
import ChatContainter from "@/components/ChatContainer.vue";
import { ref, onMounted, watch, nextTick, computed } from "vue";
import type { Chat } from "@/types/api/chat";
import type { Message } from "@/types/api/message";

const routeIdParam = useRoute().params["id"];
const chatId =
  typeof routeIdParam === "string" ? routeIdParam : routeIdParam[0];

const chat = ref<Chat | undefined>();
const ownerId = computed(() => store.state.user.id);
const messages = computed(() => store.state.messages.messages?.[chatId] ?? []);

const isLoading = ref(false);
const currentMessage = ref<string | null>(null);
const messagesForm = ref<HTMLDivElement>();

const selectedMessages = ref<string[]>([]);
const currentEditingMessage = ref<Message | null>();
function startEditingMessage(msgId: string) {
  let msg = messages?.value?.find?.((msg) => msg.id === msgId);
  if (!msg) {
    return;
  }

  currentEditingMessage.value = { ...msg };
}
async function editMessage() {
  let editingMsg = currentEditingMessage.value;
  let content = editingMsg?.text?.trim?.() ?? "";
  if (editingMsg == null || content === "") {
    return;
  }

  await store.dispatch("editMessage", {
    chatId,
    text: content,
    id: editingMsg.id,
  });

  resetMessageActions();
}
async function deleteMessages() {
  let deletingMessageIds = selectedMessages.value ?? [];

  await Promise.all(
    deletingMessageIds.map((id) =>
      store.dispatch("deleteMessage", {
        id,
        chatId,
      })
    )
  );

  resetMessageActions();
}
function resetMessageActions() {
  currentEditingMessage.value = null;
  selectedMessages.value = [];
}
function selectMessage(msgId: string) {
  let isCurrentlyEditing = currentEditingMessage.value != null;
  if (isCurrentlyEditing) {
    return;
  }

  let index = selectedMessages.value.findIndex((el) => el == msgId);
  if (index === -1) {
    selectedMessages.value.push(msgId);
    return;
  }
  selectedMessages.value.splice(index, 1);
}

onMounted(async () => {
  isLoading.value = true;
  chat.value = await store.dispatch("getChat", chatId);
  if (chat.value != null && messages.value.length == 0) {
    await store.dispatch("loadMessages", {
      chat: chat.value,
      offset: 0,
    });
  }

  isLoading.value = false;
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

async function deleteChat() {
  store.dispatch("removeChat", chat.value);
  router.push({ path: "/" });
}
</script>
