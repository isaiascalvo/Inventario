<template>
  <div>
    <div class="card column is-4 is-offset-4">
      <div class="card-content">
        <div class="media">
          <div class="media-content">
            <p class="title is-4">
              {{ commission.id ? "Editar Comisión" : "Nueva Comisión" }}
            </p>
          </div>
        </div>

        <form @submit.prevent="submit()" class="flex-text-left">
          <b-field
            label="Proveedor:"
            :type="
              prodFocus && !fieldState(commission.vendorId) ? 'is-danger' : ''
            "
            :message="
              prodFocus && !fieldState(commission.vendorId)
                ? 'Debe seleccionar un Proveedor'
                : ''
            "
          >
            <b-autocomplete
              v-model="vendNameCuil"
              :data="filteredVendors()"
              placeholder="ej.: Proveedor X"
              icon="magnify"
              icon-right="close-circle"
              icon-right-clickable
              @icon-right-click="commission.vendorId = vendNameCuil = undefined"
              @select="option => getVendorId(option)"
              @blur="prodFocus = true"
            >
              <template slot="empty">No se encontraron resultados</template>
            </b-autocomplete>
          </b-field>

          <b-field
            label="Cliente:"
            :type="
              clientFocus &&
              !fieldState(commission.clientId) &&
              (commission.clientName === '' ||
                commission.clientName === undefined)
                ? 'is-danger'
                : ''
            "
            :message="
              clientFocus &&
              !fieldState(commission.clientId) &&
              (commission.clientName === '' ||
                commission.clientName === undefined)
                ? 'Debe seleccionar un Cliente'
                : ''
            "
          >
            <b-autocomplete
              v-model="commission.clientName"
              :data="filteredClients()"
              placeholder="ej.: Cliente X"
              icon="magnify"
              icon-right="close-circle"
              icon-right-clickable
              @icon-right-click="
                commission.clientId = commission.clientName = undefined
              "
              @select="option => getClientId(option)"
              @blur="clientFocus = true"
            >
              <template slot="empty">No se encontraron resultados</template>
            </b-autocomplete>
          </b-field>

          <b-field label="Producto:">
            <b-input
              v-model="commission.product"
              placeholder="Ingrese el producto"
              type="text"
              required
              validation-message="Debe ingresar un producto"
            ></b-input>
          </b-field>

          <b-field label="Precio del producto:">
            <b-input
              v-model="commission.price"
              placeholder="Ingrese el precio"
              type="number"
              step=".01"
              required
              validation-message="Debe indicar el precio del producto"
            ></b-input>
          </b-field>

          <b-field label="Método de pago:">
            <b-select
              v-model="commission.paymentType"
              placeholder="Seleccione un método de pago"
              expanded
              required
              validation-message="Debe seleccionar un método de pago"
            >
              <option :value="0">Efectivo</option>
              <option :value="1">Cuotas</option>
              <option :value="2">Tarjeta de crédito</option>
              <option :value="3">Tarjeta de débito</option>
              <option :value="4">Cheque</option>
            </b-select>
          </b-field>

          <b-field
            label="Fecha y hora de venta"
            :type="dateFocus && !fieldState(commission.date) ? 'is-danger' : ''"
            :message="
              dateFocus && !fieldState(commission.date)
                ? 'Debe seleccionar fecha y hora'
                : ''
            "
          >
            <b-datetimepicker
              v-model="commission.date"
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

          <b-field label="Comisión:">
            <b-input
              v-model="commission.value"
              placeholder="Ingrese el monto de la comisión"
              type="number"
              step=".01"
              required
              validation-message="Debe indicar el monto de la comisión"
            ></b-input>
          </b-field>

          <b-button
            native-type="submit"
            class="is-success mr-1"
            :disabled="!formValid()"
          >
            {{ commission.id ? "Editar" : "Crear" }}
          </b-button>
          <b-button
            type="button"
            class="is-danger"
            style="float: right;"
            @click="$router.push('/commission-list')"
          >
            Cancelar
          </b-button>
        </form>
      </div>
    </div>

    <b-loading is-full-page :active.sync="isLoading"></b-loading>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { NavigatorCommissionService } from "../../services/commission-service";
import { Commission } from "../../models/commission";
import {
  fieldStateValidation,
  formValidation
} from "../../utils/common-functions";
import { Vendor } from "../../models/vendor";
import { Client } from "../../models/client";
import { NavigatorVendorService } from "../../services/vendor-service";
import { NavigatorClientService } from "../../services/client-service";

@Component
export default class EditCommission extends Vue {
  public commission: Commission = new Commission();
  public vendors: Vendor[] = [];
  public clients: Client[] = [];
  public commissionService: NavigatorCommissionService = new NavigatorCommissionService();
  public vendorService: NavigatorVendorService = new NavigatorVendorService();
  public clientService: NavigatorClientService = new NavigatorClientService();
  public isLoading = false;
  public vendNameCuil = "";
  public prodFocus = false;
  public clientFocus = false;
  public dateFocus = false;

  fieldState(field: unknown) {
    return fieldStateValidation(field);
  }

  formValid() {
    const result = formValidation(this.commission as never);
    const nulleableProps = ["", "vendor", "clientId", "client"];
    const splitedResult = result.split(";");
    return (
      result === "" ||
      splitedResult.every(x => nulleableProps.some(y => y === x))
    );
  }

  filteredVendors() {
    const filtered = this.vendors.filter(option => {
      return option.name
        ? option.name
            .toString()
            .toLowerCase()
            .indexOf((this.vendNameCuil ?? "").toLowerCase()) >= 0
        : false;
    });

    const codNameDescArray: string[] = [];

    filtered.forEach(x => {
      if (x.name) {
        codNameDescArray.push(x.name + " - " + x.cuil);
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
            .indexOf((this.commission.clientName ?? "").toLowerCase()) >= 0
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

  getVendorId(option: string) {
    const vendor = this.vendors.find(x => x.name + " - " + x.cuil === option);
    this.commission.vendorId = vendor ? vendor.id : undefined;
  }

  getClientId(option: string) {
    const client = this.clients.find(
      x => x.name + " - " + x.lastname + " - " + x.dni === option
    );
    this.commission.clientId = client ? client.id : undefined;
  }

  public submit() {
    if (!this.commission.id) {
      this.newCommission();
    } else {
      this.updateCommission();
    }
  }

  public newCommission() {
    this.isLoading = true;
    this.commissionService
      .addCommission(this.commission)
      .then(() => {
        this.isLoading = false;
        this.$router.push({ name: "CommissionList" });
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
        this.$router.push({ name: "CommissionList" });
      });
  }

  public updateCommission() {
    this.isLoading = true;
    // this.commission.quantity = +(this.commission.quantity ?? 0);
    this.commissionService
      .updateCommission(this.commission)
      .then(() => {
        this.isLoading = false;
        this.$router.push({ name: "CommissionList" });
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
        this.$router.push({ name: "CommissionList" });
      });
  }

  created() {
    this.isLoading = true;
    this.commission.id = this.$route.params.id;
    this.vendorService
      .getVendors()
      .then(response => {
        this.vendors = response;
        return this.clientService.getClients();
      })
      .then(response => {
        this.clients = response;
        if (this.commission.id) {
          this.isLoading = true;
          this.commissionService.getCommission(this.commission.id).then(
            data => {
              this.commission = data as Commission;
              this.commission.date = new Date(this.commission.date ?? "");
              const pp =
                this.vendors.find(x => x.id === this.commission.vendorId) ??
                new Vendor();
              this.vendNameCuil = pp.name + " - " + pp.cuil;
              this.isLoading = false;
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
          this.commission.date = new Date();
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
        this.$router.push({ name: "CommissionList" });
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
