<template>
  <div class="d-float">
    <v-card
      :class="{ 'float-left': isFrom, 'float-right': !isFrom }"
      max-width="50%"
      class="rounded-xl ma-2"
      variant="tonal"
    >
      <v-card-title class="d-flex flex-row">
        <strong class="text-blue text-subtitle-1">{{ props.author }}</strong>
        <p :title="prettyDate(date)" class="text-subtitle-1 ml-2">
          {{ prettyShortDate(date) }}
        </p>
      </v-card-title>
      <v-card-text>{{ props.text }}</v-card-text>
    </v-card>
  </div>
</template>

<script setup lang="ts">
import { computed, defineProps, ref } from "vue";
import { prettyDate, prettyShortDate } from "@/utils";
const props = defineProps<{
  author: string;
  date: string;
  text: string;
  direction: "from" | "to";
}>();
const date = ref<Date>(new Date(props.date));
const isFrom = computed(() => props.direction == "from");
</script>
