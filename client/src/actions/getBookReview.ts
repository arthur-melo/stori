'use server';

import httpClient from '@/lib/httpClient';

import type { paths } from '@/typings/api';

const getBookReview = async (
  bookId: paths['/api/v1/reviews/book/{bookId}']['get']['parameters']['path']['bookId'],
  queryParams?: paths['/api/v1/reviews/book/{bookId}']['get']['parameters']['query'],
) => {
  const { data, error } = await httpClient.GET(
    '/api/v1/reviews/book/{bookId}',
    {
      params: {
        path: {
          bookId,
        },
        query: queryParams,
      },
    },
  );
  return { data, error };
};

export default getBookReview;
