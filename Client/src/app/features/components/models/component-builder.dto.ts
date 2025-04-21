export interface ComponentBuilderDto {
  id?: number | null;
  name: string;
  code: string;
  description: string;
  categoryId: number;
  componentTypeId: number;
  unitId: number;
  type: string;
  enable: boolean;
  unitCost: number;
  presentations: PresentationDto[];
  attributes: AttributeDto[];
  treatments: TreatmentDto[];
  processes: ProcessDto[];
}

export interface PresentationDto {
  code: string;
  description: string;
  measure: string;
  quantityPerBase: number;
  price: number;
  unitId: number;
  baseUnitCost: number;
  weightGrams: number;
  lengthMeters: number;
  enable: boolean;
}

export interface AttributeDto {
  attributeName: string;
  attributeValue: string;
  enable: boolean;
}

export interface TreatmentDto {
  treatmentTypeId: number;
  extraCost: number;
  enable: boolean;
}

export interface ProcessDto {
  processTypeId: number;
  scopeTypeId: number;
  quantityApplied: number;
  costPerUnit: number;
  date: string;
  notes: string;
  enable: boolean;
}
