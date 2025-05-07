'use client';

import { useRouter } from 'next/navigation';

import { useBoundStore } from '@/providers/boundStoreProvider';

import useToastWrapper from '@/hooks/useToastWrapper';

// Helper hook to clear the Zustand store when the logged user no longer has a valid token.
const useClearStore = <T, A extends unknown[]>(
  callback: (...args: A) => Promise<T>,
  message?: string,
) => {
  const router = useRouter();
  const setUser = useBoundStore(state => state.setUser);
  const toastWrapperCallback = useToastWrapper(callback, message);

  const wrappedCallback = async (...args: A): Promise<T | undefined> => {
    const response = await toastWrapperCallback(...args);

    // On authed actions, `getSession` returns null when the cookie no longer exists.
    if (response === null) {
      // Clear the user from the store.
      setUser(undefined);

      router.push('/auth/signin');
    }

    // `response` can be undefined in the case of a fetch call failure.
    return response;
  };

  return wrappedCallback;
};

export default useClearStore;
