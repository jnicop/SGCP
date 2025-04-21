  export interface ComponentDto {
    componentId: number;
    quantity: number;
    unitId: number;
    enable: boolean;
  }
  
  export interface LaborCostDto {
    laborTypeId: number;
    quantity: number;
    unitId: number;
    enable: boolean;
  }
  
  export interface ProductBuilderDto {
    productId: number;
    name: string;
    description: string;
    categoryId: number;
    components: ComponentDto[];
    laborCosts: LaborCostDto[];
  }
  