<template>
  <div>
    <b-navbar
      toggleable="lg"
      type="dark"
      variant="info"
      style="padding-left: 0rem;"
    >
      <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

      <b-collapse id="nav-collapse" is-nav>
        <b-navbar-nav>
          <b-button v-b-toggle.sidebar-1 variant="default">
            <b-icon icon="list"></b-icon>
          </b-button>
          <b-sidebar id="sidebar-1" title="Menú" shadow>
            <nav class="mb-3 ml-2">
              <b-nav vertical>
                <b-nav-item v-for="item in items" :key="item.id">
                  <router-link :to="item.url">{{ item.text }}</router-link>
                </b-nav-item>
              </b-nav>
            </nav>
          </b-sidebar>
        </b-navbar-nav>

        <!-- Right aligned nav items -->
        <b-navbar-nav class="ml-auto">
          <b-nav-form>
            <b-form-input
              size="sm"
              class="mr-sm-2"
              placeholder="Search"
            ></b-form-input>
            <b-button size="sm" class="my-2 my-sm-0" type="submit"
              >Search</b-button
            >
          </b-nav-form>

          <b-nav-item-dropdown text="Lang" right>
            <b-dropdown-item href="#">EN</b-dropdown-item>
            <b-dropdown-item href="#">ES</b-dropdown-item>
            <b-dropdown-item href="#">RU</b-dropdown-item>
            <b-dropdown-item href="#">FA</b-dropdown-item>
          </b-nav-item-dropdown>

          <b-nav-item-dropdown right>
            <!-- Using 'button-content' slot -->
            <template v-slot:button-content>
              <em>User</em>
            </template>
            <b-dropdown-item href="#">Profile</b-dropdown-item>
            <b-dropdown-item href="#">Sign Out</b-dropdown-item>
          </b-nav-item-dropdown>
        </b-navbar-nav>
      </b-collapse>
    </b-navbar>
    <router-view></router-view>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
// import HelloWorld from './components/HelloWorld.vue';

export default Vue.extend({
  name: "App",

  props: {
    source: String
  },
  data: () => ({
    drawer: null,
    items3: [
      { icon: "trending_up", text: "Most Popular" },
      { icon: "subscriptions", text: "Subscriptions" },
      { icon: "history", text: "History" },
      { icon: "featured_play_list", text: "Playlists" },
      { icon: "watch_later", text: "Watch Later" }
    ],
    items2: [
      { picture: 28, text: "Joseph" },
      { picture: 38, text: "Apple" },
      { picture: 48, text: "Xbox Ahoy" },
      { picture: 58, text: "Nokia" },
      { picture: 78, text: "MKBHD" }
    ],
    items: [
      {
        icon: "rate_review",
        text: "Productos",
        url: "/product-list"
        // subItems: [
        //   { text: "Rate List", url: "/rates" },
        //   { text: "Filtered Rates", url: "/rates/filteredRates" },
        //   { text: "Find One Rate", url: "/rates/findOne" }
        // ]
      },
      { icon: "category", text: "Categorías", url: "/category-list" }
      // { icon: "view_comfy", text: "Units", url: "/units" },
      // { icon: "attach_money", text: "Currencies", url: "/currencies" },
      // { icon: "work", text: "Activities", url: "/activities" },
      // { icon: "people", text: "Customers", url: "/customers" },
      // { icon: "person", text: "Vendors", url: "/vendors" }
    ]
  }),
  methods: {
    goTo(url: string) {
      this.$router.push(url).catch(() => {
        /* */
      });
    }
  }
});
</script>

<style>
.pointer {
  cursor: pointer;
}
</style>
