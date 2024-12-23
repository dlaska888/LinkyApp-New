import { GroupRole } from '../../enums/groupRole';

export interface CreateGroupUserDto {
  userName: string;
  role: GroupRole;
}
