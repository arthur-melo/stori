'use client';

import { FC, useState, cloneElement, type ButtonHTMLAttributes } from 'react';

import Icon from '@/components/shared/Icon';

import StarFull from '@/public/assets/icons/StarFull.svg';
import StarEmpty from '@/public/assets/icons/StarEmpty.svg';

type Size = 'sm' | 'md' | 'lg' | 'xl';

enum StarIconType {
  Empty = 'empty',
  Full = 'full',
}

const roundToNearestHalf = (rating: number) =>
  rating == 0 ? 0 : Math.round(rating * 2) / 2;

const separateStarsByRatingParts = (rating: number) => {
  const starsFull = Math.floor(rating);

  const starsEmpty = 5 - starsFull;

  return { starsFull, starsEmpty };
};

const createStarsArr = (
  count: number,
  type: StarIconType,
  size: Size,
  className: string,
) => {
  const starIcons = {
    full: StarFull,
    empty: StarEmpty,
  };

  const StarIcon = starIcons[type];

  return Array.from(Array(count)).map((_, idx) => (
    <Icon src={<StarIcon />} size={size} className={className} key={idx} />
  ));
};

const renderStar = (value: number = 0, size: Size = 'lg', className = '') => {
  const starsNearestHalf = roundToNearestHalf(value ?? 0);

  const { starsFull, starsEmpty } =
    separateStarsByRatingParts(starsNearestHalf);

  const starsFullArr = createStarsArr(
    starsFull,
    StarIconType.Full,
    size,
    className,
  );
  const starsEmptyArr = createStarsArr(
    starsEmpty,
    StarIconType.Empty,
    size,
    className,
  );

  const merged = [...starsFullArr, ...starsEmptyArr];

  return merged.map((element, idx) => cloneElement(element, { key: idx }));
};

interface StarsRatingInteractiveProps
  extends Omit<ButtonHTMLAttributes<HTMLButtonElement>, 'value' | 'onClick'> {
  value?: number;
  size?: Size;
  className?: string;
  onClick?: (number: number) => void;
}

const StarsRatingInteractive: FC<StarsRatingInteractiveProps> = ({
  value = 0,
  size = 'lg',
  className = '',
  disabled,
  onClick = () => null,
  ...props
}) => {
  const [stars, setStars] = useState(renderStar(value, size, className));

  const handleHoverStars = (number: number) => {
    setStars(renderStar(number, size, className));
  };

  const handleMouseLeaveStars = () => {
    setStars(renderStar(value, size, className));
  };

  return stars.map((item, idx) => (
    <button
      disabled={disabled}
      onMouseEnter={() => handleHoverStars(idx + 1)}
      onMouseLeave={() => handleMouseLeaveStars()}
      onClick={() => onClick(idx + 1)}
      key={idx}
      className="flex cursor-pointer overflow-hidden"
      {...props}>
      {item}
    </button>
  ));
};

export default StarsRatingInteractive;
