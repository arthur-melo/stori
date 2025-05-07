'use client';

import { useRef, useState, useEffect } from 'react';
import Link from 'next/link';

import { formatDistanceToNow } from 'date-fns';

import type { components } from '@/typings/api';

import Avatar from '@/components/shared/Avatar';
import StarsRating from '@/components/shared/StarsRating';
import ShowMore from '@/components/shared/ShowMore';

import useResizeObserver from '@/hooks/useResizeObserver';

const BookDetailReviewItem = ({
  data,
}: {
  data: components['schemas']['ReviewBookResponse'];
}) => {
  const elementRef = useRef<HTMLDivElement>(null);
  const isHeightExceeded = useResizeObserver(elementRef, 110);
  const [showAllItems, setShowAllItems] = useState(true);

  useEffect(() => {
    setShowAllItems(!isHeightExceeded);
  }, [isHeightExceeded]);

  const handleShowAllItems = () => setShowAllItems(true);

  return (
    <div className="flex w-full flex-col gap-4">
      <Link
        href={`/profile/${data.author!.username}`}
        title={`Navigate to "${data.author?.username}" user profile`}>
        <div className="flex w-full items-center gap-4">
          <div className="shrink-0">
            <Avatar
              name={data.author!.name!}
              alt={`Avatar of user ${data.author!.username}`}
              src={
                data.author?.profileImg &&
                `${process.env.NEXT_PUBLIC_BACKEND_URL}/images/${data.author?.profileImg}`
              }
              size="md"
            />
          </div>
          <div className="flex w-full justify-between gap-2">
            <div className="flex shrink-0 flex-col gap-1">
              <p className="text-tertiary-500 dark:text-tertiary-100 text-xl font-bold">
                {data.author?.name}
              </p>
              <p className="text-tertiary-500 dark:text-tertiary-100 text-xl font-normal">
                @{data.author?.username}
              </p>
            </div>
            <div className="flex flex-col items-end justify-end gap-1 overflow-hidden">
              <div className="flex max-w-full">
                <StarsRating
                  value={data.rating}
                  size="md"
                  className="text-secondary-500"
                />
              </div>

              <p className="text-tertiary-500 dark:text-tertiary-100 text-xl font-light">
                {formatDistanceToNow(new Date(data.createdAt!), {
                  addSuffix: true,
                  includeSeconds: true,
                })}
              </p>
            </div>
          </div>
        </div>
      </Link>
      <p
        ref={elementRef}
        style={
          isHeightExceeded && !showAllItems
            ? { maxHeight: `${110 + 1}px`, overflow: 'hidden' }
            : {}
        }
        className="text-tertiary-500 dark:text-tertiary-100 line-clamp-14 text-lg font-normal">
        {data.text}
      </p>
      {!showAllItems && <ShowMore onShowMore={handleShowAllItems} />}
    </div>
  );
};

export default BookDetailReviewItem;
