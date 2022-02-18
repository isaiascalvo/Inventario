<template>
  <div>
    <section class="hero is-light p-1">
      <div class="hero-head">
        <div class="level">
          <div>
            <h1 class="title is-6">Gastos varios</h1>
          </div>
          <div>
            <b-button
              type="is-info"
              size="is-small"
              tag="router-link"
              to="/miscellaneous-expenses/new"
              class="mx-1"
            >
              Nuevo Gasto
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
            <b-field label-position="on-border" label="Descripción">
              <b-input
                v-model="miscellaneousExpensesFilters.description"
                placeholder="Descripción"
                size="is-small"
                icon-right="close-circle"
                icon-right-clickable
                @icon-right-click="
                  miscellaneousExpensesFilters.description = undefined
                "
              ></b-input>
            </b-field>

            <b-field label-position="on-border" label="Fecha desde">
              <b-datetimepicker
                v-model="miscellaneousExpensesFilters.dateDateFrom"
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
                  @click="miscellaneousExpensesFilters.dateDateFrom = undefined"
                ></b-button>
              </p>
            </b-field>

            <b-field label-position="on-border" label="Fecha hasta">
              <b-datetimepicker
                v-model="miscellaneousExpensesFilters.dateDateTo"
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
                  @click="miscellaneousExpensesFilters.dateDateTo = undefined"
                ></b-button>
              </p>
            </b-field>

            <b-field label-position="on-border" label="Monto">
              <b-input
                v-model="miscellaneousExpensesFilters.value"
                placeholder="Monto"
                size="is-small"
                type="number"
                step=".01"
                icon-right="close-circle"
                icon-right-clickable
                @icon-right-click="
                  miscellaneousExpensesFilters.value = undefined
                "
              ></b-input>
            </b-field>

            <b-field label-position="on-border" label="Destino">
              <b-input
                v-model="miscellaneousExpensesFilters.destination"
                placeholder="Destino"
                size="is-small"
                icon-right="close-circle"
                icon-right-clickable
                @icon-right-click="
                  miscellaneousExpensesFilters.destination = undefined
                "
              ></b-input>
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
        :data="miscellaneousExpenses"
        id="my-table"
        paginated
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
          No hay gastos varios registradas
        </template>
        <template slot-scope="props">
          <b-table-column field="description" label="Descripción">
            {{ props.row.description }}
          </b-table-column>
          <b-table-column field="date" label="Fecha y hora">
            {{ dateTimeToLocal(props.row.date) }}
          </b-table-column>
          <b-table-column field="isFixed" label="Tipo de gasto">
            {{ props.row.isFixed ? "Fijo" : "Variable" }}
          </b-table-column>
          <b-table-column field="value" label="Monto">
            $ {{ formattedAmount(props.row.value) }}
          </b-table-column>
          <b-table-column field="destination" label="Destino">
            {{ props.row.destination }}
          </b-table-column>

          <b-table-column field="action" label="Acciones">
            <b-button
              tag="router-link"
              :to="'/miscellaneous-expenses/modify/' + props.row.id"
              type="is-small"
            >
              <b-icon icon="pencil"></b-icon>
            </b-button>
            <b-button
              @click="deleteMiscellaneousExpenses(props.row)"
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
import { MiscellaneousExpensesFilters } from "@/models/filters/miscellaneousExpensesFilters";
import { dateTimeToLocal, formattedAmount } from "@/utils/common-functions";
import { Vue, Component } from "vue-property-decorator";
import { MiscellaneousExpenses } from "../../models/miscellaneousExpenses";
import { NavigatorMiscellaneousExpensesService } from "../../services/miscellaneous-expenses-service";

@Component
export default class MiscellaneousExpensesList extends Vue {
  public miscellaneousExpenses: MiscellaneousExpenses[] = [];
  public miscellaneousExpensesService: NavigatorMiscellaneousExpensesService = new NavigatorMiscellaneousExpensesService();

  public currentPage = 1;
  public perPage = 10;
  public isLoading = false;
  public openFilters = false;
  public miscellaneousExpensesFilters: MiscellaneousExpensesFilters = new MiscellaneousExpensesFilters();
  public totalPages = 0;
  public filtersApplied = false;

  dateTimeToLocal(date: Date) {
    return dateTimeToLocal(date);
  }

  formattedAmount(amount: number) {
    return formattedAmount(amount);
  }

  deleteMiscellaneousExpenses(miscellaneousExpense: MiscellaneousExpenses) {
    this.$buefy.dialog.confirm({
      title: "Eliminando gasto",
      message:
        "Estás seguro que deseas <b>eliminar</b> el gasto? Esta acción no podrá deshacerse.",
      confirmText: "Eliminar Gasto",
      cancelText: "Cancelar",
      type: "is-danger",
      hasIcon: true,
      onConfirm: () => {
        this.isLoading = true;
        this.miscellaneousExpensesService
          .deleteMiscellaneousExpenses(miscellaneousExpense.id)
          .then(() => {
            this.isLoading = false;
            this.miscellaneousExpenses.splice(
              this.miscellaneousExpenses.indexOf(miscellaneousExpense),
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

        this.$buefy.toast.open("Gasto vario eliminado!");
      }
    });
  }

  public clearFilters() {
    this.filtersApplied = false;
    this.miscellaneousExpensesFilters = new MiscellaneousExpensesFilters();
    this.getMiscellaneousExpenses();
  }

  public applyFilters() {
    this.isLoading = true;
    this.filtersApplied = true;
    this.getMiscellaneousExpenses();
  }

  getMiscellaneousExpenses() {
    this.isLoading = true;
    let rta: Promise<void>;
    if (this.filtersApplied) {
      rta = this.miscellaneousExpensesService
        .getTotalQtyByFilters(this.miscellaneousExpensesFilters)
        .then(qty => {
          this.totalPages = qty;
          return this.miscellaneousExpensesService.getByFiltersPageAndQty(
            this.miscellaneousExpensesFilters,
            (this.currentPage - 1) * this.perPage,
            this.perPage
          );
        })
        .then(response => {
          this.miscellaneousExpenses = response;
          this.isLoading = false;
        });
    } else {
      rta = this.miscellaneousExpensesService
        .getTotalQty()
        .then(qty => {
          this.totalPages = qty;
          return this.miscellaneousExpensesService.getByPageAndQty(
            (this.currentPage - 1) * this.perPage,
            this.perPage
          );
        })
        .then(response => {
          this.miscellaneousExpenses = response;
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
    this.getMiscellaneousExpenses();
  }

  created() {
    this.getMiscellaneousExpenses();
  }

  // created() {
  //   this.isLoading = true;
  //   this.miscellaneousExpensesService
  //     .getMiscellaneousExpenses()
  //     .then(response => {
  //       this.miscellaneousExpenses = response;
  //       this.isLoading = false;
  //     })
  //     .catch(e => {
  //       this.isLoading = false;
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
  //       console.log("error: ", e);
  //     });
  // }
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

.p-1 {
  padding: 1em;
}

.actionButton {
  margin-left: 5px;
}
</style>
