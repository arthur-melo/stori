'use client';

import { useEffect, useState } from 'react';
import { useMediaQuery } from 'react-responsive';

import type { components } from '@/typings/api';

import breakpoints from '@/styles/breakpoints';

import formatDate from '@/utils/formatDate';

import Image from '@/components/shared/Image';
import Icon from '@/components/shared/Icon';
import Button from '@/components/shared/Button';
import StarsRating from '@/components/shared/StarsRating';
import BookDetailDataList from '@/components/book/BookDetailDataList';
import BookDetailStarsRating from '@/components/book/BookDetailStarsRating';
import BookDetailReadlist from '@/components/book/BookDetailReadlist';
import BookDetailReviewSection from '@/components/book/BookDetailReviewSection';
import BookDetailWishlist from '@/components/book/BookDetailWishlist';

import StarFull from '@/public/assets/icons/StarFull.svg';
import External from '@/public/assets/icons/External.svg';

const bookDataSection = (title: string, data?: string | number | null) => (
  <div className="col-span-full grid grid-cols-subgrid">
    <p className="text-tertiary-500 dark:text-tertiary-100 col-span-2 text-xl font-bold sm:col-span-2">
      {title}
    </p>
    {data ? (
      <p className="text-tertiary-500 dark:text-tertiary-100 col-span-7 text-xl font-light sm:col-span-2">
        {data}
      </p>
    ) : (
      <p className="col-span-7 text-xl font-light text-neutral-300 sm:col-span-2">
        N/A
      </p>
    )}
  </div>
);

const bookDataListSection = (
  title: string,
  searchParam: string,
  items: string[],
) => {
  if (items.length === 0) {
    return bookDataSection(title);
  }

  return (
    <div className="col-span-full">
      <BookDetailDataList
        title={title}
        searchParam={searchParam}
        items={items}
      />
    </div>
  );
};

const bookRatingDetailSection = (
  book: components['schemas']['BookResponse'],
  isLG: boolean,
) => {
  const filteredRatingText = (idx: number) =>
    book.rating === null
      ? '0'
      : (book.rating![
          `star${idx + 1}` as keyof components['schemas']['RatingResponse']
        ]?.toLocaleString() ?? '0');

  return Array.from(Array(5)).map((_, idx) => (
    <div className="flex max-w-full gap-2" key={idx}>
      <div className="mr-auto flex">
        {Array.from(Array(idx + 1)).map((_, idx) => (
          <Icon
            size={isLG ? 'sm' : 'md'}
            src={<StarFull />}
            key={idx}
            className="text-tertiary-500 dark:text-tertiary-100"
          />
        ))}
      </div>
      <p
        className="text-tertiary-500 dark:text-tertiary-100 shrink overflow-hidden text-lg font-light text-ellipsis"
        title={filteredRatingText(idx)}>
        {filteredRatingText(idx)}
      </p>
    </div>
  ));
};

const BookDetail = ({
  book,
  reviews,
}: Readonly<{
  book: components['schemas']['BookResponse'];
  reviews: components['schemas']['ReviewBookResponsePaginatedListEnvelope'];
}>) => {
  const [isClient, setIsClient] = useState(false);
  const isSM = useMediaQuery({ maxWidth: breakpoints.sm });
  const isMD = useMediaQuery({ maxWidth: breakpoints.md });
  const isLG = useMediaQuery({ maxWidth: breakpoints.lg });

  useEffect(() => {
    setIsClient(true);
  }, []);

  if (!isClient) {
    return;
  }

  return (
    <div className="w-full">
      <div className="my-8 grid w-full grid-cols-12 justify-center gap-6 gap-y-16 sm:grid-cols-4 sm:justify-items-center md:grid-cols-8">
        <div className="col-span-3 sm:col-span-full sm:mx-auto sm:w-full">
          <div className="flex w-full flex-col gap-4">
            <div className="aspect-1/1.5 relative w-full">
              <Image
                src={book.coverImg!}
                alt={`Book cover image for ${book.title}`}
                fill
              />
            </div>
            <div className="flex flex-col items-center gap-4">
              <div className="flex max-w-full items-center">
                <StarsRating
                  value={book.rating?.starsAverage}
                  size={isMD ? 'xl' : isLG ? 'md' : 'xl'}
                  className="text-tertiary-500 dark:text-tertiary-100"
                />
              </div>

              <div className="align-center flex flex-wrap justify-center gap-4 sm:w-full sm:flex-col lg:justify-start lg:gap-0">
                <div className="flex w-full justify-center gap-2">
                  <p className="text-tertiary-500 dark:text-tertiary-100 text-lg font-light lg:mr-auto lg:text-base">
                    Rating:
                  </p>
                  <p className="text-tertiary-500 dark:text-tertiary-100 text-lg font-bold lg:text-base">
                    {book.rating === null
                      ? '0'
                      : book.rating?.starsAverage!.toLocaleString()}
                  </p>
                </div>

                <div className="flex w-full justify-center gap-2">
                  <p className="text-tertiary-500 dark:text-tertiary-100 text-lg font-light lg:mr-auto lg:text-base">
                    Votes:
                  </p>
                  <p className="text-tertiary-500 dark:text-tertiary-100 text-lg font-bold lg:text-base">
                    {book.rating === null
                      ? '0'
                      : book.rating?.starsTotal!.toLocaleString()}
                  </p>
                </div>
              </div>

              <div className="flex w-full max-w-full flex-col">
                {bookRatingDetailSection(book, isLG)}
              </div>
            </div>
            <div className="flex flex-col items-center gap-4">
              <div className="flex w-full max-w-full flex-col items-center gap-4 lg:gap-2">
                <div className="flex w-full max-w-full justify-center">
                  <BookDetailStarsRating bookId={book.id} />
                </div>
                <p className="text-tertiary-500 dark:text-tertiary-100 text-lg font-normal">
                  Your rating
                </p>
              </div>

              <div className="flex w-full flex-col gap-4">
                <div className="w-full">
                  <BookDetailWishlist bookId={book.id} />
                </div>

                <BookDetailReadlist bookId={book.id} />
                <Button
                  intent="secondary"
                  size={isSM ? 'md' : isLG ? 'sm' : 'md'}
                  icon={<External />}
                  href={`https://www.goodreads.com/book/show/${book.bookId}`}
                  external>
                  See on goodreads
                </Button>
              </div>
            </div>
          </div>
        </div>

        <div className="col-span-9 grid grid-cols-subgrid sm:col-span-full md:col-span-5">
          <div className="col-span-full grid grid-flow-row auto-rows-max grid-cols-subgrid gap-6">
            <p className="text-tertiary-500 dark:text-tertiary-100 col-span-full text-3xl font-bold">
              {book.title}
            </p>

            {book.description && (
              <p className="text-tertiary-500 dark:text-tertiary-100 col-span-full w-full text-lg font-normal">
                {book.description}
              </p>
            )}

            {bookDataSection('Pages', book.pages)}
            {bookDataSection('Language', book.language)}
            {bookDataSection(
              'Publish date',
              book.publishDate ? formatDate(book.publishDate) : null,
            )}
            {bookDataSection('Series', book.series)}
            {bookDataSection('Publisher', book.publisher)}
            {bookDataSection('Book format', book.bookFormat)}
            {bookDataSection('Edition', book.edition)}
            {bookDataSection('ISBN', book.isbn)}

            {bookDataListSection('Awards', 'award', book.awards!)}
            {bookDataListSection('Characters', 'character', book.characters!)}
            {bookDataListSection('Genres', 'genre', book.genres!)}
            {bookDataListSection('Settings', 'setting', book.settings!)}
          </div>
        </div>

        <div className="col-span-full grid grid-cols-subgrid gap-y-8">
          <BookDetailReviewSection bookId={book.id} reviews={reviews} />
        </div>
      </div>
    </div>
  );
};

export default BookDetail;
