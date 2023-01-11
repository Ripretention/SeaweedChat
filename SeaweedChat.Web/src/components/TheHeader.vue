<template>
  <v-app-bar app fixed color="teal accent-4">
    <router-link custom to="/">
      <v-btn icon>
        <v-icon>mdi-home</v-icon>
      </v-btn>
    </router-link>

    <v-toolbar-title>SeaweedChat</v-toolbar-title>

    <v-spacer></v-spacer>

    <router-link custom to="/messages">
      <v-btn icon>
        <v-icon>mdi-message</v-icon>
      </v-btn>
    </router-link>

    <v-menu left bottom>
      <template v-slot:activator="{ on, attrs }">
        <v-btn icon v-bind="attrs" v-on="on">
          <v-icon>mdi-login</v-icon>
        </v-btn>
      </template>

      <v-list>
        <v-list-item @click="() => console.log('qwe')">
          <v-list-item-title>Sing in</v-list-item-title>
        </v-list-item>

        <v-list-item>
          <v-list-item-title>Sing up</v-list-item-title>
        </v-list-item>
      </v-list>
    </v-menu>
  </v-app-bar>
</template>
