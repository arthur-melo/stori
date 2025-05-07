'use server';

import { getSession } from '@/lib/auth';
import httpClient from '@/lib/httpClient';

import type { paths } from '@/typings/api';

const patchUser = async (
  user: paths['/api/v1/users/{username}']['patch']['parameters']['path']['username'],
  email?: paths['/api/v1/users/{username}']['patch']['requestBody']['content']['application/json']['email'],
  password?: paths['/api/v1/users/{username}']['patch']['requestBody']['content']['application/json']['password'],
  username?: paths['/api/v1/users/{username}']['patch']['requestBody']['content']['application/json']['username'],
  name?: paths['/api/v1/users/{username}']['patch']['requestBody']['content']['application/json']['name'],
) => {
  const accessToken = await getSession();

  if (!accessToken) {
    return null;
  }

  const headers = new Headers();
  headers.append('Authorization', `Bearer ${accessToken}`);

  const { data, error } = await httpClient.PATCH('/api/v1/users/{username}', {
    headers,
    params: {
      path: {
        username: user,
      },
    },
    body: {
      email: email ?? null,
      password: password ?? null,
      username: username ?? null,
      name: name ?? null,
    },
  });

  return { data, error };
};

export default patchUser;
