import { apiClient } from "./apiClient";
import { Client, ClientForCreation } from "@/models/client";

export interface ClientService {
  getClients(): Promise<Client[]>;
  getClient(id: string): Promise<Client>;
  addClient(client: Client): Promise<Client>;
  updateClient(client: Client): Promise<void>;
  deleteClient(clientId: string): Promise<void>;
}

export class NavigatorClientService implements ClientService {
  public async getClients(): Promise<Client[]> {
    return await (await apiClient.get("/clients")).data;
  }

  public async getClient(id: string): Promise<Client> {
    return (await apiClient.get("/clients/" + id)).data;
  }

  public addClient(client: ClientForCreation): Promise<Client> {
    return apiClient.post("/clients", client);
  }

  public updateClient(client: Client): Promise<void> {
    return apiClient.put("/clients/" + client.id, client);
  }

  public deleteClient(clientId: string): Promise<void> {
    return apiClient.delete("/clients/" + clientId);
  }
}
