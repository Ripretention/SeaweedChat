<template>
  <v-card
    max-width="428"
    :class="{ 'float-left': isFrom, 'float-right': !isFrom }"
    class="rounded-xl ma-4"
    variant="tonal"
  >
    <v-card-title class="d-flex flex-row">
      <strong class="text-blue text-subtitle-1">{{ props.author }}</strong>
      <p class="text-subtitle-1 ml-2">{{ props.date }}</p>
    </v-card-title>
    <v-card-text>{{ props.text }}</v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import { computed, defineProps } from "vue";
const props = defineProps<{
  author: string;
  date: Date;
  text: string;
  direction: "from" | "to";
}>();
const isFrom = computed(() => props.direction == "from");

function displayDateHours(d: Date) {
  return [d.getHours(), d.getSeconds()].map((s) =>
    s.toString().padStart("0", 2)
  );
}
</script>
