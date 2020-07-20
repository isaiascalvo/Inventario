export class CategoryForCreation {
    public name: string | undefined = undefined;
    public description: string | undefined = undefined;
}
  
export class Category extends CategoryForCreation {
    public id!: string;
}