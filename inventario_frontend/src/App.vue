<template>
  <div>
    <Menu />
    <router-view class="box"></router-view>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import Menu from "./components/Menu.vue";

@Component({
  components: {
    Menu
  }
})
export default class App extends Vue {}
</script>

<style lang="scss">
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  background-color: #f2f2f2;
}

#nav {
  padding: 30px;

  a {
    font-weight: bold;
    color: #2c3e50;

    &.router-link-exact-active {
      color: #42b983;
    }
  }
}

.pointer {
  cursor: pointer;
}

.p-1 {
  padding: 1em;
}

.mx-1 {
  margin-left: 1em;
  margin-right: 1em;
}

.m-1 {
  margin: 1em;
}

.menu-list a {
  color: white !important;
}

.menu-label {
  color: white !important;
}

.menu-list a:hover {
  background-color: #af7979 !important;
}

// Import Bulma's core
@import "~bulma/sass/utilities/_all";

// Set your colors
$navbar-color: #3075cc4d;
// $navbar-color: #8fc0a040;
$navbar-color-invert: findColorInvert($navbar-color);
$blue-gray: #dfe4f3;
$blue-gray-invert: findColorInvert($blue-gray);

// Setup $colors to use as bulma classes (e.g. 'is-twitter')
$colors: (
  "blue-gray": (
    $blue-gray,
    $blue-gray-invert
  ),
  "navbar-color": (
    $navbar-color,
    $navbar-color-invert
  )
);

// Import Bulma and Buefy styles
@import "~bulma";
@import "~buefy/src/scss/buefy";
</style>
