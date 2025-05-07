'use server';

import httpClient from '@/lib/httpClient';

import type { paths } from '@/typings/api';

import type { FilterEndpoints } from '@/typings/components';

const getFilteredList = async (
  endpoint: keyof typeof FilterEndpoints,
  queryParams?: paths[`/api/v1/${keyof typeof FilterEndpoints}`]['get']['parameters']['query'],
) => {
  const { data, error } = await httpClient.GET(`/api/v1/${endpoint}`, {
    params: {
      query: queryParams,
    },
  });

  return { data, error };
};

export default getFilteredList;
