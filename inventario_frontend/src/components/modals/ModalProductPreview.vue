<template>
  <div>
    <form>
      <div class="modal-card" style="width: auto">
        <header class="modal-card-head">
          <p class="modal-card-title">Información del producto</p>
        </header>
        <section class="modal-card-body">
          <div>
            <label>Nombre: </label>
            <span>{{ product.name }}</span>
          </div>
          <div>
            <label>Descripción: </label>
            <span>{{ product.description }}</span>
          </div>
          <div>
            <label>Código: </label>
            <span>{{ product.code }}</span>
          </div>
          <div>
            <label>Categoría: </label>
            <span>{{ product.category ? product.category.name : "" }}</span>
          </div>
          <div>
            <label>Proveedor: </label>
            <span>{{ product.vendor ? product.vendor.name : "" }}</span>
          </div>
          <div>
            <label>Marca: </label>
            <span>{{ product.brand }}</span>
          </div>
          <div>
            <label>Stock mínimo: </label>
            <span>{{ product.minimumStock }}</span>
          </div>
          <div>
            <label>Stock: </label>
            <span
              v-bind:style="{
                color: product.stock >= product.minimumStock ? 'green' : 'red'
              }"
            >
              {{ product.stock }}
            </span>
            <b-icon
              v-if="product.stock < product.minimumStock"
              icon="alert"
              type="is-danger"
            ></b-icon>
          </div>
          <div>
            <label>Unidad: </label>
            <span>{{ product.unitOfMeasurement }}</span>
          </div>
          <div>
            <label>Precio de compra: </label>
            <span>${{ product.purchasePrice.value }}</span>
          </div>
          <div>
            <label>Precio de venta: </label>
            <span>${{ product.salePrice.value }}</span>
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
import { dateTimeToLocal } from "@/utils/common-functions";
import { Product } from "@/models/product";
import { NavigatorProductService } from "@/services/product-service";

@Component
export default class ModalProductPreview extends Vue {
  public product: Product = new Product();
  public productService: NavigatorProductService = new NavigatorProductService();
  public isLoading = false;

  @Prop({ required: true })
  public productId!: string;

  dateToLocal(date: Date) {
    return dateTimeToLocal(date).substring(0, 10);
  }

  created() {
    this.isLoading = true;
    this.productService
      .getProduct(this.productId)
      .then(response => {
        this.isLoading = false;
        this.product = response as Product;
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
  margin-left: 10px;
  float: right;
}
</style>
