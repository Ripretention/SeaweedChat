<template>
  <v-footer color="deep-purple accent-4" absolute>
    <v-col class="text-center" cols="12">
      {{ new Date().getFullYear() }} — <strong>SeaweedChat</strong>
    </v-col>
  </v-footer>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "TheFooter",
});
</script>
