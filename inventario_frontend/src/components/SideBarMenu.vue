<template>
  <div>
    <div class="sidebar-page">
      <div class="sidebar-layout">
        <b-sidebar
          class="mySidebar"
          type="has-background-dark"
          fullheight
          open
          :reduce="reduce"
          position="static"
          id="my-sidebar"
        >
          <div>
            <img
              v-if="!reduce"
              src="../assets/todoHogarLogo.png"
              alt="TodoHogar logo"
              class="p-1"
            />
            <img
              v-if="reduce"
              src="../assets/todoHogarLogo.png"
              alt="TodoHogar logo"
              class="p-1"
            />
            <b-menu :activable="false">
              <b-menu-list>
                <div v-for="item in items" :key="item.id">
                  <b-menu-item
                    v-if="item.url"
                    :label="item.text"
                    tag="router-link"
                    :to="item.url"
                    :icon="item.icon"
                  >
                  </b-menu-item>

                  <b-menu-item :icon="item.icon" v-if="!item.url">
                    <template slot="label" slot-scope="props">
                      <span v-if="!reduce">
                        {{ item.text }}
                      </span>
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
              <!-- <b-menu-list class="fixed-bottom-left">
                <b-menu-item
                  @click="reduce = !reduce"
                  :icon="
                    reduce ? 'chevron-double-right' : 'chevron-double-left'
                  "
                ></b-menu-item>
              </b-menu-list> -->
              <b-button
                @click="reduce = !reduce"
                :icon-right="
                  reduce ? 'chevron-double-right' : 'chevron-double-left'
                "
                type="is-dark"
                size="is-large"
                class="fixed-bottom-left"
              >
              </b-button>
            </b-menu>
          </div>
        </b-sidebar>
      </div>
    </div>
    <b-navbar type="is-navbar-color">
      <template slot="start">
        <b-navbar-item tag="div">
          <strong>{{ getTitle() }}</strong>
        </b-navbar-item>
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
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { JwtResult } from "../models/JwtResult";

@Component
export default class SideBarMenu extends Vue {
  public reduce = false;
  public title = "";
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
    { icon: "briefcase-outline", text: "Ventas", url: "/sale-list" },
    {
      icon: "briefcase-outline",
      text: "Cuotas Aplicables",
      url: "/fee-rule-list"
    }
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
    sessionStorage.removeItem("currentUser");
    this.$forceUpdate();
    this.$router.push("/login");
  }

  public getTitle(): string {
    const path = this.$route.path;
    const item = this.items.find(x => x.url === path);
    if (item) {
      this.title = item.text;
    }
    return this.title;
  }

  mounted() {
    const w = window;
    const e = document.documentElement;
    const g = document.getElementsByTagName("body")[0];
    const y = w.innerHeight || e.clientHeight || g.clientHeight;
    const elem = document.getElementById("my-sidebar");
    if (elem) {
      elem.style.height = y + "px";
    }
  }
}
</script>

<style lang="scss">
.fixed-bottom-left {
  position: fixed !important;
  bottom: 10px;
  left: 10px;
}
.sidebar-page {
  flex-direction: column;
  width: 100%;
  min-height: 100%;
  height: 100%;
  // min-height: 100vh;
  .sidebar-layout {
    flex-direction: row;
    min-height: 100%;
    height: 100%;
    // min-height: 100vh;
  }
}
@media screen and (max-width: 1023px) {
  .b-sidebar {
    height: 100%;
    .sidebar-content {
      &.is-mini-mobile {
        &:not(.is-mini-expand),
        &.is-mini-expand:not(:hover) {
          .menu-list {
            li {
              a {
                span:nth-child(2) {
                  display: none;
                }
                span.icon {
                  display: inline-block;
                }
              }
              ul {
                padding-left: 0;
                li {
                  a {
                    display: inline-block;
                  }
                }
              }
              font-size: 1.5rem;
            }
          }
          .menu-label:not(:last-child) {
            margin-bottom: 0;
          }
        }
      }
    }
  }
}
@media screen and (min-width: 1024px) {
  .b-sidebar {
    height: 100%;
    .sidebar-content {
      &.is-mini {
        &:not(.is-mini-expand),
        &.is-mini-expand:not(:hover) {
          .menu-list {
            li {
              a {
                span:nth-child(2) {
                  display: none;
                }
                span.icon {
                  display: inline-block;
                }
              }
              ul {
                padding-left: 0;
                li {
                  a {
                    display: inline-block;
                  }
                }
              }
              font-size: 1.5rem;
            }
          }
          .menu-label:not(:last-child) {
            margin-bottom: 0;
          }
        }
      }
    }
  }
}
.menu {
  text-align: left;
}
.mySidebar {
  float: left;
  min-height: 100%;
  // height: 10000px;
}
.b-sidebar {
  .sidebar-content {
    max-width: 225px !important;
  }
}
</style>