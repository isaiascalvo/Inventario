<template>
  <div>
    <section class="hero is-light">
      <div class="hero-head">
        <div class="container level">
          <div>
            <h1 class="title">Usuarios</h1>
            <h2 class="subtitle">
              Lista de usuarios
            </h2>
          </div>
          <div>
            <b-button
              type="is-info"
              tag="router-link"
              to="/user/new"
              class="mx-1"
            >
              Nuevo Usuario
            </b-button>
          </div>
        </div>
      </div>
    </section>

    <b-table
      striped
      hoverable
      scrollable
      :data="users"
      id="my-table"
      :paginated="true"
      :per-page="perPage"
      :current-page.sync="currentPage"
      aria-next-label="Next page"
      aria-previous-label="Previous page"
      aria-page-label="Page"
      aria-current-label="Current page"
    >
      <template slot="empty">
        No hay usuarios registrados
      </template>
      <template slot-scope="props">
        <b-table-column field="username" label="Nombre de Usuario">
          {{ props.row.username }}
        </b-table-column>
        <b-table-column field="name" label="Nombre">
          {{ props.row.name }}
        </b-table-column>
        <b-table-column field="lastname" label="Apellido">
          {{ props.row.lastname }}
        </b-table-column>
        <b-table-column field="dni" label="DNI">
          {{ props.row.dni }}
        </b-table-column>
        <b-table-column field="phone" label="Teléfono">
          {{ props.row.phone }}
        </b-table-column>
        <b-table-column field="mail" label="Mail">
          {{ props.row.mail }}
        </b-table-column>
        <b-table-column field="isAdmin" label="Administrador">
          {{ props.row.isAdmin ? "Si" : "No" }}
        </b-table-column>
        <b-table-column field="active" label="Activo">
          {{ props.row.active ? "Si" : "No" }}
        </b-table-column>

        <b-table-column field="action" label="Acciones">
          <b-button
            tag="router-link"
            :to="'/user/modify/' + props.row.id"
            type="is-small"
          >
            <b-icon icon="pencil"></b-icon>
          </b-button>
          <b-button
            @click="deleteUser(props.row)"
            type="is-small"
            class="actionButton"
          >
            <b-icon icon="delete"></b-icon>
          </b-button>
        </b-table-column>
      </template>
    </b-table>

    <b-loading is-full-page :active.sync="isLoading"></b-loading>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { User } from "../../models/user";
import { NavigatorUserService } from "../../services/user-service";

@Component
export default class UserList extends Vue {
  public users: User[] = [];
  public userService: NavigatorUserService = new NavigatorUserService();

  public currentPage = 1;
  public perPage = 10;
  public errorDialog = false;
  public confirmDialog = false;
  public isLoading = false;

  deleteUser(user: User) {
    this.$buefy.dialog.confirm({
      title: "Eliminando Usuario",
      message:
        "Estás seguro que deseas <b>eliminar</b> el usuario? Esta acción no podrá deshacerse.",
      confirmText: "Eliminar Usuario",
      cancelText: "Cancelar",
      type: "is-danger",
      hasIcon: true,
      onConfirm: () => {
        this.isLoading = true;
        this.userService
          .deleteUser(user.id)
          .then(() => {
            this.isLoading = false;
            this.users.splice(this.users.indexOf(user), 1);
          })
          .catch(e => {
            this.isLoading = false;
            this.$buefy.dialog.alert({
              title: "Error",
              message:
                "Un error inesperado ha ocurrido. Por favor inténtelo nuevamente.",
              type: "is-danger",
              hasIcon: true,
              icon: "times-circle",
              iconPack: "fa",
              ariaRole: "alertdialog",
              ariaModal: true
            });
            console.log("error: ", e);
          });
        this.$buefy.toast.open("Usuario eliminado!");
      }
    });
  }

  created() {
    this.isLoading = true;
    this.userService
      .getUsers()
      .then(response => {
        this.users = response as User[];
        this.isLoading = false;
      })
      .catch(e => {
        this.isLoading = false;
        this.$buefy.dialog.alert({
          title: "Error",
          message:
            "Un error inesperado ha ocurrido. Por favor inténtelo nuevamente.",
          type: "is-danger",
          hasIcon: true,
          icon: "times-circle",
          iconPack: "fa",
          ariaRole: "alertdialog",
          ariaModal: true
        });
        console.log("error: ", e);
      });
  }
}
</script>

<style>
.clickeable {
  cursor: pointer;
}

.filtersClass {
  margin: 0em !important;
  padding: 1em;
  background-color: #e0eaff !important;
}

table {
  font-size: 13px;
  border: 1px solid rgb(192, 192, 192) !important;
  /* margin-left: 10px; */
}

th {
  /* background-color: #dbdbdb; */
  background-color: rgb(229, 229, 229);
  border: 1px solid rgb(192, 192, 192) !important;
}

td {
  border: 1px solid rgb(192, 192, 192) !important;
}

.ml-1 {
  margin-left: 1em;
}

select {
  min-width: 120px;
}

.filterButton {
  float: right;
}

.filtersClass select,
.filtersClass input {
  width: 180px;
}

select {
  min-width: 90px;
}

input {
  min-width: 80px;
}

.fieldWidth {
  width: 80px;
}
</style>
