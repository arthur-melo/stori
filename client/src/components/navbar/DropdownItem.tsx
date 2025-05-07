import {
  FC,
  type ReactNode,
  type ReactElement,
  type MouseEventHandler,
} from 'react';

import Link from 'next/link';

import Icon from '@/components/shared/Icon';

interface DropdownItemProps {
  icon: ReactElement;
  alt: string;
  children: ReactNode;
  href?: string;
  onClick?: MouseEventHandler<HTMLElement>;
}

const DropdownItem: FC<DropdownItemProps> = ({
  icon,
  alt = '',
  href,
  children,
  onClick,
}) => {
  const Component = (
    <div
      className="flex min-h-12 cursor-pointer gap-2 bg-white px-2 hover:bg-neutral-200 dark:bg-black dark:hover:bg-neutral-400"
      onClick={href ? undefined : onClick}>
      <div className="pt-3">
        <Icon
          className="text-tertiary-500 dark:text-tertiary-100 rounded-full"
          size="md"
          alt={alt}
          src={icon}
        />
      </div>
      <div className="flex w-full items-center justify-start">
        <p className="text-tertiary-500 dark:text-tertiary-100 text-xl font-normal">
          {children}
        </p>
      </div>
    </div>
  );

  if (!href) {
    return Component;
  }

  return (
    <Link onClick={onClick} href={href}>
      {Component}
    </Link>
  );
};
export default DropdownItem;
