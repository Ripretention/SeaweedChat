<template>
  <div class="d-float">
    <v-card
      :class="{ 'float-left': isFrom, 'float-right': !isFrom }"
      max-width="50%"
      class="rounded-xl ma-4"
      variant="tonal"
    >
      <v-card-title class="d-flex flex-row">
        <strong class="text-blue text-subtitle-1">{{ props.author }}</strong>
        <p class="text-subtitle-1 ml-2">{{ prettyDate(props.date) }}</p>
      </v-card-title>
      <v-card-text>{{ props.text }}</v-card-text>
    </v-card>
  </div>
</template>

<script setup lang="ts">
import { computed, defineProps } from "vue";
import { prettyDate } from "@/utils";
const props = defineProps<{
  author: string;
  date: Date;
  text: string;
  direction: "from" | "to";
}>();
console.log(props.date);
const isFrom = computed(() => props.direction == "from");
</script>
