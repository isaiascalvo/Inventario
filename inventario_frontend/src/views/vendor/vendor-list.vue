<template>
  <div>
    <section class="hero is-light">
      <div class="hero-head">
        <div class="container level">
          <div>
            <h1 class="title">Proveedores</h1>
            <h2 class="subtitle">
              Lista de proveedores
            </h2>
          </div>
          <div>
            <b-button
              type="is-info"
              tag="router-link"
              to="/vendor/new"
              class="mx-1"
            >
              Nuevo Proveedor
            </b-button>
          </div>
        </div>
      </div>
    </section>

    <b-table
      striped
      hoverable
      :data="vendors"
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
        <b-table-column field="cuil" label="CUIL">
          {{ props.row.cuil }}
        </b-table-column>
        <b-table-column field="phone" label="Teléfono">
          {{ props.row.phone }}
        </b-table-column>
        <b-table-column field="mail" label="Mail">
          {{ props.row.mail }}
        </b-table-column>
        <b-table-column field="description" label="Descripción">
          {{ props.row.description }}
        </b-table-column>
        <b-table-column field="active" label="Activo">
          {{ props.row.active ? "Si" : "No" }}
        </b-table-column>

        <b-table-column field="action" label="Acciones">
          <b-button tag="router-link" :to="'/vendor/modify/' + props.row.id">
            <b-icon icon="pencil"></b-icon>
          </b-button>
          <b-button @click="deleteVendor(props.row)">
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
import { Vendor } from "../../models/vendor";
import { NavigatorVendorService } from "../../services/vendor-service";

@Component
export default class VendorList extends Vue {
  public vendors: Vendor[] = [];
  public vendorService: NavigatorVendorService = new NavigatorVendorService();

  public currentPage = 1;
  public perPage = 10;
  public errorDialog = false;
  public confirmDialog = false;
  public isLoading = true;

  deleteVendor(vendor: Vendor) {
    this.$buefy.dialog.confirm({
      title: "Eliminando Proveedor",
      message:
        "Estás seguro que deseas <b>eliminar</b> el proveedor? Esta acción no podrá dehacerse.",
      confirmText: "Eliminar Proveedor",
      cancelText: "Cancelar",
      type: "is-danger",
      hasIcon: true,
      onConfirm: () => {
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
        this.$buefy.toast.open("Proveedor eliminado!");
      }
    });
  }

  created() {
    this.isLoading = true;
    this.vendorService
      .getVendors()
      .then(response => {
        this.vendors = response as Vendor[];
        this.isLoading = false;
      })
      .catch(e => {
        this.isLoading = false;
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

table {
  font-size: 13px;
  border: 0px !important;
}
</style>
