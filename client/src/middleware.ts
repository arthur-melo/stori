import { NextRequest, NextResponse } from 'next/server';
import { updateSession } from '@/lib/auth';

const authRoutes = ['/auth/signin', '/auth/signup'];

export async function middleware(request: NextRequest) {
  // Redirect to home if an authed user tries to access signin/signout routes.
  if (authRoutes.some(route => request.nextUrl.pathname.includes(route))) {
    const refreshToken = request.cookies.get(
      process.env.REFRESH_TOKEN_LOCALSTORAGE_KEY!,
    )?.value;

    if (refreshToken) {
      return NextResponse.redirect(new URL('/', request.url));
    }
  }

  return await updateSession(request);
}

// Routes Middleware should not run on
export const config = {
  matcher: ['/((?!api|_next/static|_next/image|.*\\.png$).*)'],
};
