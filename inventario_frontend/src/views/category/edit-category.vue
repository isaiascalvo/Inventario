<template>
  <div>
    <b-card :title="id ? 'Editar Categoría' : 'Crear Categoría'">
      <!-- <v-card-title>{{ id ? "Update" : "Create" }} Category</v-card-title> -->
      <b-form @submit.prevent="submit">
        <b-form-group id="input-group-1" label="Nombre:" label-for="name-input">
          <b-form-input
            id="name-input"
            v-model="name"
            :state="nameState()"
            type="text"
            required
            placeholder="Ingrese el nombre de la categoría"
          ></b-form-input>
          <b-form-invalid-feedback :state="nameState()">
            El nombre es requerido
          </b-form-invalid-feedback>
        </b-form-group>

        <b-form-group
          id="input-group-2"
          label="Descripción:"
          label-for="description-input"
        >
          <b-form-input
            id="description-input"
            v-model="description"
            :state="descriptionState()"
            type="text"
            required
            placeholder="Ingrese la descripción de la categoría"
          ></b-form-input>
          <b-form-invalid-feedback :state="descriptionState()">
            La descripción es requerida
          </b-form-invalid-feedback>
        </b-form-group>

        <b-button type="submit" variant="primary" :disabled="!formValid()">
          {{ id ? "Editar" : "Crear" }}
        </b-button>
        <b-button
          type="button"
          variant="danger"
          @click="$router.push('/category-list')"
        >
          Cancelar
        </b-button>
      </b-form>
    </b-card>

    <!-- <ErrorDialog
      :error="errorMsg"
      v-if="errorDialog"
      @close:dialog="errorDialog = $event"
    /> -->
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { NavigatorCategoryService } from "../../services/category-service";
import { Category, CategoryForCreation } from "../../models/category";
// import { DialogMsg } from "../../models/dialogMsg";
// import ErrorDialog from "../../components/dialogs/error-dialog.vue";

@Component({
  components: {
    // ErrorDialog
  }
})
export default class EditCategory extends Vue {
  public errorDialog = false;
  //   public errorMsg = new DialogMsg();
  public id!: string;
  public name = "";
  public description = "";
  public categoryService: NavigatorCategoryService = new NavigatorCategoryService();

  nameState() {
    return this.name.length > 0 ? true : false;
  }

  descriptionState() {
    return this.description.length > 0 ? true : false;
  }

  formValid() {
    return this.nameState() && this.descriptionState();
  }

  public submit() {
    if (!this.id) {
      this.newCategory();
    } else {
      this.updateCategory();
    }
  }

  public newCategory() {
    const category: CategoryForCreation = {
      name: this.name,
      description: this.description
    };
    this.categoryService
      .addCategory(category)
      .then(() => {
        this.$router.push({ name: "CategoryList" });
      })
      .catch(e => {
        // this.errorMsg = {
        //   title: "Error",
        //   msg: "An unexpected error has occurred. please try again later."
        // };
        // this.errorDialog = true;
        console.log("error: ", e);
        this.$router.push({ name: "CategoryList" });
      });
  }

  public updateCategory() {
    const category: Category = {
      id: this.id,
      name: this.name,
      description: this.description
    };
    this.categoryService
      .updateCategory(category)
      .then(() => {
        this.$router.push({ name: "CategoryList" });
      })
      .catch(e => {
        // this.errorMsg = {
        //   title: "Error",
        //   msg: "An unexpected error has occurred. please try again later."
        // };
        // this.errorDialog = true;
        console.log("error: ", e);
        // this.$router.push({ name: "CategoryList" });
      });
  }

  created() {
    this.id = this.$route.params.id;
    if (this.id) {
      this.categoryService.getCategory(this.id).then(response => {
        this.name = response.name;
        this.description = response.description;
      });
    }
  }
}
</script>

<style></style>
