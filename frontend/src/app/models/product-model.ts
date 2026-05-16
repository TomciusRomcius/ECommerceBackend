import CategoryModel from './category-model';
import ManufacturerModel from './manufacturer-model';

export default interface ProductModel {
  productId: number;
  name: string;
  description: string;
  price: number;
  manufacturerId: number;
  categoryId: number;
  manufacturer: ManufacturerModel;
  category: CategoryModel;
}
