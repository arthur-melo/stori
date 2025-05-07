'use server';

import { getSession } from '@/lib/auth';
import httpClient from '@/lib/httpClient';

import type { paths } from '@/typings/api';

const patchReview = async (
  reviewId: paths['/api/v1/reviews/patch/{reviewId}']['patch']['parameters']['path']['reviewId'],
  text: paths['/api/v1/reviews/patch/{reviewId}']['patch']['requestBody']['content']['application/json']['text'],
) => {
  const accessToken = await getSession();

  if (!accessToken) {
    return null;
  }

  const headers = new Headers();
  headers.append('Authorization', `Bearer ${accessToken}`);

  const { data, error } = await httpClient.PATCH(
    '/api/v1/reviews/patch/{reviewId}',
    {
      headers,
      params: {
        path: {
          reviewId,
        },
      },
      body: {
        text,
      },
    },
  );

  return { data, error };
};

export default patchReview;
