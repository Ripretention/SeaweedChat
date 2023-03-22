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
                  v-model="createChatUsername"
                  @keypress.enter="emit('addChat', createChatUsername)"
                  color="blue"
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
          :key="chat.id"
          v-for="chat in props.chats"
        >
          <v-card
            v-bind="props"
            :color="isHovering ? 'blue' : undefined"
            :text="chat.text"
            :title="chat.title"
            @click="emit('selectChat', chat)"
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

const createChatUsername = ref<string>();
const props = withDefaults(
  defineProps<{
    chats: Chat[];
  }>(),
  {
    chats: () => [],
  }
);
const emit = defineEmits<{
  (e: "addChat", username: string): Promise<any>;
  (e: "selectChat", chat: Chat): void;
}>();
const dialog = ref(false);

function validateAddChatInput(username: string) {
  return username == null || username.trim() == ""
    ? "Username is required"
    : true;
}
</script>
