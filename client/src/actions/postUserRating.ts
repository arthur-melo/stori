'use server';

import { getSession } from '@/lib/auth';
import httpClient from '@/lib/httpClient';

import type { paths } from '@/typings/api';

const postUserRating = async (
  username: paths['/api/v1/user_ratings/{username}/{bookId}']['post']['parameters']['path']['username'],
  bookId: paths['/api/v1/user_ratings/{username}/{bookId}']['post']['parameters']['path']['bookId'],
  rating: paths['/api/v1/user_ratings/{username}/{bookId}']['post']['requestBody']['content']['application/json']['rating'],
) => {
  const accessToken = await getSession();

  if (!accessToken) {
    return null;
  }

  const headers = new Headers();
  headers.append('Authorization', `Bearer ${accessToken}`);

  const { data, error } = await httpClient.POST(
    '/api/v1/user_ratings/{username}/{bookId}',
    {
      headers,
      params: {
        path: {
          username,
          bookId,
        },
      },
      body: {
        rating,
      },
    },
  );

  return { data, error };
};

export default postUserRating;
