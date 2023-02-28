<template>
  <v-app-bar app fixed color="teal accent-4">
    <router-link custom to="/">
      <v-btn icon>
        <v-icon>mdi-home</v-icon>
      </v-btn>
    </router-link>

    <v-toolbar-title>SeaweedChat</v-toolbar-title>

    <v-spacer></v-spacer>

    <v-menu left bottom>
      <template v-slot:activator="{ props }">
        <v-btn icon dark v-bind="props">
          <v-icon>mdi-login</v-icon>
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
          <v-list-item-title>Log out</v-list-item-title>
        </v-list-item>
      </v-list>
    </v-menu>
  </v-app-bar>
</template>

<script setup lang="ts">
import { defineProps, withDefaults } from "vue";
const props = withDefaults(
  defineProps<{
    isAuthorized: boolean;
  }>(),
  {
    isAuthorized: false,
  }
);
const emit = defineEmits<{
  (e: "logout"): void;
}>();
</script>
