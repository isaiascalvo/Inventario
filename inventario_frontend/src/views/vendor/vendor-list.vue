<template>
  <b-card title="Proveedores">
    <b-button class="mb-2" to="/vendor/new">
      Nuevo Proveedor
    </b-button>
    <b-table
      striped
      hover
      :items="vendors"
      :fields="headers"
      id="my-table"
      :per-page="perPage"
      :current-page="currentPage"
    >
      <template v-slot:cell(active)="data">
        {{ data.value ? "Si" : "No" }}
      </template>

      <template v-slot:cell(action)="data">
        <b-icon
          class="clickeable mr-2"
          icon="pencil"
          @click="$router.push('/vendor/modify/' + data.item.id)"
        ></b-icon>
        <b-icon
          class="clickeable"
          icon="trash"
          @click="openDeleteDialog(item.id)"
        ></b-icon>
      </template>
    </b-table>

    <b-pagination
      v-model="currentPage"
      :total-rows="vendors.length"
      :per-page="perPage"
      aria-controls="my-table"
      align="center"
    ></b-pagination>

    <!-- <ErrorDialog
      :error="errorMsg"
      v-if="errorDialog"
      @close:dialog="errorDialog = $event"
    />

    <ConfirmDialog
      :msgDialog="confirmMsg"
      v-if="confirmDialog"
      @close:dialog="onClose"
    /> -->
  </b-card>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { Vendor } from "../../models/vendor";
import { NavigatorVendorService } from "../../services/vendor-service";
// import ConfirmDialog from "../../components/dialogs/confirm-dialog.vue";
// import { DialogMsg } from "../../models/dialogMsg";
// import ErrorDialog from "../../components/dialogs/error-dialog.vue";

@Component({
  components: {} //ConfirmDialog, ErrorDialog
})
export default class VendorList extends Vue {
  public vendors: Vendor[] = [];
  public vendorService: NavigatorVendorService = new NavigatorVendorService();

  public currentPage = 1;
  public perPage = 1;
  public errorDialog = false;
  public confirmDialog = false;
  // public errorMsg = new DialogMsg();
  // public confirmMsg = new DialogMsg();
  public headers = [
    { key: "name", label: "Nombre" },
    { key: "CUIL", label: "CUIL" },
    { key: "phone", label: "Teléfono" },
    { key: "mail", label: "Mail" },
    { key: "description", label: "Descripción" },
    { key: "active", label: "Activo" },
    { key: "action", label: "Acciones" }
  ];

  deleteVendor(vendor: Vendor) {
    this.vendorService
      .deleteVendor(vendor.id)
      .then(() => {
        this.vendors.splice(this.vendors.indexOf(vendor), 1);
      })
      .catch(e => {
        // this.errorMsg = {
        //   title: "Error",
        //   msg: "An unexpected error has occurred. please try again later."
        // };
        // this.errorDialog = true;
        console.log("error: ", e);
      });
  }

  onClose(elemId: string) {
    this.confirmDialog = false;
    if (elemId.trim() !== "") {
      const vendor = this.vendors.find(x => x.id === elemId);
      if (vendor) {
        this.deleteVendor(vendor);
      }
    }
  }

  openDeleteDialog(elemId: string) {
    // this.confirmMsg = {
    //   elemId: elemId,
    //   title: "Warning",
    //   msg: "Are you sure you want to delete the vendor?"
    // };
    // this.confirmDialog = true;
  }

  created() {
    this.vendorService
      .getVendors()
      .then(response => {
        this.vendors = response as Vendor[];
      })
      .catch(e => {
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
/* table,
table.bordered td,
table.bordered th {
  border: 1px black solid;
  margin: auto;
  margin-bottom: 10px;
}

.align-center {
  text-align: center;
} */

.clickeable {
  cursor: pointer;
}
</style>
