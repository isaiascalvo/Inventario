<template>
  <div>
    <section class="hero is-light p-1">
      <div class="hero-head">
        <div class="level">
          <div>
            <h1 class="title is-6">Lista de Ventas</h1>
          </div>
          <div>
            <b-button
              type="is-info"
              size="is-small"
              tag="router-link"
              to="/sale/new"
              class="mx-1"
            >
              Nueva Venta
            </b-button>
            <!-- <b-button
              @click="openFilters = !openFilters"
              class="is-pulled-right"
              type="is-primary"
              :icon-right="
                openFilters ? 'filter-variant-minus' : 'filter-variant'
              "
            >
              {{ openFilters ? "Ocultar Filtros" : "Mostrar Filtros" }}
            </b-button> -->
          </div>
        </div>
      </div>
    </section>

    <div>
      <!-- <div class="columns filtersClass level" v-if="openFilters">
        <div class="column is-10">
          <b-field grouped group-multiline>
            <b-field label-position="on-border" label="Nombre">
              <b-input
                v-model="saleFilters.name"
                placeholder="Nombre"
                size="is-small"
                icon-right="close-circle"
                icon-right-clickable
                @icon-right-click="clearIconClick('name')"
              ></b-input>
            </b-field>

            <b-field label-position="on-border" label="Apellido">
              <b-input
                v-model="saleFilters.lastname"
                placeholder="Apellido"
                size="is-small"
                icon-right="close-circle"
                icon-right-clickable
                @icon-right-click="clearIconClick('lastname')"
              ></b-input>
            </b-field>

            <b-field label-position="on-border" label="DNI">
              <b-input
                v-model="saleFilters.dni"
                placeholder="DNI"
                size="is-small"
                icon-right="close-circle"
                icon-right-clickable
                @icon-right-click="clearIconClick('dni')"
              ></b-input>
            </b-field>

            <b-field label-position="on-border" label="Mail">
              <b-input
                v-model="saleFilters.mail"
                placeholder="Mail"
                size="is-small"
                icon-right="close-circle"
                icon-right-clickable
                @icon-right-click="clearIconClick('mail')"
              ></b-input>
            </b-field>
          </b-field>
        </div>

        <div class="column level-right">
          <b-button type="is-info" class="mx-1" @click="applyFilters()">
            Aplicar
          </b-button>
          <b-button @click="clearFilters()" type="is-danger">
            Limpiar
          </b-button>
        </div>
      </div> -->

      <b-table
        striped
        hoverable
        scrollable
        :data="sales"
        id="my-table"
        :paginated="true"
        :per-page="perPage"
        :current-page.sync="currentPage"
        aria-next-label="Next page"
        aria-previous-label="Previous page"
        aria-page-label="Page"
        aria-current-label="Current page"
      >
        <template slot="empty">
          No hay ventas registradas
        </template>
        <template slot-scope="props">
          <b-table-column field="productId" label="Producto">
            <span
              v-if="
                (
                  props.row.product.code +
                  ' - ' +
                  props.row.product.name +
                  ' - ' +
                  props.row.product.description
                ).length < 40
              "
            >
              {{
                props.row.product.code +
                  " - " +
                  props.row.product.name +
                  " - " +
                  props.row.product.description
              }}
            </span>
            <b-tooltip
              v-else
              :label="
                props.row.product.code +
                  ' - ' +
                  props.row.product.name +
                  ' - ' +
                  props.row.product.description
              "
              position="is-right"
              size="is-large"
              type="is-dark"
              multilined
            >
              {{
                getProductDescription(
                  props.row.product.code +
                    " - " +
                    props.row.product.name +
                    " - " +
                    props.row.product.description
                )
              }}
            </b-tooltip>
          </b-table-column>
          <b-table-column field="quantity" label="Cantidad" centered>
            {{ props.row.quantity }}
          </b-table-column>
          <b-table-column field="clientName" label="Cliente" centered>
            {{ props.row.clientName }}
          </b-table-column>
          <b-table-column field="date" label="Fecha y hora" centered>
            {{ dateTimeToLocal(props.row.date) }}
          </b-table-column>
          <b-table-column field="amount" label="Importe Total" centered>
            $ {{ getTotal(props.row) }}
          </b-table-column>
          <b-table-column field="paymentType" label="Forma de Pago">
            {{ getPaymentType(props.row) }}
            <!-- Cuotas (6 de $ 4444) -->
          </b-table-column>

          <b-table-column field="action" label="Acciones" centered>
            <b-button
              @click="deleteSale(props.row)"
              type="is-small"
              class="actionButton"
            >
              <b-icon icon="delete"></b-icon>
            </b-button>
          </b-table-column>
        </template>
      </b-table>
    </div>

    <b-loading is-full-page :active.sync="isLoading"></b-loading>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { Sale } from "../../models/sale";
import { NavigatorSaleService } from "../../services/sale-service";
import { paymentTypes } from "@/enums/paymentTypes";
import {
  Cash,
  OwnFees,
  CreditCard,
  DebitCard,
  ChequesPayment
} from "@/models/payment";
import { dateTimeToLocal } from "@/utils/common-functions";
// import { SaleFilters } from "../../models/saleFilters";

@Component
export default class SaleList extends Vue {
  public sales: Sale[] = [];
  public saleService: NavigatorSaleService = new NavigatorSaleService();
  // public saleFilters: SaleFilters = new SaleFilters();
  // public openFilters = false;
  public currentPage = 1;
  public perPage = 10;
  public errorDialog = false;
  public confirmDialog = false;
  public isLoading = false;

  dateTimeToLocal(date: Date) {
    return dateTimeToLocal(date);
  }

  getProductDescription(description: string): string {
    return description.substring(0, 40) + "...";
  }

  public getTotal(sale: Sale) {
    switch (sale.paymentType) {
      case paymentTypes.cash:
        return sale.cash?.amount;
      case paymentTypes.ownFees:
        return sale.ownFees?.amount;
      case paymentTypes.creditcard:
        return sale.creditCard?.amount;
      case paymentTypes.debitcard:
        return sale.debitCard?.amount;
      case paymentTypes.cheque:
        return sale.cheques?.amount;
      default:
        break;
    }
  }

  getPaymentType(sale: Sale) {
    switch (sale.paymentType) {
      case paymentTypes.cash:
        return (
          Cash.GetPaymentType() + " (Descuento: " + sale.cash?.discount + "%)"
        );
      case paymentTypes.ownFees:
        return (
          OwnFees.GetPaymentType() +
          " (" +
          sale.ownFees?.quantity +
          " de $" +
          sale.ownFees?.feeList[0]?.value +
          ")"
        );
      case paymentTypes.creditcard:
        return (
          CreditCard.GetPaymentType() +
          " (" +
          sale.creditCard?.bank +
          " - " +
          sale.creditCard?.cardType +
          " - Descuento: " +
          sale.creditCard?.discount +
          "%)"
        );
      case paymentTypes.debitcard:
        return (
          DebitCard.GetPaymentType() +
          " (" +
          sale.debitCard?.bank +
          " - " +
          sale.debitCard?.cardType +
          " - Recargo: " +
          sale.debitCard?.surcharge +
          "%)"
        );
      case paymentTypes.cheque:
        return (
          ChequesPayment.GetPaymentType() +
          " (" +
          sale.cheques?.listOfCheques.length +
          ")"
        );
      default:
        break;
    }
  }

  deleteSale(sale: Sale) {
    this.$buefy.dialog.confirm({
      title: "Eliminando Venta",
      message:
        "Estás seguro que deseas <b>eliminar</b> la venta? Esta acción no podrá deshacerse.",
      confirmText: "Eliminar Venta",
      cancelText: "Cancelar",
      type: "is-danger",
      hasIcon: true,
      onConfirm: () => {
        this.isLoading = true;
        this.saleService
          .deleteSale(sale.id)
          .then(() => {
            this.isLoading = false;
            this.sales.splice(this.sales.indexOf(sale), 1);
          })
          .catch(e => {
            this.isLoading = false;
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
            console.log("error: ", e);
          });
        this.$buefy.toast.open("Venta eliminada!");
      }
    });
  }

  // public clearFilters() {
  //   this.isLoading = true;
  //   this.saleFilters = new SaleFilters();
  //   this.saleService
  //     .getSales()
  //     .then(response => {
  //       this.sales = response;
  //       this.isLoading = false;
  //     })
  //     .catch(e => {
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
  //       this.isLoading = false;
  //       console.log("error: ", e);
  //     });
  // }

  // public applyFilters() {
  //   this.isLoading = true;
  //   this.saleService
  //     .getSalesFiltered(this.saleFilters)
  //     .then(response => {
  //       this.sales = response;
  //       this.isLoading = false;
  //     })
  //     .catch(e => {
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
  //       this.isLoading = false;
  //       console.log("error: ", e);
  //     });
  // }

  created() {
    this.isLoading = true;
    this.saleService
      .getSales()
      .then(response => {
        this.sales = response as Sale[];
        this.isLoading = false;
      })
      .catch(e => {
        this.isLoading = false;
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
        console.log("error: ", e);
      });
  }
}
</script>

<style>
.clickeable {
  cursor: pointer;
}

.filtersClass {
  margin: 0em !important;
  padding: 1em;
  background-color: #e0eaff !important;
}

table {
  font-size: 13px;
  border: 1px solid rgb(192, 192, 192) !important;
  /* margin-left: 10px; */
}

th {
  /* background-color: #dbdbdb; */
  background-color: rgb(229, 229, 229);
  border: 1px solid rgb(192, 192, 192) !important;
}

td {
  border: 1px solid rgb(192, 192, 192) !important;
}

.ml-1 {
  margin-left: 1em;
}

select {
  min-width: 120px;
}

.filterButton {
  float: right;
}

.filtersClass select,
.filtersClass input {
  width: 180px;
}

select {
  min-width: 90px;
}

input {
  min-width: 80px;
}

.fieldWidth {
  width: 80px;
}

p-1 {
  padding: 1em;
}
</style>
