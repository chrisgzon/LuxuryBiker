export interface SaleLineModel {
  productId: number;
  productName: string;
  productCode: string;
  productValue: number;
  quantity: number;
  subtotal: number;
}

export interface CreateSaleModel {
  thirdId: number | null;
  applyIva: boolean;
  details: {
    productId: number;
    productValue: number;
    quantity: number;
  }[];
}

export interface SaleCreatedModel {
  id: number;
  code: string;
  total: number;
}

export interface SaleListItemModel {
  id: number;
  code: string;
  date: string;
  total: number;
  status: boolean;
  clientName: string;
  productsQuantity: number;
}

export interface SaleProductOption {
  id: number;
  name: string;
  code: string;
  value: number;
  stock: number;
}

export interface SaleClientOption {
  id: number;
  label: string;
}

export interface SaleFormDataModel {
  products: SaleProductOption[];
  clients: SaleClientOption[];
}

/** IVA general de Colombia (%). Coincide con `Taxes.IvaRate` del backend. */
export const IVA_RATE = 19;
