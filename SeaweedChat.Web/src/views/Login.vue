<template>
  <div class="py-4">
    <v-img class="mx-auto" max-width="428" :src="logo"></v-img>
    <v-card
      class="mx-auto pa-12 pb-4"
      elevation="8"
      max-width="448"
      rounded="lg"
    >
      <div class="text-subtitle-1 text-medium-emphasis">Account</div>
      <v-text-field
        v-model="email"
        density="compact"
        placeholder="Email address"
        prepend-inner-icon="mdi-email-outline"
        variant="outlined"
        :error-messages="errors.email"
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
        @click:append-inner="visible = !visible"
        :error-messages="errors.password"
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
        @click="submitLogin"
        block
        class="mb-4"
        color="blue"
        size="large"
        variant="tonal"
      >
        Log In
      </v-btn>

      <v-card-text class="text-center">
        <a class="text-blue text-decoration-none" href="./register">
          Sign up now <v-icon icon="mdi-chevron-right"></v-icon>
        </a>
      </v-card-text>
    </v-card>
  </div>
</template>
<script setup lang="ts">
import store from "@/store";
import router from "@/router";
import { ref } from "vue";
import logo from "@/assets/logo.svg";
import { AxiosError } from "axios";

const visible = ref(false);
const password = ref<string>();
const email = ref<string>();
const errors = ref<Record<string, string[]>>({
  password: [],
  email: [],
});
const error = ref<string | null>();

async function submitLogin() {
  errors.value = { password: [], email: [] };
  error.value = null;
  try {
    await store.dispatch("login", {
      password: password.value,
      email: email.value,
    });

    router.push("home");
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
