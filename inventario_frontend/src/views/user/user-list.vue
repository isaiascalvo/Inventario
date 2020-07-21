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
        "Estás seguro que deseas <b>eliminar</b> el usuario? Esta acción no podrá dehacerse.",
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
            // this.errorMsg = {
            //   title: "Error",
            //   msg: "An unexpected error has occurred. please try again later."
            // };
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
        // this.errorMsg = {
        //   title: "Error",
        //   msg: "An unexpected error has occurred. please try again later."
        // };
        // this.errorDialog = true;
        console.log("error: ", e);
      });
  }
}
</script>

<style>
.clickeable {
  cursor: pointer;
}

th {
  /* background-color: #dbdbdb; */
  background-color: #384caf4a;
}

.ml-1 {
  margin-left: 1em;
}

.actionButton {
  margin-left: 5px;
}

table {
  font-size: 13px;
  border: 0px !important;
}
</style>
