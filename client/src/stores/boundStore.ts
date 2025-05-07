import { create } from 'zustand';
import { devtools } from 'zustand/middleware';

import {
  UserSlice,
  UserState,
  createUserSlice,
  userInitState,
} from '@/slices/userSlice';

type BoundStore = UserSlice;

type BoundState = UserState;

const boundInitState: BoundState = {
  ...userInitState,
};

const createBoundStore = (initState: BoundState = boundInitState) =>
  create<BoundStore>()(
    devtools((...a) => ({
      ...initState,
      ...createUserSlice(...a),
    })),
  );

export { type BoundStore, createBoundStore };
