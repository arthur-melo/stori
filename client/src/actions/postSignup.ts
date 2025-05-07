'use server';

import httpClient from '@/lib/httpClient';

async function postSignup(_: unknown, formData: FormData) {
  const rawFormData = {
    username: formData.get('Username')?.toString() ?? null,
    name: formData.get('Name')?.toString() ?? null,
    email: formData.get('Email')?.toString() ?? null,
    password: formData.get('Password')?.toString() ?? null,
  };

  const { error } = await httpClient.POST('/api/v1/auth/signup', {
    body: rawFormData,
  });

  if (error) {
    return error;
  }
}

export default postSignup;
