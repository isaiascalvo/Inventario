<template>
  <div>
    <div class="card column is-6 is-offset-3">
      <div class="card-content">
        <div class="media">
          <div class="media-content">
            <p class="title is-4">
              {{ user.id ? "Editar Usuario" : "Crear Usuario" }}
            </p>
          </div>
        </div>

        <div class="content">
          <form @submit.prevent="submit()">
            <div class="columns">
              <div class="column">
                <b-field label="Nombre de Usuario:">
                  <b-input
                    v-model="user.username"
                    placeholder="Ingrese el nombre de usuario"
                    required
                    validation-message="Debe ingresar un nombre de usuario"
                  ></b-input>
                </b-field>
              </div>
              <div class="column">
                <b-field label="Mail">
                  <b-input
                    v-model="user.mail"
                    placeholder="Ingrese el mail del usuario"
                    type="email"
                    required
                    validation-message="Debe ingresar un mail válido"
                  ></b-input>
                </b-field>
              </div>
            </div>

            <div class="columns">
              <div class="column">
                <b-field v-if="!user.id" label="Contraseña:">
                  <b-input
                    v-model="user.password"
                    required
                    placeholder="Ingrese una contraseña"
                    minlength="8"
                    type="password"
                    password-reveal
                  ></b-input>
                </b-field>
              </div>
              <div class="column">
                <b-field
                  v-if="!user.id"
                  label="Confirmar contraseña:"
                  :type="!confirmPasswordState() ? '' : 'is-danger'"
                  :message="
                    !confirmPasswordState()
                      ? ''
                      : 'Las contraseñas no coinciden'
                  "
                >
                  <b-input
                    v-model="confirmPassword"
                    placeholder="Confirme la contraseña"
                    required
                    type="password"
                    password-reveal
                  ></b-input>
                </b-field>
              </div>
            </div>

            <div class="columns">
              <div class="column">
                <b-field label="Nombre:">
                  <b-input
                    v-model="user.name"
                    placeholder="Ingrese el nombre del usuario"
                    required
                  ></b-input>
                </b-field>
              </div>
              <div class="column">
                <b-field label="Apellido:">
                  <b-input
                    v-model="user.lastname"
                    placeholder="Ingrese el apellido del usuario"
                    required
                  ></b-input>
                </b-field>
              </div>
            </div>

            <div class="columns">
              <div class="column">
                <b-field label="DNI">
                  <b-input
                    v-model="user.dni"
                    placeholder="Ingrese el DNI del usuario"
                    required
                  ></b-input>
                </b-field>
              </div>
              <div class="column">
                <b-field label="Teléfono">
                  <b-input
                    v-model="user.phone"
                    placeholder="Ingrese el teléfono del usuario"
                    required
                  ></b-input>
                </b-field>
              </div>
            </div>

            <div class="field">
              <b-switch v-model="user.active">
                Activo
              </b-switch>
            </div>

            <b-button
              native-type="submit"
              class="is-success mr-1"
              :disabled="!formValid()"
            >
              {{ user.id ? "Editar" : "Crear" }}
            </b-button>
            <b-button
              type="button"
              class="is-danger"
              @click="$router.push('/user-list')"
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
import { NavigatorUserService } from "../../services/user-service";
import { User } from "../../models/user";
import {
  fieldStateValidation,
  formValidation
} from "../../utils/common-functions";

@Component
export default class EditUser extends Vue {
  public errorDialog = false;
  public user: User = new User();
  public userService: NavigatorUserService = new NavigatorUserService();
  public isLoading = false;
  public confirmPassword = "";

  fieldState(field: unknown) {
    return fieldStateValidation(field);
  }

  formValid() {
    const result = formValidation(this.user as never);
    return (
      (result === "" &&
        !this.confirmPasswordState() &&
        this.confirmPassword !== "") ||
      (result === "password;" && this.user.id)
    );
  }

  validPassword() {
    return this.user.password && this.user.password.length >= 8;
  }

  confirmPasswordState() {
    return (
      this.confirmPassword !== "" && this.confirmPassword !== this.user.password
    );
  }

  public submit() {
    if (!this.user.id) {
      this.newUser();
    } else {
      this.updateUser();
    }
  }

  public newUser() {
    this.isLoading = true;
    this.userService
      .addUser(this.user)
      .then(() => {
        this.isLoading = false;
        this.$router.push({ name: "UserList" });
      })
      .catch(e => {
        // this.errorMsg = {
        //   title: "Error",
        //   msg: "An unexpected error has occurred. please try again later."
        // };
        this.isLoading = false;
        console.log("error: ", e);
        this.$router.push({ name: "UserList" });
      });
  }

  public updateUser() {
    this.isLoading = true;
    this.userService
      .updateUser(this.user)
      .then(() => {
        this.isLoading = false;
        this.$router.push({ name: "UserList" });
      })
      .catch(e => {
        this.isLoading = false;
        // this.errorMsg = {
        //   title: "Error",
        //   msg: "An unexpected error has occurred. please try again later."
        // };
        // this.errorDialog = true;
        console.log("error: ", e);
        this.$router.push({ name: "UserList" });
      });
  }

  created() {
    this.user.id = this.$route.params.id;
    if (this.user.id) {
      this.isLoading = true;
      this.userService.getUser(this.user.id).then(
        response => {
          this.user = response as User;
          this.isLoading = false;
        },
        error => {
          console.log(error);
          this.isLoading = true;
        }
      );
    }
  }
}
</script>

<style>
.mr-1 {
  margin-right: 1em;
}
</style>
