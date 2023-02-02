import { role } from './role';

export interface User {
  username: string;
  role?: role;
  token?: string;
}
