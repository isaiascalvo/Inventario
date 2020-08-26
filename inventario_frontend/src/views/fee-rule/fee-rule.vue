<template>
  <div>
    <section class="hero is-light p-1">
      <div class="hero-head">
        <div class="level">
          <div>
            <h1 class="title is-6">Lista de Reglas</h1>
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
          </div>
        </div>
      </div>
    </section>

    <b-table
      striped
      hoverable
      scrollable
      :data="feeRules"
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
        No hay reglas registradas
      </template>
      <template slot-scope="props">
        <b-table-column field="product" label="Producto">
          {{ props.row.product.name }}
        </b-table-column>
        <b-table-column field="date" label="Fecha">
          {{ props.row.date }}
        </b-table-column>
        <b-table-column field="feesAmountTo" label="Cuotas desde">
          {{ props.row.feesAmountTo }}
        </b-table-column>
        <b-table-column field="percentage" label="Porcentage">
          {{ props.row.percentage }}
        </b-table-column>
        <b-table-column field="action" label="Acciones">
          <b-button
            tag="router-link"
            :to="'/feeRule/modify/' + props.row.id"
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

    <b-loading is-full-page :active.sync="isLoading"></b-loading>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { FeeRule } from "../../models/feeRule";
import { NavigatorFeeRuleService } from "../../services/fee-rule-service";

@Component
export default class FeeRuleList extends Vue {
  public feeRules: FeeRule[] = [];
  public feeRuleService: NavigatorFeeRuleService = new NavigatorFeeRuleService();

  public currentPage = 1;
  public perPage = 10;
  public errorDialog = false;
  public confirmDialog = false;
  public isLoading = false;

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

  created() {
    this.isLoading = true;
    this.feeRuleService
      .getFeeRules()
      .then(response => {
        this.feeRules = response as FeeRule[];
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

.p-1 {
  padding: 1em;
}
</style>
