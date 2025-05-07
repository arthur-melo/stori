'use client';

import Link from 'next/link';

import type { components } from '@/typings/api';

import Image from '@/components/shared/Image';
import StarsRating from '@/components/shared/StarsRating';
import Button from '@/components/shared/Button';

import Trashcan from '@/public/assets/icons/Trashcan.svg';

import { useBoundStore } from '@/providers/boundStoreProvider';

const ProfileReviewCard = ({
  username,
  data,
  disabled = false,
  onDelete,
}: {
  username: string;
  data: components['schemas']['BookListResponse'];
  disabled: boolean;
  onDelete: (id: number) => void;
}) => {
  const user = useBoundStore(state => state.user);

  return (
    <div className="col-span-full grid grid-cols-subgrid gap-4 md:flex md:flex-col">
      <div className="col-span-full w-full">
        <p className="text-tertiary-500 dark:text-tertiary-100 shrink-0 overflow-hidden text-xl font-bold text-nowrap text-ellipsis">
          {data.title}
        </p>
      </div>

      <div className="col-span-full grid grid-cols-subgrid gap-6 sm:block">
        <div className="col-span-full flex w-full flex-col gap-2">
          <Link href={`/book/${data.bookId}`} title={data.title!}>
            <div className="aspect-1/1.5 relative w-full">
              <Image
                src={data.coverImg!}
                alt={`Book cover image for ${data.title}`}
                fill
              />
            </div>
          </Link>
          <div className="flex flex-col items-center justify-center">
            <div className="flex shrink-0">
              <StarsRating
                value={data.rating?.starsAverage ?? 0}
                size="md"
                className="text-tertiary-500 dark:text-tertiary-100"
              />
            </div>

            <div className="flex max-w-full gap-2">
              <p className="text-tertiary-500 dark:text-tertiary-100 text-lg font-light">
                Votes:
              </p>
              <p className="text-tertiary-500 dark:text-tertiary-100 overflow-hidden text-lg font-bold text-nowrap text-ellipsis">
                {data.rating?.starsTotal?.toLocaleString() ?? 0}
              </p>
            </div>
          </div>

          {user && user.username === username && (
            <div className="flex w-full justify-center">
              <div className="w-auto">
                <Button
                  onClick={() => onDelete(data.id!)}
                  icon={<Trashcan />}
                  size="sm"
                  disabled={disabled}
                  variant="danger"
                  intent="secondary">
                  Delete
                </Button>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default ProfileReviewCard;
