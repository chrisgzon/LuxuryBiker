export interface MonthlyTotal {
  month: number;
  monthName: string;
  total: number;
}

export interface DailyTotal {
  date: string;
  total: number;
}

export interface TopProduct {
  productId: number;
  code: string | null;
  name: string;
  stock: number;
  quantitySold: number;
}

export interface DashboardModel {
  year: number;
  currentMonthName: string;
  salesToday: number;
  purchasesThisMonth: number;
  salesThisMonth: number;
  purchasesByMonth: MonthlyTotal[];
  salesByMonth: MonthlyTotal[];
  salesLast15Days: DailyTotal[];
  topSellingProducts: TopProduct[];
}
