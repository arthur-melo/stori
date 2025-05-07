'use client';

import type { ReactNode } from 'react';

import { usePathname } from 'next/navigation';

import Link from 'next/link';
import cn from '@/utils/classNames';

const TextItem = ({
  route,
  children,
}: Readonly<{
  route: string;
  children: ReactNode;
}>) => {
  const pathname = usePathname();

  const isCurrentRoute = pathname.startsWith(`${route}`);
  const isCurrentRouteStyle = isCurrentRoute
    ? 'pb-0 border-b-2 border-solid border-tertiary-500 dark:border-tertiary-100'
    : 'pb-0.5';

  return (
    <div className="flex h-11 content-start items-center">
      <div
        className={cn(
          isCurrentRouteStyle,
          'hover:border-tertiary-500 dark:hover:border-tertiary-100 transition-all duration-50 ease-in-out hover:border-b-2 hover:border-solid hover:pb-0',
        )}>
        <Link
          className="text-tertiary-500 dark:text-tertiary-100 block text-2xl font-semibold"
          href={route}>
          {children}
        </Link>
      </div>
    </div>
  );
};

export default TextItem;
