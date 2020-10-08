import { apiClient } from "@/services/apiClient";
import { JwtResult } from "@/models/JwtResult";
import router from "@/router";

export default function JwtInterceptor() {
  apiClient.interceptors.request.use(
    request => {
      const cu = sessionStorage.getItem("currentUser");
      if (cu) {
        const currentUser: JwtResult = JSON.parse(cu);
        if (currentUser && currentUser.token) {
          request.headers["Authorization"] = `Bearer ${currentUser.token}`;
        }
      }
      return request;
    },
    error => {
      console.log(error);
      return Promise.reject(error);
    }
  );

  apiClient.interceptors.response.use(
    response => {
      if (response.status === 200 || response.status === 201) {
        return Promise.resolve(response);
      } else {
        return Promise.reject(response);
      }
    },
    error => {
      console.log(error);
      if (error.response.data.status) {
        switch (error.response.data.status) {
          case 400:
            //do something
            console.log(error);
            break;
          case 401:
            alert("session expired");
            break;
          case 403:
            router.replace({
              path: "/login",
              query: { redirect: router.currentRoute.fullPath }
            });
            break;
          case 404:
            alert("page not exist");
            break;
          case 502:
            setTimeout(() => {
              router.replace({
                path: "/login",
                query: {
                  redirect: router.currentRoute.fullPath
                }
              });
            }, 1000);
        }
        return Promise.reject(error.response.data);
      }
    }
  );
}
