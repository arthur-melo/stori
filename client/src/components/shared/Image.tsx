'use client';

import { useState, type FC } from 'react';
import ImageNext, { type ImageProps } from 'next/image';

import Icon from '@/components/shared/Icon';

import Picture from '@/public/assets/icons/Picture.svg';
import Warn from '@/public/assets/icons/Warn.svg';

const Image: FC<ImageProps> = ({
  src,
  width,
  height,
  fill,
  alt,
  sizes,
  ...props
}) => {
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(false);

  const handleImageLoaded = () => setLoading(false);
  const handleImageError = () => setError(true);

  if (error || !src) {
    return (
      <div className="relative mb-4 flex h-full w-full items-center justify-center">
        <div className="bg-tertiary-500 absolute top-0 left-0 h-full w-full opacity-5"></div>
        <div className="flex flex-col content-center items-center gap-2">
          <Icon
            src={<Warn />}
            size="xl"
            className="text-tertiary-500 dark:text-tertiary-100"
          />
          <p className="text-tertiary-500 dark:text-tertiary-100 text-base font-bold">
            No preview available
          </p>
        </div>
      </div>
    );
  }

  return (
    <>
      {loading && (
        <div className="relative mb-5 flex h-full w-full items-center justify-center">
          <div className="bg-tertiary-500 absolute top-0 left-0 h-full w-full opacity-5"></div>
          <Icon
            src={<Picture />}
            size="xl"
            className="text-tertiary-500 dark:text-tertiary-100 animate-pulse"
          />
        </div>
      )}

      <ImageNext
        onLoad={handleImageLoaded}
        onError={handleImageError}
        src={src}
        alt={alt}
        width={width}
        height={height}
        fill={fill}
        priority
        hidden={loading}
        sizes={
          sizes ??
          '(min-width: 66rem) 16vw, (min-width: 80rem) 16vw, (min-width: 64rem) 25vw, (min-width: 48rem) 33vw, 50vw'
        }
        className="h-full w-full"
        {...props}
      />
    </>
  );
};

export default Image;
