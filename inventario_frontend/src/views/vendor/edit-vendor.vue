<template>
  <div>
    <b-card :title="vendor.id ? 'Editar Proveedor' : 'Crear Proveedor'">
      <!-- <v-card-title>{{ id ? "Update" : "Create" }} Vendor</v-card-title> -->
      <b-form @submit.prevent="submit">
        <b-form-group id="input-group-1" label="Nombre:" label-for="name-input">
          <b-form-input
            id="name-input"
            v-model="vendor.name"
            :state="fieldState(vendor.name)"
            type="text"
            required
            placeholder="Ingrese el nombre del proveedor"
          ></b-form-input>
          <b-form-invalid-feedback :state="fieldState(vendor.name)">
            El nombre es requerido
          </b-form-invalid-feedback>
        </b-form-group>

        <b-form-group id="input-group-2" label="CUIL:" label-for="cuil-input">
          <b-form-input
            id="cuil-input"
            v-model="vendor.cuil"
            :state="fieldState(vendor.cuil)"
            type="text"
            required
            placeholder="Ingrese el CUIL del proveedor"
          ></b-form-input>
          <b-form-invalid-feedback :state="fieldState(vendor.cuil)">
            El CUIL es requerido
          </b-form-invalid-feedback>
        </b-form-group>

        <b-form-group
          id="input-group-1"
          label="Teléfono:"
          label-for="phone-input"
        >
          <b-form-input
            id="phone-input"
            v-model="vendor.phone"
            :state="fieldState(vendor.phone)"
            type="text"
            required
            placeholder="Ingrese el teléfono del proveedor"
          ></b-form-input>
          <b-form-invalid-feedback :state="fieldState(vendor.phone)">
            El teléfono es requerido
          </b-form-invalid-feedback>
        </b-form-group>

        <b-form-group id="input-group-2" label="Mail:" label-for="mail-input">
          <b-form-input
            id="mail-input"
            v-model="vendor.mail"
            :state="fieldState(vendor.mail)"
            type="text"
            required
            placeholder="Ingrese el mail del proveedor"
          ></b-form-input>
          <b-form-invalid-feedback :state="fieldState(vendor.mail)">
            El mail es requerido
          </b-form-invalid-feedback>
        </b-form-group>

        <b-form-group
          id="input-group-2"
          label="Descripción:"
          label-for="description-input"
        >
          <b-form-input
            id="description-input"
            v-model="vendor.description"
            :state="fieldState(vendor.description)"
            type="text"
            required
            placeholder="Ingrese la descripción del proveedor"
          ></b-form-input>
          <b-form-invalid-feedback :state="fieldState(vendor.description)">
            La descripción es requerida
          </b-form-invalid-feedback>
        </b-form-group>

        <b-form-group id="input-group-1" label="" label-for="active-input">
          <b-form-checkbox id="active-input" v-model="vendor.active" switch>
            Activo
          </b-form-checkbox>
        </b-form-group>

        <b-button type="submit" variant="primary" :disabled="!formValid()">
          {{ vendor.id ? "Editar" : "Crear" }}
        </b-button>
        <b-button
          type="button"
          variant="danger"
          @click="$router.push('/vendor-list')"
        >
          Cancelar
        </b-button>
      </b-form>
    </b-card>

    <!-- <ErrorDialog
      :error="errorMsg"
      v-if="errorDialog"
      @close:dialog="errorDialog = $event"
    /> -->
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { NavigatorVendorService } from "../../services/vendor-service";
import { Vendor } from "../../models/vendor";
// import { DialogMsg } from "../../models/dialogMsg";
// import ErrorDialog from "../../components/dialogs/error-dialog.vue";

@Component({
  components: {
    // ErrorDialog
  }
})
export default class EditVendor extends Vue {
  public errorDialog = false;
  //   public errorMsg = new DialogMsg();
  public vendor: Vendor = new Vendor();
  public vendorService: NavigatorVendorService = new NavigatorVendorService();

  fieldState(field: any) {
    return field && field.length > 0 ? true : false;
  }

  formValid() {
    let flag = true;
    Object.keys(this.vendor as Vendor).forEach(key => {
      switch (typeof this.vendor[key as keyof Vendor]) {
        case "string":
          if ((this.vendor[key as keyof Vendor] as string).length <= 0) {
            flag = false;
          }
          break;
        case "object":
          // console.log(key);
          flag = false;
          break;
        case "number":
          break;
        case "bigint":
          break;
        case "boolean":
          break;
        case "symbol":
          break;
        case "undefined":
          break;
        case "function":
          break;
        default:
          break;
      }
    });
    return flag;
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

<style></style>
