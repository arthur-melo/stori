'use server';

import httpClient from '@/lib/httpClient';

import type { paths } from '@/typings/api';

const getBookReviewByUsername = async (
  username: paths['/api/v1/reviews/{username}']['get']['parameters']['path']['username'],
  queryParams?: paths['/api/v1/reviews/{username}']['get']['parameters']['query'],
) => {
  const { data, error } = await httpClient.GET('/api/v1/reviews/{username}', {
    params: {
      path: {
        username,
      },
      query: queryParams,
    },
  });
  return { data, error };
};

export default getBookReviewByUsername;
