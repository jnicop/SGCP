import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class CurrencyConfigService {
  private _currencyCode = 'ARS';
  private _locale = 'es-AR';

  get currencyCode(): string {
    return this._currencyCode;
  }

  get locale(): string {
    return this._locale;
  }

  setCurrency(code: string, locale: string) {
    this._currencyCode = code;
    this._locale = locale;
  }
}
