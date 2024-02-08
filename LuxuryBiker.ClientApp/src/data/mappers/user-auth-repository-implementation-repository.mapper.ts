import { Mapper } from "@base/mapper";
import { UserEntity } from "@entities/user.entity";
import { UserLoggedModel } from "@models/auth/user-logged.model";
import { UserModel } from "@models/user.model";


export class UserAuthImplementationRepositoryMapper extends Mapper<UserEntity, UserModel> {
    mapFrom(param: UserEntity): UserLoggedModel {
        return {
            username: param.userName,
            token: param.token
        };
    }
    mapTo(param: UserModel): UserEntity {
        return {
            userName: param.username,
            token: ''
        }
    }
}