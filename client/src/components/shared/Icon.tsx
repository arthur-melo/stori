import { cloneElement, FC, type ReactElement } from 'react';
import { cva, VariantProps } from 'class-variance-authority';

import cn from '@/utils/classNames';

const icon = cva(null, {
  variants: {
    size: {
      sm: 'flex justify-center items-center w-5 h-5',
      md: 'flex justify-center items-center w-6 h-6',
      lg: 'flex justify-center items-center w-8 h-8',
      xl: 'flex justify-center items-center w-12 h-12',
    },
  },
  defaultVariants: {
    size: 'md',
  },
});

interface IconProps extends VariantProps<typeof icon> {
  src: ReactElement;
  alt?: string;
  className?: string;
}

const Icon: FC<IconProps> = ({ size, src, className = '' }) => {
  const clonedElement = cloneElement(src, {
    className: cn(className, 'object-fit max-w-full max-h-full'),
  } as typeof src & { className: string });

  return <div className={cn(icon({ size, className }))}>{clonedElement}</div>;
};

export default Icon;
