'use server';

import { cookies } from 'next/headers';
import { NextRequest, NextResponse } from 'next/server';

import { components } from '@/typings/api';
import revokeToken from '@/actions/revokeToken';
import refreshTokenAction from '@/actions/refreshToken';

// Unix epoch in C# ticks.
const UNIX_EPOCH_TICKS = BigInt('621355968000000000');

// Helper function to convert C# Ticks to JS Date's milliseconds.
const convertTicksToMilliseconds = (ticks: string) => {
  const ticksAsBigint = BigInt(ticks);
  // 1 tick = 100 nanoseconds, so we divide by 10,000 to get milliseconds
  const ticksPerMillisecond = 10000;

  // Convert ticks to milliseconds
  const milliseconds =
    (ticksAsBigint - UNIX_EPOCH_TICKS) / BigInt(ticksPerMillisecond);

  return new Date(Number(milliseconds)).getTime();
};

const signin = async (
  tokenResponse: components['schemas']['TokenResponse'],
) => {
  const cookieStore = await cookies();

  cookieStore.set(
    process.env.ACCESS_TOKEN_LOCALSTORAGE_KEY!,
    tokenResponse.accessToken!.token!,
    {
      httpOnly: true,
      expires: convertTicksToMilliseconds(
        tokenResponse.accessToken!.expiration!.toString(),
      ),
    },
  );

  cookieStore.set(
    process.env.REFRESH_TOKEN_LOCALSTORAGE_KEY!,
    tokenResponse.refreshToken!.token!,
    {
      httpOnly: true,
      expires: convertTicksToMilliseconds(
        tokenResponse.refreshToken!.expiration!.toString(),
      ),
    },
  );
};

const signout = async () => {
  const cookieStore = await cookies();

  const refreshToken = cookieStore.get(
    process.env.REFRESH_TOKEN_LOCALSTORAGE_KEY!,
  )?.value;

  if (refreshToken) {
    let error;
    try {
      const response = await revokeToken(refreshToken);

      if (response.error) {
        error = response.error;
      }
    } catch (fetchError) {
      error = fetchError;
    }

    if (error) {
      console.log(error);
    }
  }

  cookieStore.delete(process.env.ACCESS_TOKEN_LOCALSTORAGE_KEY!);
  cookieStore.delete(process.env.REFRESH_TOKEN_LOCALSTORAGE_KEY!);
};

const getSession = async () => {
  const cookieStore = await cookies();

  const accessToken = cookieStore.get(
    process.env.ACCESS_TOKEN_LOCALSTORAGE_KEY!,
  )?.value;

  if (!accessToken) {
    return null;
  }

  return accessToken;
};

const clearSession = async () => {
  const cookieStore = await cookies();
  cookieStore.delete(process.env.ACCESS_TOKEN_LOCALSTORAGE_KEY!);
  cookieStore.delete(process.env.REFRESH_TOKEN_LOCALSTORAGE_KEY!);
};

const updateSession = async (request: NextRequest) => {
  const res = NextResponse.next();

  const refreshToken = request.cookies.get(
    process.env.REFRESH_TOKEN_LOCALSTORAGE_KEY!,
  )?.value;

  if (!refreshToken) {
    return;
  }

  const accessToken = request.cookies.get(
    process.env.ACCESS_TOKEN_LOCALSTORAGE_KEY!,
  )?.value;

  // If there is an access token available, wait for it to expire before refreshing.
  if (accessToken) {
    return;
  }

  try {
    const { data, error } = await refreshTokenAction(refreshToken);

    if (error) {
      console.log(error);
      await clearSession();
      return res;
    }

    res.cookies.set({
      name: process.env.ACCESS_TOKEN_LOCALSTORAGE_KEY!,
      value: data!.accessToken!.token!,
      httpOnly: true,
      expires: convertTicksToMilliseconds(
        data!.accessToken!.expiration!.toString(),
      ),
    });

    res.cookies.set({
      name: process.env.REFRESH_TOKEN_LOCALSTORAGE_KEY!,
      value: data!.refreshToken!.token!,
      httpOnly: true,
      expires: convertTicksToMilliseconds(
        data!.refreshToken!.expiration!.toString(),
      ),
    });
  } catch {
    console.log('Error contacting the API to generate a new pair of tokens.');
    await clearSession();
  }

  return res;
};

export { signin, signout, getSession, updateSession };
