'use client';

import Link from 'next/link';
import { formatDistanceToNow } from 'date-fns';

import type { components } from '@/typings/api';

import Image from '@/components/shared/Image';
import StarsRating from '@/components/shared/StarsRating';
import Button from '@/components/shared/Button';

import Edit from '@/public/assets/icons/Edit.svg';
import Trashcan from '@/public/assets/icons/Trashcan.svg';

import { useBoundStore } from '@/providers/boundStoreProvider';

const ProfileReview = ({
  data,
  username,
  onReviewEdit = () => null,
  onReviewDelete = () => null,
}: {
  username: string;
  onReviewEdit?: (id: number, text: string) => void;
  onReviewDelete?: (id: number) => void;
  data: components['schemas']['ReviewResponse'];
}) => {
  const user = useBoundStore(state => state.user);

  const handleOnEditClick = (text: string) => onReviewEdit(data.id!, text);

  const handleOnDeleteClick = () => onReviewDelete(data.id!);

  return (
    <div className="col-span-full grid grid-cols-subgrid gap-4">
      <div className="col-span-full flex w-full items-center gap-2">
        <p className="text-tertiary-500 dark:text-tertiary-100 shrink-0 overflow-hidden text-xl font-bold text-nowrap text-ellipsis">
          {data.book?.title}
        </p>
        <hr className="bg-tertiary-500 w-full rounded-full border-2 opacity-5" />
        <p className="shrink-0 text-xl font-normal text-neutral-300">
          {formatDistanceToNow(new Date(data.createdAt!), {
            addSuffix: true,
            includeSeconds: true,
          })}
        </p>
      </div>

      <div className="col-span-full grid grid-cols-subgrid gap-6 sm:flex sm:gap-4">
        <div className="col-span-2 flex w-full flex-col gap-2 sm:w-1/3 lg:col-span-3">
          <Link href={`/book/${data.book?.bookId}`} title={data.book!.title!}>
            <div className="aspect-1/1.5 relative w-full max-w-full">
              <Image
                src={data.book!.coverImg!}
                alt={`Book cover image for ${data.book?.title}`}
                fill
              />
            </div>
          </Link>
          <div className="flex max-w-full flex-col items-center justify-center">
            <div className="flex w-full max-w-full justify-center">
              <StarsRating
                value={data.book?.rating?.starsAverage ?? 0}
                size="md"
                className="text-tertiary-500 dark:text-tertiary-100"
              />
            </div>

            <div className="flex w-full gap-2 sm:mt-2 sm:flex-col sm:items-center sm:gap-0">
              <p className="text-tertiary-500 dark:text-tertiary-100 text-lg font-light">
                Votes:
              </p>
              <p className="text-tertiary-500 dark:text-tertiary-100 max-w-full overflow-hidden text-lg font-bold text-nowrap text-ellipsis">
                {data.book?.rating?.starsTotal?.toLocaleString() ?? 0}
              </p>
            </div>
          </div>

          {user && user.username === username && (
            <div className="flex w-full justify-center gap-2 sm:flex-col">
              <Button
                icon={<Edit />}
                size="sm"
                intent="secondary"
                onClick={() => handleOnEditClick(data.text!)}>
                Edit
              </Button>

              <Button
                icon={<Trashcan />}
                size="sm"
                variant="danger"
                intent="secondary"
                onClick={handleOnDeleteClick}>
                Delete
              </Button>
            </div>
          )}
        </div>
        <p className="text-tertiary-500 dark:text-tertiary-100 col-span-4 text-base font-medium md:col-span-5 lg:col-span-9">
          {data.text}
        </p>
      </div>
    </div>
  );
};

export default ProfileReview;
