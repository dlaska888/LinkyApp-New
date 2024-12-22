import { environment } from "../../../environments/environment";

export class LinkyApiConstant {
    public static readonly LOGIN = environment.linkyApi.url + '/auth/login';
    public static readonly REGISTER = environment.linkyApi.url + '/auth/register';
    public static readonly GOOGLE_LOGIN = environment.linkyApi.url + '/auth/google-login';
    public static readonly REFRESH_TOKEN = environment.linkyApi.url + '/auth/refresh';
    public static readonly ACCOUNT = environment.linkyApi.url + '/account';
    public static readonly GROUP = environment.linkyApi.url + '/group';
    public static readonly FILE = environment.linkyApi.url + '/file';
    public static readonly TAG = environment.linkyApi.url + '/tag';
    public static readonly GROUP_USER = environment.linkyApi.url + '/group-user';
};