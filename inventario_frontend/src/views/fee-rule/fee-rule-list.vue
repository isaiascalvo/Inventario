<template>
  <div>
    <section class="hero is-light p-1">
      <div class="hero-head">
        <div class="level">
          <div>
            <h1 class="title is-6">Reglas de cuotas aplicables</h1>
          </div>
          <div>
            <b-button
              type="is-info"
              size="is-small"
              tag="router-link"
              to="/fee-rule/new"
              class="mx-1"
            >
              Nueva Regla
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
            <b-field label-position="on-border" label="Producto">
              <b-autocomplete
                v-model="prodCodNameDesc"
                :data="filteredProducts()"
                placeholder="ej.: Producto X"
                icon="magnify"
                size="is-small"
                icon-right="close-circle"
                icon-right-clickable
                @icon-right-click="
                  feeRuleFilters.productId = prodCodNameDesc = undefined
                "
                @select="option => getProductId(option)"
              >
                <template slot="empty">No se encontraron resultados</template>
              </b-autocomplete>
            </b-field>

            <b-field label-position="on-border" label="Cantidad de cuotas">
              <b-input
                v-model="feeRuleFilters.feesAmountTo"
                placeholder="Monto"
                size="is-small"
                type="number"
                icon-right="close-circle"
                icon-right-clickable
                @icon-right-click="feeRuleFilters.feesAmountTo = undefined"
              ></b-input>
            </b-field>

            <b-field label-position="on-border" label="Interés (%)">
              <b-input
                v-model="feeRuleFilters.percentage"
                placeholder="Porcentage de interés"
                size="is-small"
                type="number"
                icon-right="close-circle"
                icon-right-clickable
                @icon-right-click="feeRuleFilters.percentage = undefined"
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
        :data="feeRules"
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
          No hay reglas registradas
        </template>
        <template slot-scope="props">
          <b-table-column field="product" label="Producto">
            {{ props.row.product.name }}
          </b-table-column>
          <!-- <b-table-column field="date" label="Fecha">
          {{ props.row.date }}
        </b-table-column> -->
          <b-table-column field="feesAmountTo" label="Cantidad de cuotas">
            hasta {{ props.row.feesAmountTo }} cuotas
          </b-table-column>
          <b-table-column field="percentage" label="Interés">
            {{ props.row.percentage }}% de interés mensual
          </b-table-column>
          <b-table-column field="action" label="Acciones">
            <b-button
              tag="router-link"
              :to="'/fee-rule/modify/' + props.row.id"
              type="is-small"
            >
              <b-icon icon="pencil"></b-icon>
            </b-button>
            <b-button
              @click="deleteFeeRule(props.row)"
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
import { FeeRuleFilters } from "@/models/filters/feeRuleFilters";
import { Product } from "@/models/product";
import { NavigatorProductService } from "@/services/product-service";
import { Vue, Component } from "vue-property-decorator";
import { FeeRule } from "../../models/feeRule";
import { NavigatorFeeRuleService } from "../../services/fee-rule-service";

@Component
export default class FeeRuleList extends Vue {
  public feeRules: FeeRule[] = [];
  public feeRuleService: NavigatorFeeRuleService = new NavigatorFeeRuleService();
  public productService: NavigatorProductService = new NavigatorProductService();

  public currentPage = 1;
  public perPage = 10;
  public isLoading = false;
  public prodCodNameDesc = "";
  public feeRuleFilters: FeeRuleFilters = new FeeRuleFilters();
  public products: Product[] = [];
  public openFilters = false;
  public filtersApplied = false;
  public totalPages = 0;

  deleteFeeRule(feeRule: FeeRule) {
    this.$buefy.dialog.confirm({
      title: "Eliminando Regla",
      message:
        "Estás seguro que deseas <b>eliminar</b> el usuario? Esta acción no podrá deshacerse.",
      confirmText: "Eliminar Regla",
      cancelText: "Cancelar",
      type: "is-danger",
      hasIcon: true,
      onConfirm: () => {
        this.isLoading = true;
        this.feeRuleService
          .deleteFeeRule(feeRule.id)
          .then(() => {
            this.isLoading = false;
            this.feeRules.splice(this.feeRules.indexOf(feeRule), 1);
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
        this.$buefy.toast.open("Regla eliminada!");
      }
    });
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
        codNameDescArray.push(x.code + " - " + x.name + " - " + x.description);
      }
    });
    return codNameDescArray;
  }

  getProductId(option: string) {
    const prod = this.products.find(
      x => x.code + " - " + x.name + " - " + x.description === option
    );
    this.feeRuleFilters.productId = prod ? prod.id : undefined;
  }

  onPageChange(page: number) {
    this.currentPage = page;
    this.getFeeRules();
  }

  public clearFilters() {
    this.filtersApplied = false;
    this.feeRuleFilters = new FeeRuleFilters();
    this.getFeeRules();
  }

  public applyFilters() {
    this.isLoading = true;
    this.filtersApplied = true;
    this.getFeeRules();
  }

  getFeeRules(): Promise<void> {
    this.isLoading = true;
    let rta: Promise<void>;
    if (this.filtersApplied) {
      rta = this.feeRuleService
        .getTotalQtyByFilters(this.feeRuleFilters)
        .then(qty => {
          this.totalPages = qty;
          return this.feeRuleService.getByFiltersPageAndQty(
            this.feeRuleFilters,
            (this.currentPage - 1) * this.perPage,
            this.perPage
          );
        })
        .then(response => {
          this.feeRules = response;
          this.isLoading = false;
        });
    } else {
      rta = this.feeRuleService
        .getTotalQty()
        .then(qty => {
          this.totalPages = qty;
          return this.feeRuleService.getByPageAndQty(
            (this.currentPage - 1) * this.perPage,
            this.perPage
          );
        })
        .then(response => {
          this.feeRules = response;
          this.isLoading = false;
        });
    }
    return rta;
  }

  created() {
    this.getFeeRules()
      .then(() => {
        this.isLoading = true;
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

.p-1 {
  padding: 1em;
}

.actionButton {
  margin-left: 5px;
}
</style>
