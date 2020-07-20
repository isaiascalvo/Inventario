<template>
  <div>
    <div class="card">
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
            <b-field
              label="Nombre:"
              :type="fieldState(vendor.name) ? 'is-success' : 'is-danger'"
              :message="
                fieldState(vendor.name) ? '' : 'Debe ingresar un nombre'
              "
            >
              <b-input
                v-model="vendor.name"
                placeholder="Ingrese el nombre del proveedor"
              ></b-input>
            </b-field>

            <b-field
              label="CUIL:"
              :type="fieldState(vendor.cuil) ? 'is-success' : 'is-danger'"
              :message="fieldState(vendor.cuil) ? '' : 'Debe ingresar un cuil'"
            >
              <b-input
                v-model="vendor.cuil"
                placeholder="Ingrese el cuil del proveedor"
              ></b-input>
            </b-field>

            <b-field
              label="Teléfono"
              :type="fieldState(vendor.phone) ? 'is-success' : 'is-danger'"
              :message="
                fieldState(vendor.phone) ? '' : 'Debe ingresar un teléfono'
              "
            >
              <b-input
                v-model="vendor.phone"
                placeholder="Ingrese el teléfono del proveedor"
              ></b-input>
            </b-field>

            <b-field
              label="Mail"
              :type="fieldState(vendor.mail) ? 'is-success' : 'is-danger'"
              :message="fieldState(vendor.mail) ? '' : 'Debe ingresar un mail'"
            >
              <b-input
                v-model="vendor.mail"
                placeholder="Ingrese el mail del proveedor"
              ></b-input>
            </b-field>

            <b-field
              label="Descripción"
              :type="
                fieldState(vendor.description) ? 'is-success' : 'is-danger'
              "
              :message="
                fieldState(vendor.description)
                  ? ''
                  : 'Debe ingresar una descripción'
              "
            >
              <b-input
                v-model="vendor.description"
                placeholder="Ingrese la descripción del proveedor"
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
  //   public errorMsg = new DialogMsg();
  public vendor: Vendor = new Vendor();
  public vendorService: NavigatorVendorService = new NavigatorVendorService();

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
    this.vendorService
      .addVendor(this.vendor)
      .then(() => {
        this.$router.push({ name: "VendorList" });
      })
      .catch(e => {
        // this.errorMsg = {
        //   title: "Error",
        //   msg: "An unexpected error has occurred. please try again later."
        // };
        // this.errorDialog = true;
        console.log("error: ", e);
        this.$router.push({ name: "VendorList" });
      });
  }

  public updateVendor() {
    this.vendorService
      .updateVendor(this.vendor)
      .then(() => {
        this.$router.push({ name: "VendorList" });
      })
      .catch(e => {
        // this.errorMsg = {
        //   title: "Error",
        //   msg: "An unexpected error has occurred. please try again later."
        // };
        // this.errorDialog = true;
        console.log("error: ", e);
        // this.$router.push({ name: "VendorList" });
      });
  }

  created() {
    this.vendor.id = this.$route.params.id;
    if (this.vendor.id) {
      this.vendorService.getVendor(this.vendor.id).then(response => {
        this.vendor = response as Vendor;
      });
    }
  }
}
</script>

<style>
.mr-1 {
  margin-right: 1em;
}
</style>
