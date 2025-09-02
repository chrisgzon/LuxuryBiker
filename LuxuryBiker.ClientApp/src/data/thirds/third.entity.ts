export interface ThirdEntity {
    id: number;
    email: string;
    identification: string;
    address: string;
    active: string;
    typeId: number;
    type: TypeThirdEntity;
    cellPhone: string;
    name: string;
    surnames: string;
  }
  
  export interface TypeThirdEntity {
    id: number;
    name: string;
  }