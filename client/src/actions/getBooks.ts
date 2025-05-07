'use server';

import httpClient from '@/lib/httpClient';

import type { paths } from '@/typings/api';

const getBooks = async (
  queryParams?: paths['/api/v1/books']['get']['parameters']['query'],
) => {
  const { data, error } = await httpClient.GET('/api/v1/books', {
    params: {
      query: queryParams,
    },
  });

  return { data, error };
};

export default getBooks;
