import { Mapper } from '@base/mapper';
import { ThirdModel } from '@domain/thirds/models/third.model';
import { ThirdEntity } from '@data/thirds/third.entity';


export class ThirdImplementationRepositoryMapper extends Mapper<ThirdEntity, ThirdModel> {
    mapFrom(param: ThirdEntity): ThirdModel {
        const typeId = param.typeId ?? param.type?.id ?? 0;
        return {
            id: param.id,
            email: param.email,
            fullName: `${param.name ?? ''} ${param.surnames ?? ''}`.trim(),
            active: param.active,
            identification: param.identification,
            name: param.name,
            surnames: param.surnames,
            address: param.address,
            cellPhone: param.cellPhone,
            typeId,
            typeName: param.typeName ?? param.type?.name,
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
                name: '',
            },
        }
    }
}
