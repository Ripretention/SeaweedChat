<template>
  <div class="py-4">
    <v-img class="mx-auto" max-width="428" :src="logo"></v-img>
    <v-card
      class="mx-auto pa-12 pb-8"
      elevation="8"
      max-width="448"
      rounded="lg"
    >
      <v-form ref="form" @submit.prevent>
        <div class="text-subtitle-1 text-medium-emphasis">Username</div>
        <v-text-field
          v-model="username"
          density="compact"
          :error-messages="errors.username"
          placeholder="Username"
          prepend-inner-icon="mdi-account-outline"
          variant="outlined"
        ></v-text-field>

        <div class="text-subtitle-1 text-medium-emphasis">Email</div>
        <v-text-field
          v-model="email"
          density="compact"
          :error-messages="errors.email"
          placeholder="Email address"
          prepend-inner-icon="mdi-email-outline"
          variant="outlined"
        ></v-text-field>

        <div
          class="text-subtitle-1 text-medium-emphasis d-flex align-center justify-space-between"
        >
          Password
        </div>
        <v-text-field
          v-model="password"
          :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'"
          :type="visible ? 'text' : 'password'"
          density="compact"
          placeholder="Enter your password"
          prepend-inner-icon="mdi-lock-outline"
          variant="outlined"
          :error-messages="errors.password"
          @click:append-inner="visible = !visible"
        ></v-text-field>

        <v-alert
          v-if="error"
          v-model="error"
          class="text-medium-emphasis mb-4"
          type="error"
          icon="$error"
          variant="outlined"
        >
          {{ error }}
        </v-alert>

        <v-btn
          @click="submitRegisterForm"
          block
          class="mb-4"
          color="blue"
          size="large"
          variant="tonal"
        >
          Sign Up
        </v-btn>
      </v-form>
    </v-card>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import router from "@/router";
import logo from "@/assets/logo.svg";
import { AxiosError } from "axios";
import store from "@/store";

const form = ref<(HTMLFormElement & { ressetValidation: () => void }) | null>(
  null
);
const error = ref<string | null>(null);
const visible = ref(false);
const username = ref();
const email = ref();
const password = ref();
const errors = ref<Record<string, string[]>>({
  email: [],
  password: [],
  username: [],
});

async function submitRegisterForm() {
  error.value = null;
  errors.value = { email: [], password: [], username: [] };
  form.value?.resetValidation();

  try {
    await store.dispatch("register", {
      password: password.value,
      username: username.value,
      email: email.value,
    });

    router.push({ path: "/" });
  } catch (e) {
    if (e instanceof AxiosError) {
      if (e.code === AxiosError.ERR_BAD_REQUEST) {
        let response = e?.response?.data;

        if (typeof response === "object" && response != null) {
          let validationErrors: Record<string, string[]> =
            response.errors ?? {};

          for (let errorKey in validationErrors) {
            errors.value[errorKey.toLowerCase()] = validationErrors[errorKey];
          }
        } else {
          error.value = response;
        }
      } else {
        error.value = e.message;
      }
    }
  }
}
</script>
