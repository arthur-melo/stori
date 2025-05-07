'use server';

import { getSession } from '@/lib/auth';
import httpClient from '@/lib/httpClient';

const getAuthedUser = async () => {
  const accessToken = await getSession();

  if (!accessToken) {
    return null;
  }

  const headers = new Headers();
  headers.append('Authorization', `Bearer ${accessToken}`);

  const { data, error } = await httpClient.GET('/api/v1/users', {
    headers,
  });

  return { data, error };
};

export default getAuthedUser;
