'use server';

import { getSession } from '@/lib/auth';
import httpClient from '@/lib/httpClient';

import type { paths } from '@/typings/api';

const postReview = async (
  bookId: paths['/api/v1/reviews/book/{bookId}']['post']['parameters']['path']['bookId'],
  text: paths['/api/v1/reviews/book/{bookId}']['post']['requestBody']['content']['application/json']['text'],
) => {
  const accessToken = await getSession();

  if (!accessToken) {
    return null;
  }

  const headers = new Headers();
  headers.append('Authorization', `Bearer ${accessToken}`);

  const { data, error } = await httpClient.POST(
    '/api/v1/reviews/book/{bookId}',
    {
      headers,
      params: {
        path: {
          bookId,
        },
      },
      body: {
        text,
      },
    },
  );

  return { data, error };
};

export default postReview;
