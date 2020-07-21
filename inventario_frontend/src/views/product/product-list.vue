<template>
  <div>
    <section class="hero is-light">
      <div class="hero-head">
        <div class="container level">
          <div>
            <h1 class="title">Productos</h1>
            <h2 class="subtitle">
              Lista de productos
            </h2>
          </div>
          <div>
            <b-button
              type="is-info"
              tag="router-link"
              to="/product/new"
              class="mx-1"
            >
              Nuevo Producto
            </b-button>
            <b-button
              @click="openFilters = !openFilters"
              class="is-pulled-right"
              type="is-primary"
              :icon-right="openFilters ? 'eye-off-outline' : 'eye-outline'"
            >
              {{ openFilters ? "Ocultar Filtros" : "Mostrar Filtros" }}
            </b-button>
          </div>
        </div>
      </div>
    </section>

    <div class="">
      <div class="columns filtersClass level" v-if="openFilters">
        <div class="column is-10">
          <b-field grouped group-multiline>
            <b-field label-position="on-border" label="Categoría">
              <b-select
                v-model="productFilters.categoryId"
                placeholder="Categoría"
                size="is-small"
              >
                <option
                  v-for="category in categories"
                  :value="category.id"
                  :key="category.id"
                >
                  {{ category.name }}
                </option>
              </b-select>
            </b-field>

            <b-field label-position="on-border" label="Proveedor">
              <b-select
                v-model="productFilters.vendorId"
                placeholder="Proveedor"
                size="is-small"
              >
                <option
                  v-for="vendor in vendors"
                  :value="vendor.id"
                  :key="vendor.id"
                >
                  {{ vendor.name }}
                </option>
              </b-select>
            </b-field>

            <!-- <b-field label-position="on-border" label="Cliente">
              <b-select
                v-model="productFilters.clientId"
                placeholder="Seleccione un cliente"
                size="is-small"
              >
                <option
                  v-for="client in clients"
                  :key="client.id"
                  :value="client.id"
                >
                  {{ client.name }}
                </option>
              </b-select>
            </b-field> -->

            <b-field label-position="on-border" label="Código">
              <b-input
                v-model="productFilters.code"
                placeholder="Código"
                size="is-small"
              ></b-input>
            </b-field>

            <b-field label-position="on-border" label="Marca">
              <b-input
                v-model="productFilters.brand"
                placeholder="Marca"
                size="is-small"
              ></b-input>
            </b-field>

            <div class="field">
              <b-checkbox v-model="productFilters.active" size="is-small">
                Activo?
              </b-checkbox>
            </div>

            <div class="field">
              <b-checkbox v-model="productFilters.available" size="is-small">
                Disponible?
              </b-checkbox>
            </div>
          </b-field>
        </div>

        <div class="column level-right">
          <b-button type="is-info" class="mx-1">
            Apply
          </b-button>
          <b-button @click="clearFilters()" type="is-danger">
            Clear
          </b-button>
        </div>
      </div>

      <b-table
        :striped="true"
        :hoverable="true"
        :data="products"
        id="my-table"
        :paginated="true"
        :per-page="perPage"
        :current-page.sync="currentPage"
        aria-next-label="Next page"
        aria-previous-label="Previous page"
        aria-page-label="Page"
        aria-current-label="Current page"
      >
        <template slot-scope="props">
          <b-table-column field="name" label="Nombre">
            {{ props.row.name }}
          </b-table-column>
          <b-table-column field="description" label="Descripción">
            {{ props.row.description }}
          </b-table-column>
          <b-table-column field="code" label="Código">
            {{ props.row.code }}
          </b-table-column>
          <b-table-column field="category" label="Categoría">
            {{ getCategory(props.row.categoryId) }}
          </b-table-column>
          <b-table-column field="vendor" label="Proveedor">
            {{ getVendor(props.row.vendorId) }}
          </b-table-column>
          <b-table-column field="brand" label="Marca">
            {{ props.row.brand }}
          </b-table-column>
          <b-table-column field="stock" label="Stock">
            {{ props.row.stock }}
          </b-table-column>
          <b-table-column field="unitOfMeasurement" label="Unidad">
            {{ props.row.unitOfMeasurement }}
          </b-table-column>
          <b-table-column field="price" label="Precio">
            $ {{ props.row.price.value }}
          </b-table-column>

          <b-table-column field="action" label="Acciones">
            <b-button
              tag="router-link"
              :to="'/product/modify/' + props.row.id"
              type="is-small"
            >
              <b-icon icon="pencil"></b-icon>
            </b-button>
            <b-button
              @click="deleteProduct(props.row)"
              type="is-small"
              class="actionButton"
            >
              <b-icon icon="delete"></b-icon>
            </b-button>
          </b-table-column>
        </template>
      </b-table>
    </div>

    <b-loading is-full-page :active.sync="isLoading"></b-loading>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { Product } from "../../models/product";
import { NavigatorProductService } from "../../services/product-service";
import { ProductFilters } from "../../models/productFilters";
import { NavigatorCategoryService } from "../../services/category-service";
import { NavigatorVendorService } from "../../services/vendor-service";
import { Category } from "../../models/category";
import { Vendor } from "../../models/vendor";

@Component
export default class ProductList extends Vue {
  public products: Product[] = [];
  public categories: Category[] = [];
  public vendors: Vendor[] = [];
  public productService: NavigatorProductService = new NavigatorProductService();
  public categoryService: NavigatorCategoryService = new NavigatorCategoryService();
  public vendorService: NavigatorVendorService = new NavigatorVendorService();

  public currentPage = 1;
  public perPage = 10;
  public openFilters = false;
  public productFilters: ProductFilters = new ProductFilters();
  public dayNames = ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"];
  public isLoading = false;

  deleteProduct(product: Product) {
    this.$buefy.dialog.confirm({
      title: "Eliminando Producto",
      message:
        "Estás seguro que deseas <b>eliminar</b> el producto? Esta acción no podrá dehacerse.",
      confirmText: "Eliminar Producto",
      cancelText: "Cancelar",
      type: "is-danger",
      hasIcon: true,
      onConfirm: () => {
        this.isLoading = true;
        this.productService
          .deleteProduct(product.id)
          .then(() => {
            this.isLoading = false;
            this.products.splice(this.products.indexOf(product), 1);
          })
          .catch(e => {
            // this.errorMsg = {
            //   title: "Error",
            //   msg: "An unexpected error has occurred. please try again later."
            // };
            this.isLoading = false;
            console.log("error: ", e);
          });
        this.$buefy.toast.open("Producto eliminado!");
      }
    });
  }

  getCategory(id: string) {
    const elem = this.categories.find(x => x.id === id);
    return elem ? elem.name : "";
  }

  getVendor(id: string) {
    const elem = this.vendors.find(x => x.id === id);
    return elem ? elem.name : "";
  }

  public clearFilters() {
    this.productFilters = new ProductFilters();
  }

  created() {
    this.isLoading = true;
    this.productService
      .getProducts()
      .then(response => {
        this.products = response;
        return this.categoryService.getCategories();
      })
      .then(response => {
        this.categories = response;
        return this.vendorService.getVendors();
      })
      .then(response => {
        this.vendors = response;
        this.isLoading = false;
      })
      .catch(e => {
        // this.errorMsg = {
        //   title: "Error",
        //   msg: "An unexpected error has occurred. please try again later."
        // };
        this.isLoading = false;
        console.log("error: ", e);
      });
  }
}
</script>

<style>
.clickeable {
  cursor: pointer;
}

th {
  /* background-color: #dbdbdb; */
  background-color: #384caf4a;
}

.filtersClass {
  margin: 0em !important;
  padding: 1em;
  background-color: #e0eaff !important;
}

.actionButton {
  margin-left: 5px;
}

table {
  font-size: 13px;
  border: 0px !important;
}

.filtersClass select {
  min-width: 120px;
}
</style>
