<template>
  <div>
    <section class="hero is-light p-1">
      <div class="hero-head">
        <div class="level">
          <div>
            <h1 class="title is-6">Lista de Movimientos</h1>
          </div>
          <div>
            <b-button
              type="is-info"
              size="is-small"
              tag="router-link"
              to="/product-entry/new"
              class="mx-1"
            >
              Nuevo movimiento
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

    <div class="">
      <div class="columns filtersClass level" v-if="openFilters">
        <div class="column is-10">
          <b-field grouped group-multiline>
            <b-field label-position="on-border" label="Fecha desde">
              <b-datetimepicker
                v-model="productEntryFilters.dateDateFrom"
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
                v-model="productEntryFilters.dateDateTo"
                rounded
                placeholder="Seleccione fecha y hora"
                icon="calendar-today"
                trap-focus
                size="is-small"
                editable
              >
              </b-datetimepicker>
            </b-field>

            <b-field label-position="on-border" label="Tipo de movimiento">
              <b-select
                v-model="productEntryFilters.isEntry"
                placeholder="Seleccione un tipo de movimiento"
                expanded
                size="is-small"
              >
                <option :value="null"></option>
                <option :value="true">Entrada</option>
                <option :value="false">Salida</option>
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
        :data="productEntries"
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
          No hay entradas/salidas de productos registradas
        </template>
        <template slot-scope="props">
          <b-table-column field="date" label="Fecha y hora">
            {{ dateTimeToLocal(props.row.date) }}
          </b-table-column>
          <b-table-column field="isEntry" label="Tipo">
            {{ props.row.isEntry ? "Entrada" : "Salida" }}
          </b-table-column>

          <b-table-column field="action" label="Acciones">
            <b-button
              tag="router-link"
              :to="'/product-entry/modify/' + props.row.id"
              type="is-small"
            >
              <b-icon icon="pencil"></b-icon>
            </b-button>
            <b-button
              @click="deleteProductEntry(props.row)"
              type="is-small"
              class="actionButton"
            >
              <b-icon icon="delete"></b-icon>
            </b-button>
          </b-table-column>
        </template>

        <template slot="bottom-left">
          <span class="ml-1">
            <b-field label="Items por página:" label-position="on-border">
              <b-select v-model="perPage">
                <option value="10">10 por página</option>
                <option value="15">15 por página</option>
                <option value="20">20 por página</option>
              </b-select>
            </b-field>
          </span>
        </template>
      </b-table>
    </div>

    <b-loading is-full-page :active.sync="isLoading"></b-loading>
  </div>
</template>

<script lang="ts">
import { ProductEntryFilters } from "@/models/filters/productEntryFilters";
import { dateTimeToLocal } from "@/utils/common-functions";
import { Vue, Component } from "vue-property-decorator";
import { ProductEntry } from "../../models/productEntry";
import { NavigatorProductEntryService } from "../../services/product-entry-service";

@Component
export default class ProductEntryList extends Vue {
  public productEntries: ProductEntry[] = [];
  public productEntryService: NavigatorProductEntryService = new NavigatorProductEntryService();

  public currentPage = 1;
  public perPage = 10;
  public isLoading = false;
  public openFilters = false;
  public totalPages = 0;
  public filtersApplied = false;
  public productEntryFilters: ProductEntryFilters = new ProductEntryFilters();

  dateTimeToLocal(date: Date) {
    return dateTimeToLocal(date);
  }

  deleteProductEntry(productEntry: ProductEntry) {
    this.$buefy.dialog.confirm({
      title: "Eliminando Entrada de Productos",
      message:
        "Estás seguro que deseas <b>eliminar</b> la etrada de productos? Esta acción no podrá deshacerse.",
      confirmText: "Eliminar Entrada de Productos",
      cancelText: "Cancelar",
      type: "is-danger",
      hasIcon: true,
      onConfirm: () => {
        this.isLoading = true;
        this.productEntryService
          .deleteProductEntry(productEntry.id)
          .then(() => {
            this.isLoading = false;
            this.productEntries.splice(
              this.productEntries.indexOf(productEntry),
              1
            );
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
          });

        this.$buefy.toast.open("Entrada de Productos eliminada!");
      }
    });
  }

  public clearFilters() {
    this.filtersApplied = false;
    this.productEntryFilters = new ProductEntryFilters();
    this.getProductEntries();
  }

  public applyFilters() {
    this.isLoading = true;
    this.filtersApplied = true;
    this.getProductEntries();
  }

  getProductEntries() {
    this.isLoading = true;
    let rta: Promise<void>;
    if (this.filtersApplied) {
      rta = this.productEntryService
        .getTotalQtyByFilters(this.productEntryFilters)
        .then(qty => {
          this.totalPages = qty;
          return this.productEntryService.getByFiltersPageAndQty(
            this.productEntryFilters,
            (this.currentPage - 1) * this.perPage,
            this.perPage
          );
        })
        .then(response => {
          this.productEntries = response;
          this.isLoading = false;
        });
    } else {
      rta = this.productEntryService
        .getTotalQty()
        .then(qty => {
          this.totalPages = qty;
          return this.productEntryService.getByPageAndQty(
            (this.currentPage - 1) * this.perPage,
            this.perPage
          );
        })
        .then(response => {
          this.productEntries = response;
          this.isLoading = false;
        });
    }

    rta.catch(e => {
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

  onPageChange(page: number) {
    this.currentPage = page;
    this.getProductEntries();
  }

  created() {
    this.getProductEntries();
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

.actionButton {
  margin-left: 5px;
}
</style>
