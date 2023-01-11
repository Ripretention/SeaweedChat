<template>
  <v-container class="pa-md-16 mx-lg auto text-center">
    <v-row>
      <v-col>
        <h1 class="text-h2">Sing up</h1>
      </v-col>
    </v-row>

    <v-row>
      <v-col>
        <v-form ref="form" v-model="valid" lazy-validation>
          <v-text-field
            v-model="email"
            :rules="emailRules"
            label="E-mail"
            required
          ></v-text-field>

          <v-text-field
            v-model="username"
            :counter="32"
            :rules="nameRules"
            label="Username"
            required
          ></v-text-field>

          <v-text-field
            v-model="password"
            :rules="nameRules"
            label="Password"
            :type="passwordShowState ? 'text' : 'password'"
            hint="At least 8 characters"
            required
            @click:append="passwordShowState = !passwordShowState"
          ></v-text-field>

          <v-btn color="success">Submit</v-btn>
        </v-form>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref } from "vue";

const passwordShowState = ref(false);
</script>
