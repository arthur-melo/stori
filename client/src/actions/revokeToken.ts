'use server';

import httpClient from '@/lib/httpClient';

import type { components } from '@/typings/api';

const revokeToken = async (
  refreshToken: components['schemas']['RefreshToken']['token'],
) => {
  const { data, error } = await httpClient.DELETE('/api/v1/auth/token/revoke', {
    body: {
      token: refreshToken,
    },
  });

  return { data, error };
};

export default revokeToken;
