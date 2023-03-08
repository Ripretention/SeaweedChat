<template>
  <v-card class="mx-auto ma-4" max-width="50vw" elevation="8" rounded="lg">
    <slot name="toolbar"></slot>
    <div class="overflow-y-auto" style="height: calc(100vh - 160px)">
      <slot name="content"></slot>
    </div>
  </v-card>
</template>

<script setup lang="ts"></script>
