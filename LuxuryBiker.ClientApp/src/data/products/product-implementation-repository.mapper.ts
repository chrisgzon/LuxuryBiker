import { Mapper } from '@base/mapper';
import { ProductEntity } from './product.entity';
import { ProductModel } from '@domain/products/models/product.model';


export class ProductImplementationRepositoryMapper extends Mapper<ProductEntity, ProductModel> {
    mapFrom(param: ProductEntity): ProductModel {
        return {
            code: param.code,
            name: param.name,
            description: param.description,
            reference: param.reference,
            status: param.status,
            stock: param.stock,
            value: param.value,
        };
    }
    mapTo(param: ProductModel): ProductEntity {
        return {
            code: param.code,
            name: param.name,
            description: param.description,
            reference: param.reference,
            status: param.status,
            stock: param.stock,
            value: param.value,
        }
    }
}