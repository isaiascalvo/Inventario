<template>
  <div>
    <section class="hero is-light p-1">
      <div class="hero-head">
        <div class="level">
          <div>
            <h1 class="title is-6">Lista de Usuarios</h1>
          </div>
          <div>
            <b-button
              type="is-info"
              size="is-small"
              tag="router-link"
              to="/user/new"
              class="mx-1"
            >
              Nuevo Usuario
            </b-button>
            <b-button
              @click="openFilters = !openFilters"
              class="is-pulled-right"
              type="is-primary"
              size="is-small"
              :icon-right="
                openFilters ? 'filter-variant-minus' : 'filter-variant'
              "
            >
              {{ openFilters ? "Ocultar Filtros" : "Mostrar Filtros" }}
            </b-button>
          </div>
        </div>
      </div>
    </section>

    <div class="columns filtersClass level" v-if="openFilters">
      <div class="column is-10">
        <b-field grouped group-multiline>
          <b-field label-position="on-border" label="Nombre de usuario">
            <b-input
              v-model="userFilters.username"
              placeholder="Nombre de usuario"
              size="is-small"
              icon-right="close-circle"
              icon-right-clickable
              @icon-right-click="userFilters.username = undefined"
            ></b-input>
          </b-field>

          <b-field label-position="on-border" label="Nombre">
            <b-input
              v-model="userFilters.name"
              placeholder="Nombre"
              size="is-small"
              icon-right="close-circle"
              icon-right-clickable
              @icon-right-click="userFilters.name = undefined"
            ></b-input>
          </b-field>

          <b-field label-position="on-border" label="Apellido">
            <b-input
              v-model="userFilters.lastname"
              placeholder="Apellido"
              size="is-small"
              icon-right="close-circle"
              icon-right-clickable
              @icon-right-click="userFilters.lastname = undefined"
            ></b-input>
          </b-field>

          <b-field label-position="on-border" label="DNI">
            <b-input
              v-model="userFilters.dni"
              placeholder="DNI"
              size="is-small"
              icon-right="close-circle"
              icon-right-clickable
              @icon-right-click="userFilters.dni = undefined"
            ></b-input>
          </b-field>

          <b-field label-position="on-border" label="Mail">
            <b-input
              v-model="userFilters.mail"
              placeholder="Mail"
              size="is-small"
              icon-right="close-circle"
              icon-right-clickable
              @icon-right-click="userFilters.mail = undefined"
            ></b-input>
          </b-field>

          <b-field label-position="on-border" label="Teléfono">
            <b-input
              v-model="userFilters.phone"
              placeholder="Teléfono"
              size="is-small"
              icon-right="close-circle"
              icon-right-clickable
              @icon-right-click="userFilters.phone = undefined"
            ></b-input>
          </b-field>

          <b-field label-position="on-border" label="Tipo de Usuario">
            <b-select
              v-model="userFilters.isAdmin"
              placeholder="Seleccione una opción"
              size="is-small"
            >
              <option :value="null"></option>
              <option :value="true">Administrador</option>
              <option :value="false">Común</option>
            </b-select>
          </b-field>

          <b-field label-position="on-border" label="Estado del cliente">
            <b-select
              v-model="userFilters.active"
              placeholder="Seleccione una opción"
              size="is-small"
            >
              <option :value="null"></option>
              <option :value="true">Activo</option>
              <option :value="false">No activo</option>
            </b-select>
          </b-field>
        </b-field>
      </div>

      <div class="column level-right">
        <b-button
          type="is-dark"
          class="mx-1"
          size="is-small"
          @click="applyFilters()"
        >
          Aplicar
        </b-button>
        <b-button @click="clearFilters()" size="is-small" type="is-default">
          Limpiar
        </b-button>
      </div>
    </div>

    <b-table
      striped
      hoverable
      scrollable
      :data="users"
      id="my-table"
      :paginated="true"
      backend-pagination
      :total="totalPages"
      :per-page="perPage"
      :current-page.sync="currentPage"
      @page-change="onPageChange"
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
import { UserFilters } from "../../models/filters/userFilters";
import { NavigatorUserService } from "../../services/user-service";

@Component
export default class UserList extends Vue {
  public users: User[] = [];
  public userService: NavigatorUserService = new NavigatorUserService();

  public currentPage = 1;
  public perPage = 10;
  public isLoading = false;
  public openFilters = false;
  public totalPages = 0;
  public filtersApplied = false;
  public userFilters: UserFilters = new UserFilters();

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

  public clearFilters() {
    this.filtersApplied = false;
    this.userFilters = new UserFilters();
    this.getVendors();
  }

  public applyFilters() {
    this.isLoading = true;
    this.filtersApplied = true;
    this.getVendors();
  }

  getVendors() {
    this.isLoading = true;
    let rta: Promise<void>;
    if (this.filtersApplied) {
      rta = this.userService
        .getTotalQtyByFilters(this.userFilters)
        .then(qty => {
          this.totalPages = qty;
          return this.userService.getByFiltersPageAndQty(
            this.userFilters,
            (this.currentPage - 1) * this.perPage,
            this.perPage
          );
        })
        .then(response => {
          this.users = response;
          this.isLoading = false;
        });
    } else {
      rta = this.userService
        .getTotalQty()
        .then(qty => {
          this.totalPages = qty;
          return this.userService.getByPageAndQty(
            (this.currentPage - 1) * this.perPage,
            this.perPage
          );
        })
        .then(response => {
          this.users = response;
          this.isLoading = false;
        });
    }

    rta.catch(e => {
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

  onPageChange(page: number) {
    this.currentPage = page;
    this.getVendors();
  }

  created() {
    this.getVendors();
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

.p-1 {
  padding: 1em;
}

.actionButton {
  margin-left: 5px;
}
</style>
