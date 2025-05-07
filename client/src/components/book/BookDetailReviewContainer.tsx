'use client';

import { useEffect } from 'react';
import { useInView } from 'react-intersection-observer';

import type { components } from '@/typings/api';

import BookDetailReviewItem from '@/components/book/BookDetailReviewItem';

import getBookReview from '@/actions/getBookReview';

import useDataLoader from '@/hooks/useDataLoader';

type ReviewBookResponse = components['schemas']['ReviewBookResponse'];

const BookDetailReviewContainer = ({
  bookId,
  reviews,
}: Readonly<{
  bookId: number;
  reviews: components['schemas']['ReviewBookResponsePaginatedListEnvelope'];
}>) => {
  const { loadMoreData, data, hasNextPage } = useDataLoader<ReviewBookResponse>(
    reviews,
    getBookReview.bind(null, bookId),
  );
  const { ref, inView } = useInView();

  useEffect(() => {
    const loadMoreDataWrapper = async () => await loadMoreData();

    if (inView && hasNextPage) {
      loadMoreDataWrapper();
    }
  }, [inView, hasNextPage, loadMoreData]);

  return (
    <div className="col-span-full grid grid-cols-subgrid gap-x-6 gap-y-12">
      {data.map((review, idx) => (
        <div key={idx} className="col-span-6 lg:col-span-full">
          <BookDetailReviewItem data={review} />
        </div>
      ))}

      {/* Intersection observer ref for infinite scroll */}
      <div ref={ref}></div>
    </div>
  );
};

export default BookDetailReviewContainer;
