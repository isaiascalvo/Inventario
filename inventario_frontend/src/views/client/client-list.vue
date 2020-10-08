<template>
  <div>
    <section class="hero is-light p-1">
      <div class="hero-head">
        <div class="level">
          <div>
            <h1 class="title is-6">Clientes</h1>
          </div>
          <div>
            <b-button
              type="is-info"
              size="is-small"
              tag="router-link"
              to="/client/new"
              class="mx-1"
            >
              Nuevo Cliente
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

    <div>
      <div class="columns filtersClass level" v-if="openFilters">
        <div class="column is-10">
          <b-field grouped group-multiline>
            <b-field label-position="on-border" label="Apellido">
              <b-input
                v-model="clientFilters.lastname"
                placeholder="Apellido"
                size="is-small"
                icon-right="close-circle"
                icon-right-clickable
                @icon-right-click="clearIconClick('lastname')"
              ></b-input>
            </b-field>

            <b-field label-position="on-border" label="Nombre">
              <b-input
                v-model="clientFilters.name"
                placeholder="Nombre"
                size="is-small"
                icon-right="close-circle"
                icon-right-clickable
                @icon-right-click="clearIconClick('name')"
              ></b-input>
            </b-field>

            <b-field label-position="on-border" label="DNI">
              <b-input
                v-model="clientFilters.dni"
                placeholder="DNI"
                size="is-small"
                icon-right="close-circle"
                icon-right-clickable
                @icon-right-click="clearIconClick('dni')"
              ></b-input>
            </b-field>

            <b-field label-position="on-border" label="Teléfono">
              <b-input
                v-model="clientFilters.phone"
                placeholder="Teléfono"
                size="is-small"
                icon-right="close-circle"
                icon-right-clickable
                @icon-right-click="clearIconClick('phone')"
              ></b-input>
            </b-field>

            <b-field label-position="on-border" label="Mail">
              <b-input
                v-model="clientFilters.mail"
                placeholder="Mail"
                size="is-small"
                icon-right="close-circle"
                icon-right-clickable
                @icon-right-click="clearIconClick('mail')"
              ></b-input>
            </b-field>

            <b-field label-position="on-border" label="Tipo de cliente">
              <b-select
                v-model="clientFilters.debtor"
                placeholder="Seleccione una opción"
                size="is-small"
              >
                <option :value="null"></option>
                <option :value="true">Deudor</option>
                <option :value="false">No deudor</option>
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
        :data="clients"
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
          No hay clientes registrados
        </template>
        <template slot-scope="props">
          <b-table-column field="lastname" label="Apellido">
            {{ props.row.lastname }}
          </b-table-column>
          <b-table-column field="name" label="Nombre">
            {{ props.row.name }}
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
          <b-table-column field="birthdate" label="Fecha de Nacimiento">
            {{ dateTimeToLocal(props.row.birthdate) }}
          </b-table-column>
          <b-table-column field="debtor" label="Deudor">
            {{ props.row.debtor ? "Si" : "No" }}
          </b-table-column>

          <b-table-column field="action" label="Acciones">
            <b-button
              tag="router-link"
              :to="'/client/modify/' + props.row.id"
              type="is-small"
            >
              <b-icon icon="pencil"></b-icon>
            </b-button>
            <b-button
              @click="deleteClient(props.row)"
              type="is-small"
              class="actionButton"
            >
              <b-icon icon="delete"></b-icon>
            </b-button>
          </b-table-column>
        </template>
      </b-table>
    </div>

    <b-loading is-full-page :active.sync="isLoading"></b-loading>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { Client } from "../../models/client";
import { NavigatorClientService } from "../../services/client-service";
import { ClientFilters } from "../../models/filters/clientFilters";
import { dateTimeToLocal } from "@/utils/common-functions";

@Component
export default class ClientList extends Vue {
  public clients: Client[] = [];
  public clientService: NavigatorClientService = new NavigatorClientService();
  public clientFilters: ClientFilters = new ClientFilters();
  public openFilters = false;
  public currentPage = 1;
  public perPage = 10;
  public errorDialog = false;
  public confirmDialog = false;
  public isLoading = false;
  public filtersApplied = false;
  public totalPages = 0;

  dateTimeToLocal(date: Date) {
    return dateTimeToLocal(date);
  }

  clearIconClick(key: keyof ClientFilters) {
    this.clientFilters[key] = undefined;
  }

  deleteClient(client: Client) {
    this.$buefy.dialog.confirm({
      title: "Eliminando Cliente",
      message:
        "Estás seguro que deseas <b>eliminar</b> el cliente? Esta acción no podrá deshacerse.",
      confirmText: "Eliminar Cliente",
      cancelText: "Cancelar",
      type: "is-danger",
      hasIcon: true,
      onConfirm: () => {
        this.isLoading = true;
        this.clientService
          .deleteClient(client.id)
          .then(() => {
            this.isLoading = false;
            this.clients.splice(this.clients.indexOf(client), 1);
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
        this.$buefy.toast.open("Cliente eliminado!");
      }
    });
  }

  public clearFilters() {
    this.filtersApplied = false;
    this.clientFilters = new ClientFilters();
    this.getClients();
  }

  public applyFilters() {
    this.isLoading = true;
    this.filtersApplied = true;
    this.getClients();
  }

  getClients() {
    this.isLoading = true;
    let rta: Promise<void>;
    if (this.filtersApplied) {
      rta = this.clientService
        .getTotalQtyByFilters(this.clientFilters)
        .then(qty => {
          this.totalPages = qty;
          return this.clientService.getByFiltersPageAndQty(
            this.clientFilters,
            (this.currentPage - 1) * this.perPage,
            this.perPage
          );
        })
        .then(response => {
          this.clients = response;
          this.isLoading = false;
        });
    } else {
      rta = this.clientService
        .getTotalQty()
        .then(qty => {
          this.totalPages = qty;
          return this.clientService.getByPageAndQty(
            (this.currentPage - 1) * this.perPage,
            this.perPage
          );
        })
        .then(response => {
          this.clients = response;
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
    this.getClients();
  }

  created() {
    this.getClients();
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
