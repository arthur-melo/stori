import type { ReactNode } from 'react';
import Link from 'next/link';

const Pill = ({
  title,
  href,
  children,
}: Readonly<{
  title?: string;
  href?: string;
  children: ReactNode;
}>) => {
  const Component = (
    <button
      title={title}
      className="group hover:bg-secondary-500 full flex max-w-7xl cursor-pointer items-center rounded-lg bg-neutral-200 px-2 py-1 lg:max-w-full dark:bg-neutral-500">
      <p className="text-tertiary-500 dark:text-tertiary-100 overflow-hidden text-sm font-bold text-nowrap text-ellipsis group-hover:text-white">
        {children}
      </p>
    </button>
  );

  return href ? (
    <Link href={href} className="overflow-hidden">
      {Component}
    </Link>
  ) : (
    Component
  );
};

export default Pill;
