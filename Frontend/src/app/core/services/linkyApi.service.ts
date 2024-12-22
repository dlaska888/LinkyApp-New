import {
  HttpClient,
  HttpContext,
  HttpEvent,
  HttpHeaders,
  HttpParams,
  HttpResponse,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { LoginDto } from '../../features/auth/login/models/loginDto';
import { RegisterDto } from '../../features/auth/register/models/registerDto';
import { ExternalAuthDto } from '../../features/auth/models/externalAuthDto';
import { TokenDto } from '../../features/auth/models/tokenDto';
import { ApiTokenConstant } from '../constants/apiToken.constant';
import { LinkyApiConstant } from '../constants/linkyApi.constant';
import { GetAccountDto } from '../models/dtos/account/getAccountDto';
import { SieveModelDto } from '../models/dtos/sieveModelDto';
// import { GetFileDto } from '../models/dtos/file/getFileDto';
// import { GetGroupDto } from '../models/dtos/group/getGroupDto';
// import { UpdateFileDto } from '../models/dtos/file/updateFileDto';
// import { CreateGroupDto } from '../models/dtos/group/createGroupDto';
// import { UpdateGroupDto } from '../models/dtos/group/updateGroupDto';
// import { GetGroupUserDto } from '../models/dtos/groupUser/getGroupUserDto';
// import { CreateGroupUserDto } from '../models/dtos/groupUser/createGroupUserDto';
// import { UpdateGroupUserDto } from '../models/dtos/groupUser/updateGroupUserDto';

@Injectable({
  providedIn: 'root',
})
export class LinkyApiService {
  constructor(private http: HttpClient) {}

  public login(loginDto: LoginDto): Observable<HttpResponse<TokenDto>> {
    return this.http.post<TokenDto>(LinkyApiConstant.LOGIN, loginDto, {
      observe: 'response',
      context: new HttpContext().set(ApiTokenConstant.IS_PUBLIC_API, true),
    });
  }

  public register(
    registerDto: RegisterDto
  ): Observable<HttpResponse<TokenDto>> {
    return this.http.post<TokenDto>(LinkyApiConstant.REGISTER, registerDto, {
      observe: 'response',
      context: new HttpContext().set(ApiTokenConstant.IS_PUBLIC_API, true),
    });
  }

  public googleLogin(
    externalAuthDto: ExternalAuthDto
  ): Observable<HttpResponse<TokenDto>> {
    return this.http.post<TokenDto>(
      LinkyApiConstant.GOOGLE_LOGIN,
      externalAuthDto,
      {
        observe: 'response',
        context: new HttpContext().set(ApiTokenConstant.IS_PUBLIC_API, true),
      }
    );
  }

  public refreshToken(
    refreshToken: string
  ): Observable<HttpResponse<TokenDto>> {
    return this.http.post<TokenDto>(
      LinkyApiConstant.REFRESH_TOKEN,
      JSON.stringify(refreshToken),
      {
        headers: {
          'Content-Type': 'application/json',
        },
        observe: 'response',
        context: new HttpContext().set(ApiTokenConstant.IS_PUBLIC_API, true),
      }
    );
  }

  public getAccount(): Observable<HttpResponse<GetAccountDto>> {
    return this.http.get<GetAccountDto>(LinkyApiConstant.ACCOUNT, {
      observe: 'response',
    });
  }

  private getQueryParams(query: SieveModelDto): HttpParams {
    let params = new HttpParams();

    Object.entries(query).forEach(([key, value]) => {
      if (value !== null && value !== undefined) {
        params = params.set(key, value.toString());
      }
    });

    return params;
  }
}
