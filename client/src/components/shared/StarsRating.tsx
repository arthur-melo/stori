import { cloneElement } from 'react';

import Icon from '@/components/shared/Icon';

import StarHalf from '@/public/assets/icons/StarHalf.svg';
import StarFull from '@/public/assets/icons/StarFull.svg';
import StarEmpty from '@/public/assets/icons/StarEmpty.svg';

type Size = 'sm' | 'md' | 'lg' | 'xl';

enum StarIconType {
  Empty = 'empty',
  Half = 'half',
  Full = 'full',
}

const roundToNearestHalf = (rating: number) =>
  rating == 0 ? 0 : Math.round(rating * 2) / 2;

const separateStarsByRatingParts = (rating: number) => {
  const starsFull = Math.floor(rating);

  const starsHalf = rating - starsFull == 0.5 ? 1 : 0;

  const starsEmpty = 5 - starsFull - starsHalf;

  return { starsFull, starsHalf, starsEmpty };
};

const createStarsArr = (
  count: number,
  type: StarIconType,
  size: Size,
  className: string,
) => {
  const starIcons = {
    full: StarFull,
    half: StarHalf,
    empty: StarEmpty,
  };

  const StarIcon = starIcons[type];

  return Array.from(Array(count)).map((_, idx) => (
    <Icon src={<StarIcon />} size={size} className={className} key={idx} />
  ));
};

const StarsRating = ({
  value,
  size = 'lg',
  className = '',
}: Readonly<{
  value: number | undefined | null;
  size?: Size;
  className?: string;
}>) => {
  const starsNearestHalf = roundToNearestHalf(value ?? 0);
  const { starsFull, starsHalf, starsEmpty } =
    separateStarsByRatingParts(starsNearestHalf);

  const starsFullArr = createStarsArr(
    starsFull,
    StarIconType.Full,
    size,
    className,
  );
  const starsHalfArr = createStarsArr(
    starsHalf,
    StarIconType.Half,
    size,
    className,
  );
  const starsEmptyArr = createStarsArr(
    starsEmpty,
    StarIconType.Empty,
    size,
    className,
  );

  const merged = [...starsFullArr, ...starsHalfArr, ...starsEmptyArr];

  const mergedWithKey = merged.map((element, idx) =>
    cloneElement(element, { key: idx }),
  );

  return mergedWithKey;
};

export default StarsRating;
