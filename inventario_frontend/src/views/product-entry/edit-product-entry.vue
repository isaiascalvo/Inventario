<template>
  <div>
    <div class="card column is-8 is-offset-2">
      <div class="card-content">
        <div class="media">
          <div class="media-content">
            <p class="title is-4">
              {{
                productEntry.id
                  ? "Editar Entrada/Salida de Productos"
                  : "Crear Entrada/Salida de Productos"
              }}
            </p>
          </div>
        </div>
        <div class="content">
          <form @submit.prevent="submit()" class="flex-text-left">
            <div class="columns">
              <div class="column">
                <b-field
                  label="Fecha y hora"
                  :type="
                    dateFocus && !fieldState(productEntry.date)
                      ? 'is-danger'
                      : ''
                  "
                  :message="
                    dateFocus && !fieldState(productEntry.date)
                      ? 'Debe seleccionar fecha y hora'
                      : ''
                  "
                >
                  <b-datetimepicker
                    v-model="productEntry.date"
                    rounded
                    placeholder="Seleccione fecha y hora"
                    icon="calendar-today"
                    trap-focus
                    horizontal-time-picker
                    editable
                    size="is-small"
                    @blur="dateFocus = true"
                  >
                  </b-datetimepicker>
                </b-field>
              </div>
              <div class="column">
                <b-field label="Tipo">
                  <b-select
                    v-model="productEntry.isEntry"
                    placeholder="Seleccione un Tipo"
                    expanded
                    size="is-small"
                    required
                    validation-message="Seleccione un Tipo"
                    @input="
                      () =>
                        (productEntry.vendorId = productEntry.cost = undefined)
                    "
                  >
                    <option :value="true">Entrada</option>
                    <option :value="false">Salida</option>
                  </b-select>
                </b-field>
              </div>
            </div>

            <div class="card" style="margin-bottom: 10px;">
              <b-table striped hoverable :data="productEntry.productEntryLines">
                <template slot-scope="props">
                  <b-table-column field="productId" label="Producto">
                    <b-field
                      v-if="!props.row.id"
                      :type="
                        prodCodNameDesc[getIndex(props.row)].focus &&
                        !fieldState(props.row.productId)
                          ? 'is-danger'
                          : ''
                      "
                      :message="
                        prodCodNameDesc[getIndex(props.row)].focus &&
                        !fieldState(props.row.productId)
                          ? 'Debe seleccionar un Producto'
                          : ''
                      "
                    >
                      <b-autocomplete
                        v-model="prodCodNameDesc[getIndex(props.row)].text"
                        :data="filteredNamesProducts(props.row)"
                        placeholder="ej.: Producto X"
                        icon="magnify"
                        icon-right="close-circle"
                        icon-right-clickable
                        @icon-right-click="
                          props.row.productId = prodCodNameDesc[
                            getIndex(props.row)
                          ].text = ''
                        "
                        size="is-small"
                        @select="
                          option => (props.row.productId = getProductId(option))
                        "
                        @blur="
                          prodCodNameDesc[getIndex(props.row)].focus = true
                        "
                      >
                        <template slot="empty"
                          >No se encontraron resultados</template
                        >
                      </b-autocomplete>
                    </b-field>
                    <span v-if="props.row.id">
                      {{ getProduct(props.row.productId) }}
                    </span>
                  </b-table-column>

                  <b-table-column field="quantity" label="Cantidad">
                    <b-field v-if="!props.row.id">
                      <b-input
                        v-model="props.row.quantity"
                        placeholder="Ingrese una cantidad"
                        size="is-small"
                        type="number"
                        step=".01"
                        required
                        min="1"
                        validation-message="Debe ingresar una cantidad mayor a 0"
                      ></b-input>
                    </b-field>
                    <span v-if="props.row.id">
                      {{ props.row.quantity }}
                    </span>
                  </b-table-column>

                  <b-table-column field="action" label="Acciones">
                    <b-button
                      @click="deleteProductEntryLine(props.row)"
                      type="is-small"
                      class="actionButton"
                    >
                      <b-icon icon="delete"></b-icon>
                    </b-button>
                  </b-table-column>
                </template>

                <template slot="footer">
                  <b-button
                    class="is-blue"
                    size="is-small"
                    @click="pushProduct()"
                  >
                    Agregar Producto
                  </b-button>
                </template>
              </b-table>
            </div>

            <div v-if="productEntry.isEntry" class="columns">
              <div class="column">
                <b-field label="Proveedor">
                  <b-select
                    v-model="productEntry.vendorId"
                    placeholder="Seleccione un Proveedor"
                    expanded
                    required
                    size="is-small"
                    validation-message="Seleccione un Proveedor"
                  >
                    <option
                      v-for="vendor in vendors"
                      :key="vendor.id"
                      :value="vendor.id"
                    >
                      {{ vendor.name }}
                    </option>
                  </b-select>
                </b-field>
              </div>

              <div class="column">
                <b-field label="Costo ($)">
                  <b-input
                    v-model="productEntry.cost"
                    type="number"
                    step=".01"
                    placeholder="Ingrese el costo de la compra"
                    size="is-small"
                    required
                    validation-message="Debe ingresar el costo de la compra"
                  ></b-input>
                </b-field>
              </div>
            </div>

            <div class="columns">
              <div class="column" v-if="productEntry.isEntry">
                <b-field label="Método de pago:">
                  <b-select
                    v-model="productEntry.paymentType"
                    placeholder="Seleccione un método de pago"
                    expanded
                    size="is-small"
                    required
                    validation-message="Seleccione un método de pago"
                  >
                    <option :value="0">Efectivo</option>
                    <option :value="1">Cuotas</option>
                    <option :value="2">Tarjeta de crédito</option>
                    <option :value="3">Tarjeta de débito</option>
                    <option :value="4">Cheque</option>
                  </b-select>
                </b-field>
              </div>

              <div class="column">
                <b-field label="Observaciones">
                  <b-input
                    v-model="productEntry.observations"
                    placeholder="Observaciones"
                    expanded
                    type="textarea"
                    size="is-small"
                  >
                  </b-input>
                </b-field>
              </div>
            </div>

            <div class="buttons">
              <b-button
                native-type="submit"
                class="is-success mr-1"
                :disabled="!formValid()"
              >
                {{ productEntry.id ? "Editar" : "Crear" }}
              </b-button>
              <b-button
                type="button"
                class="is-danger"
                @click="$router.push('/product-entry-list')"
              >
                Cancelar
              </b-button>
            </div>
          </form>
        </div>
      </div>
    </div>

    <b-loading is-full-page :active.sync="isLoading"></b-loading>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { NavigatorProductEntryService } from "../../services/product-entry-service";
import { ProductEntry } from "../../models/productEntry";
import {
  fieldStateValidation,
  formValidation
} from "../../utils/common-functions";
import { ProductEntryLine } from "../../models/productEntryLine";
import { Product } from "../../models/product";
import { NavigatorProductService } from "../../services/product-service";
import { NavigatorVendorService } from "@/services/vendor-service";
import { Vendor } from "@/models/vendor";

@Component
export default class EditProductEntry extends Vue {
  public errorDialog = false;
  public productEntry: ProductEntry = new ProductEntry();
  public products: Product[] = [];
  public isLoading = false;
  public vendors: Vendor[] = [];
  public prodCodNameDesc: { text: string; focus: boolean }[] = [];
  public dateFocus = false;

  public productEntryService: NavigatorProductEntryService = new NavigatorProductEntryService();
  public productService: NavigatorProductService = new NavigatorProductService();
  public vendorService: NavigatorVendorService = new NavigatorVendorService();

  fieldState(field: unknown) {
    return fieldStateValidation(field);
  }

  formValid() {
    const productsInvalid = this.productEntry.productEntryLines.filter(
      x => x.quantity === undefined || x.productId === undefined
    );

    if (productsInvalid.length > 0) {
      this.prodCodNameDesc.forEach((elem, index) => {
        this.productEntry.productEntryLines[index].productId =
          this.getProductId(elem.text) ?? "";
      });
      return false;
    }

    if (
      this.productEntry.isEntry &&
      (!this.fieldState(this.productEntry.vendorId) ||
        !this.fieldState(this.productEntry.cost) ||
        !this.fieldState(this.productEntry.paymentType))
    ) {
      return false;
    }

    const result = formValidation(this.productEntry as never);
    const nulleableProps = [
      "",
      "cost",
      "vendorId",
      "observations",
      "productEntryLines"
    ];
    const splitedResult = result.split(";");
    return splitedResult.every(x => nulleableProps.some(y => y === x));
  }

  filteredNamesProducts(row: ProductEntryLine) {
    const filtered = this.products.filter(option => {
      return (
        (option.name
          ? option.name
              .toString()
              .toLowerCase()
              .indexOf(
                (
                  this.prodCodNameDesc[this.getIndex(row)].text ?? ""
                ).toLowerCase()
              ) >= 0
          : false) &&
        !this.productEntry.productEntryLines.some(
          pl => pl.productId === option.id && pl.productId !== row.id
        )
      );
    });

    const codNameDescArray: string[] = [];

    filtered.forEach(x => {
      if (x.name) {
        codNameDescArray.push(x.name + " - " + x.description + " - " + x.code);
      }
    });
    return codNameDescArray;
  }

  getProductId(option: string) {
    const prod = this.products.find(
      x => x.name + " - " + x.description + " - " + x.code === option
    );
    return prod ? prod.id : undefined;
  }

  getIndex(row: ProductEntryLine) {
    const index = this.productEntry.productEntryLines.indexOf(row);
    return index;
  }

  getProduct(productId: string) {
    const product = this.products.find(x => x.id === productId);
    return product
      ? product.name + " - " + product.description + " - " + product.code
      : "";
  }

  pushProduct() {
    this.productEntry.productEntryLines.push(new ProductEntryLine());
    this.prodCodNameDesc.push({ text: "", focus: false });
  }

  deleteProductEntryLine(productEntryLine: ProductEntryLine) {
    const index = this.getIndex(productEntryLine);
    this.productEntry.productEntryLines.splice(index, 1);
    this.prodCodNameDesc.splice(index, 1);

    this.prodCodNameDesc.forEach((elem, index) => {
      this.productEntry.productEntryLines[index].productId =
        this.getProductId(elem.text) ?? "";
    });
    this.formValid();
  }

  public submit() {
    this.productEntry.productEntryLines.forEach(pel => {
      pel.quantity = pel.quantity ? +pel.quantity : undefined;
    });
    if (!this.productEntry.id) {
      this.newProductEntry();
    } else {
      this.updateProductEntry();
    }
  }

  public newProductEntry() {
    // const productEntry: ProductEntryForCreation = {
    //   date: this.productEntry.date,
    //   isEntry: this.productEntry.isEntry,
    //   productEntryLines: this.productEntry.productEntryLines
    // };
    this.isLoading = true;
    this.productEntryService
      .addProductEntry(this.productEntry)
      .then(() => {
        this.isLoading = false;
        this.$router.push({ name: "ProductEntryList" });
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
        this.$router.push({ name: "ProductEntryList" });
      });
  }

  public updateProductEntry() {
    // const productEntry: ProductEntry = {
    //   id: this.productEntry.id,
    //   date: this.productEntry.date,
    //   isEntry: this.productEntry.isEntry,
    //   productEntryLines: this.productEntry.productEntryLines
    // };
    this.isLoading = true;
    this.productEntryService
      .updateProductEntry(this.productEntry)
      .then(() => {
        this.$router.push({ name: "ProductEntryList" });
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
        this.$router.push({ name: "ProductEntryList" });
      });
  }

  created() {
    this.productEntry.id = this.$route.params.id;
    this.isLoading = true;
    this.productService
      .getProducts()
      .then(data => {
        this.products = data;
        return this.vendorService.getVendors();
      })
      .then(response => {
        this.vendors = response;
        if (this.productEntry.id) {
          this.productEntryService.getProductEntry(this.productEntry.id).then(
            response => {
              this.isLoading = false;
              this.productEntry = response as ProductEntry;
              this.productEntry.date = new Date(this.productEntry.date ?? "");
              if (this.productEntry.productEntryLines.length === 0) {
                this.productEntry.productEntryLines.push(
                  new ProductEntryLine()
                );
                this.prodCodNameDesc.push({ text: "", focus: false });
              }

              this.productEntry.productEntryLines.forEach(pel => {
                const prod =
                  this.products.find(x => x.id === pel.productId) ??
                  new Product();
                this.prodCodNameDesc.push({
                  text:
                    prod.name + " - " + prod.description + " - " + prod.code,
                  focus: true
                });
              });
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
              this.$router.push({ name: "ProductEntryList" });
            }
          );
        } else {
          this.isLoading = false;
          this.productEntry.productEntryLines.push(new ProductEntryLine());
          this.prodCodNameDesc.push({ text: "", focus: false });
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
        this.$router.push({ name: "ProductEntryList" });
      });
  }
}
</script>

<style>
.mr-1 {
  margin-right: 1em;
}

.actionButton {
  margin-left: 5px;
}

.buttons {
  margin-top: 10px;
}

table {
  border: 0px !important;
  font-size: 13px;
}

th {
  /* background-color: #dbdbdb; */
  background-color: #384caf4a;
}

.flex-text-left {
  display: flow-root;
  text-align: left;
}
</style>
