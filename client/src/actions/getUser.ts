'use server';

import httpClient from '@/lib/httpClient';

import type { paths } from '@/typings/api';

const getUser = async (
  username: paths['/api/v1/users/{username}']['get']['parameters']['path']['username'],
) => {
  const { data, error } = await httpClient.GET('/api/v1/users/{username}', {
    params: {
      path: {
        username,
      },
    },
  });

  return { data, error };
};

export default getUser;
