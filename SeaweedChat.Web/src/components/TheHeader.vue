<template>
  <v-app-bar app fixed color="teal accent-4">
    <v-btn icon>
      <v-icon>mdi-home</v-icon>
    </v-btn>

    <v-toolbar-title>SeaweedChat</v-toolbar-title>

    <v-spacer></v-spacer>

    <v-menu left bottom>
      <template v-slot:activator="{ props }">
        <v-btn v-bind="props">
          <p if="isAuthorized">{{ username }}</p>
          <v-icon size="large" end icon="mdi-account-circle"></v-icon>
        </v-btn>
      </template>

      <v-list v-if="!props.isAuthorized">
        <v-list-item href="./login">
          <v-list-item-title>Sing in</v-list-item-title>
        </v-list-item>
        <v-list-item href="./register">
          <v-list-item-title>Sing up</v-list-item-title>
        </v-list-item>
      </v-list>

      <v-list v-else>
        <v-list-item link @click="emit('logout')">
          <template v-slot:prepend>
            <v-icon icon="mdi-logout"></v-icon>
          </template>
          <v-list-item-title>Log out</v-list-item-title>
        </v-list-item>
      </v-list>
    </v-menu>
  </v-app-bar>
</template>

<script setup lang="ts">
import { defineProps, withDefaults, computed } from "vue";
const props = withDefaults(
  defineProps<{
    isAuthorized: boolean;
    username?: string;
  }>(),
  {
    isAuthorized: false,
  }
);
const emit = defineEmits<{
  (e: "logout"): void;
}>();

const username = computed(() => props?.username ?? "User");
</script>
