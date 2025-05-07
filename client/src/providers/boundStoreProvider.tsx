'use client';

import { type ReactNode, createContext, useRef, useContext } from 'react';
import { useStore } from 'zustand';

import { type BoundStore, createBoundStore } from '@/stores/boundStore';

type BoundStoreApi = ReturnType<typeof createBoundStore>;

const BoundStoreContext = createContext<BoundStoreApi | null>(null);

interface BoundStoreProviderProps {
  children: ReactNode;
}

const BoundStoreProvider = ({ children }: BoundStoreProviderProps) => {
  const storeRef = useRef<BoundStoreApi>(null);
  if (!storeRef.current) {
    storeRef.current = createBoundStore();
  }

  return (
    <BoundStoreContext.Provider value={storeRef.current}>
      {children}
    </BoundStoreContext.Provider>
  );
};

const useBoundStore = <T,>(selector: (store: BoundStore) => T): T => {
  const boundContext = useContext(BoundStoreContext);

  if (!boundContext) {
    throw new Error(`useBoundStore must be used within BoundStoreProvider`);
  }

  return useStore(boundContext, selector);
};

export {
  type BoundStoreApi,
  type BoundStoreProviderProps,
  BoundStoreContext,
  BoundStoreProvider,
  useBoundStore,
};
