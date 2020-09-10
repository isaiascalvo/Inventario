<template>
  <div>
    <form>
      <div class="modal-card" style="width: auto">
        <header class="modal-card-head">
          <p class="modal-card-title">Cuotas</p>
        </header>
        <section class="modal-card-body">
          <b-table
            striped
            hoverable
            scrollable
            :data="ownFees.feeList"
            id="my-table"
          >
            <template slot="empty">
              No hay cuotas registradas
            </template>
            <template slot-scope="props">
              <b-table-column
                field="expirationDate"
                label="Fecha de vencimiento"
              >
                {{ dateToLocal(props.row.expirationDate) }}
              </b-table-column>
              <b-table-column field="paymentDate" label="Fecha de pago">
                {{ dateToLocal(props.row.paymentDate) }}
              </b-table-column>
              <b-table-column field="value" label="Monto">
                ${{ props.row.value }}
              </b-table-column>
              <b-table-column field="state" label="Estado">
                <div v-html="getState(props.row)"></div>
              </b-table-column>
              <b-table-column field="action" label="Acciones" centered>
                <b-button
                  @click="payFee(props.index)"
                  type="is-small"
                  :disabled="paymentDisabled(props.index)"
                >
                  Registrar pago
                </b-button>
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
import { Fee, OwnFees } from "@/models/payment";
import { NavigatorSaleService } from "@/services/sale-service";
import { dateTimeToLocal } from "@/utils/common-functions";

@Component
export default class ModalOwnFeesPreview extends Vue {
  public saleService: NavigatorSaleService = new NavigatorSaleService();
  public isLoading = false;

  @Prop({ required: true })
  public ownFees!: OwnFees;

  public dateToLocal(date: Date) {
    if (date) {
      return dateTimeToLocal(date).substring(0, 10);
    }
  }

  paymentDisabled(index: number) {
    if (this.ownFees.feeList[index].paymentDate) {
      return true;
    } else {
      if (index === 0) {
        return false;
      } else {
        return this.ownFees.feeList[index - 1].paymentDate ? false : true;
      }
    }
  }

  getState(fee: Fee): string {
    let color;
    let text;
    if (fee.paymentDate) {
      color = "green";
      text = "Pagada";
    } else {
      if (new Date(fee.expirationDate) >= new Date()) {
        color = "orange";
        text = "Pendiente";
      } else {
        color = "red";
        text = "Vencida";
      }
    }
    return `<span style="color: ${color};">${text}</span>`;
  }

  payFee(index: number) {
    this.$buefy.dialog.prompt({
      message: `<strong>Ingrese la fecha del pago de la cuota</strong><br />(esta acción no podrá deshacerse):`,
      inputAttrs: {
        type: "date"
      },
      confirmText: "Aceptar",
      cancelText: "Cancelar",
      trapFocus: true,
      onConfirm: value => {
        this.$buefy.toast.open(`Pago registrado`);
        this.isLoading = true;
        this.saleService
          .payFee(this.ownFees.feeList[index].id, value)
          .then(() => {
            this.isLoading = false;
            this.ownFees.feeList[index].paymentDate = new Date(value);
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
