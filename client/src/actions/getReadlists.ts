'use server';

import httpClient from '@/lib/httpClient';

import type { paths } from '@/typings/api';

const getReadlists = async (
  username: paths['/api/v1/readlists/{username}']['get']['parameters']['path']['username'],
  queryParams?: paths['/api/v1/readlists/{username}']['get']['parameters']['query'],
) => {
  const { data, error } = await httpClient.GET('/api/v1/readlists/{username}', {
    params: {
      path: {
        username,
      },
      query: queryParams,
    },
  });
  return { data, error };
};

export default getReadlists;
