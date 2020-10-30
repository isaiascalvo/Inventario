<template>
  <div>
    <section class="hero is-light p-1">
      <div class="hero-head">
        <div class="level">
          <div>
            <h1 class="title is-6">Reporte de cuotas</h1>
          </div>
          <div>
            <b-field>
              <b-datetimepicker
                v-model="feesReportfilters.dateDateFrom"
                rounded
                placeholder="Fecha y hora desde"
                icon="calendar-today"
                trap-focus
                horizontal-time-picker
                editable
                position="is-bottom-left"
              >
              </b-datetimepicker>
              <p class="control">
                <b-button
                  icon-left="close"
                  type="is-dark"
                  @click="feesReportfilters.dateDateFrom = undefined"
                ></b-button>
              </p>
              <b-datetimepicker
                v-model="feesReportfilters.dateDateTo"
                rounded
                placeholder="Fecha y hora hasta"
                icon="calendar-today"
                trap-focus
                horizontal-time-picker
                editable
                position="is-bottom-left"
              >
              </b-datetimepicker>
              <p class="control">
                <b-button
                  icon-left="close"
                  type="is-dark"
                  @click="feesReportfilters.dateDateTo = undefined"
                ></b-button>
              </p>
              <p class="control">
                <b-button
                  @click="getFeesReport()"
                  class="buton"
                  type="is-primary"
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
      <b-table striped hoverable scrollable :data="feesReports" id="my-table">
        <template slot="empty">
          No hay datos registrados
        </template>
        <template slot-scope="props">
          <b-table-column field="date" label="Fecha y hora">
            {{ dateTimeToLocal(props.row.date) }}
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
          <b-table-column field="feesQty" label="Plazo" centered>
            {{ props.row.feesQty }}
          </b-table-column>
          <b-table-column field="totalAmount" label="Monto" centered>
            $ {{ formattedAmount(props.row.totalAmount) }}
          </b-table-column>
          <b-table-column field="capital" label="Capital" centered>
            $ {{ formattedAmount(props.row.capital) }}
          </b-table-column>
          <b-table-column field="interest" label="Interés" centered>
            $ {{ formattedAmount(props.row.interest) }}
          </b-table-column>
          <b-table-column field="feeValue" label="Cuota" centered>
            $ {{ formattedAmount(props.row.feeValue) }}
          </b-table-column>
        </template>
      </b-table>
    </div>

    <b-loading is-full-page :active.sync="isLoading"></b-loading>
  </div>
</template>

<script lang="ts">
import ModalClientPreview from "@/components/modals/ModalClientPreview.vue";
import { FeesReport } from "@/models/reports/feesReport";
import { dateTimeToLocal, formattedAmount } from "@/utils/common-functions";
import { Vue, Component } from "vue-property-decorator";
import { NavigatorReportService } from "../../services/report-service";
import { FeesReportFilters } from "../../models/filters/feesReportFilters";

@Component
export default class FeesReportList extends Vue {
  public feesReports: FeesReport[] = [];
  public reportsService: NavigatorReportService = new NavigatorReportService();
  public feesReportfilters: FeesReportFilters = new FeesReportFilters();
  public isLoading = false;
  // public year: number = new Date().getFullYear();

  formattedAmount(amount: number) {
    return formattedAmount(amount);
  }

  dateTimeToLocal(date: Date) {
    return dateTimeToLocal(date);
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

  getFeesReport() {
    this.isLoading = true;
    this.reportsService
      .getFeesReport(this.feesReportfilters)
      .then(response => {
        this.feesReports = response;
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
    // this.year = new Date().getFullYear();
    this.getFeesReport();
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
</style>
