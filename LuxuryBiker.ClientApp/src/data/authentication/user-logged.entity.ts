import { UserEntity } from "@data/users/user.entity";

export interface UserLoggedEntity extends UserEntity{
    token: string;
}