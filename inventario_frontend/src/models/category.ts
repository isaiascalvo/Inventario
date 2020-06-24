export class CategoryForCreation {
    public name!: string;
    public description!: string;
}
  
export class Category extends CategoryForCreation {
    public id!: string;
}