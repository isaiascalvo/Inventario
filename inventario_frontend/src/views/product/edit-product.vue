<template>
  <div>
    <div class="card column is-8 is-offset-2">
      <div class="card-content">
        <div class="media">
          <div class="media-content">
            <p class="title is-4">
              {{ product.id ? "Editar Producto" : "Crear Producto" }}
            </p>
          </div>
        </div>

        <div class="content">
          <form @submit.prevent="submit()" class="flex-text-left">
            <div class="columns">
              <div class="column">
                <b-field label="Nombre:">
                  <b-input
                    v-model="product.name"
                    placeholder="Ingrese el nombre del producto"
                    required
                    validation-message="Ingrese el nombre del producto"
                  ></b-input>
                </b-field>

                <b-field label="Marca:">
                  <b-input
                    v-model="product.brand"
                    placeholder="Ingrese la marca del producto"
                    required
                    validation-message="Ingrese la marca del producto"
                  ></b-input>
                </b-field>
              </div>
              <div class="column">
                <b-field label="Descripción:">
                  <b-input
                    v-model="product.description"
                    placeholder="Ingrese la descripción del producto"
                    type="textarea"
                    required
                    validation-message="Ingrese la descripción del producto"
                  ></b-input>
                </b-field>
              </div>
            </div>

            <div class="columns">
              <div class="column">
                <b-field label="Categoría">
                  <b-select
                    v-model="product.categoryId"
                    placeholder="Seleccione una categoría"
                    expanded
                    required
                    validation-message="Ingrese la categoría del producto"
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
              </div>
              <div class="column">
                <b-field label="Proveedor">
                  <b-select
                    v-model="product.vendorId"
                    placeholder="Seleccione un Proveedor"
                    expanded
                    required
                    validation-message="Seleccione un Proveedor"
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
              </div>
            </div>

            <div class="columns">
              <div class="column">
                <b-field label="Precio de Compra:">
                  <b-input
                    v-model="product.purchasePrice.value"
                    type="number"
                    placeholder="Ingrese el precio de compra del producto"
                    required
                    validation-message="Ingrese el precio de compra del producto"
                  ></b-input>
                </b-field>
              </div>

              <div class="column">
                <b-field label="Precio de Venta:">
                  <b-input
                    v-model="product.salePrice.value"
                    type="number"
                    placeholder="Ingrese el precio de venta del producto"
                    required
                    validation-message="Ingrese el precio de venta del producto"
                  ></b-input>
                </b-field>
              </div>
            </div>

            <div class="columns">
              <div class="column">
                <b-field label="Stock:">
                  <b-input
                    v-model="product.stock"
                    type="number"
                    placeholder="Ingrese el stock del producto"
                    disabled
                  ></b-input>
                </b-field>
              </div>

              <div class="column">
                <b-field label="Unidad de medida:">
                  <b-input
                    v-model="product.unitOfMeasurement"
                    placeholder="Ej: unidad, kg, m, cm, etc."
                    required
                    validation-message="Ingrese la unidad de medida"
                  ></b-input>
                </b-field>
              </div>
            </div>

            <div class="columns">
              <div class="column">
                <b-field label="Stock mínimo: (Opcional)">
                  <b-input
                    v-model="product.minimumStock"
                    type="number"
                    placeholder="Ingrese el stock mínimo del producto"
                  ></b-input>
                </b-field>
              </div>

              <div class="column">
                <b-field label="Código: (Opcional)">
                  <b-input
                    v-model="product.code"
                    placeholder="Ingrese el código del producto"
                  ></b-input>
                </b-field>
              </div>
            </div>

            <b-button
              native-type="submit"
              class="is-success mr-1"
              :disabled="!formValid()"
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
          </form>
        </div>
      </div>
    </div>
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
import { PriceForCreation } from "../../models/price";
import {
  fieldStateValidation,
  formValidation
} from "../../utils/common-functions";

@Component
export default class EditProduct extends Vue {
  public product: Product = new Product();
  public productService: NavigatorProductService = new NavigatorProductService();
  public categoryService: NavigatorCategoryService = new NavigatorCategoryService();
  public vendorService: NavigatorVendorService = new NavigatorVendorService();
  public categories: Category[] = [];
  public vendors: Vendor[] = [];
  public isLoading = false;

  fieldState(field: unknown) {
    return fieldStateValidation(field);
  }

  formValid() {
    // const result = formValidation(this.product as never);
    // console.log(result);
    // return (
    //   (result === "price;" || result === "category;vendor;price;") &&
    //   this.product.price.value
    // );

    const result = formValidation(this.product as never);
    const nulleableProps = [
      "",
      "minimumStock",
      "purchasePrice",
      "salePrice",
      "category",
      "vendor",
      "code"
    ];
    const splitedResult = result.split(";");
    return (
      (result === "" ||
        splitedResult.every(x => nulleableProps.some(y => y === x))) &&
      this.product.purchasePrice.value &&
      this.product.salePrice.value
    );
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
      categoryId: this.product.categoryId,
      vendorId: this.product.vendorId,
      purchasePrice: new PriceForCreation(),
      salePrice: new PriceForCreation(),
      minimumStock: this.product.minimumStock
        ? +this.product.minimumStock
        : undefined,
      stock: +this.product.stock,
      unitOfMeasurement: this.product.unitOfMeasurement
    };
    pr.purchasePrice.value = +this.product.purchasePrice.value;
    pr.salePrice.value = +this.product.salePrice.value;
    this.isLoading = true;
    this.productService
      .addProduct(pr)
      .then(() => {
        this.isLoading = false;
        this.$router.push({ name: "ProductList" });
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
        this.$router.push({ name: "ProductList" });
      });
  }

  public updateProduct() {
    this.isLoading = true;
    this.product.purchasePrice.value = +this.product.purchasePrice.value;
    this.product.salePrice.value = +this.product.salePrice.value;
    this.productService
      .updateProduct(this.product)
      .then(() => {
        this.isLoading = false;
        this.$router.push({ name: "ProductList" });
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
        this.$router.push({ name: "ProductList" });
      });
  }

  created() {
    this.product.id = this.$route.params.id;
    this.isLoading = true;
    const p1 = this.categoryService.getCategories();
    const p2 = this.vendorService.getVendors();
    Promise.all([p1, p2]).then(collections => {
      this.categories = collections[0] as Category[];
      this.vendors = collections[1] as Vendor[];
      if (this.product.id) {
        this.productService
          .getProduct(this.product.id)
          .then(response => {
            this.product = response as Product;
            this.isLoading = false;
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
      } else {
        this.isLoading = false;
      }
    });
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
