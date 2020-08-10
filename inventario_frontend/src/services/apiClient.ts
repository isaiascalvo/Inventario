import axios from "axios";

export const apiClient = axios.create({
  baseURL: `https://localhost:44386/api`,
  withCredentials: false,
  headers: {
    Accept: "application/json",
    "Content-Type": "application/json"
  }
});
