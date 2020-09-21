<template>
  <div>
    <div
      class="card column"
      v-bind:class="{
        'is-4 is-offset-4':
          activeStep !== 1 && (activeStep !== 2 || sale.paymentType !== 4),
        'is-6 is-offset-3': activeStep === 2 && sale.paymentType === 4,
        'is-8 is-offset-2': activeStep === 1
      }"
    >
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
            <b-step-item step="1" label="Paso 1">
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
                  @blur="dateFocus = true"
                >
                </b-datetimepicker>
              </b-field>
            </b-step-item>

            <b-step-item step="2" label="Paso 2">
              <b-table striped hoverable :data="sale.details">
                <template slot-scope="props">
                  <b-table-column
                    field="productId"
                    label="Producto"
                    width="60%"
                  >
                    <b-field
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
                        :data="filteredProducts(props.row)"
                        placeholder="Producto"
                        icon-right="close-circle"
                        size="is-small"
                        expanded
                        append-to-body
                        icon-right-clickable
                        @icon-right-click="
                          props.row.productId = prodCodNameDesc[
                            getIndex(props.row)
                          ].text = ''
                        "
                        @select="
                          option =>
                            (props.row.productId = getProductIdByDetail(
                              option,
                              props.index
                            ))
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
                  </b-table-column>

                  <b-table-column field="quantity" label="Cantidad">
                    <b-field>
                      <b-input
                        v-model="props.row.quantity"
                        placeholder="Cantidad"
                        size="is-small"
                        type="number"
                        required
                        validation-message="Debe ingresar la cantidad"
                        @input="forceUpdate()"
                      >
                      </b-input>
                    </b-field>
                  </b-table-column>

                  <b-table-column label="Precio unitario">
                    <b-field>
                      <b-input
                        size="is-small"
                        :value="
                          '$' + prodCodNameDesc[getIndex(props.row)].unitPrice
                        "
                        disabled
                      ></b-input>
                    </b-field>
                  </b-table-column>

                  <b-table-column label="Subtotal">
                    <b-field>
                      <b-input
                        size="is-small"
                        :value="
                          '$' +
                            prodCodNameDesc[getIndex(props.row)].unitPrice *
                              (props.row.quantity ? props.row.quantity : 0)
                        "
                        disabled
                      ></b-input>
                    </b-field>
                  </b-table-column>

                  <b-table-column field="action">
                    <b-button
                      @click="deleteDetail(props.row)"
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
                    @click="pushDetail()"
                  >
                    Agregar Producto
                  </b-button>
                </template>
              </b-table>
            </b-step-item>

            <b-step-item step="3" label="Paso 3">
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
                    v-model="sale.cash.discount"
                    placeholder="Ingrese el descuento"
                    min="0"
                    type="is-dark"
                    required
                    validation-message="Debe ingresar el descuento"
                    @input="forceUpdate()"
                  ></b-numberinput>
                </b-field>
              </template>

              <template v-if="sale.paymentType === 1">
                <div v-if="impossibleFee">
                  <strong>
                    Elija otro método de pago. Los siguientes productos no se
                    pueden pagar en cuotas:
                  </strong>
                  <ul>
                    <li v-for="ipf in impossibleFeeProducts" :key="ipf.id">
                      {{ getProductDescription(ipf) }}
                    </li>
                  </ul>
                </div>
                <template v-else>
                  <b-field label="Cantidad de cuotas:">
                    <b-numberinput
                      v-model="sale.ownFees.quantity"
                      placeholder="Ingrese la cantidad de cuotas"
                      min="0"
                      :max="maxFeesAmount"
                      type="is-dark"
                      required
                      validation-message="Debe ingresar la cantidad de cuotas"
                      @input="getFeeValue()"
                      :disabled="impossibleFee"
                    ></b-numberinput>
                  </b-field>

                  <div
                    v-if="sale.ownFees.quantity"
                    style="text-align: center; margin-bottom: 5px;"
                  >
                    ({{
                      sale.ownFees.quantity +
                        " cuota" +
                        (sale.ownFees.quantity === 1 ? "" : "s")
                    }}
                    de $ {{ feeValue }})
                  </div>

                  <b-field label="Fecha de vencimiento (primer cuota)">
                    <b-datepicker
                      v-model="sale.ownFees.expirationDate"
                      placeholder="Seleccione una fecha"
                      icon="calendar-today"
                      trap-focus
                      position="is-top-right"
                      editable
                      required
                      validation-message="Debe ingresar una fecha"
                      @input="forceUpdate()"
                    >
                    </b-datepicker>
                  </b-field>
                </template>
              </template>

              <template v-if="sale.paymentType === 2">
                <b-field label="Banco">
                  <b-input
                    v-model="sale.creditCard.bank"
                    placeholder="Ej: Santander, Macro, etc."
                    required
                    validation-message="Debe ingresar el nombre del banco"
                    @input="forceUpdate()"
                  >
                  </b-input>
                </b-field>
                <b-field label="Tipo de Tarjeta">
                  <b-input
                    v-model="sale.creditCard.cardType"
                    placeholder="Ej: Visa, Mastercard, etc."
                    required
                    validation-message="Debe ingresar el tipo de tarjeta"
                    @input="forceUpdate()"
                  >
                  </b-input>
                </b-field>
                <b-field label="Descuento (%):">
                  <b-numberinput
                    v-model="sale.creditCard.discount"
                    placeholder="Ingrese el descuento"
                    min="0"
                    type="is-dark"
                    required
                    validation-message="Debe ingresar el descuento"
                    @input="forceUpdate()"
                  ></b-numberinput>
                </b-field>
                <b-field label="Recargo (%):">
                  <b-numberinput
                    v-model="sale.creditCard.surcharge"
                    placeholder="Ingrese el recargo"
                    min="0"
                    type="is-dark"
                    required
                    validation-message="Debe ingresar el recargo"
                    @input="forceUpdate()"
                  ></b-numberinput>
                </b-field>
              </template>

              <template v-if="sale.paymentType === 3">
                <b-field label="Banco">
                  <b-input
                    v-model="sale.debitCard.bank"
                    placeholder="Ej: Santander, Macro, etc."
                    required
                    validation-message="Debe ingresar el nombre del banco"
                    @input="forceUpdate()"
                  >
                  </b-input>
                </b-field>
                <b-field label="Tipo de Tarjeta">
                  <b-input
                    v-model="sale.debitCard.cardType"
                    placeholder="Ej: Visa, Mastercard, etc."
                    required
                    validation-message="Debe ingresar el tipo de tarjeta"
                    @input="forceUpdate()"
                  >
                  </b-input>
                </b-field>
                <b-field label="Descuento (%):">
                  <b-numberinput
                    v-model="sale.debitCard.discount"
                    placeholder="Ingrese el descuento"
                    min="0"
                    type="is-dark"
                    required
                    validation-message="Debe ingresar el descuento"
                    @input="forceUpdate()"
                  ></b-numberinput>
                </b-field>
                <b-field label="Recargo (%):">
                  <b-numberinput
                    v-model="sale.debitCard.surcharge"
                    placeholder="Ingrese el recargo"
                    min="0"
                    type="is-dark"
                    required
                    validation-message="Debe ingresar el recargo"
                    @input="forceUpdate()"
                  ></b-numberinput>
                </b-field>
              </template>

              <template v-if="sale.paymentType === 4">
                <div class="">
                  <b-table striped hoverable :data="sale.cheques.listOfCheques">
                    <template slot-scope="props">
                      <b-table-column field="bank" label="Banco">
                        <b-field>
                          <b-input
                            v-model="props.row.bank"
                            placeholder="Ej: Santander, Macro, etc."
                            size="is-small"
                            required
                            validation-message="Debe ingresar el nombre del banco"
                            @input="forceUpdate()"
                          >
                          </b-input>
                        </b-field>
                      </b-table-column>

                      <b-table-column field="quantity" label="Número">
                        <b-field>
                          <b-input
                            v-model="props.row.nro"
                            placeholder="Número de cheque"
                            size="is-small"
                            required
                            validation-message="Debe ingresar el número del cheque"
                            @input="forceUpdate()"
                          >
                          </b-input>
                        </b-field>
                      </b-table-column>

                      <b-table-column field="quantity" label="Monto">
                        <b-field>
                          <b-input
                            v-model="props.row.value"
                            placeholder="Monto del cheque"
                            size="is-small"
                            type="number"
                            required
                            validation-message="Debe ingresar el monto del cheque"
                            @input="forceUpdate()"
                          >
                          </b-input>
                        </b-field>
                      </b-table-column>

                      <b-table-column field="action">
                        <b-button
                          @click="deleteCheque(props.row)"
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
                        @click="pushCheque()"
                      >
                        Agregar Cheque
                      </b-button>
                    </template>
                  </b-table>
                </div>
              </template>

              <div
                v-if="!impossibleFee"
                style="text-align: center; margin-bottom: 5px;"
              >
                $ {{ saleAmount }}
              </div>
            </b-step-item>

            <b-step-item step="4" label="Paso Final">
              <b-field>
                <p class="pMargin">
                  <strong>Importe Total: $</strong>
                </p>
                <b-input v-model="saleAmount" disabled></b-input>
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
                v-if="activeStep !== 3"
                outlined
                :disabled="next.disabled || nextDisabled()"
                @click.prevent="next.action"
              >
                Siguiente
              </b-button>
              <b-button
                v-if="activeStep === 3"
                native-type="submit"
                class="is-success mr-1"
                :disabled="nextDisabled()"
              >
                Confirmar
              </b-button>
              <b-button
                type="button"
                class="is-danger mr-1"
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
  Cash,
  OwnFees,
  CreditCard,
  DebitCard,
  Cheque,
  ChequesPayment,
  CashForCreation,
  CreditCardForCreation,
  DebitCardForCreation
} from "@/models/payment";
import { paymentTypes } from "@/enums/paymentTypes";
import { FeeRule } from "@/models/feeRule";
import { NavigatorFeeRuleService } from "@/services/fee-rule-service";
import { Detail } from "@/models/detail";

@Component
export default class EditSale extends Vue {
  public errorDialog = false;
  public sale: Sale = new Sale();
  public products: Product[] = [];
  public clients: Client[] = [];
  public feeRules: FeeRule[] = [];
  public saleService: NavigatorSaleService = new NavigatorSaleService();
  public productService: NavigatorProductService = new NavigatorProductService();
  public clientService: NavigatorClientService = new NavigatorClientService();
  public feeRuleService: NavigatorFeeRuleService = new NavigatorFeeRuleService();
  public isLoading = false;
  public priceValue: number | undefined = undefined;
  public prodCodNameDesc: {
    text: string;
    focus: boolean;
    unitPrice: number;
  }[] = [];
  public prodFocus = false;
  public dateFocus = false;
  public activeStep = 0;
  public feeValue?: number = undefined;
  public maxFeesAmount = 0;
  public saleAmount = 0;
  public impossibleFee = false;
  public impossibleFeeProducts: string[] = [];

  fieldState(field: unknown) {
    return fieldStateValidation(field);
  }

  formValid() {
    const result = formValidation(this.sale as never);
    const nulleableProps = [
      "",
      "product",
      "clientId",
      "client",
      "paymentId",
      "ownFees",
      "cash",
      "creditCard",
      "debitCard",
      "cheques"
    ];
    const splitedResult = result.split(";");
    return (
      result === "" ||
      splitedResult.every(x => nulleableProps.some(y => y === x))
    );
  }

  forceUpdate() {
    this.getImporte();
    this.$forceUpdate();
  }

  pushCheque() {
    this.sale.cheques?.listOfCheques.push(new Cheque());
  }

  pushDetail() {
    this.sale.details.push(new Detail());
    this.prodCodNameDesc.push({
      text: "",
      focus: false,
      unitPrice: 0
    });
  }

  deleteCheque(cheque: Cheque) {
    const index = this.sale.cheques?.listOfCheques.indexOf(cheque);
    if (index !== undefined) {
      this.sale.cheques?.listOfCheques.splice(index, 1);
    }
  }

  deleteDetail(detail: Detail) {
    const index = this.getIndex(detail);
    this.sale.details.splice(index, 1);
    this.prodCodNameDesc.splice(index, 1);

    this.prodCodNameDesc.forEach((elem, index) => {
      this.sale.details[index].productId = this.getProductIdByText(elem.text);
    });
    // this.formValid();
  }

  getFeeValue() {
    this.saleAmount = 0;
    this.sale.details.forEach((detail, index) => {
      const foundRule = this.feeRules.find(
        x =>
          x.feesAmountTo !== undefined &&
          this.sale.ownFees?.quantity !== undefined &&
          x.productId === detail.productId &&
          x.feesAmountTo >= this.sale.ownFees.quantity
      );
      if (foundRule) {
        const percentage: number = foundRule.percentage ?? 0;
        if (
          foundRule.feesAmountTo &&
          detail.quantity &&
          this.sale.ownFees?.quantity
        ) {
          const v =
            this.prodCodNameDesc[index].unitPrice *
            detail.quantity *
            (1 + (percentage * this.sale.ownFees.quantity) / 100);

          this.saleAmount += Math.ceil(v * 100) / 100;
        }
      }
    });
    this.feeValue =
      // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
      Math.ceil((this.saleAmount * 100) / this.sale.ownFees?.quantity!) / 100;
    this.$forceUpdate();
  }

  public nextDisabled() {
    switch (this.activeStep) {
      case 0:
        return !(
          fieldStateValidation(this.sale.date) &&
          fieldStateValidation(this.sale.clientName)
        );
      case 1: {
        let band = false;
        this.sale.details.forEach(detail => {
          if (
            !this.fieldState(detail.productId) ||
            !this.fieldState(detail.quantity)
          ) {
            band = true;
          }
        });
        return band;
      }
      case 2:
        return !this.validatePayment();
      default:
        return false;
    }
  }

  validatePayment() {
    switch (this.sale.paymentType) {
      case paymentTypes.cash: {
        return this.sale.cash && this.sale.cash.discount !== undefined;
      }
      case paymentTypes.ownFees: {
        return (
          this.sale.ownFees &&
          this.sale.ownFees.quantity &&
          this.sale.ownFees.expirationDate
        );
      }
      case paymentTypes.creditcard: {
        return (
          this.sale.creditCard &&
          this.sale.creditCard.bank &&
          this.sale.creditCard.cardType &&
          this.fieldState(this.sale.creditCard.discount) &&
          this.fieldState(this.sale.creditCard.surcharge)
        );
      }
      case paymentTypes.debitcard:
        return (
          this.sale.debitCard &&
          this.sale.debitCard.bank &&
          this.sale.debitCard.cardType &&
          this.fieldState(this.sale.debitCard.discount) &&
          this.fieldState(this.sale.debitCard.surcharge)
        );
      case paymentTypes.cheque: {
        let band =
          this.sale.cheques && this.sale.cheques.listOfCheques.length > 0;
        this.sale.cheques?.listOfCheques.forEach(che => {
          if (
            !(
              this.fieldState(che.nro) &&
              this.fieldState(che.bank) &&
              this.fieldState(che.value)
            )
          ) {
            band = false;
          }
        });
        return band;
      }
      default:
        break;
    }
  }

  public setPayment() {
    this.impossibleFee = false;
    this.impossibleFeeProducts = [];
    switch (this.sale.paymentType) {
      case paymentTypes.cash:
        this.sale.cash = new Cash();
        this.sale.cash.discount = 0;
        this.sale.ownFees = undefined;
        this.sale.creditCard = undefined;
        this.sale.debitCard = undefined;
        this.sale.cheques = undefined;
        break;
      case paymentTypes.ownFees:
        this.sale.ownFees = new OwnFees();
        this.sale.ownFees.expirationDate = new Date(
          new Date().setMonth(new Date().getMonth() + 1)
        );
        this.sale.cash = undefined;
        this.sale.creditCard = undefined;
        this.sale.debitCard = undefined;
        this.sale.cheques = undefined;
        this.getFeeRules();
        break;
      case paymentTypes.creditcard:
        this.sale.creditCard = new CreditCard();
        this.sale.creditCard.discount = 0;
        this.sale.creditCard.surcharge = 0;
        this.sale.cash = undefined;
        this.sale.ownFees = undefined;
        this.sale.debitCard = undefined;
        this.sale.cheques = undefined;
        break;
      case paymentTypes.debitcard:
        this.sale.debitCard = new DebitCard();
        this.sale.debitCard.discount = 0;
        this.sale.debitCard.surcharge = 0;
        this.sale.cash = undefined;
        this.sale.ownFees = undefined;
        this.sale.creditCard = undefined;
        this.sale.cheques = undefined;
        break;
      case paymentTypes.cheque:
        this.sale.cheques = new ChequesPayment();
        this.sale.cheques.listOfCheques.push(new Cheque());
        this.sale.cash = undefined;
        this.sale.ownFees = undefined;
        this.sale.creditCard = undefined;
        this.sale.debitCard = undefined;
        break;
      default:
        break;
    }
    this.getImporte();
  }

  public getFeeRules() {
    const productsIds: string[] = [];
    this.sale.details.forEach(element => {
      if (element.productId) {
        productsIds.push(element.productId);
      }
    });
    this.isLoading = true;
    this.feeRuleService
      .getFeeRulesByProducts(productsIds)
      .then(response => {
        this.isLoading = false;
        this.feeRules = response;
        this.getMaximumFeesAmount(productsIds);
      })
      .catch(error => {
        this.isLoading = true;
        console.log(error);
      });
  }

  getMaximumFeesAmount(productsIds: string[]) {
    const maxFees: number[] = [];
    this.impossibleFee = false;
    this.impossibleFeeProducts = [];
    productsIds.forEach(productId => {
      const pfr = this.feeRules.filter(x => x.productId === productId);
      if (pfr.length > 0) {
        maxFees.push(
          // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
          pfr.sort((a, b) => b.feesAmountTo! - a.feesAmountTo!)[0].feesAmountTo!
        );
      } else {
        this.impossibleFeeProducts.push(productId);
        this.impossibleFee = true;
      }
    });
    this.maxFeesAmount = maxFees.sort((a, b) => a - b)[0];
  }

  getImporte() {
    if (this.sale.details.length !== 0) {
      this.getSaleAmount();
      switch (this.sale.paymentType) {
        case paymentTypes.cash:
          {
            const payCash = this.sale.cash as CashForCreation;
            if (payCash.discount !== undefined) {
              this.saleAmount *= 1 - payCash.discount / 100;
            } else {
              this.saleAmount = 0;
            }
          }
          break;
        case paymentTypes.ownFees:
          break;
        case paymentTypes.creditcard:
          {
            const payCredit = this.sale.creditCard as CreditCardForCreation;
            if (
              payCredit.discount !== undefined &&
              payCredit.surcharge != undefined
            ) {
              this.saleAmount *=
                1 + (payCredit.surcharge - payCredit.discount) / 100;
            } else {
              this.saleAmount = 0;
            }
          }
          break;
        case paymentTypes.debitcard:
          {
            const payDebit = this.sale.debitCard as DebitCardForCreation;
            if (
              payDebit.surcharge !== undefined &&
              payDebit.discount !== undefined
            ) {
              this.saleAmount *=
                1 + (payDebit.surcharge - payDebit.discount) / 100;
            } else {
              this.saleAmount = 0;
            }
          }
          break;
        case paymentTypes.cheque:
          break;
        default:
          break;
      }
    }
  }

  getSaleAmount() {
    this.saleAmount = 0;
    this.sale.details.forEach((detail, index) => {
      this.saleAmount +=
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        detail.quantity! * this.prodCodNameDesc[index].unitPrice;
    });
  }

  getUnitPrice() {
    return this.priceValue !== undefined ? this.priceValue : "";
  }

  getPriceValue(productId: string): Promise<number> {
    const date = this.sale.date
      ? this.sale.date.toISOString()
      : new Date().toISOString();
    return this.productService.getPriceByDate(productId, date);
  }

  filteredProducts(row: Detail) {
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
        !this.sale.details.some(
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

  getIndex(row: Detail) {
    const index = this.sale.details.indexOf(row);
    return index;
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

  getProductIdByText(text: string) {
    const prod = this.products.find(
      x => x.name + " - " + x.description + " - " + x.code === text
    );
    return prod ? prod.id : undefined;
  }

  getProductIdByDetail(option: string, index: number) {
    const prod = this.products.find(
      x => x.name + " - " + x.description + " - " + x.code === option
    );
    this.sale.paymentType = undefined;
    if (prod) {
      this.isLoading = true;
      this.getPriceValue(prod.id)
        .then(response => {
          this.isLoading = false;
          this.prodCodNameDesc[index].unitPrice = response as number;
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
    return prod ? prod.id : undefined;
  }

  getProductDescription(productId: string) {
    const prod = this.products.find(x => x.id === productId);
    if (prod && prod.name !== undefined) {
      return prod.name + (prod.description ? " - " + prod.description : "");
    }
  }

  getClientId(option: string) {
    const client = this.clients.find(
      x => x.name + " - " + x.lastname + " - " + x.dni === option
    );
    this.sale.clientId = client ? client.id : undefined;
  }

  public submit() {
    this.isLoading = true;
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
        // if (this.sale.id) {
        //   this.isLoading = true;
        //   this.saleService.getSale(this.sale.id).then(
        //     data => {
        //       this.sale = data as Sale;
        //       this.sale.date = new Date(this.sale.date ?? "");
        //       // const pp =
        //       //   this.products.find(x => x.id === this.sale.productId) ??
        //       //   new Product();
        //       // this.prodCodNameDesc =
        //       //   pp.code + " - " + pp.name + " - " + pp.description;
        //       this.isLoading = false;
        //       this.getPriceValue();
        //     },
        //     error => {
        //       this.$buefy.dialog.alert({
        //         title: "Error",
        //         message:
        //           "Un error inesperado ha ocurrido. Por favor inténtelo nuevamente.",
        //         type: "is-danger",
        //         hasIcon: true,
        //         icon: "times-circle",
        //         iconPack: "fa",
        //         ariaRole: "alertdialog",
        //         ariaModal: true
        //       });
        //       console.log(error);
        //       this.isLoading = true;
        //     }
        //   );
        // } else {
        this.sale.date = new Date();
        this.pushDetail();
        this.isLoading = false;
        // }
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
</style>
