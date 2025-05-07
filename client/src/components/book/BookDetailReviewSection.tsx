'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';

import { useBoundStore } from '@/providers/boundStoreProvider';

import postReview from '@/actions/postReview';

import useClearStore from '@/hooks/useClearStore';

import type { components } from '@/typings/api';

import BookDetailReviewContainer from '@/components/book/BookDetailReviewContainer';
import ReviewInputContainer from '@/components/shared/ReviewInputContainer';
import Button from '@/components/shared/Button';
import Mountain from '@/components/draws/Mountain';

import Edit from '@/public/assets/icons/Edit.svg';

const BookDetailReviewSection = ({
  bookId,
  reviews,
}: Readonly<{
  bookId: components['schemas']['BookResponse']['id'];
  reviews: components['schemas']['ReviewBookResponsePaginatedListEnvelope'];
}>) => {
  const [isPostReviewLoading, setIsPostReviewLoading] = useState(false);
  const [showAddReview, setShowAddReview] = useState(false);
  const router = useRouter();
  const user = useBoundStore(state => state.user);

  const postReviewWrapper = useClearStore(postReview);

  const handleReviewInputCancelClick = () => setShowAddReview(false);

  const handleShowAddReview = () => {
    if (!user) {
      router.push('/auth/signin');
      return;
    }
    setShowAddReview(!showAddReview);
  };

  const handleOnAddReview = async (text: string) => {
    setIsPostReviewLoading(true);
    const response = await postReviewWrapper(bookId!, text);

    setIsPostReviewLoading(false);

    if (response) {
      router.refresh();
    }
  };

  return (
    <>
      <div className="col-span-full flex flex-wrap gap-4">
        <p className="text-tertiary-500 dark:text-tertiary-100 mr-auto text-3xl font-bold">
          Reviews ({reviews.totalItems})
        </p>
        <div className="">
          <Button
            icon={<Edit />}
            intent="primary"
            size="md"
            onClick={handleShowAddReview}>
            Write review
          </Button>
        </div>
      </div>

      {showAddReview && (
        <div className="col-span-full">
          <ReviewInputContainer
            disabled={isPostReviewLoading}
            onCancel={handleReviewInputCancelClick}
            onAddReview={handleOnAddReview}
          />
        </div>
      )}

      {reviews?.data?.length === 0 ? (
        <div className="col-span-full flex h-full w-full items-center justify-center">
          <div className="flex flex-col items-end gap-11">
            <Mountain className="h-full max-w-full object-contain" />
            <div className="flex flex-col items-end gap-4">
              <p className="text-tertiary-500 dark:text-tertiary-100 text-end text-4xl font-bold">
                No reviews available
              </p>
              <p className="text-tertiary-500 dark:text-tertiary-100 text-end text-2xl font-normal">
                Be the first to review this book!
              </p>
            </div>
          </div>
        </div>
      ) : (
        <div className="col-span-full grid w-full grid-cols-subgrid">
          <BookDetailReviewContainer bookId={bookId!} reviews={reviews} />
        </div>
      )}
    </>
  );
};

export default BookDetailReviewSection;
