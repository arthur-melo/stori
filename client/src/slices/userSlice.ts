import { StateCreator } from 'zustand';
import 'zustand/middleware';

import type { components } from '@/typings/api';

type UserState = {
  user: components['schemas']['UserAuthorizedResponse'] | undefined;
};

type UserActions = {
  setUser: (userData: UserState['user']) => void;
};

type UserSlice = UserState & UserActions;

const userInitState: UserState = {
  user: undefined,
};

const createUserSlice: StateCreator<
  UserSlice,
  [['zustand/devtools', never]],
  []
> = set => ({
  ...userInitState,
  setUser: userData => set({ user: userData }, undefined, 'user/setUser'),
});

export {
  type UserSlice,
  type UserActions,
  type UserState,
  userInitState,
  createUserSlice,
};
