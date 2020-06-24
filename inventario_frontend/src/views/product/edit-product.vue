<template>
  <div>
    <b-card :title="product.id ? 'Editar Producto' : 'Crear Producto'">
      <!-- <v-card-title>{{ id ? "Update" : "Create" }} Product</v-card-title> -->
      <b-form @submit.prevent="submit">
        <b-form-group id="input-group-1" label="Nombre:" label-for="name-input">
          <b-form-input
            id="name-input"
            v-model="product.name"
            :state="fieldState(product.name)"
            type="text"
            required
            placeholder="Ingrese el nombre del producto"
          ></b-form-input>
          <b-form-invalid-feedback :state="fieldState(product.name)">
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
            v-model="product.description"
            :state="fieldState(product.description)"
            type="text"
            required
            placeholder="Ingrese la descripción del producto"
          ></b-form-input>
          <b-form-invalid-feedback :state="fieldState(product.description)">
            La descripción es requerida
          </b-form-invalid-feedback>
        </b-form-group>

        <b-form-group
          id="input-group-2"
          label="Categoría:"
          label-for="description-input"
        >
          <b-form-select v-model="product.categoryId" class="mb-3">
            <!-- This slot appears above the options from 'options' prop -->
            <template v-slot:first>
              <b-form-select-option :value="null" disabled>
                -- Por favor seleccione una categoría --
              </b-form-select-option>
            </template>

            <!-- These options will appear after the ones from 'options' prop -->
            <b-form-select-option
              v-for="category in categories"
              :key="category.id"
              :value="category.id"
              >{{ category.name }}</b-form-select-option
            >
          </b-form-select>
          <b-form-invalid-feedback :state="fieldState(product.code)">
            La categoría es requerida
          </b-form-invalid-feedback>
        </b-form-group>

        <b-form-group
          id="input-group-2"
          label="Proveedor:"
          label-for="vendor-input"
        >
          <b-form-select v-model="product.vendorId" class="mb-3">
            <!-- This slot appears above the options from 'options' prop -->
            <template v-slot:first>
              <b-form-select-option :value="null" disabled>
                -- Por favor seleccione una categoría --
              </b-form-select-option>
            </template>

            <!-- These options will appear after the ones from 'options' prop -->
            <b-form-select-option
              v-for="vendor in vendors"
              :key="vendor.id"
              :value="vendor.id"
              >{{ vendor.name }}</b-form-select-option
            >
          </b-form-select>
          <b-form-invalid-feedback :state="fieldState(product.code)">
            La categoría es requerida
          </b-form-invalid-feedback>
        </b-form-group>

        <b-form-group id="input-group-2" label="Código:" label-for="code-input">
          <b-form-input
            id="code-input"
            v-model="product.code"
            :state="fieldState(product.code)"
            type="text"
            required
            placeholder="Ingrese el código del producto"
          ></b-form-input>
          <b-form-invalid-feedback :state="fieldState(product.code)">
            El código es requerido
          </b-form-invalid-feedback>
        </b-form-group>

        <b-form-group id="input-group-1" label="Marca:" label-for="brand-input">
          <b-form-input
            id="brand-input"
            v-model="product.brand"
            :state="fieldState(product.brand)"
            type="text"
            required
            placeholder="Ingrese la marca del producto"
          ></b-form-input>
          <b-form-invalid-feedback :state="fieldState(product.brand)">
            El teléfono es requerido
          </b-form-invalid-feedback>
        </b-form-group>

        <b-form-group
          id="input-group-2"
          label="Precio:"
          label-for="price-input"
        >
          <b-form-input
            id="price-input"
            v-model="product.price.value"
            :state="fieldState(product.price.value)"
            type="number"
            required
            placeholder="Ingrese el precio del producto"
          ></b-form-input>
          <b-form-invalid-feedback :state="fieldState(product.price.value)">
            El precio es requerido
          </b-form-invalid-feedback>
        </b-form-group>
        <b-form-group id="input-group-1" label="" label-for="available-input">
          <b-form-checkbox
            id="available-input"
            v-model="product.available"
            switch
          >
            Disponible
          </b-form-checkbox>
        </b-form-group>

        <b-form-group id="input-group-1" label="" label-for="active-input">
          <b-form-checkbox id="active-input" v-model="product.active" switch>
            Activo
          </b-form-checkbox>
        </b-form-group>

        <b-button type="submit" variant="primary" :disabled="!formValid()">
          {{ product.id ? "Editar" : "Crear" }}
        </b-button>
        <b-button
          type="button"
          variant="danger"
          @click="$router.push('/product-list')"
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
import { NavigatorProductService } from "../../services/product-service";
import { NavigatorCategoryService } from "../../services/category-service";
import { NavigatorVendorService } from "../../services/vendor-service";
import { Product } from "../../models/product";
import { Category } from "../../models/category";
import { Vendor } from "../../models/vendor";
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
    return field && field.length > 0 ? true : false;
  }

  formValid() {
    let flag = true;
    Object.keys(this.product as Product).forEach(key => {
      switch (typeof this.product[key as keyof Product]) {
        case "string":
          if ((this.product[key as keyof Product] as string).length <= 0) {
            flag = false;
          }
          break;
        case "object":
          flag = false;
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
    this.productService
      .addProduct(this.product)
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
        this.$router.push({ name: "ProductList" });
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
