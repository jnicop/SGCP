export interface ProductFilterDto {
    search?: string;
    pageIndex: number;
    pageSize: number;
    enable?: boolean | null;
  }