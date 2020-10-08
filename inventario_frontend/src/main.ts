import Vue from "vue";
import App from "./App.vue";
import "./registerServiceWorker";
import router from "./router";
import store from "./store";
import Buefy from "buefy";
import "buefy/dist/buefy.css";
import JwtInterceptor from "./interceptors/JwtInterceptor";

Vue.config.productionTip = false;

Vue.use(Buefy, {
  defaultDateParser: (date: string) => {
    const dateSplited = date.split("/");
    return new Date(
      Date.parse(dateSplited[1] + "/" + dateSplited[0] + "/" + dateSplited[2])
    );
  },
  defaultDayNames: ["Dom", "Lun", "Mar", "Mie", "Jue", "Vie", "Sab"],
  defaultMonthNames: [
    "Enero",
    "Febrero",
    "Marzo",
    "Abril",
    "Mayo",
    "Junio",
    "Julio",
    "Agosto",
    "Septiembre",
    "Octubre",
    "Noviembre",
    "Diceimbre"
  ]
});

JwtInterceptor();

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount("#app");
