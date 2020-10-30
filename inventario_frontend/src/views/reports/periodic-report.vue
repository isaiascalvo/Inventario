<template>
  <div>
    <section class="hero is-light p-1">
      <div class="hero-head">
        <div class="level">
          <div>
            <h1 class="title is-6">Reporte anual</h1>
          </div>
          <div>
            <b-field label-position="on-border" label="Año">
              <b-input
                v-model="year"
                placeholder="Año"
                size="is-small"
                type="number"
                required
                validation-message=" "
              ></b-input>
              <p class="control">
                <b-button
                  @click="getPeriodicReport()"
                  class="buton"
                  type="is-primary"
                  size="is-small"
                >
                  Buscar
                </b-button>
              </p>
            </b-field>
          </div>
        </div>
      </div>
    </section>

    <div>
      <b-table
        striped
        hoverable
        scrollable
        :data="periodicReports"
        id="my-table"
        :row-class="(row, index) => index === 12 && 'trColor'"
      >
        <template slot="empty">
          No hay datos registrados
        </template>
        <template slot-scope="props">
          <b-table-column field="period" label="Período">
            <strong>{{ props.row.period }}</strong>
          </b-table-column>
          <b-table-column field="fixedCosts" label="Gastos Fijos" centered>
            $ {{ formattedAmount(props.row.fixedCosts) }}
          </b-table-column>
          <b-table-column field="fixedCosts" label="Gastos Variables" centered>
            $ {{ formattedAmount(props.row.variableCosts) }}
          </b-table-column>
          <b-table-column field="commissions" label="Comisiones" centered>
            $ {{ formattedAmount(props.row.commissions) }}
          </b-table-column>
          <b-table-column field="sales" label="Ventas" centered>
            $ {{ formattedAmount(props.row.sales) }}
          </b-table-column>
          <b-table-column field="purchases" label="Compras" centered>
            $ {{ formattedAmount(props.row.purchases) }}
          </b-table-column>
          <b-table-column
            field="balance"
            label="Saldo"
            centered
            :cell-class="props.index !== 12 ? 'balanceColumnClass' : ''"
          >
            $ {{ formattedAmount(props.row.balance) }}
          </b-table-column>
        </template>
      </b-table>
    </div>

    <b-loading is-full-page :active.sync="isLoading"></b-loading>
  </div>
</template>

<script lang="ts">
import { formattedAmount } from "@/utils/common-functions";
import { Vue, Component } from "vue-property-decorator";
import { PeriodicReport } from "../../models/reports/periodicReport";
import { NavigatorReportService } from "../../services/report-service";

@Component
export default class PeriodicReportList extends Vue {
  public periodicReports: PeriodicReport[] = [];
  public reportsService: NavigatorReportService = new NavigatorReportService();

  public isLoading = false;
  public year: number = new Date().getFullYear();

  formattedAmount(amount: number) {
    return formattedAmount(amount);
  }

  getPeriodicReport() {
    if (!this.year) {
      this.$buefy.dialog.alert({
        title: "Error",
        message: "Debe ingresar un año.",
        type: "is-danger",
        hasIcon: true,
        icon: "times-circle",
        iconPack: "fa",
        ariaRole: "alertdialog",
        ariaModal: true
      });
      return;
    }

    this.isLoading = true;
    this.reportsService
      .getAnnualReport(this.year)
      .then(response => {
        this.periodicReports = response;
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

  created() {
    this.year = new Date().getFullYear();
    this.getPeriodicReport();
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

.monthClass {
  background: goldenrod;
}

.trColor {
  background: yellow !important;
}

.balanceColumnClass {
  background: darkgray !important;
}
</style>
