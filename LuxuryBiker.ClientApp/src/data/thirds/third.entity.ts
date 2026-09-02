export interface ThirdEntity {
  id: number;
  email: string;
  identification: string;
  address: string;
  active: string;
  typeId: number;
  typeName?: string;
  type?: TypeThirdEntity;
  cellPhone: string;
  name: string;
  surnames: string;
}

export interface TypeThirdEntity {
  id: number;
  name: string;
}
