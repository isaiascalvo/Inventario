import axios from "axios";

export const apiClient = axios.create({
  baseURL:
    process.env.NODE_ENV === "development"
      ? "https://localhost:44386/api"
      : "http://localhost:8081/api",
  withCredentials: false,
  headers: {
    Accept: "application/json",
    "Content-Type": "application/json"
  }
});
