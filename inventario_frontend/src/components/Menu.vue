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
        :can-cancel="true"
        :overlay="false"
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
      <b-navbar-dropdown :label="getName()" v-if="loggedIn()">
        <b-navbar-item tag="router-link" :to="'/user/modify/' + getUserId()">
          Mi Perfil
        </b-navbar-item>
        <b-navbar-item @click="logOut()">Cerrar Sesión</b-navbar-item>
      </b-navbar-dropdown>

      <b-navbar-item v-if="!loggedIn()" tag="router-link" to="/login">
        Iniciar Sesión
      </b-navbar-item>
    </template>
  </b-navbar>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { JwtResult } from "../models/JwtResult";

@Component
export default class Menu extends Vue {
  public openSidebar = false;
  public items = [
    {
      icon: "text-box-outline",
      text: "Productos",
      url: "/product-list"
    },
    { icon: "shape", text: "Categorías", url: "/category-list" },
    {
      icon: "truck-delivery-outline",
      text: "Proveedores",
      url: "/vendor-list"
    },
    { icon: "human", text: "Clientes", url: "/client-list" },
    { icon: "account-group", text: "Usuarios", url: "/user-list" },
    {
      icon: "ballot-recount-outline",
      text: "Movimientos de Stock",
      url: "/product-entry-list"
    },
    { icon: "briefcase-outline", text: "Ventas", url: "/sañe-list" }
  ];

  public loggedIn() {
    return sessionStorage.getItem("currentUser") !== null;
  }

  public getName() {
    const currentUser: JwtResult = JSON.parse(
      sessionStorage.getItem("currentUser") ?? ""
    );
    return currentUser.name + " " + currentUser.lastname;
  }

  public getUserId() {
    const currentUser: JwtResult = JSON.parse(
      sessionStorage.getItem("currentUser") ?? ""
    );
    return currentUser.id;
  }

  public logOut() {
    // this.$store.commit("setCurrentUser", null);
    sessionStorage.removeItem("currentUser");
    this.$forceUpdate();
    this.$router.push("/login");
  }
}
</script>

<style></style>
