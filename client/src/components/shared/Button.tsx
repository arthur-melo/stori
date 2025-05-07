'use client';

import type {
  ReactElement,
  FC,
  ElementType,
  HTMLAttributes,
  ComponentPropsWithoutRef,
} from 'react';
import Link from 'next/link';
import { cva, type VariantProps } from 'class-variance-authority';

import classNames from '@/utils/classNames';

import Icon from '@/components/shared/Icon';

import Spinner from '@/public/assets/icons/Spinner.svg';

const button = cva('button group w-full cursor-pointer font-bold', {
  variants: {
    variant: {
      none: null,
      danger: 'text-white',
    },
    intent: {
      primary: 'bg-primary-500 hello text-white',
      secondary:
        'border border-primary-500 text-primary-500 bg-white dark:bg-transparent',
    },
    size: {
      sm: 'h-8 px-3 rounded-lg text-sm',
      md: 'h-11 px-4 rounded-xl text-lg',
      lg: 'h-14 px-6 rounded-2xl text-xl',
    },
    disabled: {
      false: null,
      true: 'disabled:pointer-events-none disabled:cursor-text',
    },
  },
  compoundVariants: [
    {
      intent: 'primary',
      disabled: false,
      className: 'hover:bg-primary-600 active:bg-primary-700',
    },
    {
      intent: 'primary',
      disabled: true,
      className: 'disabled:bg-primary-300',
    },

    {
      intent: 'primary',
      variant: 'danger',
      disabled: false,
      className: 'bg-danger-500 hover:bg-danger-600 active:bg-danger-700',
    },
    {
      intent: 'primary',
      variant: 'danger',
      disabled: true,
      className: 'bg-danger-500 disabled:bg-danger-300',
    },

    {
      intent: 'secondary',
      disabled: false,
      className:
        'hover:text-primary-600 active:text-primary-700 hover:border-primary-600 active:border-primary-700',
    },
    {
      intent: 'secondary',
      disabled: true,
      className: 'text-primary-300 disabled:border-primary-300',
    },

    {
      intent: 'secondary',
      disabled: false,
      variant: 'danger',
      className:
        'border-danger-500 text-danger-500 hover:text-danger-600 active:text-danger-700 hover:border-danger-600 active:border-danger-700 blablablab',
    },
    {
      intent: 'secondary',
      disabled: true,
      variant: 'danger',
      className: 'border-danger-300 text-danger-300 disabled:border-danger-300',
    },
  ],
  defaultVariants: {
    variant: 'none',
    intent: 'primary',
    disabled: false,
    size: 'md',
  },
});

interface ButtonProps
  extends ComponentPropsWithoutRef<ElementType>,
    Omit<HTMLAttributes<HTMLElement>, 'disabled'>,
    VariantProps<typeof button> {
  external?: boolean;
  href?: string;
  loading?: boolean;
  icon?: ReactElement;
  as?: ElementType;
}

const Button: FC<ButtonProps> = ({
  onClick = () => null,
  href,
  external = false,
  size,
  intent,
  loading = false,
  disabled = false,
  icon,
  variant,
  children,
  className,
  as: PolymorphicElement = 'button',
  ...props
}) => {
  const Component = (
    <PolymorphicElement
      onClick={onClick}
      className={classNames(
        button({
          intent,
          size,
          disabled: disabled || loading,
          variant,
          className,
        }),
      )}
      disabled={disabled || loading}
      {...props}>
      <div className="flex h-full items-center justify-center gap-1">
        <p className="overflow-hidden text-nowrap text-ellipsis">{children}</p>

        {loading ? (
          <Icon
            className="animate-spin"
            src={<Spinner />}
            size="md"
            alt="Loading spinner"
          />
        ) : (
          icon && <Icon src={icon} size={size === 'sm' ? 'sm' : 'md'} />
        )}
      </div>
    </PolymorphicElement>
  );

  if (disabled || loading) {
    return Component;
  }

  return href ? (
    <Link target={external ? `_blank` : '_self'} href={href}>
      {Component}
    </Link>
  ) : (
    Component
  );
};

export default Button;
