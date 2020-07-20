<template>
  <div>
    <div class="card">
      <div class="card-content">
        <div class="media">
          <div class="media-content">
            <p class="title is-4">
              {{ product.id ? "Editar Producto" : "Crear Producto" }}
            </p>
          </div>
        </div>

        <div class="content">
          <section>
            <b-field
              label="Nombre:"
              :type="fieldState(product.name) ? 'is-success' : 'is-danger'"
            >
              <b-input
                v-model="product.name"
                placeholder="Ingrese el nombre del producto"
              ></b-input>
            </b-field>

            <b-field
              label="Descripción:"
              :type="
                fieldState(product.description) ? 'is-success' : 'is-danger'
              "
            >
              <b-input
                v-model="product.description"
                placeholder="Ingrese la descripción del producto"
                type="textarea"
              ></b-input>
            </b-field>

            <b-field
              label="Categoría"
              :type="
                fieldState(product.categoryId) ? 'is-success' : 'is-danger'
              "
            >
              <b-select
                v-model="product.categoryId"
                placeholder="Seleccione una categoría"
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

            <b-field
              label="Proveedor"
              :type="fieldState(product.vendorId) ? 'is-success' : 'is-danger'"
            >
              <b-select
                v-model="product.vendorId"
                placeholder="Seleccione un Proveedor"
              >
                <option
                  v-for="vendor in vendors"
                  :key="vendor.id"
                  :value="vendor.id"
                >
                  {{ vendor.name }}
                </option>
              </b-select>
            </b-field>

            <b-field
              label="Código:"
              :type="fieldState(product.code) ? 'is-success' : 'is-danger'"
            >
              <b-input
                v-model="product.code"
                placeholder="Ingrese el código del producto"
              ></b-input>
            </b-field>

            <b-field
              label="Marca:"
              :type="fieldState(product.brand) ? 'is-success' : 'is-danger'"
            >
              <b-input
                v-model="product.brand"
                placeholder="Ingrese la marca del producto"
              ></b-input>
            </b-field>

            <b-field
              label="Precio:"
              :type="
                fieldState(product.price.value) ? 'is-success' : 'is-danger'
              "
            >
              <b-input
                v-model="product.price.value"
                type="number"
                placeholder="Ingrese el precio del producto"
              ></b-input>
            </b-field>

            <div class="field">
              <b-switch v-model="product.available">
                Disponible
              </b-switch>
            </div>

            <div class="field">
              <b-switch v-model="product.active">
                Activo
              </b-switch>
            </div>

            <b-button
              type="submit"
              class="is-success"
              :disabled="!formValid()"
              @click="submit"
            >
              {{ product.id ? "Editar" : "Crear" }}
            </b-button>
            <b-button
              type="button"
              class="is-danger"
              @click="$router.push('/product-list')"
            >
              Cancelar
            </b-button>

            <b-button type="button" class="is-danger" @click="mostrar()">
              Mostrar
            </b-button>
          </section>
        </div>
      </div>
    </div>

    <!-- <ErrorDialog
      :error="errorMsg"
      v-if="errorDialog"
      @close:dialog="errorDialog = $event"
    /> -->
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { NavigatorProductService } from "../../services/product-service";
import { NavigatorCategoryService } from "../../services/category-service";
import { NavigatorVendorService } from "../../services/vendor-service";
import { Product, ProductForCreation } from "../../models/product";
import { Category } from "../../models/category";
import { Vendor } from "../../models/vendor";
import { Price, PriceForCreation } from "../../models/price";
// import { DialogMsg } from "../../models/dialogMsg";
// import ErrorDialog from "../../components/dialogs/error-dialog.vue";

@Component({
  components: {
    // ErrorDialog
  }
})
export default class EditProduct extends Vue {
  public errorDialog = false;
  //   public errorMsg = new DialogMsg();
  public product: Product = new Product();
  public productService: NavigatorProductService = new NavigatorProductService();
  public categoryService: NavigatorCategoryService = new NavigatorCategoryService();
  public vendorService: NavigatorVendorService = new NavigatorVendorService();
  public categories: Category[] = [];
  public vendors: Vendor[] = [];

  fieldState(field: any) {
    switch (typeof field) {
      case "number":
        return field >= 0;
      case "string":
        return field && field.length > 0 ? true : false;
      default:
        return false;
    }
  }

  mostrar() {
    Object.keys(this.product as Product).forEach(key => {
      console.log(key + " " + this.product[key as keyof Product]);
    });
  }

  formValid() {
    let flag = true;
    Object.keys(this.product as Product).forEach(key => {
      console.log(key + ": " + this.product[key as keyof Product] + " type: " + typeof this.product[key as keyof Product]);

      const field = this.product[key as keyof Product];
      switch (typeof this.product[key as keyof Product]) {
        case "string":
          if ((this.product[key as keyof Product] as string).length <= 0) {
            flag = false;
          }
          break;
        case "object":
          if (field instanceof Price && !field.value) {
            flag = false;
          }
          break;
        case "number":
          break;
        case "bigint":
          break;
        case "boolean":
          break;
        case "symbol":
          break;
        case "undefined":
          break;
        case "function":
          break;
        default:
          break;
      }
    });
    return flag;
  }

  public submit() {
    if (!this.product.id) {
      this.newProduct();
    } else {
      this.updateProduct();
    }
  }

  public newProduct() {
    const pr: ProductForCreation = {
      name: this.product.name,
      description: this.product.description,
      code: this.product.code,
      brand: this.product.brand,
      active: this.product.active,
      available: this.product.available,
      categoryId: this.product.categoryId,
      vendorId: this.product.vendorId,
      price: new PriceForCreation()
    };
    pr.price.value = this.product.price.value;
    this.productService
      .addProduct(pr)
      .then(() => {
        this.$router.push({ name: "ProductList" });
      })
      .catch(e => {
        // this.errorMsg = {
        //   title: "Error",
        //   msg: "An unexpected error has occurred. please try again later."
        // };
        // this.errorDialog = true;
        console.log("error: ", e);
        // this.$router.push({ name: "ProductList" });
      });
  }

  public updateProduct() {
    this.productService
      .updateProduct(this.product)
      .then(() => {
        this.$router.push({ name: "ProductList" });
      })
      .catch(e => {
        // this.errorMsg = {
        //   title: "Error",
        //   msg: "An unexpected error has occurred. please try again later."
        // };
        // this.errorDialog = true;
        console.log("error: ", e);
        // this.$router.push({ name: "ProductList" });
      });
  }

  created() {
    this.product.id = this.$route.params.id;
    const p1 = this.categoryService.getCategories();
    const p2 = this.vendorService.getVendors();
    Promise.all([p1, p2]).then(collections => {
      this.categories = collections[0] as Category[];
      this.vendors = collections[1] as Vendor[];
      if (this.product.id) {
        this.productService.getProduct(this.product.id).then(response => {
          this.product = response as Product;
        });
      }
    });
  }
}
</script>

<style></style>
