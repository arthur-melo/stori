'use server';

import { getSession } from '@/lib/auth';
import httpClient from '@/lib/httpClient';

import type { paths } from '@/typings/api';

const deleteReview = async (
  username: paths['/api/v1/reviews/{username}']['delete']['parameters']['path']['username'],
  reviewId: paths['/api/v1/reviews/{username}']['delete']['requestBody']['content']['application/json']['reviewId'],
) => {
  const accessToken = await getSession();

  if (!accessToken) {
    return null;
  }

  const headers = new Headers();
  headers.append('Authorization', `Bearer ${accessToken}`);

  const { data, error } = await httpClient.DELETE(
    '/api/v1/reviews/{username}',
    {
      headers,
      params: {
        path: { username },
      },
      body: {
        reviewId,
      },
    },
  );

  return { data, error };
};

export default deleteReview;
