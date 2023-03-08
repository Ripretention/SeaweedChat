<template>
  <ChatContainer>
    <template #toolbar>
      <v-toolbar title="Chats">
        <v-spacer></v-spacer>

        <v-btn @click="dialog = true" icon>
          <v-icon>mdi-plus</v-icon>
        </v-btn>

        <v-dialog v-model="dialog" width="auto">
          <v-card title="Add chat">
            <v-sheet width="428" class="mx-auto pa-6">
              <v-form>
                <v-text-field
                  @keypress.enter="emit('addChat', createChatInput)"
                  v-model="createChatInput"
                  color="blue"
                  :loading="loading"
                  density="compact"
                  label="Username"
                  :rules="[validateAddChatInput]"
                  append-inner-icon="mdi-plus"
                ></v-text-field>
              </v-form>
            </v-sheet>

            <v-card-actions>
              <v-btn class="text-blue" block @click="dialog = false"
                >Close</v-btn
              >
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-toolbar>
    </template>

    <template #content>
      <div
        v-if="props.chats.length > 0"
        class="overflow-y-auto"
        style="height: calc(100vh - 160px)"
      >
        <v-hover
          v-slot="{ isHovering, props }"
          :key="chat.date"
          v-for="chat in props.chats"
          @click="emit('chatSelect', chat)"
        >
          <v-card
            v-bind="props"
            :color="isHovering ? 'blue' : undefined"
            :text="chat.text"
            :title="chat.author"
          ></v-card>
        </v-hover>
      </div>

      <v-sheet
        v-else
        class="w-auto d-flex align-center justify-center flex-wrap text-center mx-auto"
        elevation="4"
        height="calc(100vh - 160px)"
      >
        <div>
          <h2 class="text-h4 font-weight-black text-green">Welcome!</h2>

          <div class="text-h5 font-weight-medium mb-2">
            You are ready for new acquaintances.
          </div>

          <v-btn variant="text" color="blue" @click="dialog = !dialog"
            >Find friends</v-btn
          >
        </div>
      </v-sheet>
    </template>
  </ChatContainer>
</template>

<script setup lang="ts">
import type { Chat } from "@/types/Chat";
import { defineProps, ref, withDefaults, defineEmits } from "vue";
import ChatContainer from "./ChatContainer.vue";

const props = withDefaults(
  defineProps<{
    chats: Chat[];
  }>(),
  {
    chats: () => [],
  }
);
defineEmits<{
  (e: "selectChat", chat: Chat): void;
}>();
const dialog = ref(false);

function validateAddChatInput(username: string) {
  return username == null || username.trim() == ""
    ? "Username is required"
    : true;
}
</script>
