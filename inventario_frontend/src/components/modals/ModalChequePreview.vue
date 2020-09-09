<template>
  <div>
    <form>
      <div class="modal-card" style="width: auto">
        <header class="modal-card-head">
          <p class="modal-card-title">Cheques</p>
        </header>
        <section class="modal-card-body">
          <b-table
            striped
            hoverable
            scrollable
            :data="chequesPayment.listOfCheques"
            id="my-table"
          >
            <template slot="empty">
              No hay cheques registrados
            </template>
            <template slot-scope="props">
              <b-table-column field="bank" label="Banco">
                {{ props.row.bank }}
              </b-table-column>
              <b-table-column field="nro" label="Nro">
                {{ props.row.nro }}
              </b-table-column>
              <b-table-column field="value" label="Monto">
                ${{ props.row.value }}
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
    <b-loading is-full-page :active.sync="isLoading"></b-loading>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from "vue-property-decorator";
import { ChequesPayment } from "@/models/payment";
import { NavigatorSaleService } from "@/services/sale-service";

@Component
export default class ModalChequePreview extends Vue {
  public saleService: NavigatorSaleService = new NavigatorSaleService();
  public isLoading = false;

  @Prop({ required: true })
  public chequesPayment!: ChequesPayment;

  // created() {
  //   this.isLoading = true;
  //   this.saleService
  //     .getClient(this.chequesPaymentId)
  //     .then(response => {
  //       this.isLoading = false;
  //       this.chequesPayment = response as ChequesPayment;
  //     })
  //     .catch(error => {
  //       this.isLoading = false;
  //       console.log(error);
  //       this.$buefy.dialog.alert({
  //         title: "Error",
  //         message:
  //           "Un error inesperado ha ocurrido. Por favor inténtelo nuevamente.",
  //         type: "is-danger",
  //         hasIcon: true,
  //         icon: "times-circle",
  //         iconPack: "fa",
  //         ariaRole: "alertdialog",
  //         ariaModal: true
  //       });
  //     });
  // }
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
