<template>
  <div>
    <div class="card column is-4 is-offset-4">
      <div class="card-content">
        <div class="media">
          <div class="media-content">
            <p class="title is-4">
              {{ miscellaneousExpense.id ? "Editar Gasto" : "Crear Gasto" }}
            </p>
          </div>
        </div>

        <div class="content">
          <form @submit.prevent="submit()" class="flex-text-left">
            <b-field label="Descripción:">
              <b-input
                v-model="miscellaneousExpense.description"
                placeholder="Ingrese la descripción de la categoría"
                type="textarea"
                required
                validation-message="Debe ingresar una descripción"
              ></b-input>
            </b-field>

            <b-field
              label="Fecha y hora del gasto"
              :type="
                dateFocus && !fieldState(miscellaneousExpense.date)
                  ? 'is-danger'
                  : ''
              "
              :message="
                dateFocus && !fieldState(miscellaneousExpense.date)
                  ? 'Debe seleccionar fecha y hora'
                  : ''
              "
            >
              <b-datetimepicker
                v-model="miscellaneousExpense.date"
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

            <b-field label="Tipo de gasto:">
              <b-select
                v-model="miscellaneousExpense.isFixed"
                placeholder="Seleccione una opción"
                expanded
                required
                validation-message="Seleccione una opción"
              >
                <option :value="true">Fijo</option>
                <option :value="false">Variable</option>
              </b-select>
            </b-field>

            <b-field label="Monto ($):">
              <b-input
                v-model="miscellaneousExpense.value"
                placeholder="Ingrese el monto"
                required
                validation-message="Debe ingresar un monto"
              ></b-input>
            </b-field>

            <b-field label="Destino (opcional):">
              <b-input
                v-model="miscellaneousExpense.destination"
                placeholder="Ingrese el destino"
              ></b-input>
            </b-field>

            <b-button
              native-type="submit"
              class="is-success mr-1"
              :disabled="!formValid()"
            >
              {{ miscellaneousExpense.id ? "Editar" : "Crear" }}
            </b-button>
            <b-button
              type="button"
              class="is-danger"
              @click="$router.push('/miscellaneous-expenses-list')"
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
import { NavigatorMiscellaneousExpensesService } from "../../services/miscellaneous-expenses-service";
import { MiscellaneousExpenses } from "../../models/miscellaneousExpenses";
import {
  fieldStateValidation,
  formValidation
} from "../../utils/common-functions";

@Component({
  components: {
    // ErrorDialog
  }
})
export default class EditMiscellaneousExpenses extends Vue {
  public errorDialog = false;
  public miscellaneousExpense: MiscellaneousExpenses = new MiscellaneousExpenses();
  public isLoading = false;
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
  public dateFocus = false;

  public miscellaneousExpenseService: NavigatorMiscellaneousExpensesService = new NavigatorMiscellaneousExpensesService();

  fieldState(field: unknown) {
    return fieldStateValidation(field);
  }

  formValid() {
    const result = formValidation(this.miscellaneousExpense as never);
    console.log(result);
    return result === "" || result === "destination;";
  }

  parseDate(date: string) {
    const dateSplited = date.split("/");
    return new Date(
      Date.parse(dateSplited[1] + "/" + dateSplited[0] + "/" + dateSplited[2])
    );
  }

  public submit() {
    if (!this.miscellaneousExpense.id) {
      this.newMiscellaneousExpenses();
    } else {
      this.updateMiscellaneousExpenses();
    }
  }

  public newMiscellaneousExpenses() {
    this.isLoading = true;
    this.miscellaneousExpenseService
      .addMiscellaneousExpenses(this.miscellaneousExpense)
      .then(() => {
        this.isLoading = false;
        this.$router.push({ name: "MiscellaneousExpensesList" });
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
        this.$router.push({ name: "MiscellaneousExpensesList" });
      });
  }

  public updateMiscellaneousExpenses() {
    this.isLoading = true;
    this.miscellaneousExpenseService
      .updateMiscellaneousExpenses(this.miscellaneousExpense)
      .then(() => {
        this.$router.push({ name: "MiscellaneousExpensesList" });
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
        this.$router.push({ name: "MiscellaneousExpensesList" });
      });
  }

  created() {
    this.miscellaneousExpense.id = this.$route.params.id;
    if (this.miscellaneousExpense.id) {
      this.isLoading = true;
      this.miscellaneousExpenseService
        .getMiscellaneousExpense(this.miscellaneousExpense.id)
        .then(
          response => {
            this.miscellaneousExpense = response as MiscellaneousExpenses;
            this.miscellaneousExpense.date = new Date(response.date ?? "");
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
            this.isLoading = false;
          }
        );
    } else {
      this.miscellaneousExpense.date = new Date();
    }
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
