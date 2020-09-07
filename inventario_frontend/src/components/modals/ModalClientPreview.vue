<template>
  <div>
    <form>
      <div class="modal-card" style="width: auto">
        <header class="modal-card-head">
          <p class="modal-card-title">Información del cliente</p>
        </header>
        <section class="modal-card-body">
          <div>
            <label>Nombre: </label>
            <span>{{ client.name }}</span>
          </div>
          <div>
            <label>Apellido: </label>
            <span>{{ client.lastname }}</span>
          </div>
          <div>
            <label>Dni: </label>
            <span>{{ client.dni }}</span>
          </div>
          <div>
            <label>Teléfono: </label>
            <span>{{ client.phone }}</span>
          </div>
          <div>
            <label>Mail: </label>
            <span>{{ client.mail }}</span>
          </div>
          <div>
            <label>Deudor: </label>
            <span>{{ client.debtor ? "Si" : "No" }}</span>
          </div>
          <div>
            <label>Fecha de cumpleaños: </label>
            <span>{{ dateToLocal(client.birthdate) }}</span>
          </div>
        </section>
        <footer class="modal-card-foot">
          <div class="buttons">
            <b-button class="button" @click="$parent.close()">
              Cerrar
            </b-button>
          </div>
        </footer>
      </div>
    </form>
    <b-loading is-full-page :active.sync="isLoading"></b-loading>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from "vue-property-decorator";
import { Client } from "@/models/client";
import { NavigatorClientService } from "@/services/client-service";
import { dateTimeToLocal } from "@/utils/common-functions";

@Component
export default class ModalClientPreview extends Vue {
  public client: Client = new Client();
  public clientService: NavigatorClientService = new NavigatorClientService();
  public isLoading = false;

  @Prop({ required: true })
  public clientId!: string;

  dateToLocal(date: Date) {
    return dateTimeToLocal(date).substring(0, 10);
  }

  created() {
    this.isLoading = true;
    this.clientService
      .getClient(this.clientId)
      .then(response => {
        this.isLoading = false;
        this.client = response as Client;
      })
      .catch(error => {
        this.isLoading = false;
        console.log(error);
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
      });
  }
}
</script>

<style scoped>
label {
  font-weight: bold;
}

span {
  margin-left: 5px;
  float: right;
}
</style>
