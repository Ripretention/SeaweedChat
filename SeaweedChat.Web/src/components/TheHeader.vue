<template>
  <v-app-bar app fixed color="teal accent-4">
    <router-link custom to="/">
      <v-btn icon>
        <v-icon>mdi-home</v-icon>
      </v-btn>
    </router-link>

    <v-toolbar-title>SeaweedChat</v-toolbar-title>

    <v-spacer></v-spacer>

    <v-menu left bottom>
      <template v-slot:activator="{ props }">
        <v-btn icon dark v-bind="props">
          <v-icon>mdi-login</v-icon>
        </v-btn>
      </template>

      <v-list>
        <v-list-item href="./login">
          <v-list-item-title>Sing in</v-list-item-title>
        </v-list-item>

        <v-list-item href="./register">
          <v-list-item-title>Sing up</v-list-item-title>
        </v-list-item>
      </v-list>
    </v-menu>
  </v-app-bar>
</template>
