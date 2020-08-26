<template>
  <div>
    <div class="card  column is-4 is-offset-4">
      <div class="card-content">
        <div class="media">
          <div class="media-content">
            <p class="title is-4">
              {{ sale.id ? "Editar Venta" : "Crear Venta" }}
            </p>
          </div>
        </div>

        <form @submit.prevent="submit()" class="flex-text-left">
          <b-steps
            v-model="activeStep"
            animated
            rounded
            has-navigation
            icon-prev="chevron-left"
            icon-next="chevron-right"
            label-position="bottom"
            mobile-mode="minimalist"
          >
            <b-step-item step="1" label="Paso 1" clickable>
              <!-- <h1 class="title has-text-centered">Account</h1> -->
              <b-field label="Cliente comprador:">
                <b-autocomplete
                  v-model="sale.clientName"
                  :data="filteredClients()"
                  placeholder="ej.: Cliente X"
                  icon="magnify"
                  icon-right="close-circle"
                  icon-right-clickable
                  @icon-right-click="
                    sale.clientId = sale.clientName = undefined
                  "
                  @select="option => getClientId(option)"
                  @blur="clientFocus = true"
                >
                  <template slot="empty">No se encontraron resultados</template>
                </b-autocomplete>
              </b-field>

              <b-field
                label="Fecha y hora de venta"
                :type="dateFocus && !fieldState(sale.date) ? 'is-danger' : ''"
                :message="
                  dateFocus && !fieldState(sale.date)
                    ? 'Debe seleccionar fecha y hora'
                    : ''
                "
              >
                <b-datetimepicker
                  v-model="sale.date"
                  rounded
                  placeholder="Seleccione fecha y hora"
                  icon="calendar-today"
                  trap-focus
                  horizontal-time-picker
                  editable
                  @input="getPriceValue()"
                  @blur="dateFocus = true"
                >
                </b-datetimepicker>
              </b-field>

              <b-field
                label="Producto"
                :type="
                  prodFocus && !fieldState(sale.productId) ? 'is-danger' : ''
                "
                :message="
                  prodFocus && !fieldState(sale.productId)
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
                    sale.productId = prodCodNameDesc = undefined
                  "
                  @select="option => getProductId(option)"
                  @blur="prodFocus = true"
                >
                  <template slot="empty">No se encontraron resultados</template>
                </b-autocomplete>
              </b-field>

              <b-field label="Cantidad:">
                <b-numberinput
                  controls-position="compact"
                  controls-rounded
                  v-model="sale.quantity"
                  placeholder="Ingrese la cantidad"
                  min="1"
                  type="is-dark"
                  required
                  validation-message="Debe ingresar una cantidad"
                ></b-numberinput>
              </b-field>

              <b-field>
                <p class="pMargin"><strong>Precio Unitario: $</strong></p>
                <b-input :value="getUnitPrice()" disabled></b-input>
              </b-field>
            </b-step-item>

            <b-step-item step="2" label="Paso 2" clickable>
              <!-- <h1 class="title has-text-centered">Profile</h1> -->

              <b-field label="Método de pago:">
                <b-select
                  v-model="sale.paymentType"
                  placeholder="Seleccione un método de pago"
                  expanded
                  required
                  validation-message="Seleccione un método de pago"
                  @input="setPayment()"
                >
                  <option :value="0">Efectivo</option>
                  <option :value="1">Cuotas</option>
                  <option :value="2">Tarjeta de crédito</option>
                  <option :value="3">Tarjeta de débito</option>
                  <option :value="4">Cheque</option>
                </b-select>
              </b-field>

              <template v-if="sale.paymentType === 0">
                <b-field label="Descuento (%):">
                  <b-numberinput
                    v-model="sale.payment.discount"
                    placeholder="Ingrese el descuento"
                    min="0"
                    type="is-dark"
                    required
                    validation-message="Debe ingresar el descuento"
                  ></b-numberinput>
                </b-field>
              </template>

              <template v-if="sale.paymentType === 1">
                <b-field label="Cantidad de cuotas:">
                  <b-numberinput
                    controls-position="compact"
                    controls-rounded
                    v-model="sale.payment.quantity"
                    placeholder="Ingrese la cantidad de cuotas"
                    min="1"
                    type="is-dark"
                    required
                    validation-message="Ingrese la cantidad de cuotas"
                  ></b-numberinput>
                </b-field>

                <b-field>
                  <p class="pMargin">
                    <strong>Cuotas de $</strong>
                  </p>
                  <b-input :value="getValorCuota()" disabled></b-input>
                </b-field>
              </template>
            </b-step-item>

            <b-step-item step="3" label="Paso Final" clickable>
              <b-field>
                <p class="pMargin">
                  <strong>Importe Total: $</strong>
                </p>
                <b-input
                  v-model="sale.amount"
                  :value="getImporte()"
                  disabled
                ></b-input>
              </b-field>
            </b-step-item>

            <template slot="navigation" slot-scope="{ previous, next }">
              <b-button
                outlined
                :disabled="previous.disabled"
                @click.prevent="previous.action"
                class="mx-1"
              >
                Atrás
              </b-button>
              <b-button
                v-if="activeStep !== 2"
                outlined
                :disabled="next.disabled"
                @click.prevent="next.action"
              >
                Siguiente
              </b-button>
              <b-button
                v-if="activeStep === 2"
                native-type="submit"
                class="is-success mr-1"
                :disabled="!formValid()"
              >
                Confirmar
              </b-button>
              <b-button
                type="button"
                class="is-danger"
                style="float: right;"
                @click="$router.push('/sale-list')"
              >
                Cancelar
              </b-button>
            </template>
          </b-steps>
        </form>
      </div>
    </div>

    <b-loading is-full-page :active.sync="isLoading"></b-loading>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { NavigatorSaleService } from "../../services/sale-service";
import { Sale } from "../../models/sale";
import {
  fieldStateValidation,
  formValidation
} from "../../utils/common-functions";
import { Product } from "../../models/product";
import { Client } from "../../models/client";
import { NavigatorProductService } from "../../services/product-service";
import { NavigatorClientService } from "../../services/client-service";
import {
  OwnFeesForCreation,
  CashForCreation,
  CreditCardForCreation,
  DebitCardForCreation,
  ChequeForCreation
} from "@/models/payment";
import { paymentTypes } from "@/enums/paymentTypes";

@Component
export default class EditSale extends Vue {
  public errorDialog = false;
  public sale: Sale = new Sale();
  public products: Product[] = [];
  public clients: Client[] = [];
  public saleService: NavigatorSaleService = new NavigatorSaleService();
  public productService: NavigatorProductService = new NavigatorProductService();
  public clientService: NavigatorClientService = new NavigatorClientService();
  public isLoading = false;
  public priceValue: number | undefined = undefined;
  public monthNames = [
    "Enero",
    "Febrero",
    "Marzo",
    "Abril",
    "Mayo",
    "Junio",
    "Julio",
    "Agosto",
    "Septiembre",
    "Octubre",
    "Noviembre",
    "Diceimbre"
  ];
  public dayNames = ["Dom", "Lun", "Mar", "Mie", "Jue", "Vie", "Sab"];
  public prodCodNameDesc = "";
  public prodFocus = false;
  public dateFocus = false;
  public activeStep = 0;

  fieldState(field: unknown) {
    return fieldStateValidation(field);
  }

  formValid() {
    const result = formValidation(this.sale as never);
    return (
      result === "product;client;" || result === "product;clientId;client;"
    );
  }

  parseDate(date: string) {
    const dateSplited = date.split("/");
    return new Date(
      Date.parse(dateSplited[1] + "/" + dateSplited[0] + "/" + dateSplited[2])
    );
  }

  public setPayment() {
    switch (this.sale.paymentType) {
      case paymentTypes.cash:
        this.sale.payment = new CashForCreation();
        break;
      case paymentTypes.ownFees:
        this.sale.payment = new OwnFeesForCreation();
        //Get%CuotasProd
        break;
      case paymentTypes.creditcard:
        this.sale.payment = new CreditCardForCreation();
        break;
      case paymentTypes.debitcard:
        this.sale.payment = new DebitCardForCreation();
        break;
      case paymentTypes.cheque:
        this.sale.payment = new ChequeForCreation();
        break;
      default:
        break;
    }
  }

  getImporte() {
    if (this.priceValue && this.sale.quantity && this.sale.payment) {
      switch (this.sale.paymentType) {
        case paymentTypes.cash:
          {
            const payCash = this.sale.payment as CashForCreation;
            if (payCash.discount) {
              this.sale.amount =
                this.priceValue *
                this.sale.quantity *
                (1 - payCash.discount / 100);
            } else {
              this.sale.amount = undefined;
            }
          }
          break;
        case paymentTypes.ownFees:
          {
            const payOwnFee = this.sale.payment as OwnFeesForCreation;
            if (payOwnFee.quantity && this.priceValue && this.sale.quantity) {
              this.sale.amount =
                this.priceValue *
                this.sale.quantity *
                (1 + (3 * payOwnFee.quantity) / 100);
            } else {
              this.sale.amount = undefined;
            }
          }
          break;
        case paymentTypes.creditcard:
          this.sale.amount = this.priceValue * this.sale.quantity;
          break;
        case paymentTypes.debitcard:
          this.sale.amount = this.priceValue * this.sale.quantity;
          break;
        case paymentTypes.cheque:
          this.sale.amount = this.priceValue * this.sale.quantity;
          break;
        default:
          break;
      }
    }
    return this.sale.amount;
  }

  getValorCuota() {
    const payment = this.sale.payment as OwnFeesForCreation;
    if (this.sale.amount) {
      return Math.ceil((this.sale.amount * 100) / payment.quantity) / 100;
    }
  }

  getUnitPrice() {
    return this.priceValue !== undefined ? this.priceValue : "";
  }

  getPriceValue() {
    if (this.sale.productId && this.sale.date) {
      const date = this.sale.date
        ? this.sale.date.toISOString()
        : new Date().toISOString();
      this.isLoading = true;
      this.productService
        .getPriceByDate(this.sale.productId, date)
        .then(response => {
          this.priceValue = response as number;
          this.isLoading = false;
          this.$forceUpdate();
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
    }
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

  filteredClients() {
    const filtered = this.clients.filter(option => {
      return option.name
        ? option.name
            .toString()
            .toLowerCase()
            .indexOf((this.sale.clientName ?? "").toLowerCase()) >= 0
        : false;
    });

    const dniNameArray: string[] = [];

    filtered.forEach(x => {
      if (x.name) {
        dniNameArray.push(x.name + " - " + x.lastname + " - " + x.dni);
      }
    });
    return dniNameArray;
  }

  getProductId(option: string) {
    const prod = this.products.find(
      x => x.code + " - " + x.name + " - " + x.description === option
    );
    this.sale.productId = prod ? prod.id : undefined;
    this.getPriceValue();
  }

  getClientId(option: string) {
    const client = this.clients.find(
      x => x.name + " - " + x.lastname + " - " + x.dni === option
    );
    this.sale.clientId = client ? client.id : undefined;
  }

  public submit() {
    if (!this.sale.id) {
      this.newSale();
    } else {
      this.updateSale();
    }
  }

  public newSale() {
    this.isLoading = true;
    // this.sale.quantity = +(this.sale.quantity ?? 0);
    this.saleService
      .addSale(this.sale)
      .then(() => {
        this.isLoading = false;
        this.$router.push({ name: "SaleList" });
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
        this.$router.push({ name: "SaleList" });
      });
  }

  public updateSale() {
    this.isLoading = true;
    // this.sale.quantity = +(this.sale.quantity ?? 0);
    this.saleService
      .updateSale(this.sale)
      .then(() => {
        this.isLoading = false;
        this.$router.push({ name: "SaleList" });
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
        this.$router.push({ name: "SaleList" });
      });
  }

  created() {
    this.isLoading = true;
    this.sale.id = this.$route.params.id;
    this.productService
      .getProducts()
      .then(response => {
        this.products = response;
        return this.clientService.getClients();
      })
      .then(response => {
        this.clients = response;
        if (this.sale.id) {
          this.isLoading = true;
          this.saleService.getSale(this.sale.id).then(
            data => {
              this.sale = data as Sale;
              this.sale.date = new Date(this.sale.date ?? "");
              const pp =
                this.products.find(x => x.id === this.sale.productId) ??
                new Product();
              this.prodCodNameDesc =
                pp.code + " - " + pp.name + " - " + pp.description;
              this.isLoading = false;
              this.getPriceValue();
            },
            error => {
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
              this.isLoading = true;
            }
          );
        } else {
          this.sale.date = new Date();
          this.isLoading = false;
        }
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
        this.$router.push({ name: "SaleList" });
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
.pMargin {
  margin-right: 5px;
  margin-top: 5px;
}
.mx-1 {
  margin: 0em 1em 0em 1em;
}
</style>
