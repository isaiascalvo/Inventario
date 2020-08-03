import { apiClient } from "./apiClient";
import { Client, ClientForCreation } from "@/models/client";
import { ClientFilters } from "@/models/clientFilters";

export interface ClientService {
  getClients(): Promise<Client[]>;
  getClientsFiltered(clientFilters: ClientFilters): Promise<Client[]>;
  getClient(id: string): Promise<Client>;
  addClient(client: ClientForCreation): Promise<Client>;
  updateClient(client: Client): Promise<void>;
  deleteClient(clientId: string): Promise<void>;
}

export class NavigatorClientService implements ClientService {
  public async getClients(): Promise<Client[]> {
    return await (await apiClient.get("/clients")).data;
  }

  public async getClientsFiltered(
    clientFilters: ClientFilters
  ): Promise<Client[]> {
    return (
      await apiClient.get(
        "/clients/Filtered?" + this.getQueryString(clientFilters)
      )
    ).data;
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

  public getQueryString(clientFilters: ClientFilters): string {
    const queryString = Object.keys(clientFilters).map(key =>
      clientFilters[key as keyof ClientFilters]
        ? key + "=" + clientFilters[key as keyof ClientFilters]
        : null
    );
    return queryString.filter(x => x !== null).join("&");
  }
}
