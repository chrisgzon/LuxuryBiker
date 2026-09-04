export interface ThirdModel {
  id: number;
  email: string;
  identification: string;
  address: string;
  active: string;
  typeId: number;
  typeName?: string;
  cellPhone: string;
  name: string;
  surnames: string;
  fullName: string;
}

export interface TypeThirdModel {
  id: number;
  name: string;
}

export const THIRD_TYPE = {
  provider: 1,
  client: 2,
} as const;
