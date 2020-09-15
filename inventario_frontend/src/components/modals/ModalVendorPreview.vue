<template>
  <div>
    <form>
      <div class="modal-card" style="width: auto">
        <header class="modal-card-head">
          <p class="modal-card-title">Información del proveedor</p>
        </header>
        <section class="modal-card-body">
          <div>
            <label>Nombre: </label>
            <span>{{ vendor.name }}</span>
          </div>
          <div>
            <label>CUIL: </label>
            <span>{{ vendor.cuil }}</span>
          </div>
          <div>
            <label>Teléfono: </label>
            <span>{{ vendor.phone }}</span>
          </div>
          <div>
            <label>Mail: </label>
            <span>{{ vendor.mail }}</span>
          </div>
          <div>
            <label>Descripción: </label>
            <span>{{ vendor.description }}</span>
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
import { Vendor } from "@/models/vendor";
import { NavigatorVendorService } from "@/services/vendor-service";

@Component
export default class ModalVendorPreview extends Vue {
  public vendor: Vendor = new Vendor();
  public vendorService: NavigatorVendorService = new NavigatorVendorService();
  public isLoading = false;

  @Prop({ required: true })
  public vendorId!: string;

  created() {
    this.isLoading = true;
    this.vendorService
      .getVendor(this.vendorId)
      .then(response => {
        this.isLoading = false;
        this.vendor = response as Vendor;
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
