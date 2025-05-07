'use client';

import { type ReactNode, useEffect, useState } from 'react';

import { useBoundStore } from '@/providers/boundStoreProvider';

import getAuthedUser from '@/actions/getAuthedUser';
import useToastWrapper from '@/hooks/useToastWrapper';

const UserContainer = ({
  children,
}: Readonly<{
  children: ReactNode;
}>) => {
  const { user, setUser } = useBoundStore(state => state);
  const getAuthedUserToastWrapper = useToastWrapper(
    getAuthedUser,
    'Error fetching the signed in user data from the server.',
  );
  const [isClient, setIsClient] = useState(false);

  useEffect(() => {
    setIsClient(true);
  }, [isClient]);

  useEffect(() => {
    const getAuthedUserWrapper = async () => {
      const response = await getAuthedUserToastWrapper();

      // Response is undefined when the server cannot be reached.
      if (!response) {
        return;
      }

      if (response.data) {
        setUser(response?.data.data?.at(0));
      }
    };

    if (!user && isClient) {
      getAuthedUserWrapper();
    }
  }, [user, setUser, getAuthedUserToastWrapper, isClient]);

  return children;
};

export default UserContainer;
