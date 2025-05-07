'use server';

import httpClient from '@/lib/httpClient';

import type { paths } from '@/typings/api';

const getBook = async (
  bookId: paths['/api/v1/books/bookId/{bookId}']['get']['parameters']['path']['bookId'],
) => {
  const { data, error } = await httpClient.GET(
    '/api/v1/books/bookId/{bookId}',
    {
      params: {
        path: {
          bookId,
        },
      },
    },
  );

  return { data, error };
};

export default getBook;
