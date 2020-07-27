<template>
  <div>
    <div class="card  column is-4 is-offset-4">
      <div class="card-content">
        <div class="media">
          <div class="media-content">
            <p class="title is-4">
              {{ vendor.id ? "Editar Proveedor" : "Crear Proveedor" }}
            </p>
          </div>
        </div>

        <div class="content">
          <section>
            <b-field label="Nombre:">
              <b-input
                v-model="vendor.name"
                placeholder="Ingrese el nombre del proveedor"
                required
                validation-message="Debe ingresar un nombre"
              ></b-input>
            </b-field>

            <b-field label="CUIL:">
              <b-input
                v-model="vendor.cuil"
                placeholder="Ingrese el cuil del proveedor"
                required
                validation-message="Debe ingresar un cuil"
              ></b-input>
            </b-field>

            <b-field label="Teléfono">
              <b-input
                v-model="vendor.phone"
                placeholder="Ingrese el teléfono del proveedor"
                required
                validation-message="Debe ingresar un teléfono"
              ></b-input>
            </b-field>

            <b-field label="Mail">
              <b-input
                v-model="vendor.mail"
                placeholder="Ingrese el mail del proveedor"
                required
                type="email"
                validation-message="Debe ingresar un mail válido"
              ></b-input>
            </b-field>

            <b-field label="Descripción">
              <b-input
                v-model="vendor.description"
                placeholder="Ingrese la descripción del proveedor"
                required
                validation-message="Debe ingresar una descripción"
                type="textarea"
              ></b-input>
            </b-field>

            <div class="field">
              <b-switch v-model="vendor.active">
                Activo
              </b-switch>
            </div>

            <b-button
              type="submit"
              class="is-success mr-1"
              :disabled="!formValid()"
              @click="submit"
            >
              {{ vendor.id ? "Editar" : "Crear" }}
            </b-button>
            <b-button
              type="button"
              class="is-danger"
              @click="$router.push('/vendor-list')"
            >
              Cancelar
            </b-button>
          </section>
        </div>
      </div>
    </div>

    <b-loading is-full-page :active.sync="isLoading"></b-loading>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { NavigatorVendorService } from "../../services/vendor-service";
import { Vendor } from "../../models/vendor";
import {
  fieldStateValidation,
  formValidation
} from "../../utils/common-functions";

@Component
export default class EditVendor extends Vue {
  public errorDialog = false;
  public vendor: Vendor = new Vendor();
  public vendorService: NavigatorVendorService = new NavigatorVendorService();
  public isLoading = false;

  fieldState(field: unknown) {
    return fieldStateValidation(field);
  }

  formValid() {
    const result = formValidation(this.vendor as never);
    return result === "";
  }

  public submit() {
    if (!this.vendor.id) {
      this.newVendor();
    } else {
      this.updateVendor();
    }
  }

  public newVendor() {
    this.isLoading = true;
    this.vendorService
      .addVendor(this.vendor)
      .then(() => {
        this.isLoading = false;
        this.$router.push({ name: "VendorList" });
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
        this.$router.push({ name: "VendorList" });
      });
  }

  public updateVendor() {
    this.isLoading = true;
    this.vendorService
      .updateVendor(this.vendor)
      .then(() => {
        this.isLoading = false;
        this.$router.push({ name: "VendorList" });
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
        this.$router.push({ name: "VendorList" });
      });
  }

  created() {
    this.vendor.id = this.$route.params.id;
    if (this.vendor.id) {
      this.isLoading = true;
      this.vendorService.getVendor(this.vendor.id).then(
        response => {
          this.vendor = response as Vendor;
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
</style>
