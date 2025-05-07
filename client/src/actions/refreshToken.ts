'use server';

import httpClient from '@/lib/httpClient';

import type { components } from '@/typings/api';

const refreshToken = async (
  refreshToken: components['schemas']['RefreshTokenRequest']['token'],
) => {
  const { data, error } = await httpClient.POST('/api/v1/auth/token/refresh', {
    body: { token: refreshToken },
  });

  return { data, error };
};

export default refreshToken;
