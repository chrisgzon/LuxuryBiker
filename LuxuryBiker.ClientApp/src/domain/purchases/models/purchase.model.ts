/** Línea de compra tal como se edita en el formulario. */
export interface PurchaseLineModel {
  productId: number;
  productName: string;
  productCode: string;
  productValue: number;
  quantity: number;
  subtotal: number;
}

/** Payload para registrar una compra. */
export interface CreatePurchaseModel {
  thirdId: number | null;
  datePurchase: string;
  applyIva: boolean;
  details: {
    productId: number;
    productValue: number;
    quantity: number;
  }[];
}

export interface PurchaseCreatedModel {
  id: number;
  code: string;
  total: number;
}

export interface PurchaseListItemModel {
  id: number;
  code: string;
  date: string;
  total: number;
  status: boolean;
  supplierName: string;
  productsQuantity: number;
}

export interface PurchaseProductOption {
  id: number;
  name: string;
  code: string;
  value: number;
  stock: number;
}

export interface PurchaseSupplierOption {
  id: number;
  label: string;
}

export interface PurchaseFormDataModel {
  products: PurchaseProductOption[];
  suppliers: PurchaseSupplierOption[];
}

/** IVA general de Colombia (%). Coincide con `Taxes.IvaRate` del backend. */
export const IVA_RATE = 19;
