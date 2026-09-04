export interface CreatePurchaseRequestEntity {
  thirdId: number | null;
  datePurchase: string;
  applyIva: boolean;
  details: PurchaseDetailRequestEntity[];
}

export interface PurchaseDetailRequestEntity {
  productId: number;
  productValue: number;
  quantity: number;
}

export interface PurchaseCreatedEntity {
  id: number;
  code: string;
  total: number;
}

export interface PurchaseBriefEntity {
  id: number;
  code: string;
  date: string;
  total: number;
  status: boolean;
  supplierName: string | null;
  productsQuantity: number;
}

export interface PurchaseFormDataEntity {
  products: PurchaseProductEntity[];
  suppliers: PurchaseSupplierEntity[];
}

export interface PurchaseProductEntity {
  id: number;
  name: string;
  code: string | null;
  reference: string;
  status: boolean | null;
  stock: number | null;
  value: number | null;
}

export interface PurchaseSupplierEntity {
  id: number;
  identification: string;
  name: string | null;
  surnames: string | null;
  typeName: string | null;
}
