<template>
  <v-card class="mx-auto elevation-5" max-width="1800">
    <v-card-title>Find One Rate</v-card-title>
    <v-form @submit.prevent="submit" v-model="formValid">
      <v-container fluid>
        <v-row align="center" dense>
          <v-col class="d-flex" cols="2">
            <v-select
              v-model="rateFilters.srcLangId"
              :items="languages"
              item-value="id"
              label="Src. Lang."
              :rules="requiredRule"
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
              :rules="requiredRule"
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
              :rules="requiredRule"
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
              :rules="requiredRule"
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

    <v-divider></v-divider>

    <v-card v-if="submited && !rate" align="center">
      <v-col cols="2">
        <v-alert type="warning" class="mb-2 ml-8" dense align="left">
          No matches found.
        </v-alert>
      </v-col>
    </v-card>

    <v-card
      v-if="rate.id"
      align="center"
      color="#009688"
      max-width="400"
      class="mx-auto my-2"
    >
      <v-card-title class="subheading font-weight-bold">
        Rate
        <v-spacer></v-spacer>
        <router-link v-bind:to="'/rates/modify/' + rate.id" tag="button">
          <v-icon small class="mr-2">
            edit
          </v-icon>
        </router-link>
        <v-icon small @click="openDeleteDialog(rate.id)">
          delete
        </v-icon>
      </v-card-title>

      <v-divider></v-divider>

      <v-list dense color="#90A4AE">
        <v-list-item>
          <v-list-item-content class="font-weight-bold">
            Source Language:
          </v-list-item-content>
          <v-list-item-content class="align-end">
            {{ rate.sourceLanguage.name }}
          </v-list-item-content>
        </v-list-item>

        <v-list-item>
          <v-list-item-content class="font-weight-bold">
            Target Language:
          </v-list-item-content>
          <v-list-item-content class="align-end">
            {{ rate.targetLanguage.name }}
          </v-list-item-content>
        </v-list-item>

        <v-list-item>
          <v-list-item-content class="font-weight-bold">
            Partner:
          </v-list-item-content>
          <v-list-item-content class="align-end">
            {{ rate.partner.name }}
          </v-list-item-content>
        </v-list-item>

        <v-list-item>
          <v-list-item-content class="font-weight-bold">
            Customer:
          </v-list-item-content>
          <v-list-item-content class="align-end">
            {{ rate.customer.name }}
          </v-list-item-content>
        </v-list-item>

        <v-list-item>
          <v-list-item-content class="font-weight-bold">
            Activity:
          </v-list-item-content>
          <v-list-item-content class="align-end">
            {{ rate.activity.name }}
          </v-list-item-content>
        </v-list-item>

        <v-list-item>
          <v-list-item-content class="font-weight-bold">
            Unit:
          </v-list-item-content>
          <v-list-item-content class="align-end">
            {{ rate.unit.name }}
          </v-list-item-content>
        </v-list-item>

        <v-list-item>
          <v-list-item-content class="font-weight-bold">
            Currency:
          </v-list-item-content>
          <v-list-item-content class="align-end">
            {{ rate.currency.name }}
          </v-list-item-content>
        </v-list-item>

        <v-list-item>
          <v-list-item-content class="font-weight-bold">
            Valid From:
          </v-list-item-content>
          <v-list-item-content class="align-end">
            {{ rate.validFrom.substr(0, 10) }}
          </v-list-item-content>
        </v-list-item>

        <v-list-item>
          <v-list-item-content class="font-weight-bold">
            Valid To:
          </v-list-item-content>
          <v-list-item-content class="align-end">
            {{ rate.validTo.substr(0, 10) }}
          </v-list-item-content>
        </v-list-item>

        <v-list-item>
          <v-list-item-content class="font-weight-bold">
            Fiscal Year:
          </v-list-item-content>
          <v-list-item-content class="align-end">
            {{ rate.fiscalYear }}
          </v-list-item-content>
        </v-list-item>

        <v-list-item>
          <v-list-item-content class="font-weight-bold">
            Cost:
          </v-list-item-content>
          <v-list-item-content class="align-end">
            {{ rate.cost }}
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-card>

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
// export default class FindOneRate extends Vue {
//   public formValid = false;
//   public customerDisabled = false;
//   public filtersOn = false;
//   public submited = false;
//   public date = new Date().toISOString().substr(0, 10);
//   public date2 = new Date().toISOString().substr(0, 10);
//   public menu = false;
//   public menu2 = false;
//   public rate: Rate = new Rate();
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
//     { text: "Fiscal Year", value: "fiscalYear", dataType: "Date" },
//     { text: "Cost", value: "cost" },
//     { text: "Actions", value: "action", sortable: false }
//   ];

//   public requiredRule = [(v: string) => !!v || "Required"];

//   public submit() {
//     this.rateFilters.partnerType = this.getPartnerType();
//     this.rateService.getOneRateByFilters(this.rateFilters).then(response => {
//       this.submited = true;
//       this.rate = response;
//     });
//   }

//   deleteRate(rate: Rate) {
//     this.rateService
//       .deleteRate(rate.id)
//       .then(() => {
//         this.$router.push({ name: "RateList" });
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
//       if (this.rate) {
//         this.deleteRate(this.rate);
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

<style></style>
