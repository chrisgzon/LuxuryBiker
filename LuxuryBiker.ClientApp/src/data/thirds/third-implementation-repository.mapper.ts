import { Mapper } from '@base/mapper';
import { ThirdModel } from '@domain/thirds/models/third.model';
import { ThirdEntity } from '@data/thirds/third.entity';


export class ThirdImplementationRepositoryMapper extends Mapper<ThirdEntity, ThirdModel> {
    mapFrom(param: ThirdEntity): ThirdModel {
        return {
            id: param.id,
            email: param.email,
            fullName: `${param.name} ${param.surnames}`,
            active: param.active,
            identification: param.identification,
            name: param.name,
            surnames: param.surnames,
            address: param.address,
            cellPhone: param.cellPhone,
            typeId: param.type.id
        };
    }
    mapTo(param: ThirdModel): ThirdEntity {
        return {
            id: param.id,
            email: param.email,
            active: param.active,
            identification: param.identification,
            surnames: param.surnames,
            name: param.name,
            address: param.address,
            cellPhone: param.cellPhone,
            typeId: param.typeId,
            type: {
                id: param.typeId,
                name: ''
            },
        }
    }
}