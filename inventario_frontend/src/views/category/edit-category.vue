<template>
  <div>
    <div class="card column is-4 is-offset-4">
      <div class="card-content">
        <div class="media">
          <div class="media-content">
            <p class="title is-4">
              {{ category.id ? "Editar Categoría" : "Crear Categoría" }}
            </p>
          </div>
        </div>

        <div class="content">
          <form @submit.prevent="submit()" class="flex-text-left">
            <b-field label="Nombre:">
              <b-input
                v-model="category.name"
                placeholder="Ingrese el nombre de la categoría"
                required
                validation-message="Debe ingresar un nombre"
              ></b-input>
            </b-field>

            <b-field label="Descripción:">
              <b-input
                v-model="category.description"
                placeholder="Ingrese la descripción de la categoría"
                type="textarea"
                required
                validation-message="Debe ingresar una descripción"
              ></b-input>
            </b-field>

            <b-button
              native-type="submit"
              class="is-success mr-1"
              :disabled="!formValid()"
            >
              {{ category.id ? "Editar" : "Crear" }}
            </b-button>
            <b-button
              type="button"
              class="is-danger"
              @click="$router.push('/category-list')"
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
import { NavigatorCategoryService } from "../../services/category-service";
import { Category, CategoryForCreation } from "../../models/category";
import {
  fieldStateValidation,
  formValidation
} from "../../utils/common-functions";

@Component({
  components: {
    // ErrorDialog
  }
})
export default class EditCategory extends Vue {
  public errorDialog = false;
  public category: Category = new Category();
  public isLoading = false;

  public categoryService: NavigatorCategoryService = new NavigatorCategoryService();

  fieldState(field: unknown) {
    return fieldStateValidation(field);
  }

  formValid() {
    const result = formValidation(this.category as never);
    return result === "";
  }

  public submit() {
    if (!this.category.id) {
      this.newCategory();
    } else {
      this.updateCategory();
    }
  }

  public newCategory() {
    const category: CategoryForCreation = {
      name: this.category.name,
      description: this.category.description
    };
    this.isLoading = true;
    this.categoryService
      .addCategory(category)
      .then(() => {
        this.isLoading = false;
        this.$router.push({ name: "CategoryList" });
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
        this.$router.push({ name: "CategoryList" });
      });
  }

  public updateCategory() {
    const category: Category = {
      id: this.category.id,
      name: this.category.name,
      description: this.category.description
    };
    this.isLoading = true;
    this.categoryService
      .updateCategory(category)
      .then(() => {
        this.$router.push({ name: "CategoryList" });
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
        this.$router.push({ name: "CategoryList" });
      });
  }

  created() {
    this.category.id = this.$route.params.id;
    if (this.category.id) {
      this.isLoading = true;
      this.categoryService.getCategory(this.category.id).then(
        response => {
          this.category.name = response.name;
          this.category.description = response.description;
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
