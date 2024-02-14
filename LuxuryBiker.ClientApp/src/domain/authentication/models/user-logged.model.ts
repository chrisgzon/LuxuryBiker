import { UserModel } from "@domain/users/models/user.model";

export interface UserLoggedModel extends UserModel {
  token: string;
  roles: RoleModel[];
}

export interface RoleModel {
  role: string;
}
