<template>
  <div>
    <div class="card  column is-4 is-offset-4">
      <div class="card-content">
        <div class="media">
          <div class="media-content">
            <p class="title is-4">
              {{ client.id ? "Editar Cliente" : "Crear Cliente" }}
            </p>
          </div>
        </div>

        <div class="content">
          <form @submit.prevent="submit()" class="flex-text-left">
            <b-field label="Nombre:">
              <b-input
                v-model="client.name"
                placeholder="Ingrese el nombre del cliente"
                required
                validation-message="Debe ingresar un nombre"
              ></b-input>
            </b-field>

            <b-field label="Apellido:">
              <b-input
                v-model="client.lastname"
                placeholder="Ingrese el apellido del cliente"
                required
                validation-message="Debe apellido un nombre"
              ></b-input>
            </b-field>

            <b-field label="DNI:">
              <b-input
                v-model="client.dni"
                placeholder="Ingrese el dni del cliente"
                required
                validation-message="Debe ingresar un dni"
              ></b-input>
            </b-field>

            <b-field label="Teléfono">
              <b-input
                v-model="client.phone"
                placeholder="Ingrese el teléfono del cliente"
                required
                validation-message="Debe ingresar un teléfono"
              ></b-input>
            </b-field>

            <b-field label="Mail">
              <b-input
                v-model="client.mail"
                placeholder="Ingrese el mail del cliente"
                type="email"
              ></b-input>
            </b-field>

            <b-field label="Fecha de nacimiento">
              <b-datepicker
                v-model="client.birthdate"
                placeholder="Seleccione una fecha"
                icon="calendar-today"
                trap-focus
                :day-names="dayNames"
                :month-names="monthNames"
                position="is-top-right"
                editable
                :date-parser="parseDate"
                required
                validation-message="Debe ingresar una fecha"
              >
              </b-datepicker>
            </b-field>

            <div class="field">
              <b-switch v-model="client.debtor">
                Deudor
              </b-switch>
            </div>

            <b-button
              native-type="submit"
              class="is-success mr-1"
              :disabled="!formValid()"
            >
              {{ client.id ? "Editar" : "Crear" }}
            </b-button>
            <b-button
              type="button"
              class="is-danger"
              @click="$router.push('/client-list')"
            >
              Cancelar
            </b-button>
          </form>
        </div>
      </div>
    </div>

    <b-loading is-full-page :active.sync="isLoading"></b-loading>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { NavigatorClientService } from "../../services/client-service";
import { Client } from "../../models/client";
import {
  fieldStateValidation,
  formValidation
} from "../../utils/common-functions";

@Component
export default class EditClient extends Vue {
  public errorDialog = false;
  public client: Client = new Client();
  public clientService: NavigatorClientService = new NavigatorClientService();
  public isLoading = false;
  public monthNames = [
    "Enero",
    "Febrero",
    "Marzo",
    "Abril",
    "Mayo",
    "Junio",
    "Julio",
    "Agosto",
    "Septiembre",
    "Octubre",
    "Noviembre",
    "Diceimbre"
  ];
  public dayNames = ["Dom", "Lun", "Mar", "Mie", "Jue", "Vie", "Sab"];

  fieldState(field: unknown) {
    return fieldStateValidation(field);
  }

  formValid() {
    const result = formValidation(this.client as never);
    console.log(result);
    return result === "";
  }

  parseDate(date: string) {
    const dateSplited = date.split("/");
    return new Date(
      Date.parse(dateSplited[1] + "/" + dateSplited[0] + "/" + dateSplited[2])
    );
  }

  public submit() {
    if (!this.client.id) {
      this.newClient();
    } else {
      this.updateClient();
    }
  }

  public newClient() {
    this.isLoading = true;
    this.clientService
      .addClient(this.client)
      .then(() => {
        this.isLoading = false;
        this.$router.push({ name: "ClientList" });
      })
      .catch(e => {
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
        this.isLoading = false;
        console.log("error: ", e);
        this.$router.push({ name: "ClientList" });
      });
  }

  public updateClient() {
    this.isLoading = true;
    this.clientService
      .updateClient(this.client)
      .then(() => {
        this.isLoading = false;
        this.$router.push({ name: "ClientList" });
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
        this.$router.push({ name: "ClientList" });
      });
  }

  created() {
    this.client.id = this.$route.params.id;
    if (this.client.id) {
      this.isLoading = true;
      this.clientService.getClient(this.client.id).then(
        response => {
          this.client = response as Client;
          this.isLoading = false;
        },
        error => {
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
          console.log(error);
          this.isLoading = true;
        }
      );
    }
  }
}
</script>

<style>
.mr-1 {
  margin-right: 1em;
}
.flex-text-left {
  display: flow-root;
  text-align: left;
}
</style>
