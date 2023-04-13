<template>
  <ChatContainer>
    <template #toolbar>
      <v-toolbar color="grey-lighten-2" title="Chats">
        <v-spacer></v-spacer>

        <v-btn @click="dialog = true" icon>
          <v-icon>mdi-plus</v-icon>
        </v-btn>

        <v-dialog width="auto" v-model="dialog">
          <v-card width="428" class="px-4 pt-2">
            <v-card-title>
              <v-row class="align-center">
                <v-col class="v-col-2">
                  <v-icon icon="mdi-chat-plus"></v-icon>
                </v-col>
                <v-col class="v-col-2">
                  <p>Create {{ isCreateChat ? "chat" : "channel" }}</p>
                </v-col>
                <v-spacer></v-spacer>
                <v-col class="v-col-auto">
                  <v-switch
                    v-model="isCreateChat"
                    hide-details
                    inset
                    :label="isCreateChat ? 'Chat' : 'Channel'"
                  ></v-switch>
                </v-col>
              </v-row>
            </v-card-title>

            <v-form class="mt-2">
              <v-text-field
                v-if="isCreateChat"
                v-model="createChatUsername"
                @keypress.enter="
                  store.dispatch('createChatWithUser', createChatUsername)
                "
                color="blue"
                density="compact"
                label="Username"
                :rules="[validateAddChatInput]"
                append-inner-icon="mdi-plus"
              ></v-text-field>
              <v-text-field
                v-else
                v-model="createChannelTitle"
                @keypress.enter="
                  store.dispatch('createChannel', createChannelTitle)
                "
                color="blue"
                density="compact"
                label="Channel Title"
                :rules="[validateAddChannelInput]"
                append-inner-icon="mdi-plus"
              ></v-text-field>
            </v-form>

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
      <div v-if="chats.length > 0" class="d-flex flex-column">
        <v-hover
          v-slot="{ isHovering, props }"
          :key="chat.id"
          v-for="chat in chats"
        >
          <v-card
            v-bind="props"
            :color="isHovering ? 'blue' : undefined"
            :text="chat.text"
            :title="chat.title"
            :prepend-icon="
              chat.type == ChatType.Chat ? 'mdi-account' : 'mdi-account-group'
            "
            @click="selectChat(chat)"
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
import { type Chat, ChatType } from "@/types/api/chat";
import ChatContainer from "@/components/ChatContainer.vue";
import store from "@/store";
import { useRouter } from "vue-router";
import { ref, computed, onMounted } from "vue";

const router = useRouter();

const isCreateChat = ref(true);
const dialog = ref(false);
const createChatUsername = ref<string>();
const createChannelTitle = ref<string>();

const chats = computed(() => store.state.chats.chats);
onMounted(async () => {
  await store.dispatch("loadChats");
});

function validateAddChatInput(username: string) {
  return username == null || username.trim() == ""
    ? "Username is required"
    : true;
}
function validateAddChannelInput(title: string) {
  return title == null || title.trim() == "" ? "Title is required" : true;
}

function selectChat(chat: Chat) {
  router.replace({ name: "chat", params: { id: chat.id } });
}
</script>
