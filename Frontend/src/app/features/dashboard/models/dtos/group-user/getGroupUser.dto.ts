import { GetDto } from 'app/core/models/dtos/get.dto';
import { GroupRole } from '../../enums/groupRole';

export interface GetGroupUserDto extends GetDto {
  userId: string;
  groupId: string;
  role: GroupRole;
}
