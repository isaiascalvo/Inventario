<template>
  <div>
    <b-card class="mx-auto elevation-5" max-width="1000">
      <b-card-title>{{ id ? "Update" : "Create" }} Rate</b-card-title>
      <v-form @submit.prevent="submit" v-model="formValid">
        <v-container fluid>
          <v-row align="center" dense>
            <v-col class="d-flex" cols="6">
              <v-select
                prepend-icon="language"
                v-model="sourceLanguage"
                :rules="srcLangRules"
                @change="filterLanguages"
                :items="languages"
                label="Src. Lang."
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

            <v-col class="d-flex" cols="6">
              <v-select
                prepend-icon="language"
                v-model="targetLanguage"
                :rules="tgtLangRules"
                :items="filteredLanguages"
                label="Target. Lang."
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
          </v-row>

          <v-row align="center" dense>
            <v-col class="d-flex" cols="6">
              <v-select
                prepend-icon="people"
                v-model="partner"
                :rules="partnerRules"
                :items="partners"
                label="Partner"
                clearable
                dense
                outlined
                @change="customerSelectValidate"
              >
                <template slot="selection" slot-scope="data">
                  {{ data.item.name }} - {{ data.item.cuit }}
                </template>
                <template slot="item" slot-scope="data">
                  {{ data.item.name }} - {{ data.item.cuit }}
                </template>
              </v-select>
            </v-col>

            <v-col class="d-flex" cols="6">
              <v-select
                prepend-icon="person"
                v-model="customer"
                :items="customers"
                label="Customer"
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
          </v-row>

          <!-- :rules="unitRules" -->
          <v-row align="center" dense>
            <v-col class="d-flex" cols="6">
              <v-select
                prepend-icon="view_comfy"
                v-model="unit"
                :items="units"
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

            <v-col class="d-flex" cols="6">
              <v-select
                prepend-icon="work"
                v-model="activity"
                :rules="activityRules"
                :items="activities"
                label="Activity"
                dense
                outlined
              >
                <template slot="selection" slot-scope="data">
                  {{ data.item.name }} - {{ data.item.description }}
                </template>
                <template slot="item" slot-scope="data">
                  {{ data.item.name }} - {{ data.item.description }}
                </template>
              </v-select>
            </v-col>
          </v-row>

          <v-row align="center" dense>
            <v-col class="d-flex" cols="6">
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
                    v-model="date"
                    label="Picker in menu"
                    prepend-icon="event"
                    readonly
                    v-on="on"
                    dense
                    outlined
                  ></v-text-field>
                </template>
                <v-date-picker v-model="date" no-title scrollable>
                  <v-spacer></v-spacer>
                  <v-btn text color="primary" @click="menu = false"
                    >Cancel</v-btn
                  >
                  <v-btn text color="primary" @click="$refs.menu.save(date)">
                    OK
                  </v-btn>
                </v-date-picker>
              </v-menu>
            </v-col>

            <v-col class="d-flex" cols="6">
              <v-menu
                ref="menu2"
                v-model="menu2"
                :close-on-content-click="false"
                :return-value.sync="date2"
                transition="scale-transition"
                offset-y
                min-width="290px"
              >
                <template v-slot:activator="{ on }">
                  <v-text-field
                    v-model="date2"
                    label="Picker in menu"
                    prepend-icon="event"
                    readonly
                    v-on="on"
                    dense
                    outlined
                  ></v-text-field>
                </template>
                <v-date-picker v-model="date2" no-title scrollable>
                  <v-spacer></v-spacer>
                  <v-btn text color="primary" @click="menu2 = false"
                    >Cancel</v-btn
                  >
                  <v-btn text color="primary" @click="$refs.menu2.save(date2)">
                    OK
                  </v-btn>
                </v-date-picker>
              </v-menu>
            </v-col>
          </v-row>

          <!-- :rules="currencyRules" -->
          <v-row align="center" dense>
            <v-col class="d-flex" cols="6">
              <v-select
                prepend-icon="attach_money"
                v-model="currency"
                :items="currencies"
                label="Currency"
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

            <v-col class="d-flex" cols="6">
              <v-text-field
                prepend-icon="event"
                v-model="fiscalYear"
                label="Fiscal Year"
                type="number"
                dense
                outlined
              ></v-text-field>
            </v-col>
          </v-row>

          <v-row>
            <v-col cols="6">
              <v-text-field
                prepend-icon="business_center"
                v-model="cost"
                :rules="costRules"
                label="Cost"
                type="number"
                dense
                outlined
              ></v-text-field>
            </v-col>
          </v-row>

          <v-btn
            type="submit"
            color="primary"
            class="mb-2 ml-8"
            :disabled="!formValid"
          >
            {{ id ? "Update" : "Create" }}
          </v-btn>
          <router-link to="/rates" tag="button">
            <v-btn type="button" color="red" dark class="mb-2 ml-2">
              Cancel
            </v-btn>
          </router-link>

          <v-row v-if="!formValid" align="center" dense>
            <v-col cols="12">
              <v-alert type="error" class="mb-2 ml-8" dense>
                You must complete all the fields.
              </v-alert>
            </v-col>
          </v-row>
        </v-container>
      </v-form>
    </b-card>

    <ErrorDialog
      :error="errorMsg"
      v-if="errorDialog"
      @close:dialog="errorDialog = $event"
    />
  </div>
</template>

<script lang="ts">
// import { Vue, Component } from "vue-property-decorator";
// import { RateForCreation, Rate } from "../../models/rate";
// import { Language } from "../../models/language";
// import { Unit } from "../../models/unit";
// import { Currency } from "../../models/currency";
// import { NavigatorRateService } from "../../services/rate-service";
// import { NavigatorLanguageService } from "../../services/language-service";
// import { NavigatorUnitService } from "../../services/unit-service";
// import { NavigatorCurrencyService } from "../../services/currency-service";
// import { DialogMsg } from "../../models/dialogMsg";
// import ErrorDialog from "../../components/dialogs/error-dialog.vue";
// import { Customer } from "../../models/customer";
// import { Vendor } from "../../models/vendor";
// import { NavigatorCustomerService } from "@/services/customer-service";
// import { NavigatorVendorService } from "@/services/vendor-service";
// import { NavigatorActivityService } from "@/services/activity-service";
// import { Activity } from "@/models/activity";
// import { Partner } from "../../models/partner";
// import { PartnerType } from "../../enums/partnerType";

// @Component({
//   components: {
//     ErrorDialog
//   }
// })
// export default class NewRate extends Vue {
//   public formValid = false;
//   public customerDisabled = false;
//   public errorDialog = false;
//   public errorMsg = new DialogMsg();
//   public date = new Date().toISOString().substr(0, 10);
//   public date2 = new Date().toISOString().substr(0, 10);
//   public menu = false;
//   public menu2 = false;
//   public id!: string;
//   public sourceLanguage? = new Language();
//   public targetLanguage? = new Language();
//   public contentTypeId = "02661649-f077-4698-aaf9-729683b216ff";
//   public unit? = new Unit(); //c703a741-12b9-4d67-98ca-a3c342ceb8c6
//   public currency? = new Currency();
//   public activity? = new Activity();
//   public partner? = new Customer();
//   public customer? = new Vendor();
//   public fiscalYear? = new Date().getFullYear();
//   public cost = 0;
//   public languages: Language[] = [];
//   public filteredLanguages: Language[] = [];
//   public units: Unit[] = [];
//   public currencies: Currency[] = [];
//   public activities: Activity[] = [];
//   public customers: Customer[] = [];
//   public partners: Vendor[] = [];
//   public rateService: NavigatorRateService = new NavigatorRateService();
//   public languageService: NavigatorLanguageService = new NavigatorLanguageService();
//   public unitService: NavigatorUnitService = new NavigatorUnitService();
//   public currencyService: NavigatorCurrencyService = new NavigatorCurrencyService();
//   public activityService: NavigatorActivityService = new NavigatorActivityService();
//   public customerService: NavigatorCustomerService = new NavigatorCustomerService();
//   public vendorService: NavigatorVendorService = new NavigatorVendorService();

//   public srcLangRules = [
//     (v: Language) => !!v.id || "Source Language is required"
//   ];
//   public tgtLangRules = [
//     (v: Language) => !!v.id || "Target Language is required"
//   ];
//   // public unitRules = [(v: Unit) => !!v.id || "Unit is required"];
//   // public currencyRules = [(v: Currency) => !!v.id || "Currency is required"];
//   public activityRules = [(v: Activity) => !!v?.id || "Activity is required"];
//   public partnerRules = [(v: Partner) => !!v?.id || "Partner is required"];
//   // public vendorRules = [(v: Vendor) => !!v.id || "Vendor is required"];
//   public costRules = [
//     (v: number) => !!v || "Cost is required",
//     (v: number) => v >= 0 || "Cost must be bigger than 0"
//   ];

//   // public customerVendorId() {
//   //   if (this.partner?.id || this.customer?.id) {
//   //     console.log("true");
//   //     return true;
//   //   } else {
//   //     console.log("false");
//   //     return "Customer or Vendor must be selected";
//   //   }
//   // }

//   public submit() {
//     if (!this.id) {
//       this.NewRate();
//     } else {
//       this.updateRate();
//     }
//   }

//   public NewRate() {
//     if (
//       this.sourceLanguage?.id &&
//       this.targetLanguage?.id &&
//       this.activity?.id &&
//       this.partner?.id
//     ) {
//       // const ms = Date.parse(this.date);
//       const rate: RateForCreation = {
//         sourceLanguageId: this.sourceLanguage.id,
//         targetLanguageId: this.targetLanguage.id,
//         contentTypeId: this.contentTypeId,
//         unitId: this.unit?.id,
//         currencyId: this.currency?.id,
//         activityId: this.activity.id,
//         partnerId: this.partner?.id,
//         customerId: this.customer?.id,
//         validFrom: new Date(this.date),
//         validTo: new Date(this.date2),
//         fiscalYear: 2020,
//         partnerType: this.getPartnerType(),
//         cost: this.cost
//       };

//       this.rateService
//         .addRate(rate)
//         .then(() => {
//           this.$router.push({ name: "RateList" });
//         })
//         .catch(e => {
//           this.errorMsg = {
//             title: "Error",
//             msg: "An unexpected error has occurred. please try again later."
//           };
//           this.errorDialog = true;
//           console.log("error: ", e);
//         });
//     }
//   }

//   public updateRate() {
//     if (
//       this.sourceLanguage?.id &&
//       this.targetLanguage?.id &&
//       this.activity?.id &&
//       this.partner?.id
//     ) {
//       const rate: Rate = {
//         id: this.id,
//         sourceLanguageId: this.sourceLanguage.id,
//         targetLanguageId: this.targetLanguage.id,
//         contentTypeId: this.contentTypeId,
//         unitId: this.unit?.id,
//         currencyId: this.currency?.id,
//         activityId: this.activity.id,
//         partnerId: this.partner?.id,
//         customerId: this.customer?.id,
//         validFrom: new Date(this.date),
//         validTo: new Date(this.date2),
//         fiscalYear: 2020,
//         partnerType: this.getPartnerType(),
//         cost: this.cost
//       };
//       this.rateService
//         .updateRate(rate)
//         .then(() => {
//           this.$router.push({ name: "RateList" });
//         })
//         .catch(e => {
//           this.errorMsg = {
//             title: "Error",
//             msg: "An unexpected error has occurred. please try again later."
//           };
//           this.errorDialog = true;
//           console.log("error: ", e);
//           this.$router.push({ name: "RateList" });
//         });
//     }
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
//         this.customers = response;
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

//     this.id = this.$route.params.id;
//     if (this.id) {
//       this.rateService.getRate(this.id).then(response => {
//         this.id = response.id;
//         this.sourceLanguage = this.languages.find(
//           x => x.id == response.sourceLanguageId
//         );
//         this.targetLanguage = this.languages.find(
//           x => x.id == response.targetLanguageId
//         );
//         this.contentTypeId = response.contentTypeId;
//         this.unit = this.units.find(x => x.id == response.unitId);
//         this.currency = this.currencies.find(x => x.id == response.currencyId);
//         this.activity = this.activities.find(x => x.id == response.activityId);
//         this.partner = this.partners.find(x => x.id == response.partnerId);
//         this.customer = this.customers.find(x => x.id == response.customerId);
//         this.cost = response.cost;
//         this.date = response.validFrom.toString().substr(0, 10);
//         this.date2 = response.validTo.toString().substr(0, 10);
//       });
//     }
//   }

//   public filterLanguages() {
//     this.filteredLanguages = this.languages.filter(
//       x => x.id !== this.sourceLanguage?.id
//     );
//   }

//   public getPartnerType(): PartnerType {
//     if (this.customers.some(x => x.id == this.partner?.id)) {
//       this.customer = undefined;
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
// }
</script>

<style></style>
