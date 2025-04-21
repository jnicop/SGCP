import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { FormControl } from '@angular/forms';

/**
 * Crea un observable que filtra una lista de objetos a partir de un FormControl.
 * 
 * @param control El FormControl que maneja la entrada del usuario.
 * @param allItems Lista completa de objetos a filtrar.
 * @param keyFn Función que devuelve el texto representativo del objeto.
 */
export function createObjectFilterObservable<T>(
  control: FormControl,
  allItems: T[],
  keyFn: (item: T) => string
): Observable<T[]> {
  return control.valueChanges.pipe(
    startWith(control.value || ''),
    map((input) => {
      const filterValue = typeof input === 'string'
        ? input.toLowerCase()
        : keyFn(input)?.toLowerCase();

      return allItems.filter(item =>
        keyFn(item).toLowerCase().includes(filterValue)
      );
    })
  );
}

/**
 * Función para usar con displayWith de mat-autocomplete
 */
export function displayWithFn<T>(keyFn: (item: T) => string): (item: T) => string {
  return (item: T) => (item ? keyFn(item) : '');
}
