export interface PaginationQueryDto {
    pageIndex: number;
    pageSize: number;
    search?: string;
    enable?: boolean | null;
  }
  