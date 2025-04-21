import { Pipe, PipeTransform } from '@angular/core';
import { CurrencyConfigService } from '../currency-config.service';

@Pipe({
  name: 'customCurrency',
  standalone:false,
})
export class CustomCurrencyPipe implements PipeTransform {
  constructor(private currencyConfig: CurrencyConfigService) {}

  transform(value: number | null | undefined, digits: string = '1.2-2'): string {
    if (value === null || value === undefined) return '';

    const locale = this.currencyConfig.locale;
    const currency = this.currencyConfig.currencyCode;

    return new Intl.NumberFormat(locale, {
      style: 'currency',
      currency,
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    }).format(value);
  }
}
