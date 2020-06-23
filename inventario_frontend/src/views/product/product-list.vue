<template>
  <b-card title="Productos">
    <b-button class="mb-2" to="/product/new">
      Nuevo Producto
    </b-button>
    <b-table
      striped
      hover
      :items="products"
      :fields="headers"
      id="my-table"
      :per-page="perPage"
      :current-page="currentPage"
    >
      <template v-slot:cell(action)="item">
        <b-icon
          class="clickeable  mr-2"
          icon="pencil"
          @click="$router.push('/product/modify/' + item.id)"
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
      :total-rows="products.length"
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
import { Product } from "../../models/product";
import { NavigatorProductService } from "../../services/product-service";
// import ConfirmDialog from "../../components/dialogs/confirm-dialog.vue";
// import { DialogMsg } from "../../models/dialogMsg";
// import ErrorDialog from "../../components/dialogs/error-dialog.vue";

@Component({
  components: {} //ConfirmDialog, ErrorDialog
})
export default class ProductList extends Vue {
  public products: Product[] = [];
  public productService: NavigatorProductService = new NavigatorProductService();

  public currentPage = 1;
  public perPage = 1;
  public errorDialog = false;
  public confirmDialog = false;
  // public errorMsg = new DialogMsg();
  // public confirmMsg = new DialogMsg();
  public headers = [
    { key: "name", label: "Nombre" },
    { key: "description", label: "Descripción" },
    { key: "code", label: "Código" },
    { key: "categoryId", label: "categoryId" },
    { key: "vendorId", label: "vendorId" },
    { key: "brand", label: "Marca" },
    { key: "available", label: "Disponible" },
    { key: "active", label: "Activo" },
    { key: "price", label: "Precio" },
    { key: "action", label: "Acciones" }
  ];

  deleteProduct(product: Product) {
    this.productService
      .deleteProduct(product.id)
      .then(() => {
        this.products.splice(this.products.indexOf(product), 1);
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
      const product = this.products.find(x => x.id === elemId);
      if (product) {
        this.deleteProduct(product);
      }
    }
  }

  openDeleteDialog(elemId: string) {
    // this.confirmMsg = {
    //   elemId: elemId,
    //   title: "Warning",
    //   msg: "Are you sure you want to delete the product?"
    // };
    // this.confirmDialog = true;
  }

  created() {
    this.productService
      .getProducts()
      .then(response => {
        this.products = response;
      })
      .catch(e => {
        // this.errorMsg = {
        //   title: "Error",
        //   msg: "An unexpected error has occurred. please try again later."
        // };
        // this.errorDialog = true;
        console.log("error: ", e);
      });

    // this.products = [
    //   {
    //     id: "jjj",
    //     name: "trending_up",
    //     description: "Most Popular",
    //     code: "Most Popular",
    //     categoryId: "Most Popular",
    //     vendorId: "Most Popular",
    //     brand: "Most Popular",
    //     available: true,
    //     active: true,
    //     price: 16.5
    //   },
    //   {
    //     id: "jje",
    //     name: "trending_up",
    //     description: "Most Popular",
    //     code: "Most Popular",
    //     categoryId: "Most Popular",
    //     vendorId: "Most Popular",
    //     brand: "Most Popular",
    //     available: true,
    //     active: true,
    //     price: 16.5
    //   }
    // ];
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
