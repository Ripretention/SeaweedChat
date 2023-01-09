<template>
  <v-app-bar color="deep-purple accent-4" dense dark>
    <v-btn icon>
      <v-icon>mdi-home</v-icon>
    </v-btn>

    <v-toolbar-title>SeaweedChat</v-toolbar-title>

    <v-spacer></v-spacer>

    <v-btn icon>
      <v-icon>mdi-message</v-icon>
    </v-btn>

    <v-btn icon>
      <v-icon>mdi-account</v-icon>
    </v-btn>
  </v-app-bar>
</template>
<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  type: "TheHeader",
});
</script>
