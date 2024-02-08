import { UserModel } from "@models/user.model";

export interface UserLoggedModel extends UserModel {
  token: string;
}
