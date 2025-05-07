'use server';

import { getSession } from '@/lib/auth';
import httpClient from '@/lib/httpClient';

import type { paths } from '@/typings/api';

const deleteUserProfileImage = async (
  username: paths['/api/v1/users/{username}/upload']['delete']['parameters']['path']['username'],
) => {
  const accessToken = await getSession();

  if (!accessToken) {
    return null;
  }

  const headers = new Headers();
  headers.append('Authorization', `Bearer ${accessToken}`);

  const { data, error } = await httpClient.DELETE(
    '/api/v1/users/{username}/upload',
    {
      headers,
      params: {
        path: {
          username,
        },
      },
    },
  );

  return { data, error };
};

export default deleteUserProfileImage;
