'use server';

import { getSession } from '@/lib/auth';
import httpClient from '@/lib/httpClient';

import type { paths } from '@/typings/api';

const postReadlist = async (
  username: paths['/api/v1/readlists/{username}/{bookId}']['post']['parameters']['path']['username'],
  bookId: paths['/api/v1/readlists/{username}/{bookId}']['post']['parameters']['path']['bookId'],
) => {
  const accessToken = await getSession();

  if (!accessToken) {
    return null;
  }

  const headers = new Headers();
  headers.append('Authorization', `Bearer ${accessToken}`);

  const { data, error } = await httpClient.POST(
    '/api/v1/readlists/{username}/{bookId}',
    {
      headers,
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

export default postReadlist;
