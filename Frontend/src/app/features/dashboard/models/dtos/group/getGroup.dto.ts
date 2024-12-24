import { GetDto } from 'app/core/models/dtos/get.dto';

export interface GetGroupDto extends GetDto {
  name: string;
  description?: string;
}
