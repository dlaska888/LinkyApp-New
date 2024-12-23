import { HttpContext } from '@angular/common/http';
import { ApiTokenConstant } from '../constants/apiToken.constant';

export function setPublicApi(): HttpContext {
  return new HttpContext().set(ApiTokenConstant.IS_PUBLIC_API, true);
}
