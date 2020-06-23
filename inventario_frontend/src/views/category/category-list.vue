<template>
  <b-card title="Categorías">
    <b-button class="mb-2" to="/category/new">
      Nueva Categoría
    </b-button>
    <b-table
      striped
      hover
      :items="categories"
      :fields="headers"
      id="my-table"
      :per-page="perPage"
      :current-page="currentPage"
    >
      <template v-slot:cell(action)="data">
        <b-icon
          class="clickeable mr-2"
          icon="pencil"
          @click="$router.push('/category/modify/' + data.item.id)"
        ></b-icon>
        <b-icon
          class="clickeable"
          icon="trash"
          @click="openDeleteDialog(item.id)"
        ></b-icon>
      </template>
    </b-table>

    <b-pagination
      v-model="currentPage"
      :total-rows="categories.length"
      :per-page="perPage"
      aria-controls="my-table"
      align="center"
    ></b-pagination>

    <!-- <ErrorDialog
      :error="errorMsg"
      v-if="errorDialog"
      @close:dialog="errorDialog = $event"
    />

    <ConfirmDialog
      :msgDialog="confirmMsg"
      v-if="confirmDialog"
      @close:dialog="onClose"
    /> -->
  </b-card>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { Category } from "../../models/category";
import { NavigatorCategoryService } from "../../services/category-service";
// import ConfirmDialog from "../../components/dialogs/confirm-dialog.vue";
// import { DialogMsg } from "../../models/dialogMsg";
// import ErrorDialog from "../../components/dialogs/error-dialog.vue";

@Component({
  components: {} //ConfirmDialog, ErrorDialog
})
export default class CategoryList extends Vue {
  public categories: Category[] = [];
  public categoryService: NavigatorCategoryService = new NavigatorCategoryService();

  public currentPage = 1;
  public perPage = 1;
  public errorDialog = false;
  public confirmDialog = false;
  // public errorMsg = new DialogMsg();
  // public confirmMsg = new DialogMsg();
  public headers = [
    { key: "name", label: "Nombre" },
    { key: "description", label: "Descripción" },
    { key: "action", label: "Acciones" }
  ];

  deleteCategory(category: Category) {
    this.categoryService
      .deleteCategory(category.id)
      .then(() => {
        this.categories.splice(this.categories.indexOf(category), 1);
      })
      .catch(e => {
        // this.errorMsg = {
        //   title: "Error",
        //   msg: "An unexpected error has occurred. please try again later."
        // };
        // this.errorDialog = true;
        console.log("error: ", e);
      });
  }

  onClose(elemId: string) {
    this.confirmDialog = false;
    if (elemId.trim() !== "") {
      const category = this.categories.find(x => x.id === elemId);
      if (category) {
        this.deleteCategory(category);
      }
    }
  }

  openDeleteDialog(elemId: string) {
    // this.confirmMsg = {
    //   elemId: elemId,
    //   title: "Warning",
    //   msg: "Are you sure you want to delete the category?"
    // };
    // this.confirmDialog = true;
  }

  created() {
    this.categoryService
      .getCategories()
      .then(response => {
        this.categories = response;
      })
      .catch(e => {
        // this.errorMsg = {
        //   title: "Error",
        //   msg: "An unexpected error has occurred. please try again later."
        // };
        // this.errorDialog = true;
        console.log("error: ", e);
      });
  }
}
</script>

<style>
/* table,
table.bordered td,
table.bordered th {
  border: 1px black solid;
  margin: auto;
  margin-bottom: 10px;
}

.align-center {
  text-align: center;
} */

.clickeable {
  cursor: pointer;
}
</style>
