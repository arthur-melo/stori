'use server';

import { getSession } from '@/lib/auth';
import httpClient from '@/lib/httpClient';

import type { paths } from '@/typings/api';

const postUserImageUpload = async (
  username: paths['/api/v1/users/{username}/upload']['post']['parameters']['path']['username'],
  profileImg: File,
) => {
  const accessToken = await getSession();

  if (!accessToken) {
    return null;
  }

  const headers = new Headers();
  headers.append('Authorization', `Bearer ${accessToken}`);

  const { data, error } = await httpClient.POST(
    '/api/v1/users/{username}/upload',
    {
      headers,
      params: {
        path: {
          username,
        },
      },
      body: {
        profileImg,
      },
      bodySerializer: body => {
        const formData = new FormData();
        formData.set('profileImg', body!.profileImg!);
        return formData;
      },
    },
  );

  return { data, error };
};

export default postUserImageUpload;
