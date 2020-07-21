import { apiClient } from "./apiClient";
import { User, UserForCreation } from "@/models/user";

export interface UserService {
  getUsers(): Promise<User[]>;
  getUser(id: string): Promise<User>;
  addUser(user: User): Promise<User>;
  updateUser(user: User): Promise<void>;
  deleteUser(userId: string): Promise<void>;
}

export class NavigatorUserService implements UserService {
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
}
