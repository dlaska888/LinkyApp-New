import { environment } from '../../../environments/environment';

export class LinkyApiConstant {
  public static readonly LOGIN = environment.linkyApi.url + '/auth/login';
  public static readonly REGISTER = environment.linkyApi.url + '/auth/register';
  public static readonly GOOGLE_LOGIN =
    environment.linkyApi.url + '/auth/google-login';
  public static readonly REFRESH_TOKEN =
    environment.linkyApi.url + '/auth/refresh';
  public static readonly ACCOUNT = environment.linkyApi.url + '/account';
  public static readonly GROUP = environment.linkyApi.url + '/group';
  public static readonly LINK = (groupId: string) =>
    `${environment.linkyApi.url}/group/${groupId}/link`;
  public static readonly GROUP_USER = (groupId: string) =>
    `${environment.linkyApi.url}/group/${groupId}/group-user`;
}
