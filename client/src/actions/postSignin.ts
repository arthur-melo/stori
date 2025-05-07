'use server';

import httpClient from '@/lib/httpClient';
import { signin } from '@/lib/auth';

async function postSignin(_: unknown, formData: FormData) {
  const rawFormData = {
    email: formData.get('Email')?.toString() ?? null,
    password: formData.get('Password')?.toString() ?? null,
  };

  const { data, error } = await httpClient.POST('/api/v1/auth/signin', {
    body: rawFormData,
  });

  if (error) {
    return error;
  }

  await signin(data!);
}

export default postSignin;
