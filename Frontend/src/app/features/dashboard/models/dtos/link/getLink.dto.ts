import { GetDto } from 'app/core/models/dtos/get.dto';

export interface GetLinkDto extends GetDto {
  title: string;
  url: string;
  creatorId: string;
}
