import type { Metadata } from 'next';
import { type ReactNode } from 'react';
import { Raleway, Alegreya } from 'next/font/google';
import { ThemeProvider } from 'next-themes';

import './globals.css';

import { BoundStoreProvider } from '@/providers/boundStoreProvider';
import NavbarContainer from '@/components/navbar/NavbarContainer';
import ToastContainer from '@/components/global/ToastContainer';
import UserContainer from '@/components/global/UserContainer';

const raleway = Raleway({
  variable: '--font_family_sans',
  weight: ['100', '200', '300', '400', '500', '600', '700', '800', '900'],
  style: ['normal'],
  subsets: ['latin'],
  display: 'swap',
});

const alegreya = Alegreya({
  variable: '--font_family_serif',
  weight: ['700'],
  style: ['italic'],
  subsets: ['latin'],
  display: 'swap',
});

export const metadata: Metadata = {
  title: 'Stori',
  description: 'Let your imagination flow.',
};

export default function RootLayout({
  children,
}: Readonly<{
  children: ReactNode;
}>) {
  return (
    <html
      suppressHydrationWarning
      lang="en"
      className={`h-full w-full [font-variant-numeric:lining-nums] ${raleway.variable} ${alegreya.variable}`}>
      <body className="h-full w-full">
        <BoundStoreProvider>
          <ThemeProvider attribute="class">
            <UserContainer>
              <ToastContainer />
              <div className="relative h-full w-screen overflow-x-hidden [scrollbar-gutter:stable_both-edges]">
                <div className="absolute inset-x-0 mx-auto w-full max-w-(--screen-2xl)">
                  <NavbarContainer />
                </div>
                <div className="h-full w-full">
                  <div className="mx-auto grid h-full w-full max-w-(--screen-2xl) grid-cols-12 gap-x-6 px-20 sm:grid-cols-4 md:grid-cols-8 md:px-10">
                    <div className="col-span-full mt-20">{children}</div>
                  </div>
                </div>
              </div>
            </UserContainer>
          </ThemeProvider>
        </BoundStoreProvider>
      </body>
    </html>
  );
}
