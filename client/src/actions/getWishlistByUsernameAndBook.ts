'use server';

import httpClient from '@/lib/httpClient';

import type { paths } from '@/typings/api';

const getWishlistByUsernameAndBook = async (
  username: paths['/api/v1/wishlists/{username}/{bookId}']['get']['parameters']['path']['username'],
  bookId: paths['/api/v1/wishlists/{username}/{bookId}']['get']['parameters']['path']['bookId'],
) => {
  const { data, error } = await httpClient.GET(
    '/api/v1/wishlists/{username}/{bookId}',
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

export default getWishlistByUsernameAndBook;
