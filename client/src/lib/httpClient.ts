import createClient from 'openapi-fetch';
import type { paths } from '@/typings/api';

const httpClient = createClient<paths>({
  baseUrl: process.env.NEXT_PUBLIC_BACKEND_URL,
});

export default httpClient;
