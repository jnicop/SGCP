export interface ComponentFilterDto {
    search?: string;
    pageIndex: number;
    pageSize: number;
    enable?: boolean | null;
  }