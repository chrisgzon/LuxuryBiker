export interface CreateSaleRequestEntity {
  thirdId: number | null;
  applyIva: boolean;
  details: SaleDetailRequestEntity[];
}

export interface SaleDetailRequestEntity {
  productId: number;
  productValue: number;
  quantity: number;
}

export interface SaleCreatedEntity {
  id: number;
  code: string;
  total: number;
}

export interface SaleBriefEntity {
  id: number;
  code: string;
  date: string;
  total: number;
  status: boolean;
  clientName: string | null;
  productsQuantity: number;
}

export interface SaleFormDataEntity {
  products: SaleProductEntity[];
  clients: SaleClientEntity[];
}

export interface SaleProductEntity {
  id: number;
  name: string;
  code: string | null;
  reference: string;
  status: boolean | null;
  stock: number | null;
  value: number | null;
}

export interface SaleClientEntity {
  id: number;
  identification: string;
  name: string | null;
  surnames: string | null;
  typeName: string | null;
}
