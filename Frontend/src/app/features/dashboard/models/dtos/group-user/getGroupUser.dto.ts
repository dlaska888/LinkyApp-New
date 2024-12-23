import { GroupRole } from '../../enums/groupRole';

export interface GetGroupUserDto {
  userId: string;
  groupId: string;
  role: GroupRole;
}
