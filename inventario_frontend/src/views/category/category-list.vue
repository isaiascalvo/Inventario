<template>
  <div>
    <section class="hero is-light p-1">
      <div class="hero-head">
        <div class="level">
          <div>
            <h1 class="title is-6">Lista de Categorías</h1>
          </div>
          <div>
            <b-button
              type="is-info"
              size="is-small"
              tag="router-link"
              to="/category/new"
            >
              Nueva Categoría
            </b-button>
          </div>
        </div>
      </div>
    </section>

    <div class="">
      <b-table
        striped
        hoverable
        scrollable
        :data="categories"
        id="my-table"
        paginated
        :per-page="perPage"
        :current-page.sync="currentPage"
        aria-next-label="Next page"
        aria-previous-label="Previous page"
        aria-page-label="Page"
        aria-current-label="Current page"
      >
        <template slot="empty">
          No hay categorías registradas
        </template>
        <template slot-scope="props">
          <b-table-column field="name" label="Nombre">
            {{ props.row.name }}
          </b-table-column>
          <b-table-column field="description" label="Descripción">
            {{ props.row.description }}
          </b-table-column>

          <b-table-column field="action" label="Acciones">
            <b-button
              tag="router-link"
              :to="'/category/modify/' + props.row.id"
              type="is-small"
            >
              <b-icon icon="pencil"></b-icon>
            </b-button>
            <b-button
              @click="deleteCategory(props.row)"
              type="is-small"
              class="actionButton"
            >
              <b-icon icon="delete"></b-icon>
            </b-button>
          </b-table-column>
        </template>

        <template slot="bottom-left">
          <span class="ml-1">
            <b-field label="Items por página:" label-position="on-border">
              <b-select v-model="perPage">
                <option value="10">10 por página</option>
                <option value="15">15 por página</option>
                <option value="20">20 por página</option>
              </b-select>
            </b-field>
          </span>
        </template>
      </b-table>
    </div>

    <b-loading is-full-page :active.sync="isLoading"></b-loading>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { Category } from "../../models/category";
import { NavigatorCategoryService } from "../../services/category-service";

@Component
export default class CategoryList extends Vue {
  public categories: Category[] = [];
  public categoryService: NavigatorCategoryService = new NavigatorCategoryService();

  public currentPage = 1;
  public perPage = 10;
  public isLoading = false;

  deleteCategory(category: Category) {
    this.$buefy.dialog.confirm({
      title: "Eliminando Categoría",
      message:
        "Estás seguro que deseas <b>eliminar</b> la categoría? Esta acción no podrá deshacerse.",
      confirmText: "Eliminar Categoría",
      cancelText: "Cancelar",
      type: "is-danger",
      hasIcon: true,
      onConfirm: () => {
        this.isLoading = true;
        this.categoryService
          .deleteCategory(category.id)
          .then(() => {
            this.isLoading = false;
            this.categories.splice(this.categories.indexOf(category), 1);
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

        this.$buefy.toast.open("Categoría eliminada!");
      }
    });
  }

  created() {
    this.isLoading = true;
    this.categoryService
      .getCategories()
      .then(response => {
        this.categories = response;
        this.isLoading = false;
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
      });
  }
}
</script>

<style>
.clickeable {
  cursor: pointer;
}

.filtersClass {
  margin: 0em !important;
  padding: 1em;
  background-color: #e0eaff !important;
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

.ml-1 {
  margin-left: 1em;
}

select {
  min-width: 120px;
}

.filterButton {
  float: right;
}

.filtersClass select,
.filtersClass input {
  width: 180px;
}

select {
  min-width: 90px;
}

input {
  min-width: 80px;
}

.fieldWidth {
  width: 80px;
}

.p-1 {
  padding: 1em;
}
</style>
