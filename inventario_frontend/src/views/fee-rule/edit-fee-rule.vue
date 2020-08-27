<template>
  <div>
    <div class="card column is-4 is-offset-4">
      <div class="card-content">
        <div class="media">
          <div class="media-content">
            <p class="title is-4">
              {{ feeRule.id ? "Editar Regla" : "Crear Regla" }}
            </p>
          </div>
        </div>

        <b-tabs position="is-centered" class="block">
          <b-tab-item label="Por Producto">
            <form @submit.prevent="submit()" class="flex-text-left">
              <b-field
                label="Producto"
                :type="
                  prodFocus && !fieldState(feeRule.productId) ? 'is-danger' : ''
                "
                :message="
                  prodFocus && !fieldState(feeRule.productId)
                    ? 'Debe seleccionar un Producto'
                    : ''
                "
              >
                <b-autocomplete
                  v-model="prodCodNameDesc"
                  :data="filteredProducts()"
                  placeholder="ej.: Producto X"
                  icon="magnify"
                  icon-right="close-circle"
                  icon-right-clickable
                  @icon-right-click="
                    feeRule.productId = prodCodNameDesc = undefined
                  "
                  @select="option => getProductId(option)"
                  @blur="prodFocus = true"
                >
                  <template slot="empty">No se encontraron resultados</template>
                </b-autocomplete>
              </b-field>

              <b-field label="Cantidad de cuotas:">
                <b-numberinput
                  controls-position="compact"
                  controls-rounded
                  v-model="feeRule.feesAmountTo"
                  placeholder="Ingrese la cantidad"
                  min="1"
                  type="is-dark"
                  required
                  validation-message="Debe ingresar una cantidad"
                ></b-numberinput>
              </b-field>

              <b-field label="Porcentaje de interés mensual:">
                <b-numberinput
                  controls-position="compact"
                  controls-rounded
                  v-model="feeRule.percentage"
                  placeholder="Ingrese el porcentaje"
                  min="0"
                  type="is-dark"
                  required
                  validation-message="Debe ingresar un porcentaje"
                ></b-numberinput>
              </b-field>

              <b-button
                native-type="submit"
                class="is-success mr-1"
                :disabled="!formValid()"
              >
                {{ feeRule.id ? "Editar" : "Crear" }}
              </b-button>
              <b-button
                type="button"
                class="is-danger"
                @click="$router.push('/fee-rule-list')"
              >
                Cancelar
              </b-button>
            </form>
          </b-tab-item>

          <b-tab-item
            label="Por Categoría"
            :disabled="feeRule.id !== undefined"
          >
            <form
              @submit.prevent="newFeeRuleByCategory()"
              class="flex-text-left"
            >
              <b-field
                label="Categoría"
                :type="
                  catFocus && !fieldState(feeRuleByCategory.categoryId)
                    ? 'is-danger'
                    : ''
                "
                :message="
                  catFocus && !fieldState(feeRuleByCategory.categoryId)
                    ? 'Debe seleccionar una categoría'
                    : ''
                "
              >
                <b-autocomplete
                  v-model="catNameDesc"
                  :data="filteredCategories()"
                  placeholder="ej.: Categoría X"
                  icon="magnify"
                  icon-right="close-circle"
                  icon-right-clickable
                  @icon-right-click="
                    feeRuleByCategory.categoryId = catNameDesc = undefined
                  "
                  @select="option => getCategoryId(option)"
                  @blur="catFocus = true"
                >
                  <template slot="empty">No se encontraron resultados</template>
                </b-autocomplete>
              </b-field>

              <b-field label="Cantidad de cuotas:">
                <b-numberinput
                  controls-position="compact"
                  controls-rounded
                  v-model="feeRuleByCategory.feesAmountTo"
                  placeholder="Ingrese la cantidad"
                  min="1"
                  type="is-dark"
                  required
                  validation-message="Debe ingresar una cantidad"
                ></b-numberinput>
              </b-field>

              <b-field label="Porcentaje de interés mensual:">
                <b-numberinput
                  controls-position="compact"
                  controls-rounded
                  v-model="feeRuleByCategory.percentage"
                  placeholder="Ingrese el porcentaje"
                  min="0"
                  type="is-dark"
                  required
                  validation-message="Debe ingresar un porcentaje"
                ></b-numberinput>
              </b-field>

              <b-button
                native-type="submit"
                class="is-success mr-1"
                :disabled="!categFormValid()"
              >
                Crear
              </b-button>
              <b-button
                type="button"
                class="is-danger"
                @click="$router.push('/fee-rule-list')"
              >
                Cancelar
              </b-button>
            </form>
          </b-tab-item>
        </b-tabs>
      </div>
    </div>

    <b-loading is-full-page :active.sync="isLoading"></b-loading>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { NavigatorFeeRuleService } from "../../services/fee-rule-service";
import { NavigatorProductService } from "@/services/product-service";
import { FeeRule } from "../../models/feeRule";
import { Product } from "@/models/product";
import {
  fieldStateValidation,
  formValidation
} from "../../utils/common-functions";
import { NavigatorCategoryService } from "@/services/category-service";
import { Category } from "@/models/category";
import { FeeRuleByCategory } from "@/models/feeRuleByCategory";

@Component
export default class EditFeeRule extends Vue {
  public feeRule: FeeRule = new FeeRule();
  public feeRuleByCategory: FeeRuleByCategory = new FeeRuleByCategory();
  public feeRuleService: NavigatorFeeRuleService = new NavigatorFeeRuleService();
  public productService: NavigatorProductService = new NavigatorProductService();
  public categoryService: NavigatorCategoryService = new NavigatorCategoryService();
  public products: Product[] = [];
  public categories: Category[] = [];
  public isLoading = false;
  public prodFocus = false;
  public catFocus = false;
  public dateFocus = false;
  public prodCodNameDesc = "";
  public catNameDesc = "";

  fieldState(field: unknown) {
    return fieldStateValidation(field);
  }

  formValid() {
    const result = formValidation(this.feeRule as never);
    return result === "date;product;" || result === "product;";
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
    this.feeRule.productId = prod ? prod.id : undefined;
  }

  categFormValid() {
    const result = formValidation(this.feeRuleByCategory as never);
    return result === "";
  }

  filteredCategories() {
    const filtered = this.categories.filter(option => {
      return option.name
        ? option.name
            .toString()
            .toLowerCase()
            .indexOf((this.catNameDesc ?? "").toLowerCase()) >= 0
        : false;
    });

    const nameDescArray: string[] = [];

    filtered.forEach(x => {
      if (x.name) {
        nameDescArray.push(x.name + " - " + x.description);
      }
    });
    return nameDescArray;
  }

  getCategoryId(option: string) {
    const cat = this.categories.find(
      x => x.name + " - " + x.description === option
    );
    this.feeRuleByCategory.categoryId = cat ? cat.id : undefined;
  }

  public submit() {
    if (!this.feeRule.id) {
      this.newFeeRule();
    } else {
      this.updateFeeRule();
    }
  }

  public newFeeRule() {
    this.isLoading = true;
    this.feeRuleService
      .addFeeRule(this.feeRule)
      .then(() => {
        this.isLoading = false;
        this.$router.push({ name: "FeeRuleList" });
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
        this.$router.push({ name: "FeeRuleList" });
      });
  }

  public updateFeeRule() {
    this.isLoading = true;
    this.feeRuleService
      .updateFeeRule(this.feeRule)
      .then(() => {
        this.isLoading = false;
        this.$router.push({ name: "FeeRuleList" });
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
        this.$router.push({ name: "FeeRuleList" });
      });
  }

  public newFeeRuleByCategory() {
    this.isLoading = true;
    this.feeRuleService
      .addFeeRuleByCategory(this.feeRuleByCategory)
      .then(() => {
        this.isLoading = false;
        this.$router.push({ name: "FeeRuleList" });
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
        this.$router.push({ name: "FeeRuleList" });
      });
  }

  created() {
    this.feeRule.id = this.$route.params.id;
    this.isLoading = true;
    this.productService
      .getProducts()
      .then(response => {
        this.isLoading = false;
        this.products = response;
        if (this.feeRule.id) {
          this.isLoading = true;
          this.feeRuleService.getFeeRule(this.feeRule.id).then(
            response => {
              this.feeRule = response as FeeRule;
              this.feeRule.date = new Date(this.feeRule.date ?? "");
              const pp =
                this.products.find(x => x.id === this.feeRule.productId) ??
                new Product();
              this.prodCodNameDesc =
                pp.code + " - " + pp.name + " - " + pp.description;
              this.isLoading = false;
            },
            error => {
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
              console.log(error);
            }
          );
        } else {
          this.feeRule.feesAmountTo = 1;
          this.feeRule.percentage = 0;
          this.feeRuleByCategory.feesAmountTo = 1;
          this.feeRuleByCategory.percentage = 0;
          this.isLoading = true;
          this.categoryService.getCategories().then(
            response => {
              this.categories = response;
              this.isLoading = false;
            },
            error => {
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
              console.log(error);
            }
          );
        }
      })
      .catch(error => {
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
        console.log(error);
      });
  }
}
</script>

<style>
.mr-1 {
  margin-right: 1em;
}
.flex-text-left {
  display: flow-root;
  text-align: left;
}
</style>
