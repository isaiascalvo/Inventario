<template>
  <b-navbar class="is-primary">
    <template slot="brand">
      <b-navbar-item @click="openSidebar = true">
        <b-icon icon="menu"></b-icon>
      </b-navbar-item>
      <b-navbar-item tag="router-link" :to="{ path: '/' }">
        <img src="../assets/todoHogarLogo.png" alt="Todo Hogar logo" />
      </b-navbar-item>
      <b-sidebar
        type="has-background-dark"
        :fullheight="true"
        :overlay="true"
        :open.sync="openSidebar"
      >
        <div class="p-1">
          <img src="../assets/todoHogarLogo.png" alt="Todo Hogar logo" />
          <b-menu :activable="false">
            <b-menu-list label="Menu">
              <div v-for="item in items" :key="item.id">
                <b-menu-item
                  v-if="item.url"
                  :label="item.text"
                  tag="router-link"
                  :to="item.url"
                  :icon="item.icon"
                >
                </b-menu-item>

                <b-menu-item
                  v-if="!item.url"
                  @click="item.open = !item.open"
                  :icon="item.icon"
                >
                  <template slot="label" slot-scope="props">
                    {{ item.text }}
                    <b-icon
                      class="is-pulled-right"
                      :icon="props.expanded ? 'menu-down' : 'menu-right'"
                    ></b-icon>
                  </template>
                  <b-menu-item
                    v-for="subItem in item.subItems"
                    :key="subItem.id"
                    :label="subItem.text"
                    tag="router-link"
                    :to="subItem.url"
                    :icon="subItem.icon"
                  ></b-menu-item>
                </b-menu-item>
              </div>
            </b-menu-list>
          </b-menu>
        </div>
      </b-sidebar>
    </template>

    <template slot="end">
      <b-navbar-dropdown label="Lang">
        <b-navbar-item href="#">EN</b-navbar-item>
        <b-navbar-item href="#">ES</b-navbar-item>
        <b-navbar-item href="#">RU</b-navbar-item>
        <b-navbar-item href="#">FA</b-navbar-item>
      </b-navbar-dropdown>

      <b-navbar-dropdown label="">
        <template v-slot:button-content>
          <em>User</em>
        </template>
        <b-navbar-item href="#">Profile</b-navbar-item>
        <b-navbar-item href="#">Sign Out</b-navbar-item>
      </b-navbar-dropdown>
    </template>
  </b-navbar>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";

@Component
export default class Menu extends Vue {
  public openSidebar = false;
  public items3 = [
    { icon: "trending_up", text: "Most Popular" },
    { icon: "subscriptions", text: "Subscriptions" },
    { icon: "history", text: "History" },
    { icon: "featured_play_list", text: "Playlists" },
    { icon: "watch_later", text: "Watch Later" }
  ];
  public items2 = [
    { picture: 28, text: "Joseph" },
    { picture: 38, text: "Apple" },
    { picture: 48, text: "Xbox Ahoy" },
    { picture: 58, text: "Nokia" },
    { picture: 78, text: "MKBHD" }
  ];
  public items = [
    {
      icon: "text-box-outline",
      text: "Productos",
      url: "/product-list"
      // subItems: [
      //   { text: "Rate List", url: "/rates" },
      //   { text: "Filtered Rates", url: "/rates/filteredRates" },
      //   { text: "Find One Rate", url: "/rates/findOne" }
      // ]
    },
    { icon: "shape", text: "Categorías", url: "/category-list" },
    { icon: "database", text: "Proveedores", url: "/vendor-list" },
    { icon: "human", text: "Clientes", url: "/client-list" }
  ];
}
</script>

<style></style>
