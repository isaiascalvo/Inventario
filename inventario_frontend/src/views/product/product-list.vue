<template>
  <div>
    <section class="hero is-light p-1">
      <div class="hero-head">
        <div class="level">
          <div>
            <h1 class="title is-6">Lista de Productos</h1>
          </div>
          <div>
            <b-button
              type="is-dark"
              @click="genetarePdf()"
              class="mx-1"
              size="is-small"
            >
              Imprimir Listado
            </b-button>
            <b-button
              type="is-info"
              tag="router-link"
              to="/product/new"
              class="mx-1"
              size="is-small"
            >
              Nuevo Producto
            </b-button>
            <b-button
              @click="openFilters = !openFilters"
              class="is-pulled-right"
              type="is-primary"
              size="is-small"
              :icon-right="
                openFilters ? 'filter-variant-minus' : 'filter-variant'
              "
            >
              {{ openFilters ? "Ocultar Filtros" : "Mostrar Filtros" }}
            </b-button>
          </div>
        </div>
      </div>
    </section>

    <div>
      <div class="columns filtersClass level" v-if="openFilters">
        <div class="column is-10">
          <b-field grouped group-multiline>
            <b-field label-position="on-border" label="Código">
              <b-input
                v-model="productFilters.code"
                placeholder="Código"
                size="is-small"
              ></b-input>
            </b-field>

            <b-field label-position="on-border" label="Nombre">
              <b-input
                v-model="productFilters.name"
                placeholder="Nombre"
                size="is-small"
              ></b-input>
            </b-field>

            <b-field label-position="on-border" label="Descripción">
              <b-input
                v-model="productFilters.description"
                placeholder="Descripción"
                size="is-small"
              ></b-input>
            </b-field>

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

            <b-field label-position="on-border" label="Marca">
              <b-input
                v-model="productFilters.brand"
                placeholder="Marca"
                size="is-small"
              ></b-input>
            </b-field>

            <b-field label-position="on-border" label="Fecha">
              <b-datetimepicker
                v-model="productFilters.dateDate"
                rounded
                placeholder="Seleccione fecha y hora"
                icon="calendar-today"
                trap-focus
                horizontal-time-picker
                size="is-small"
                editable
                :date-parser="parseDate"
                append-to-body
              >
              </b-datetimepicker>
            </b-field>
          </b-field>
        </div>

        <div class="column level-right">
          <b-button
            type="is-dark"
            class="mx-1"
            size="is-small"
            @click="applyFilters()"
          >
            Aplicar
          </b-button>
          <b-button @click="clearFilters()" size="is-small" type="is-default">
            Limpiar
          </b-button>
        </div>
      </div>

      <b-table
        striped
        hoverable
        scrollable
        :data="products"
        id="my-table"
        paginated
        backend-pagination
        :total="totalPages"
        :per-page="perPage"
        :current-page.sync="currentPage"
        @page-change="onPageChange"
        aria-next-label="Next page"
        aria-previous-label="Previous page"
        aria-page-label="Page"
        aria-current-label="Current page"
      >
        <template slot="empty">
          No hay productos registrados
        </template>
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
          <b-table-column field="minimumStock" label="Stock mínimo" centered>
            {{ props.row.minimumStock }}
          </b-table-column>
          <b-table-column field="stock" label="Stock" centered>
            <span
              :class="[
                'tag',
                {
                  'is-danger':
                    props.row.minimumStock &&
                    props.row.stock < props.row.minimumStock
                },
                {
                  'is-success': !(
                    props.row.minimumStock &&
                    props.row.stock < props.row.minimumStock
                  )
                }
              ]"
            >
              {{ props.row.stock }}
            </span>
          </b-table-column>
          <b-table-column field="unitOfMeasurement" label="Unidad">
            {{ props.row.unitOfMeasurement }}
          </b-table-column>
          <b-table-column field="purchasePrice" label="Precio de compra">
            $ {{ props.row.purchasePrice.value }}
          </b-table-column>
          <b-table-column field="salePrice" label="Precio de venta">
            $ {{ props.row.salePrice.value }}
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

        <template slot="bottom-left">
          <span class="ml-1">
            <b-field label="Productos por página:" label-position="on-border">
              <b-select v-model="perPage" @select="getProducts()">
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
import { Product } from "../../models/product";
import { NavigatorProductService } from "../../services/product-service";
import { ProductFilters } from "../../models/filters/productFilters";
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
  public totalPages = 0;
  public filtersApplied = false;

  deleteProduct(product: Product) {
    this.$buefy.dialog.confirm({
      title: "Eliminando Producto",
      message:
        "Estás seguro que deseas <b>eliminar</b> el producto? Esta acción no podrá deshacerse.",
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
        this.$buefy.toast.open("Producto eliminado!");
      }
    });
  }

  onPageChange(page: number) {
    this.currentPage = page;
    this.getProducts();
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
    this.filtersApplied = false;
    this.productFilters = new ProductFilters();
    this.getProducts();
  }

  public applyFilters() {
    this.isLoading = true;
    this.filtersApplied = true;
    this.getProducts();
  }

  getProducts(): Promise<void> {
    this.isLoading = true;
    let rta: Promise<void>;
    if (this.filtersApplied) {
      rta = this.productService
        .getTotalQtyOfProductsByFilters(this.productFilters)
        .then(qty => {
          this.totalPages = qty;
          return this.productService.getProductsByFiltersPageAndQty(
            this.productFilters,
            (this.currentPage - 1) * this.perPage,
            this.perPage
          );
        })
        .then(response => {
          this.products = response;
        });
    } else {
      rta = this.productService
        .getTotalQtyOfProducts()
        .then(qty => {
          this.totalPages = qty;
          return this.productService.getProductsByPageAndQty(
            (this.currentPage - 1) * this.perPage,
            this.perPage
          );
        })
        .then(response => {
          this.products = response;
        });
    }

    this.isLoading = false;
    return rta;
  }

  genetarePdf() {
    this.isLoading = true;
    // this.productService.generatePdf().then(response => {
    //   this.isLoading = false;
    //   console.log(response);
    //   //Create a Blob from the PDF Stream
    //   const file = new Blob([response.data], { type: "application/pdf" });
    //   //Build a URL from the file
    //   const fileURL = URL.createObjectURL(file);
    //   //Open the URL on new Window
    //   window.open(fileURL);
    // });
    this.productService.generatePdf().then(
      data => {
        const downloadedFile = new Blob([data.data], {
          type: data.data.type
        });
        const a = document.createElement("a");
        a.setAttribute("style", "display:none;");
        document.body.appendChild(a);
        a.download =
          "ListadoDeProductos-" +
          new Date().toISOString().substring(0, 10) +
          ".pdf";
        a.href = URL.createObjectURL(downloadedFile);
        a.target = "_blank";
        a.click();
        document.body.removeChild(a);
        this.isLoading = false;
      },
      error => {
        this.isLoading = false;
        console.log(error);
      }
    );
  }

  parseDate(date: string) {
    const dateSplited = date.split("/");
    return new Date(
      Date.parse(dateSplited[1] + "/" + dateSplited[0] + "/" + dateSplited[2])
    );
  }

  created() {
    this.getProducts()
      .then(() => {
        this.isLoading = true;
        return this.categoryService.getCategories();
      })
      .then(response => {
        this.categories = response;
        return this.vendorService.getVendors();
      })
      .then(response => {
        this.vendors = response;
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
      })
      .finally(() => {
        this.isLoading = false;
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

/* tr.is-danger {
  background: #ff000042;
  color: black;
} */
</style>
