<template>
  <div>
    <section class="hero is-light p-1">
      <div class="hero-head">
        <div class="level">
          <div>
            <h1 class="title is-6">Lista de Comisiones</h1>
          </div>
          <div>
            <b-button
              type="is-info"
              size="is-small"
              tag="router-link"
              to="/commission/new"
              class="mx-1"
            >
              Nueva Comisión
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
            <b-field label-position="on-border" label="Vendor">
              <b-autocomplete
                class="autocompleteClass"
                v-model="vendorNameCuil"
                :data="filteredVendors()"
                placeholder="ej.: Vendoro X"
                icon="magnify"
                size="is-small"
                icon-right="close-circle"
                icon-right-clickable
                @icon-right-click="
                  commissionFilters.vendorId = vendorNameCuil = undefined
                "
                @select="option => getVendorId(option)"
              >
                <template slot="empty">No se encontraron resultados</template>
              </b-autocomplete>
            </b-field>

            <b-field label-position="on-border" label="Cliente">
              <b-autocomplete
                class="autocompleteClass"
                v-model="commissionFilters.clientName"
                :data="filteredClients()"
                placeholder="ej.: Cliente X"
                icon="magnify"
                size="is-small"
                icon-right="close-circle"
                icon-right-clickable
                @icon-right-click="
                  commissionFilters.clientId = commissionFilters.clientName = undefined
                "
                @select="option => getClientId(option)"
              >
                <template slot="empty">No se encontraron resultados</template>
              </b-autocomplete>
            </b-field>

            <b-field label-position="on-border" label="Fecha desde">
              <b-datetimepicker
                v-model="commissionFilters.dateDateFrom"
                rounded
                placeholder="Seleccione fecha y hora"
                icon="calendar-today"
                trap-focus
                size="is-small"
                editable
              >
              </b-datetimepicker>
              <p class="control">
                <b-button
                  icon-left="close"
                  type="is-dark"
                  size="is-small"
                  @click="commissionFilters.dateDateFrom = undefined"
                ></b-button>
              </p>
            </b-field>

            <b-field label-position="on-border" label="Fecha hasta">
              <b-datetimepicker
                v-model="commissionFilters.dateDateTo"
                rounded
                placeholder="Seleccione fecha y hora"
                icon="calendar-today"
                trap-focus
                size="is-small"
                editable
              >
              </b-datetimepicker>
              <p class="control">
                <b-button
                  icon-left="close"
                  type="is-dark"
                  size="is-small"
                  @click="commissionFilters.dateDateTo = undefined"
                ></b-button>
              </p>
            </b-field>

            <b-field label-position="on-border" label="Forma de pago">
              <b-select
                v-model="commissionFilters.paymentType"
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
        :data="commissions"
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
          No hay comisiones registradas
        </template>
        <template slot-scope="props">
          <b-table-column field="vendorId" label="Proveedor">
            <span
              class="link"
              @click="openModalVendorPreview(props.row.vendorId)"
            >
              {{ getVendorDescription(props.row.vendor) }}
            </span>
          </b-table-column>
          <b-table-column field="clientName" label="Cliente">
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
          <b-table-column field="product" label="Producto">
            {{ props.row.product }}
          </b-table-column>
          <b-table-column field="price" label="Precio" centered>
            $ {{ formattedAmount(props.row.price) }}
          </b-table-column>
          <b-table-column field="paymentType" label="Forma de Pago">
            {{ getPaymentType(props.row.paymentType) }}
          </b-table-column>
          <b-table-column field="date" label="Fecha y hora" centered>
            {{ dateTimeToLocal(props.row.date) }}
          </b-table-column>
          <b-table-column field="value" label="Comisión" centered>
            $ {{ formattedAmount(props.row.value) }}
          </b-table-column>

          <b-table-column field="action" label="Acciones" centered>
            <b-button
              tag="router-link"
              :to="'/commission/modify/' + props.row.id"
              type="is-small"
            >
              <b-icon icon="pencil"></b-icon>
            </b-button>
            <b-button
              @click="deleteCommission(props.row)"
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
import { Commission } from "../../models/commission";
import { NavigatorCommissionService } from "../../services/commission-service";
import { dateTimeToLocal, formattedAmount } from "@/utils/common-functions";
import { CommissionFilters } from "../../models/filters/commissionFilters";
import { Client } from "@/models/client";
import { NavigatorClientService } from "@/services/client-service";
import { NavigatorVendorService } from "@/services/vendor-service";
import { Vendor } from "@/models/vendor";
import ModalClientPreview from "@/components/modals/ModalClientPreview.vue";
import ModalVendorPreview from "@/components/modals/ModalVendorPreview.vue";
import { paymentTypes } from "@/enums/paymentTypes";
import {
  Cash,
  ChequesPayment,
  CreditCard,
  DebitCard,
  OwnFees
} from "@/models/payment";

@Component({
  components: {
    ModalClientPreview
  }
})
export default class CommissionList extends Vue {
  public commissions: Commission[] = [];
  public clients: Client[] = [];
  public vendors: Vendor[] = [];
  public commissionService: NavigatorCommissionService = new NavigatorCommissionService();
  public clientService: NavigatorClientService = new NavigatorClientService();
  public vendorService: NavigatorVendorService = new NavigatorVendorService();
  public commissionFilters: CommissionFilters = new CommissionFilters();
  public openFilters = false;
  public currentPage = 1;
  public perPage = 10;
  public isLoading = false;
  public filtersApplied = false;
  public totalPages = 0;
  public vendorNameCuil = "";

  dateTimeToLocal(date: Date) {
    return dateTimeToLocal(date);
  }

  formattedAmount(amount: number) {
    return formattedAmount(amount);
  }

  getPaymentType(paymentType: paymentTypes) {
    switch (paymentType) {
      case paymentTypes.cash:
        return Cash.GetPaymentType();
      case paymentTypes.ownFees:
        return OwnFees.GetPaymentType();
      case paymentTypes.creditcard:
        return CreditCard.GetPaymentType();
      case paymentTypes.debitcard:
        return DebitCard.GetPaymentType();
      case paymentTypes.cheque:
        return ChequesPayment.GetPaymentType();
      default:
        return "sss";
    }
  }

  getVendorDescription(vendor: Vendor): string {
    return vendor.name + (vendor.cuil ? " - " + vendor.cuil : "");
  }

  deleteCommission(commission: Commission) {
    this.$buefy.dialog.confirm({
      title: "Eliminando Comisión",
      message:
        "Estás seguro que deseas <b>eliminar</b> la comisión? Esta acción no podrá deshacerse.",
      confirmText: "Eliminar Comisión",
      cancelText: "Cancelar",
      type: "is-danger",
      hasIcon: true,
      onConfirm: () => {
        this.isLoading = true;
        this.commissionService
          .deleteCommission(commission.id)
          .then(() => {
            this.isLoading = false;
            this.commissions.splice(this.commissions.indexOf(commission), 1);
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
        this.$buefy.toast.open("Comisión eliminada!");
      }
    });
  }

  public clearFilters() {
    this.filtersApplied = false;
    this.commissionFilters = new CommissionFilters();
    this.getCommissions();
  }

  public applyFilters() {
    this.isLoading = true;
    this.filtersApplied = true;
    this.getCommissions();
  }

  onPageChange(page: number) {
    this.currentPage = page;
    this.getCommissions();
  }

  getCommissions(): Promise<void> {
    this.isLoading = true;
    return this.commissionService
      .getTotalQtyByFilters(this.commissionFilters)
      .then(qty => {
        this.totalPages = qty;
        return this.commissionService.getByFiltersPageAndQty(
          this.commissionFilters,
          (this.currentPage - 1) * this.perPage,
          this.perPage
        );
      })
      .then(response => {
        this.commissions = response as Commission[];
        this.isLoading = false;
      });
  }

  filteredVendors() {
    const filtered = this.vendors.filter(option => {
      return option.name
        ? option.name
            .toString()
            .toLowerCase()
            .indexOf((this.vendorNameCuil ?? "").toLowerCase()) >= 0
        : false;
    });

    const codNameDescArray: string[] = [];

    filtered.forEach(x => {
      if (x.name) {
        codNameDescArray.push(x.name + " - " + x.cuil);
      }
    });
    return codNameDescArray;
  }

  getVendorId(option: string) {
    const vend = this.vendors.find(x => x.name + " - " + x.cuil === option);
    this.commissionFilters.vendorId = vend ? vend.id : undefined;
  }

  filteredClients() {
    const filtered = this.clients.filter(option => {
      return option.name
        ? option.name
            .toString()
            .toLowerCase()
            .indexOf((this.commissionFilters.clientName ?? "").toLowerCase()) >=
            0
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
    this.commissionFilters.clientId = client ? client.id : undefined;
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

  openModalVendorPreview(vendorId: string) {
    this.$buefy.modal.open({
      parent: this,
      component: ModalVendorPreview,
      hasModalCard: true,
      customClass: "custom-class custom-class-2",
      trapFocus: true,
      props: {
        vendorId: vendorId
      }
    });
  }

  created() {
    this.getCommissions()
      .then(() => {
        this.isLoading = true;
        return this.clientService.getClients();
      })
      .then(response => {
        this.clients = response;
        return this.vendorService.getVendors();
      })
      .then(response => {
        this.vendors = response;
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
