'use client';

import { FC, useState } from 'react';
import Image, { type ImageProps } from 'next/image';
import { cva, type VariantProps } from 'class-variance-authority';
import cn from '@/utils/classNames';

const avatar = cva(null, {
  variants: {
    imageSize: {
      xs: 24,
      sm: 48,
      md: 80,
      lg: 128,
    },
    fontSize: {
      xs: 'text-normal',
      sm: 'text-xl',
      md: 'text-3xl',
      lg: 'text-5xl',
    },
    size: {
      xs: 'w-6 h-6 min-w-6 min-h-6',
      sm: 'w-12 h-12 min-w-12 min-h-12',
      md: 'w-20 h-20 min-w-20 min-h-20',
      lg: 'w-32 h-32 min-w-32 min-h-32',
    },
  },
});

const generateInitials = (name: string) => {
  const nameParts = name.split(' ');

  // Has firstname and lastname, get initials from both.
  if (nameParts.length > 1) {
    return (
      nameParts.at(0)!.charAt(0).toUpperCase() +
      nameParts.at(1)!.charAt(0).toUpperCase()
    );
  } else {
    // Only firstname, get first letter
    return name.substring(0, 1).toUpperCase();
  }
};

interface AvatarProps
  extends Omit<ImageProps, 'src'>,
    VariantProps<typeof avatar> {
  src?: string | null;
  name: string;
}

const Avatar: FC<AvatarProps> = ({
  alt,
  src,
  imageSize = 'md',
  size = 'md',
  name = '',
  ...props
}) => {
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(false);

  const handleImageLoaded = () => setLoading(false);
  const handleImageError = () => setError(true);

  if (error || !src) {
    const initials = generateInitials(name);

    return (
      <div
        className={cn(
          avatar({ size }),
          'bg-tertiary-500 dark:bg-tertiary-100 flex items-center justify-center rounded-full',
        )}>
        <p
          className={cn(
            avatar({ fontSize: size }),
            'font-bold text-white dark:text-black',
          )}>
          {initials}
        </p>
      </div>
    );
  }

  return (
    <>
      {loading && (
        <div
          className={cn(
            avatar({ size }),
            'animate-pulse rounded-full bg-neutral-300',
          )}></div>
      )}

      <Image
        onLoad={handleImageLoaded}
        onError={handleImageError}
        src={src}
        alt={alt}
        width={Number(avatar({ imageSize }))}
        height={Number(avatar({ imageSize }))}
        priority
        hidden={loading}
        className={cn(
          avatar({ size }),
          'bg-tertiary-500 flex items-center justify-center rounded-full border-2 border-black dark:border-white',
        )}
        {...props}
      />
    </>
  );
};

export default Avatar;
