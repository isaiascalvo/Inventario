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
            <b-button
              @click="openFilters = !openFilters"
              class="is-pulled-right"
              type="is-primary"
              size="is-small"
              :icon-right="
                openFilters ? 'filter-variant-minus' : 'filter-variant'
              "
            >
              {{ openFilters ? "Ocultar Filtros" : "Mostrar Filtros" }}
            </b-button>
          </div>
        </div>
      </div>
    </section>

    <div>
      <div class="columns filtersClass level" v-if="openFilters">
        <div class="column is-10">
          <b-field grouped group-multiline>
            <b-field label-position="on-border" label="Cliente">
              <b-autocomplete
                class="autocompleteClass"
                v-model="saleFilters.clientName"
                :data="filteredClients()"
                placeholder="ej.: Cliente X"
                icon="magnify"
                size="is-small"
                icon-right="close-circle"
                icon-right-clickable
                @icon-right-click="
                  saleFilters.clientId = saleFilters.clientName = undefined
                "
                @select="option => getClientId(option)"
              >
                <template slot="empty">No se encontraron resultados</template>
              </b-autocomplete>
            </b-field>

            <b-field label-position="on-border" label="Producto">
              <b-autocomplete
                class="autocompleteClass"
                v-model="prodCodNameDesc"
                :data="filteredProducts()"
                placeholder="ej.: Producto X"
                icon="magnify"
                size="is-small"
                icon-right="close-circle"
                icon-right-clickable
                @icon-right-click="
                  saleFilters.productId = prodCodNameDesc = undefined
                "
                @select="option => getProductId(option)"
              >
                <template slot="empty">No se encontraron resultados</template>
              </b-autocomplete>
            </b-field>

            <b-field label-position="on-border" label="Fecha desde">
              <b-datetimepicker
                v-model="saleFilters.dateDateFrom"
                rounded
                placeholder="Seleccione fecha y hora"
                icon="calendar-today"
                trap-focus
                size="is-small"
                editable
              >
              </b-datetimepicker>
            </b-field>

            <b-field label-position="on-border" label="Fecha hasta">
              <b-datetimepicker
                v-model="saleFilters.dateDateTo"
                rounded
                placeholder="Seleccione fecha y hora"
                icon="calendar-today"
                trap-focus
                size="is-small"
                editable
              >
              </b-datetimepicker>
            </b-field>

            <b-field label-position="on-border" label="Forma de pago">
              <b-select
                v-model="saleFilters.paymentType"
                placeholder="Seleccione un método de pago"
                expanded
                size="is-small"
              >
                <option :value="null"></option>
                <option :value="0">Efectivo</option>
                <option :value="1">Cuotas Propias</option>
                <option :value="2">Tarjeta de crédito</option>
                <option :value="3">Tarjeta de débito</option>
                <option :value="4">Cheque</option>
              </b-select>
            </b-field>
          </b-field>
        </div>

        <div class="column level-right">
          <b-button
            type="is-dark"
            class="mx-1"
            size="is-small"
            @click="applyFilters()"
          >
            Aplicar
          </b-button>
          <b-button @click="clearFilters()" size="is-small" type="is-default">
            Limpiar
          </b-button>
        </div>
      </div>

      <b-table
        striped
        hoverable
        scrollable
        :data="sales"
        id="my-table"
        :paginated="true"
        backend-pagination
        :total="totalPages"
        :per-page="perPage"
        :current-page.sync="currentPage"
        @page-change="onPageChange"
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
            <b-tooltip
              :label="getProductDescription(props.row.product)"
              position="is-right"
              size="is-large"
              type="is-dark"
              :active="getProductDescription(props.row.product).length > 40"
              multilined
            >
              <span
                class="link"
                @click="openModalProductPreview(props.row.productId)"
              >
                {{ getShortProductDescription(props.row.product) }}
              </span>
            </b-tooltip>
          </b-table-column>
          <b-table-column field="quantity" label="Cantidad" centered>
            {{ props.row.quantity }}
          </b-table-column>
          <b-table-column field="clientName" label="Cliente" centered>
            <span
              v-if="props.row.clientId"
              class="link"
              @click="openModalClientPreview(props.row.clientId)"
            >
              {{ props.row.clientName }}
            </span>
            <span v-else>
              {{ props.row.clientName }}
            </span>
          </b-table-column>
          <b-table-column field="date" label="Fecha y hora" centered>
            {{ dateTimeToLocal(props.row.date) }}
          </b-table-column>
          <b-table-column field="amount" label="Importe Total" centered>
            $ {{ getTotal(props.row) }}
          </b-table-column>
          <b-table-column field="paymentType" label="Forma de Pago">
            <template v-if="props.row.cheques || props.row.ownFees">
              <span
                v-if="props.row.cheques"
                class="link"
                @click="openModalChequesPreview(props.row.cheques)"
              >
                {{ getPaymentType(props.row) }}
              </span>
              <span
                v-if="props.row.ownFees"
                class="link"
                @click="openModalOwnFeesPreview(props.row.ownFees)"
              >
                {{ getPaymentType(props.row) }}
              </span>
            </template>
            <span v-else>
              {{ getPaymentType(props.row) }}
            </span>
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
import { SaleFilters } from "../../models/filters/saleFilters";
import { Client } from "@/models/client";
import { Product } from "@/models/product";
import { NavigatorClientService } from "@/services/client-service";
import { NavigatorProductService } from "@/services/product-service";
import ModalClientPreview from "@/components/modals/ModalClientPreview.vue";
import ModalProductPreview from "@/components/modals/ModalProductPreview.vue";
import ModalChequePreview from "@/components/modals/ModalChequePreview.vue";
import ModalOwnFeesPreview from "@/components/modals/ModalOwnFeesPreview.vue";

@Component({
  components: {
    ModalClientPreview
  }
})
export default class SaleList extends Vue {
  public sales: Sale[] = [];
  public clients: Client[] = [];
  public products: Product[] = [];
  public saleService: NavigatorSaleService = new NavigatorSaleService();
  public clientService: NavigatorClientService = new NavigatorClientService();
  public productService: NavigatorProductService = new NavigatorProductService();
  public saleFilters: SaleFilters = new SaleFilters();
  public openFilters = false;
  public currentPage = 1;
  public perPage = 10;
  public isLoading = false;
  public filtersApplied = false;
  public totalPages = 0;
  public prodCodNameDesc = "";

  dateTimeToLocal(date: Date) {
    return dateTimeToLocal(date);
  }

  getProductDescription(product: Product): string {
    return (
      product.name +
      " - " +
      product.description +
      (product.code ? " - " + product.code : "")
    );
  }

  getShortProductDescription(product: Product): string {
    return this.getProductDescription(product).substring(0, 40) + "...";
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

  public clearFilters() {
    this.filtersApplied = false;
    this.saleFilters = new SaleFilters();
    this.getSales();
  }

  public applyFilters() {
    this.isLoading = true;
    this.filtersApplied = true;
    this.getSales();
  }

  onPageChange(page: number) {
    this.currentPage = page;
    this.getSales();
  }

  getSales(): Promise<void> {
    this.isLoading = true;
    let rta: Promise<void>;
    if (this.filtersApplied) {
      rta = this.saleService
        .getTotalQtyByFilters(this.saleFilters)
        .then(qty => {
          this.totalPages = qty;
          return this.saleService.getByFiltersPageAndQty(
            this.saleFilters,
            (this.currentPage - 1) * this.perPage,
            this.perPage
          );
        })
        .then(response => {
          this.sales = response;
        });
    } else {
      rta = this.saleService
        .getTotalQty()
        .then(qty => {
          this.totalPages = qty;
          return this.saleService.getByPageAndQty(
            (this.currentPage - 1) * this.perPage,
            this.perPage
          );
        })
        .then(response => {
          this.sales = response;
        });
    }

    this.isLoading = false;
    return rta;
  }

  filteredProducts() {
    const filtered = this.products.filter(option => {
      return option.name
        ? option.name
            .toString()
            .toLowerCase()
            .indexOf((this.prodCodNameDesc ?? "").toLowerCase()) >= 0
        : false;
    });

    const codNameDescArray: string[] = [];

    filtered.forEach(x => {
      if (x.name) {
        codNameDescArray.push(x.name + " - " + x.description + " - " + x.code);
      }
    });
    return codNameDescArray;
  }

  getProductId(option: string) {
    const prod = this.products.find(
      x => x.name + " - " + x.description + " - " + x.code === option
    );
    this.saleFilters.productId = prod ? prod.id : undefined;
  }

  filteredClients() {
    const filtered = this.clients.filter(option => {
      return option.name
        ? option.name
            .toString()
            .toLowerCase()
            .indexOf((this.saleFilters.clientName ?? "").toLowerCase()) >= 0
        : false;
    });

    const dniNameArray: string[] = [];

    filtered.forEach(x => {
      if (x.name) {
        dniNameArray.push(x.name + " - " + x.lastname + " - " + x.dni);
      }
    });
    return dniNameArray;
  }

  getClientId(option: string) {
    const client = this.clients.find(
      x => x.name + " - " + x.lastname + " - " + x.dni === option
    );
    this.saleFilters.clientId = client ? client.id : undefined;
  }

  openModalClientPreview(clientId: string) {
    this.$buefy.modal.open({
      parent: this,
      component: ModalClientPreview,
      hasModalCard: true,
      customClass: "custom-class custom-class-2",
      trapFocus: true,
      props: {
        clientId: clientId
      }
    });
  }

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

  openModalChequesPreview(cheques: ChequesPayment) {
    this.$buefy.modal.open({
      parent: this,
      component: ModalChequePreview,
      hasModalCard: true,
      customClass: "custom-class custom-class-2",
      trapFocus: true,
      props: {
        chequesPayment: cheques
      }
    });
  }

  openModalOwnFeesPreview(ownFees: OwnFees) {
    this.$buefy.modal.open({
      parent: this,
      component: ModalOwnFeesPreview,
      hasModalCard: true,
      customClass: "custom-class custom-class-2",
      trapFocus: true,
      props: {
        ownFees: ownFees
      }
    });
  }

  created() {
    this.getSales()
      .then(() => {
        this.isLoading = true;
        return this.clientService.getClients();
      })
      .then(response => {
        this.clients = response;
        return this.productService.getProducts();
      })
      .then(response => {
        this.products = response;
      })
      .catch(e => {
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
        this.isLoading = false;
        console.log("error: ", e);
      })
      .finally(() => {
        this.isLoading = false;
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

.link {
  cursor: pointer;
  color: blue;
}

.actionButton {
  margin-left: 5px;
}

.autocompleteClass input {
  width: 500px !important;
}
</style>
