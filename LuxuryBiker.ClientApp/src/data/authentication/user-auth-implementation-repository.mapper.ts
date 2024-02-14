import { Mapper } from '@base/mapper';
import { UserLoggedModel } from '@domain/authentication/models/user-logged.model';
import { UserLoggedEntity } from './user-logged.entity';


export class UserAuthImplementationRepositoryMapper extends Mapper<UserLoggedEntity, UserLoggedModel> {
    mapFrom(param: UserLoggedEntity): UserLoggedModel {
        return {
            id: param.id,
            fullName: `${param.names} ${param.surnames}`,
            userName: param.userName,
            active: param.active,
            fechaNacimiento: param.fechaNacimiento,
            identification: param.identification,
            names: param.names,
            surnames: param.surnames,
            token: param.token,
            roles: param.roles
        };
    }
    mapTo(param: UserLoggedModel): UserLoggedEntity {
        return {
            id: param.id,
            userName: param.userName,
            active: param.active,
            fechaNacimiento: param.fechaNacimiento,
            identification: param.identification,
            names: param.names,
            surnames: param.surnames,
            token: param.token,
            roles: param.roles
        }
    }
}