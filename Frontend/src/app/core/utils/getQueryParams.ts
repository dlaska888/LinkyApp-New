import { HttpParams } from '@angular/common/http';
import { SieveModelDto } from '../models/dtos/sieveModelDto';

export function getQueryParams(query: SieveModelDto): HttpParams {
  let params = new HttpParams();

  Object.entries(query).forEach(([key, value]) => {
    if (value !== null && value !== undefined) {
      params = params.set(key, value.toString());
    }
  });

  return params;
}
