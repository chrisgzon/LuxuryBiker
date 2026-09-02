import { UserModel } from "@domain/users/models/user.model";

export interface UserLoggedModel extends UserModel {
  token: string;
  roles: string[];
}

/** Roles definidos en el backend (`LuxuryBiker.Domain.Constants.Roles`). */
export const ROLES = {
  administrator: 'Administrator',
  seller: 'Seller',
} as const;
