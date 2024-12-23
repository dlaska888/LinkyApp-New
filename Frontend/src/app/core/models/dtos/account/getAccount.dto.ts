import { GetDto } from '../get.dto';

export interface GetAccountDto extends GetDto {
  userName: string;
  email: string;
  emailConfirmed: boolean;
}
