'use server';

import httpClient from '@/lib/httpClient';

import type { paths } from '@/typings/api';

const getUserRatingByUsernameAndBook = async (
  username: paths['/api/v1/user_ratings/{username}/{bookId}']['get']['parameters']['path']['username'],
  bookId: paths['/api/v1/user_ratings/{username}/{bookId}']['get']['parameters']['path']['bookId'],
) => {
  const { data, error } = await httpClient.GET(
    '/api/v1/user_ratings/{username}/{bookId}',
    {
      params: {
        path: {
          username,
          bookId,
        },
      },
    },
  );
  return { data, error };
};

export default getUserRatingByUsernameAndBook;
