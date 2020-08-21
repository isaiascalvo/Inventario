<template>
  <div>
    <section class="hero is-light p-1">
      <div class="hero-head">
        <div class="level">
          <div>
            <h1 class="title is-6">Lista de Compras</h1>
          </div>
          <div>
            <b-button
              type="is-info"
              size="is-small"
              tag="router-link"
              to="/purchase/new"
              class="mx-1"
            >
              Nueva Compra
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
                v-model="purchaseFilters.name"
                placeholder="Nombre"
                size="is-small"
                icon-right="close-circle"
                icon-right-clickable
                @icon-right-click="clearIconClick('name')"
              ></b-input>
            </b-field>

            <b-field label-position="on-border" label="Apellido">
              <b-input
                v-model="purchaseFilters.lastname"
                placeholder="Apellido"
                size="is-small"
                icon-right="close-circle"
                icon-right-clickable
                @icon-right-click="clearIconClick('lastname')"
              ></b-input>
            </b-field>

            <b-field label-position="on-border" label="DNI">
              <b-input
                v-model="purchaseFilters.dni"
                placeholder="DNI"
                size="is-small"
                icon-right="close-circle"
                icon-right-clickable
                @icon-right-click="clearIconClick('dni')"
              ></b-input>
            </b-field>

            <b-field label-position="on-border" label="Mail">
              <b-input
                v-model="purchaseFilters.mail"
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
        :data="purchases"
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
          No hay compras registradas
        </template>
        <template slot-scope="props">
          <b-table-column field="productId" label="Producto">
            {{
              props.row.product.code +
                " - " +
                props.row.product.name +
                " - " +
                props.row.product.description
            }}
          </b-table-column>
          <b-table-column field="clientName" label="Cliente">
            {{ props.row.clientName }}
          </b-table-column>
          <b-table-column field="date" label="Fecha">
            {{ dateToISO(props.row.date) }}
          </b-table-column>
          <b-table-column field="quantity" label="Cantidad">
            {{ props.row.quantity }}
          </b-table-column>
          <b-table-column field="amount" label="Importe">
            {{ props.row.amount }}
          </b-table-column>

          <b-table-column field="action" label="Acciones">
            <b-button
              tag="router-link"
              :to="'/purchase/modify/' + props.row.id"
              type="is-small"
            >
              <b-icon icon="pencil"></b-icon>
            </b-button>
            <b-button
              @click="deletePurchase(props.row)"
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
import { Purchase } from "../../models/purchase";
import { NavigatorPurchaseService } from "../../services/purchase-service";
// import { PurchaseFilters } from "../../models/purchaseFilters";

@Component
export default class PurchaseList extends Vue {
  public purchases: Purchase[] = [];
  public purchaseService: NavigatorPurchaseService = new NavigatorPurchaseService();
  // public purchaseFilters: PurchaseFilters = new PurchaseFilters();
  // public openFilters = false;
  public currentPage = 1;
  public perPage = 10;
  public errorDialog = false;
  public confirmDialog = false;
  public isLoading = false;

  dateToISO(date: Date) {
    return new Date(date).toISOString().substr(0, 10);
  }

  // clearIconClick(key: keyof PurchaseFilters) {
  //   this.purchaseFilters[key] = undefined;
  // }

  deletePurchase(purchase: Purchase) {
    this.$buefy.dialog.confirm({
      title: "Eliminando Compra",
      message:
        "Estás seguro que deseas <b>eliminar</b> la compra? Esta acción no podrá deshacerse.",
      confirmText: "Eliminar Compra",
      cancelText: "Cancelar",
      type: "is-danger",
      hasIcon: true,
      onConfirm: () => {
        this.isLoading = true;
        this.purchaseService
          .deletePurchase(purchase.id)
          .then(() => {
            this.isLoading = false;
            this.purchases.splice(this.purchases.indexOf(purchase), 1);
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
        this.$buefy.toast.open("Compra eliminada!");
      }
    });
  }

  // public clearFilters() {
  //   this.isLoading = true;
  //   this.purchaseFilters = new PurchaseFilters();
  //   this.purchaseService
  //     .getPurchases()
  //     .then(response => {
  //       this.purchases = response;
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
  //   this.purchaseService
  //     .getPurchasesFiltered(this.purchaseFilters)
  //     .then(response => {
  //       this.purchases = response;
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
    this.purchaseService
      .getPurchases()
      .then(response => {
        this.purchases = response as Purchase[];
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
