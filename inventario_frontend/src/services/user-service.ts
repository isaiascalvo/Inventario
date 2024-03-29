import { apiClient } from "./apiClient";
import { User, UserForCreation } from "@/models/user";
import { JwtResult } from "@/models/JwtResult";
import { UserFilters } from "@/models/filters/userFilters";

export interface UserService {
  getUsers(): Promise<User[]>;
  getUser(id: string): Promise<User>;
  addUser(user: UserForCreation): Promise<User>;
  updateUser(user: User): Promise<void>;
  deleteUser(userId: string): Promise<void>;
  getTotalQty(): Promise<number>;
  getTotalQtyByFilters(userFilters: UserFilters): Promise<number>;
  getByPageAndQty(skip: number, qty: number): Promise<User[]>;
  getByFiltersPageAndQty(
    userFilters: UserFilters,
    skip: number,
    qty: number
  ): Promise<User[]>;
}

export class NavigatorUserService implements UserService {
  public async getTotalQty(): Promise<number> {
    return (await apiClient.get("/users/GetTotalQty")).data;
  }

  public async getTotalQtyByFilters(userFilters: UserFilters): Promise<number> {
    return (
      await apiClient.get(
        "/users/GetTotalQtyByFilters?" + this.getQueryString(userFilters)
      )
    ).data;
  }

  public async getByPageAndQty(skip: number, qty: number): Promise<User[]> {
    return (
      await apiClient.get("/users/GetByPageAndQty?skip=" + skip + "&qty=" + qty)
    ).data;
  }

  async getByFiltersPageAndQty(
    userFilters: UserFilters,
    skip: number,
    qty: number
  ): Promise<User[]> {
    return (
      await apiClient.get(
        "/users/GetByFiltersPageAndQty?" +
          this.getQueryString(userFilters) +
          "&skip=" +
          skip +
          "&qty=" +
          qty
      )
    ).data;
  }

  public async getUsers(): Promise<User[]> {
    return await (await apiClient.get("/users")).data;
  }

  public async getUser(id: string): Promise<User> {
    return (await apiClient.get("/users/" + id)).data;
  }

  public addUser(user: UserForCreation): Promise<User> {
    return apiClient.post("/users", user);
  }

  public updateUser(user: User): Promise<void> {
    return apiClient.put("/users/" + user.id, user);
  }

  public deleteUser(userId: string): Promise<void> {
    return apiClient.delete("/users/" + userId);
  }

  public login(
    username: string,
    password: string
  ): Promise<{
    config: object;
    data: JwtResult;
    headers: object;
    request: object;
    status: number;
    statusText: string;
  }> {
    return apiClient.post("login", {
      username: username,
      password: password
    });
  }

  public logout() {
    // remove user from local storage to log user out
    localStorage.removeItem("currentUser");
  }

  public getQueryString(userFilters: UserFilters): string {
    const queryString = Object.keys(userFilters).map(key =>
      userFilters[key as keyof UserFilters]
        ? key + "=" + userFilters[key as keyof UserFilters]
        : null
    );
    return queryString.filter(x => x !== null).join("&");
  }
}
