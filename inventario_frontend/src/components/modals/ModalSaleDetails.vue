<template>
  <div>
    <form>
      <div class="modal-card" style="width: auto">
        <header class="modal-card-head">
          <p class="modal-card-title">Productos comprados</p>
        </header>
        <section class="modal-card-body">
          <b-table striped hoverable scrollable :data="details" id="my-table">
            <template slot="empty">
              No hay Productos en la compra
            </template>
            <template slot-scope="props">
              <b-table-column field="product" label="Producto">
                <span
                  class="link"
                  @click="openModalProductPreview(props.row.productId)"
                >
                  {{ props.row.product.name }}
                </span>
              </b-table-column>
              <b-table-column field="quantity" label="Cantidad">
                {{ props.row.quantity }}
              </b-table-column>
            </template>
          </b-table>
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
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from "vue-property-decorator";
import { Detail } from "@/models/detail";
import ModalProductPreview from "./ModalProductPreview.vue";

@Component
export default class ModalSaleDetails extends Vue {
  @Prop({ required: true })
  public details!: Detail[];

  openModalProductPreview(productId: string) {
    this.$buefy.modal.open({
      parent: this,
      component: ModalProductPreview,
      hasModalCard: true,
      customClass: "custom-class custom-class-2",
      trapFocus: true,
      props: {
        productId: productId
      }
    });
  }
}
</script>

<style scoped>
label {
  font-weight: bold;
}

/* span {
  margin-left: 5px;
  float: right;
} */

.link {
  cursor: pointer;
  color: blue;
}
</style>
