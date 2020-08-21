<template>
  <div>
    <div class="card column is-4 is-offset-4">
      <div class="card-content">
        <div class="media">
          <div class="media-content">
            <p class="title is-4">
              Inicio de Sesión
            </p>
          </div>
        </div>

        <div class="content flex-text-left">
          <form @submit.prevent="submit()">
            <b-field label="Nombre de usuario:">
              <b-input
                v-model="username"
                placeholder="Ingrese el nombre de usuario"
                required
                validation-message="Debe ingresar el nombre de usuario"
              ></b-input>
            </b-field>

            <b-field label="Contraseña:">
              <b-input
                v-model="password"
                placeholder="Ingrese la contraseña"
                type="password"
                password-reveal
                required
                validation-message="Debe ingresar la contraseña"
              ></b-input>
            </b-field>

            <b-button
              native-type="submit"
              class="is-success mr-1"
              :disabled="!formValid()"
            >
              Ingresar
            </b-button>
            <!-- <b-button
              type="button"
              class="is-danger"
              @click="$router.push('/user-list')"
            >
              Cancelar
            </b-button> -->
          </form>
        </div>
      </div>
    </div>

    <b-loading is-full-page :active.sync="isLoading"></b-loading>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { NavigatorUserService } from "../services/user-service";
import { fieldStateValidation } from "../utils/common-functions";
import { JwtResult } from "../models/JwtResult";
import Menu from "../components/Menu.vue";

@Component({
  components: { Menu }
})
export default class Login extends Vue {
  public username = "";
  public password = "";
  public userService: NavigatorUserService = new NavigatorUserService();
  public isLoading = false;

  fieldState(field: unknown) {
    return fieldStateValidation(field);
  }

  formValid() {
    return this.fieldState(this.username) && this.fieldState(this.password);
  }

  public submit() {
    this.isLoading = true;
    this.userService
      .login(this.username, this.password)
      .then(response => {
        const result: JwtResult = response.data;
        // login successful if there's a jwt token in the response
        if (result && result.token) {
          // store user details and jwt token in local storage to keep user logged in between page refreshes
          sessionStorage.setItem("currentUser", JSON.stringify(result));
          //this.createMenu(result.menues);
        }
        this.isLoading = false;
        this.$parent.$children[0].$forceUpdate();
        this.$router.push({ name: "ProductList" });
      })
      .catch(e => {
        this.$buefy.dialog.alert({
          title: "Usuario o Contraseña incorrectos",
          message: "Usuario o Contraseña incorrectos.",
          confirmText: "Aceptar",
          cancelText: "Cancelar",
          type: "is-danger",
          hasIcon: true
        });
        this.isLoading = false;
        console.log("error: ", e);
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
