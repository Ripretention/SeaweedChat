<template>
  <v-app>
    <TheHeader />
    <v-main>
      <router-view></router-view>
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
import TheHeader from "@/components/TheHeader.vue";
</script>
