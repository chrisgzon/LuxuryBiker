import { UserModel } from "@domain/users/models/user.model";

export interface UserLoggedModel extends UserModel {
  token: string;
}
