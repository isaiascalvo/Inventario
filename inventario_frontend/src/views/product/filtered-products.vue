<template>
  <v-card class="mx-auto elevation-5" max-width="1800">
    <v-card-title>Filtered Rates</v-card-title>
    <v-form @submit.prevent="submit" v-model="formValid">
      <v-container fluid>
        <v-row align="center" dense>
          <v-col class="d-flex" cols="2">
            <v-select
              v-model="rateFilters.srcLangId"
              @change="filterLanguages"
              :items="languages"
              item-value="id"
              label="Src. Lang."
              clearable
              dense
              outlined
            >
              <template slot="selection" slot-scope="data">
                {{ data.item.name }} - {{ data.item.abbreviation }}
              </template>
              <template slot="item" slot-scope="data">
                {{ data.item.name }} - {{ data.item.abbreviation }}
              </template>
            </v-select>
          </v-col>

          <v-col class="d-flex" cols="2">
            <v-select
              v-model="rateFilters.tgtLangId"
              :items="filteredLanguages"
              item-value="id"
              label="Tgt. Lang."
              clearable
              dense
              outlined
            >
              <template slot="selection" slot-scope="data">
                {{ data.item.name }} - {{ data.item.abbreviation }}
              </template>
              <template slot="item" slot-scope="data">
                {{ data.item.name }} - {{ data.item.abbreviation }}
              </template>
            </v-select>
          </v-col>

          <v-col class="d-flex" cols="2">
            <v-select
              v-model="rateFilters.partnerId"
              :items="partners"
              @change="customerSelectValidate"
              item-value="id"
              label="Partner"
              clearable
              dense
              outlined
            >
              <template slot="selection" slot-scope="data">
                {{ data.item.name }}
              </template>
              <template slot="item" slot-scope="data">
                {{ data.item.name }} - {{ data.item.cuit }}
              </template>
            </v-select>
          </v-col>

          <v-col class="d-flex" cols="2">
            <v-select
              v-model="rateFilters.actId"
              :items="activities"
              item-value="id"
              label="Activity"
              clearable
              dense
              outlined
            >
              <template slot="selection" slot-scope="data">
                {{ data.item.name }}
              </template>
              <template slot="item" slot-scope="data">
                {{ data.item.name }} - {{ data.item.description }}
              </template>
            </v-select>
          </v-col>

          <v-col class="d-flex" cols="2">
            <v-btn
              type="button"
              color="primary"
              class="mb-7 ml-2"
              @click="filtersOnOff"
            >
              {{ filtersOn ? "Less Filters" : "More Filters" }}
            </v-btn>
          </v-col>

          <v-col cols="2" class="d-flex align-end flex-column">
            <v-btn
              type="submit"
              color="primary"
              class="mb-7 justify-end"
              :disabled="!formValid"
            >
              Search
            </v-btn>
          </v-col>
        </v-row>

        <v-row align="center" dense v-show="filtersOn">
          <v-col class="d-flex" cols="2">
            <v-select
              v-model="rateFilters.unitId"
              :items="units"
              item-value="id"
              label="Unit"
              clearable
              dense
              outlined
            >
              <template slot="selection" slot-scope="data">
                {{ data.item.name }} - {{ data.item.abbreviation }}
              </template>
              <template slot="item" slot-scope="data">
                {{ data.item.name }} - {{ data.item.abbreviation }}
              </template>
            </v-select>
          </v-col>

          <v-col class="d-flex" cols="2">
            <v-select
              v-model="rateFilters.currencyId"
              :items="currencies"
              item-value="id"
              label="Currency"
              clearable
              dense
              outlined
            >
              <template slot="selection" slot-scope="data">
                {{ data.item.name }}
              </template>
              <template slot="item" slot-scope="data">
                {{ data.item.name }} - {{ data.item.abbreviation }}
              </template>
            </v-select>
          </v-col>

          <v-col class="d-flex" cols="2">
            <v-select
              v-model="rateFilters.customerId"
              :items="customers"
              label="Customer"
              item-value="id"
              clearable
              dense
              outlined
              :disabled="customerDisabled"
            >
              <template slot="selection" slot-scope="data">
                {{ data.item.name }} - {{ data.item.cuit }}
              </template>
              <template slot="item" slot-scope="data">
                {{ data.item.name }} - {{ data.item.cuit }}
              </template>
            </v-select>
          </v-col>

          <v-col class="d-flex" cols="2">
            <v-text-field
              v-model="rateFilters.fiscalYear"
              label="Fiscal Year"
              type="number"
              dense
              outlined
            ></v-text-field>
          </v-col>
        </v-row>

        <v-row align="center" dense v-show="filtersOn">
          <v-col class="d-flex" cols="2">
            <v-menu
              ref="menu"
              v-model="menu"
              :close-on-content-click="false"
              :return-value.sync="date"
              transition="scale-transition"
              offset-y
              min-width="290px"
            >
              <template v-slot:activator="{ on }">
                <v-text-field
                  v-model="rateFilters.validFrom"
                  label="Picker in menu"
                  readonly
                  v-on="on"
                  dense
                  outlined
                  clearable
                ></v-text-field>
              </template>
              <v-date-picker
                v-model="rateFilters.validFrom"
                no-title
                scrollable
              >
                <v-spacer></v-spacer>
                <v-btn text color="primary" @click="menu = false">Cancel</v-btn>
                <v-btn
                  text
                  color="primary"
                  @click="$refs.menu.save(rateFilters.validFrom)"
                >
                  OK
                </v-btn>
              </v-date-picker>
            </v-menu>
          </v-col>

          <v-col class="d-flex" cols="2">
            <v-menu
              ref="menu2"
              v-model="menu2"
              :close-on-content-click="false"
              :return-value.sync="date"
              transition="scale-transition"
              offset-y
              min-width="290px"
            >
              <template v-slot:activator="{ on }">
                <v-text-field
                  v-model="rateFilters.validTo"
                  label="Picker in menu"
                  readonly
                  v-on="on"
                  dense
                  outlined
                  clearable
                ></v-text-field>
              </template>
              <v-date-picker v-model="rateFilters.validTo" no-title scrollable>
                <v-spacer></v-spacer>
                <v-btn text color="primary" @click="menu2 = false"
                  >Cancel</v-btn
                >
                <v-btn
                  text
                  color="primary"
                  @click="$refs.menu2.save(rateFilters.validTo)"
                >
                  OK
                </v-btn>
              </v-date-picker>
            </v-menu>
          </v-col>
        </v-row>
      </v-container>
    </v-form>

    <v-card v-if="submited && !rates.length" align="center">
      <v-col cols="2">
        <v-alert type="warning" class="mb-2 ml-8" dense align="left">
          No matches found.
        </v-alert>
      </v-col>
    </v-card>

    <v-data-table v-if="rates.length" :headers="headers" :items="rates">
      <template v-slot:item.validFrom="{ item }">
        <span>{{ item.validFrom.substr(0, 10) }}</span>
      </template>
      <template v-slot:item.validTo="{ item }">
        <span>{{ item.validTo.substr(0, 10) }}</span>
      </template>
      <template v-slot:item.action="{ item }">
        <router-link v-bind:to="'/rates/modify/' + item.id" tag="button">
          <v-icon small class="mr-2">
            edit
          </v-icon>
        </router-link>
        <v-icon small @click="openDeleteDialog(item.id)">
          delete
        </v-icon>
      </template>
    </v-data-table>

    <ErrorDialog
      :error="errorMsg"
      v-if="errorDialog"
      @close:dialog="errorDialog = $event"
    />

    <ConfirmDialog
      :msgDialog="confirmMsg"
      v-if="confirmDialog"
      @close:dialog="onClose"
    />
  </v-card>
</template>

<script lang="ts">
// import { Vue, Component } from "vue-property-decorator";
// import { Rate } from "../../models/rate";
// import { RateFilters } from "../../models/rateFilters";
// import { NavigatorRateService } from "../../services/rate-service";
// import ConfirmDialog from "../../components/dialogs/confirm-dialog.vue";
// import { DialogMsg } from "../../models/dialogMsg";
// import ErrorDialog from "../../components/dialogs/error-dialog.vue";
// import { Language } from "../../models/language";
// import { Unit } from "../../models/unit";
// import { Currency } from "../../models/currency";
// import { Activity } from "../../models/activity";
// import { Customer } from "../../models/customer";
// import { NavigatorLanguageService } from "@/services/language-service";
// import { NavigatorUnitService } from "@/services/unit-service";
// import { NavigatorActivityService } from "@/services/activity-service";
// import { NavigatorCurrencyService } from "@/services/currency-service";
// import { NavigatorCustomerService } from "@/services/customer-service";
// import { NavigatorVendorService } from "@/services/vendor-service";
// import { Partner } from "../../models/partner";
// import { PartnerType } from "../../enums/partnerType";

// @Component({
//   components: { ConfirmDialog, ErrorDialog }
// })
// export default class FilteredRates extends Vue {
//   public formValid = false;
//   public customerDisabled = false;
//   public filtersOn = false;
//   public submited = false;
//   public date = new Date().toISOString().substr(0, 10);
//   public date2 = new Date().toISOString().substr(0, 10);
//   public menu = false;
//   public menu2 = false;
//   public rates: Rate[] = [];
//   public rateFilters: RateFilters = new RateFilters();
//   public languages: Language[] = [];
//   public filteredLanguages: Language[] = [];
//   public units: Unit[] = [];
//   public currencies: Currency[] = [];
//   public activities: Activity[] = [];
//   public customers: Customer[] = [];
//   public partners: Partner[] = [];
//   public rateService: NavigatorRateService = new NavigatorRateService();
//   public languageService: NavigatorLanguageService = new NavigatorLanguageService();
//   public unitService: NavigatorUnitService = new NavigatorUnitService();
//   public currencyService: NavigatorCurrencyService = new NavigatorCurrencyService();
//   public activityService: NavigatorActivityService = new NavigatorActivityService();
//   public customerService: NavigatorCustomerService = new NavigatorCustomerService();
//   public vendorService: NavigatorVendorService = new NavigatorVendorService();

//   public errorDialog = false;
//   public confirmDialog = false;
//   public errorMsg = new DialogMsg();
//   public confirmMsg = new DialogMsg();
//   public headers = [
//     {
//       text: "Source Lang.",
//       align: "left",
//       value: "sourceLanguage.name"
//     },
//     { text: "Target Lang.", value: "targetLanguage.name" },
//     { text: "Partner", value: "partner.name" },
//     { text: "Customer", value: "customer.name" },
//     { text: "Unit", value: "unit.name" },
//     { text: "Activity", value: "activity.name" },
//     { text: "Currency", value: "currency.name" },
//     { text: "Valid From", value: "validFrom", dataType: "Date" },
//     { text: "Valid To", value: "validTo", dataType: "Date" },
//     { text: "Fiscal Year", value: "fiscalYear" },
//     { text: "Cost", value: "cost" },
//     { text: "Actions", value: "action", sortable: false }
//   ];

//   public submit() {
//     this.rateService.getRatesFiltered(this.rateFilters).then(response => {
//       this.rates = response;
//       this.submited = true;
//     });
//   }

//   deleteRate(rate: Rate) {
//     this.rateService
//       .deleteRate(rate.id)
//       .then(() => {
//         this.rates.splice(this.rates.indexOf(rate), 1);
//       })
//       .catch(e => {
//         this.errorMsg = {
//           title: "Error",
//           msg: "An unexpected error has occurred. please try again later."
//         };
//         this.errorDialog = true;
//         console.log("error: ", e);
//       });
//   }

//   onClose(elemId: string) {
//     this.confirmDialog = false;
//     if (elemId.trim() !== "") {
//       const rate = this.rates.find(x => x.id === elemId);
//       if (rate) {
//         this.deleteRate(rate);
//       }
//     }
//   }

//   openDeleteDialog(elemId: string) {
//     this.confirmMsg = {
//       elemId: elemId,
//       title: "Warning",
//       msg: "Are you sure you want to delete the rate?"
//     };
//     this.confirmDialog = true;
//   }

//   created() {
//     this.languageService
//       .getLanguages()
//       .then(response => {
//         this.languages = response;
//         this.filteredLanguages = response;
//       })
//       .catch(error => {
//         console.log(error);
//       });

//     this.unitService
//       .getUnits()
//       .then(response => {
//         this.units = response;
//       })
//       .catch(error => {
//         console.log(error);
//       });

//     this.currencyService
//       .getCurrencies()
//       .then(response => {
//         this.currencies = response;
//       })
//       .catch(error => {
//         this.errorMsg = {
//           title: "Error",
//           msg: "An unexpected error has occurred. please try again later."
//         };
//         this.errorDialog = true;
//         console.log(error);
//       });

//     this.activityService
//       .getActivities()
//       .then(response => {
//         this.activities = response;
//       })
//       .catch(error => {
//         this.errorMsg = {
//           title: "Error",
//           msg: "An unexpected error has occurred. please try again later."
//         };
//         this.errorDialog = true;
//         console.log(error);
//       });

//     this.customerService
//       .getCustomers()
//       .then(response => {
//         this.partners = this.partners.concat(response);
//         this.customers = response;
//       })
//       .catch(error => {
//         this.errorMsg = {
//           title: "Error",
//           msg: "An unexpected error has occurred. please try again later."
//         };
//         this.errorDialog = true;
//         console.log(error);
//       });

//     this.vendorService
//       .getVendors()
//       .then(response => {
//         this.partners = this.partners.concat(response);
//       })
//       .catch(error => {
//         this.errorMsg = {
//           title: "Error",
//           msg: "An unexpected error has occurred. please try again later."
//         };
//         this.errorDialog = true;
//         console.log(error);
//       });
//   }

//   public filterLanguages() {
//     this.filteredLanguages = this.languages.filter(
//       x => x.id !== this.rateFilters.srcLangId
//     );
//   }

//   public getPartnerType(): PartnerType {
//     if (this.customers.some(x => x.id == this.rateFilters.partnerId)) {
//       this.rateFilters.customerId = undefined;
//       return PartnerType.Customer;
//     }
//     return PartnerType.Vendor;
//   }

//   private customerSelectValidate(): void {
//     if (this.getPartnerType() == PartnerType.Vendor) {
//       this.customerDisabled = false;
//     } else {
//       this.customerDisabled = true;
//     }
//   }

//   private filtersOnOff() {
//     const src = this.rateFilters.srcLangId;
//     const tgt = this.rateFilters.tgtLangId;
//     const partner = this.rateFilters.partnerId;
//     const act = this.rateFilters.actId;
//     this.filtersOn = !this.filtersOn;
//     this.rateFilters = new RateFilters();
//     this.rateFilters.srcLangId = src;
//     this.rateFilters.tgtLangId = tgt;
//     this.rateFilters.partnerId = partner;
//     this.rateFilters.actId = act;
//   }
// }
</script>

<style>
/* table,
table.bordered td,
table.bordered th {
  border: 1px black solid;
  margin: auto;
  margin-bottom: 10px;
}

.align-center {
  text-align: center;
} */
</style>
