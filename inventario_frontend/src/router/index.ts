import Vue from "vue";
import VueRouter, { RouteConfig } from "vue-router";
import Home from "../views/Home.vue";

Vue.use(VueRouter);

const routes: Array<RouteConfig> = [
  {
    path: "/",
    name: "Home",
    component: Home
  },
  {
    path: "/product-list",
    name: "ProductList",
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () =>
      import(/* webpackChunkName: "product" */ "../views/product/product-list.vue")
  },
  {
    path: "/product/new",
    name: "NewProduct",
    component: () =>
      import(/* webpackChunkName: "product" */ "../views/product/edit-product.vue")
  },
  {
    path: "/product/modify/:id",
    name: "ModifyProduct",
    component: () =>
      import(/* webpackChunkName: "product" */ "../views/product/edit-product.vue"),
    props: true
  },
  {
    path: "/category-list",
    name: "CategoryList",
    component: () =>
      import(/* webpackChunkName: "category" */ "../views/category/category-list.vue"),
  },
  {
    path: "/category/new",
    name: "NewCategory",
    component: () =>
      import(/* webpackChunkName: "category" */ "../views/category/edit-category.vue"),
  },
  {
    path: "/category/modify/:id",
    name: "ModifyCategory",
    component: () =>
      import(/* webpackChunkName: "category" */ "../views/category/edit-category.vue"),
    props: true
  },
  {
    path: "/vendor-list",
    name: "VendorList",
    component: () =>
      import(/* webpackChunkName: "vendor" */ "../views/vendor/vendor-list.vue"),
  },
  {
    path: "/vendor/new",
    name: "NewVendor",
    component: () =>
      import(/* webpackChunkName: "vendor" */ "../views/vendor/edit-vendor.vue"),
  },
  {
    path: "/vendor/modify/:id",
    name: "ModifyVendor",
    component: () =>
      import(/* webpackChunkName: "vendor" */ "../views/vendor/edit-vendor.vue"),
    props: true
  },
  {
    path: "/user-list",
    name: "UserList",
    component: () =>
      import(/* webpackChunkName: "user" */ "../views/user/user-list.vue"),
  },
  {
    path: "/user/new",
    name: "NewUser",
    component: () =>
      import(/* webpackChunkName: "user" */ "../views/user/edit-user.vue"),
  },
  {
    path: "/user/modify/:id",
    name: "ModifyUser",
    component: () =>
      import(/* webpackChunkName: "user" */ "../views/user/edit-user.vue"),
    props: true
  },
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes
});

export default router;
