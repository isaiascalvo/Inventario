export class UserForCreation {
    public username: string | undefined = undefined;
    public password: string | undefined = undefined;
    public name: string | undefined = undefined;
    public lastname: string | undefined = undefined;
    public dni: string | undefined = undefined;
    public phone: string | undefined = undefined;
    public mail: string | undefined = undefined;
    public active = true;
}
  
export class User extends UserForCreation {
    public id!: string;
}