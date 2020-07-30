<template>
  <div>
    <section class="hero is-light">
      <div class="hero-head">
        <div class="container level">
          <div>
            <h1 class="title">Clientes</h1>
            <h2 class="subtitle">
              Lista de clientes
            </h2>
          </div>
          <div>
            <b-button
              type="is-info"
              tag="router-link"
              to="/client/new"
              class="mx-1"
            >
              Nuevo Cliente
            </b-button>
          </div>
        </div>
      </div>
    </section>

    <b-table
      striped
      hoverable
      :data="clients"
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
        <b-table-column field="birthdate" label="Fecha de Nacimiento">
          {{ dateToISO(props.row.birthdate) }}
        </b-table-column>
        <b-table-column field="active" label="Activo">
          {{ props.row.active ? "Si" : "No" }}
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

    <b-loading is-full-page :active.sync="isLoading"></b-loading>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { Client } from "../../models/client";
import { NavigatorClientService } from "../../services/client-service";

@Component
export default class ClientList extends Vue {
  public clients: Client[] = [];
  public clientService: NavigatorClientService = new NavigatorClientService();

  public currentPage = 1;
  public perPage = 10;
  public errorDialog = false;
  public confirmDialog = false;
  public isLoading = false;

  dateToISO(date: Date) {
    return new Date(date).toISOString().substr(0, 10);
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

  created() {
    this.isLoading = true;
    this.clientService
      .getClients()
      .then(response => {
        this.clients = response as Client[];
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
