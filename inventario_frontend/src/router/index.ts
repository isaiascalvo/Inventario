import Vue from "vue";
import VueRouter, { RouteConfig } from "vue-router";
// import Home from "../views/Home.vue";
import { JwtResult } from "@/models/JwtResult";
import { getCurrentUser } from "@/utils/common-functions";

Vue.use(VueRouter);

const routes: Array<RouteConfig> = [
  {
    path: "/",
    name: "Login",
    component: () =>
      import(/* webpackChunkName: "login" */ "../views/login.vue")
  },
  {
    path: "/product-list",
    name: "ProductList",
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () =>
      import(
        /* webpackChunkName: "product" */ "../views/product/product-list.vue"
      )
  },
  {
    path: "/product/new",
    name: "NewProduct",
    component: () =>
      import(
        /* webpackChunkName: "product" */ "../views/product/edit-product.vue"
      )
  },
  {
    path: "/product/modify/:id",
    name: "ModifyProduct",
    component: () =>
      import(
        /* webpackChunkName: "product" */ "../views/product/edit-product.vue"
      ),
    props: true
  },
  {
    path: "/category-list",
    name: "CategoryList",
    component: () =>
      import(
        /* webpackChunkName: "category" */ "../views/category/category-list.vue"
      )
  },
  {
    path: "/category/new",
    name: "NewCategory",
    component: () =>
      import(
        /* webpackChunkName: "category" */ "../views/category/edit-category.vue"
      )
  },
  {
    path: "/category/modify/:id",
    name: "ModifyCategory",
    component: () =>
      import(
        /* webpackChunkName: "category" */ "../views/category/edit-category.vue"
      ),
    props: true
  },
  {
    path: "/vendor-list",
    name: "VendorList",
    component: () =>
      import(/* webpackChunkName: "vendor" */ "../views/vendor/vendor-list.vue")
  },
  {
    path: "/vendor/new",
    name: "NewVendor",
    component: () =>
      import(
        /* webpackChunkName: "vendor" */
        "../views/vendor/edit-vendor.vue"
      )
  },
  {
    path: "/vendor/modify/:id",
    name: "ModifyVendor",
    component: () =>
      import(
        /* webpackChunkName: "vendor" */ "../views/vendor/edit-vendor.vue"
      ),
    props: true
  },
  {
    path: "/user-list",
    name: "UserList",
    component: () =>
      import(/* webpackChunkName: "user" */ "../views/user/user-list.vue")
  },
  {
    path: "/user/new",
    name: "NewUser",
    component: () =>
      import(/* webpackChunkName: "user" */ "../views/user/edit-user.vue")
  },
  {
    path: "/user/modify/:id",
    name: "ModifyUser",
    component: () =>
      import(/* webpackChunkName: "user" */ "../views/user/edit-user.vue"),
    props: true
  },
  {
    path: "/client-list",
    name: "ClientList",
    component: () =>
      import(/* webpackChunkName: "client" */ "../views/client/client-list.vue")
  },
  {
    path: "/client/new",
    name: "NewClient",
    component: () =>
      import(
        /* webpackChunkName: "client" */
        "../views/client/edit-client.vue"
      )
  },
  {
    path: "/client/modify/:id",
    name: "ModifyClient",
    component: () =>
      import(
        /* webpackChunkName: "client" */ "../views/client/edit-client.vue"
      ),
    props: true
  },
  {
    path: "/product-entry-list",
    name: "ProductEntryList",
    component: () =>
      import(
        /* webpackChunkName: "productEntry" */
        "../views/product-entry/product-entry-list.vue"
      )
  },
  {
    path: "/product-entry/new",
    name: "NewProductEntry",
    component: () =>
      import(
        /* webpackChunkName: "productEntry" */
        "../views/product-entry/edit-product-entry.vue"
      )
  },
  {
    path: "/product-entry/modify/:id",
    name: "ModifyProductEntry",
    component: () =>
      import(
        /* webpackChunkName: "productEntry" */
        "../views/product-entry/edit-product-entry.vue"
      ),
    props: true
  },
  {
    path: "/sale-list",
    name: "SaleList",
    component: () =>
      import(
        /* webpackChunkName: "sale" */
        "../views/sale/sale-list.vue"
      )
  },
  {
    path: "/sale/new",
    name: "NewSale",
    component: () =>
      import(
        /* webpackChunkName: "sale" */
        "../views/sale/edit-sale.vue"
      )
  },
  {
    path: "/sale/modify/:id",
    name: "ModifySale",
    component: () =>
      import(
        /* webpackChunkName: "sale" */
        "../views/sale/edit-sale.vue"
      ),
    props: true
  },
  {
    path: "/fee-rule-list",
    name: "FeeRuleList",
    component: () =>
      import(
        /* webpackChunkName: "fee-rule" */
        "../views/fee-rule/fee-rule-list.vue"
      )
  },
  {
    path: "/fee-rule/new",
    name: "NewFeeRule",
    component: () =>
      import(
        /* webpackChunkName: "fee-rule" */
        "../views/fee-rule/edit-fee-rule.vue"
      )
  },
  {
    path: "/fee-rule/modify/:id",
    name: "ModifyFeeRule",
    component: () =>
      import(
        /* webpackChunkName: "fee-rule" */
        "../views/fee-rule/edit-fee-rule.vue"
      ),
    props: true
  },
  {
    path: "/miscellaneous-expenses-list",
    name: "MiscellaneousExpensesList",
    component: () =>
      import(
        /* webpackChunkName: "miscellaneous-expenses" */
        "../views/miscellaneous-expenses/miscellaneous-expenses-list.vue"
      )
  },
  {
    path: "/miscellaneous-expenses/new",
    name: "NewMiscellaneousExpenses",
    component: () =>
      import(
        /* webpackChunkName: "miscellaneous-expenses" */
        "../views/miscellaneous-expenses/edit-miscellaneous-expenses.vue"
      )
  },
  {
    path: "/miscellaneous-expenses/modify/:id",
    name: "ModifyMiscellaneousExpenses",
    component: () =>
      import(
        /* webpackChunkName: "miscellaneous-expenses" */
        "../views/miscellaneous-expenses/edit-miscellaneous-expenses.vue"
      ),
    props: true
  },
  {
    path: "/commission-list",
    name: "CommissionList",
    component: () =>
      import(
        /* webpackChunkName: "commission" */
        "../views/commission/commission-list.vue"
      )
  },
  {
    path: "/commission/new",
    name: "NewCommission",
    component: () =>
      import(
        /* webpackChunkName: "commission" */
        "../views/commission/edit-commission.vue"
      )
  },
  {
    path: "/commission/modify/:id",
    name: "ModifyCommission",
    component: () =>
      import(
        /* webpackChunkName: "commission" */
        "../views/commission/edit-commission.vue"
      ),
    props: true
  },
  {
    path: "/periodic-report",
    name: "PeriodicReport",
    component: () =>
      import(
        /* webpackChunkName: "report" */
        "../views/reports/periodic-report.vue"
      ),
    meta: { requiredAuth: true }
  },
  {
    path: "/fees-report",
    name: "FeesReport",
    component: () =>
      import(
        /* webpackChunkName: "report" */
        "../views/reports/fees-report.vue"
      ),
    meta: { requiredAuth: true }
  }
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes
});

router.beforeEach((to, from, next) => {
  if (to.path !== "/" && sessionStorage.getItem("currentUser") === null) {
    next({ name: "Login" });
    alert("Debe iniciar sesión para continuar.");
  } else {
    next();
  }

  // if (!to.meta.noRequiredAuth) {
  //   if (sessionStorage.getItem("currentUser") === null) {
  //     next({ name: "Login" });
  //   } else {
  //     next();
  //   }
  // } else {
  //   next();
  // }
});

router.beforeEach((to, from, next) => {
  if (to.meta.requiredAuth) {
    const currentUser: JwtResult | null = getCurrentUser();
    if (!currentUser || !currentUser.isAdmin) {
      alert("Acceso denegado.\nDebe ser administrador para poder acceder.");
      next({ name: "ProductList" });
    } else {
      next();
    }
  } else {
    next();
  }
});

router.beforeEach((to, from, next) => {
  if (to.path === "/login" && sessionStorage.getItem("currentUser") !== null) {
    next({ name: "ProductList" });
  } else {
    next();
  }
});
export default router;
