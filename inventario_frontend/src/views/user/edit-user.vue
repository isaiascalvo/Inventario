<template>
  <div>
    <div class="card">
      <div class="card-content">
        <div class="media">
          <div class="media-content">
            <p class="title is-4">
              {{ user.id ? "Editar Usuario" : "Crear Usuario" }}
            </p>
          </div>
        </div>

        <div class="content">
          <section>
            <b-field
              label="Nombre de Usuario:"
              :type="fieldState(user.username) ? 'is-success' : 'is-danger'"
              :message="
                fieldState(user.username)
                  ? ''
                  : 'Debe ingresar un nombre de usuario'
              "
            >
              <b-input
                v-model="user.username"
                placeholder="Ingrese el nombre de usuario"
              ></b-input>
            </b-field>

            <b-field
              v-if="!user.id"
              label="Contraseña:"
              :type="
                fieldState(user.password) && validPassword()
                  ? 'is-success'
                  : 'is-danger'
              "
              :message="
                fieldState(user.password) && validPassword()
                  ? ''
                  : 'La contraeña debe conterner al menos 8 caracteres'
              "
            >
              <b-input
                v-model="user.password"
                placeholder="Ingrese una contraseña"
              ></b-input>
            </b-field>

            <b-field
              v-if="!user.id"
              label="Confirmar contraseña:"
              :type="!confirmPasswordState() ? 'is-success' : 'is-danger'"
              :message="
                !confirmPasswordState() ? '' : 'Debe confirmar la contraseña'
              "
            >
              <b-input
                v-model="confirmPassword"
                placeholder="Confirme la contraseña"
              ></b-input>
            </b-field>

            <b-field
              label="Nombre:"
              :type="fieldState(user.name) ? 'is-success' : 'is-danger'"
              :message="fieldState(user.name) ? '' : 'Debe ingresar un nombre'"
            >
              <b-input
                v-model="user.name"
                placeholder="Ingrese el nombre del usuario"
              ></b-input>
            </b-field>

            <b-field
              label="Apellido:"
              :type="fieldState(user.lastname) ? 'is-success' : 'is-danger'"
              :message="
                fieldState(user.lastname) ? '' : 'Debe ingresar un apellido'
              "
            >
              <b-input
                v-model="user.lastname"
                placeholder="Ingrese el apellido del usuario"
              ></b-input>
            </b-field>

            <b-field
              label="DNI"
              :type="fieldState(user.dni) ? 'is-success' : 'is-danger'"
              :message="fieldState(user.dni) ? '' : 'Debe ingresar un DNI'"
            >
              <b-input
                v-model="user.dni"
                placeholder="Ingrese el DNI del usuario"
              ></b-input>
            </b-field>

            <b-field
              label="Teléfono"
              :type="fieldState(user.phone) ? 'is-success' : 'is-danger'"
              :message="
                fieldState(user.phone) ? '' : 'Debe ingresar un teléfono'
              "
            >
              <b-input
                v-model="user.phone"
                placeholder="Ingrese el teléfono del usuario"
              ></b-input>
            </b-field>

            <b-field
              label="Mail"
              :type="fieldState(user.mail) ? 'is-success' : 'is-danger'"
              :message="fieldState(user.mail) ? '' : 'Debe ingresar un mail'"
            >
              <b-input
                v-model="user.mail"
                placeholder="Ingrese el mail del usuario"
              ></b-input>
            </b-field>

            <div class="field">
              <b-switch v-model="user.active">
                Activo
              </b-switch>
            </div>

            <b-button
              type="submit"
              class="is-success mr-1"
              :disabled="!formValid()"
              @click="submit"
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
          </section>
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
      (result === "" && !this.confirmPasswordState()) ||
      (result === "password;" && this.user.id)
    );
  }

  validPassword() {
    return this.user.password && this.user.password.length >= 8;
  }
  confirmPasswordState() {
    return this.confirmPassword !== this.user.password;
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
