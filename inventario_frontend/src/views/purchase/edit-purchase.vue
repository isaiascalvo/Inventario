<template>
  <div>
    <div class="card  column is-4 is-offset-4">
      <div class="card-content">
        <div class="media">
          <div class="media-content">
            <p class="title is-4">
              {{ purchase.id ? "Editar Compra" : "Crear Compra" }}
            </p>
          </div>
        </div>

        <div class="content">
          <form @submit.prevent="submit()" class="flex-text-left">
            <b-field
              label="Producto"
              :type="
                prodFocus && !fieldState(purchase.productId) ? 'is-danger' : ''
              "
              :message="
                prodFocus && !fieldState(purchase.productId)
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
                  purchase.productId = prodCodNameDesc = undefined
                "
                size="is-small"
                @select="option => getProductId(option)"
                @blur="prodFocus = true"
              >
                <template slot="empty">No se encontraron resultados</template>
              </b-autocomplete>
            </b-field>

            <b-field label="Cliente comprador:">
              <b-autocomplete
                v-model="purchase.clientName"
                :data="filteredClients()"
                placeholder="ej.: Cliente X"
                icon="magnify"
                icon-right="close-circle"
                icon-right-clickable
                @icon-right-click="
                  purchase.clientId = purchase.clientName = undefined
                "
                size="is-small"
                @select="option => getClientId(option)"
                @blur="clientFocus = true"
              >
                <template slot="empty">No results found</template>
              </b-autocomplete>
            </b-field>

            <b-field
              label="Fecha y hora de compra"
              :type="dateFocus && !fieldState(purchase.date) ? 'is-danger' : ''"
              :message="
                dateFocus && !fieldState(purchase.date)
                  ? 'Debe seleccionar fecha y hora'
                  : ''
              "
            >
              <b-datetimepicker
                v-model="purchase.date"
                rounded
                placeholder="Seleccione fecha y hora"
                icon="calendar-today"
                trap-focus
                horizontal-time-picker
                size="is-small"
                editable
                @input="getPriceValue()"
                @blur="dateFocus = true"
              >
              </b-datetimepicker>
            </b-field>

            <b-field label="Cantidad:">
              <b-input
                v-model="purchase.quantity"
                placeholder="Ingrese la cantidad"
                required
                validation-message="Debe ingresar una cantidad"
                size="is-small"
                type="number"
              ></b-input>
            </b-field>

            <b-field>
              <p class="pMargin"><strong>Precio Unitario: $</strong></p>
              <b-input
                :value="getUnitPrice()"
                disabled
                size="is-small"
              ></b-input>
            </b-field>

            <b-field>
              <p class="pMargin">
                <strong>Importe Total: $</strong>
              </p>
              <b-input
                v-model="purchase.amount"
                :value="getImporte()"
                disabled
                size="is-small"
              ></b-input>
            </b-field>

            <b-button
              native-type="submit"
              class="is-success mr-1"
              :disabled="!formValid()"
            >
              {{ purchase.id ? "Editar" : "Crear" }}
            </b-button>
            <b-button
              type="button"
              class="is-danger"
              @click="$router.push('/purchase-list')"
            >
              Cancelar
            </b-button>
          </form>
        </div>
      </div>
    </div>

    <b-loading is-full-page :active.sync="isLoading"></b-loading>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { NavigatorPurchaseService } from "../../services/purchase-service";
import { Purchase } from "../../models/purchase";
import {
  fieldStateValidation,
  formValidation
} from "../../utils/common-functions";
import { Product } from "../../models/product";
import { Client } from "../../models/client";
import { NavigatorProductService } from "../../services/product-service";
import { NavigatorClientService } from "../../services/client-service";

@Component
export default class EditPurchase extends Vue {
  public errorDialog = false;
  public purchase: Purchase = new Purchase();
  public products: Product[] = [];
  public clients: Client[] = [];
  public purchaseService: NavigatorPurchaseService = new NavigatorPurchaseService();
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

  fieldState(field: unknown) {
    return fieldStateValidation(field);
  }

  formValid() {
    const result = formValidation(this.purchase as never);
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

  getImporte() {
    if (this.priceValue && this.purchase.quantity) {
      this.purchase.amount = this.priceValue * this.purchase.quantity;
    }
    return this.purchase.amount;
  }

  getUnitPrice() {
    return this.priceValue !== undefined ? this.priceValue : "";
  }

  getPriceValue() {
    if (this.purchase.productId && this.purchase.date) {
      const date = this.purchase.date
        ? this.purchase.date.toISOString()
        : new Date().toISOString();
      this.productService
        .getPriceByDate(this.purchase.productId, date)
        .then(response => {
          this.priceValue = response as number;
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
            .indexOf((this.purchase.clientName ?? "").toLowerCase()) >= 0
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
    this.purchase.productId = prod ? prod.id : undefined;
    this.getPriceValue();
  }

  getClientId(option: string) {
    const client = this.clients.find(
      x => x.name + " - " + x.lastname + " - " + x.dni === option
    );
    this.purchase.clientId = client ? client.id : undefined;
  }

  public submit() {
    if (!this.purchase.id) {
      this.newPurchase();
    } else {
      this.updatePurchase();
    }
  }

  public newPurchase() {
    this.isLoading = true;
    // this.purchase.quantity = +(this.purchase.quantity ?? 0);
    this.purchaseService
      .addPurchase(this.purchase)
      .then(() => {
        this.isLoading = false;
        this.$router.push({ name: "PurchaseList" });
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
        this.$router.push({ name: "PurchaseList" });
      });
  }

  public updatePurchase() {
    this.isLoading = true;
    // this.purchase.quantity = +(this.purchase.quantity ?? 0);
    this.purchaseService
      .updatePurchase(this.purchase)
      .then(() => {
        this.isLoading = false;
        this.$router.push({ name: "PurchaseList" });
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
        this.$router.push({ name: "PurchaseList" });
      });
  }

  created() {
    this.isLoading = true;
    this.purchase.id = this.$route.params.id;
    this.productService
      .getProducts()
      .then(response => {
        this.products = response;
        return this.clientService.getClients();
      })
      .then(response => {
        this.clients = response;
        if (this.purchase.id) {
          this.isLoading = true;
          this.purchaseService.getPurchase(this.purchase.id).then(
            data => {
              this.purchase = data as Purchase;
              this.purchase.date = new Date(this.purchase.date ?? "");
              const pp =
                this.products.find(x => x.id === this.purchase.productId) ??
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
          this.purchase.date = new Date();
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
        this.$router.push({ name: "PurchaseList" });
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
}
</style>
