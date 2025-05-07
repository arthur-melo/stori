'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';

import type { components } from '@/typings/api';

import ProfileReview from '@/components/profile/ProfileReview';
import ReviewInputContainer from '@/components/shared/ReviewInputContainer';
import ShowMore from '@/components/shared/ShowMore';

import { useBoundStore } from '@/providers/boundStoreProvider';
import getBookReviewByUsername from '@/actions/getBookReviewByUsername';
import patchReview from '@/actions/patchReview';

import useDataLoader from '@/hooks/useDataLoader';
import useClearStore from '@/hooks/useClearStore';
import deleteReview from '@/actions/deleteReview';

type ReviewResponse = components['schemas']['ReviewResponse'];

const ProfileReviewListContainer = ({
  username,
  reviews,
}: Readonly<{
  username: string;
  reviews: components['schemas']['ReviewResponsePaginatedListEnvelope'];
}>) => {
  const [isPostReviewLoading, setIsPostReviewLoading] = useState(false);
  const [editReviewId, setEditReviewId] = useState(0);
  const [showReviewInput, setShowReviewInput] = useState(false);
  const [reviewText, setReviewText] = useState('');
  const patchReviewWrapper = useClearStore(patchReview);
  const deleteReviewWrapper = useClearStore(deleteReview);
  const user = useBoundStore(state => state.user);
  const { loadMoreData, data, hasNextPage } = useDataLoader<ReviewResponse>(
    reviews,
    getBookReviewByUsername.bind(null, username),
  );
  const router = useRouter();

  const handleShowMore = async () => {
    await loadMoreData();
  };

  const handleEditClick = (id: number, text: string) => {
    setReviewText(text);
    setEditReviewId(id);
    setShowReviewInput(true);
  };

  const handleDeleteClick = async (id: number) => {
    const response = await deleteReviewWrapper(user!.username!, id);

    if (response) {
      router.refresh();
    }
  };

  const handleEditReview = async (text: string) => {
    setIsPostReviewLoading(true);
    const response = await patchReviewWrapper(editReviewId, text);
    setIsPostReviewLoading(false);

    if (response) {
      setShowReviewInput(false);
      router.refresh();
    }
  };

  const handleCancelEditReview = () => {
    setShowReviewInput(false);
  };

  return (
    <div className="grid w-full auto-rows-min grid-cols-12 gap-x-6 gap-y-8 sm:grid-cols-4 md:grid-cols-8">
      <div className="col-span-full flex flex-col gap-8">
        <p className="text-tertiary-500 dark:text-tertiary-100 mr-auto shrink-0 text-3xl font-bold">
          Reviews ({reviews?.totalItems})
        </p>
        {showReviewInput && (
          <div className="mx-auto flex w-3/4 sm:w-full">
            <ReviewInputContainer
              disabled={isPostReviewLoading}
              initialText={reviewText}
              onCancel={handleCancelEditReview}
              onAddReview={handleEditReview}
            />
          </div>
        )}
      </div>

      <div className="col-span-full grid grid-cols-subgrid gap-6">
        {data!.map((review, idx) => (
          <div
            key={idx}
            className="col-span-6 grid auto-rows-min grid-cols-subgrid lg:col-span-full">
            <ProfileReview
              username={username}
              data={review}
              onReviewEdit={handleEditClick}
              onReviewDelete={handleDeleteClick}
            />
          </div>
        ))}

        {hasNextPage && <ShowMore onShowMore={handleShowMore} />}
      </div>
    </div>
  );
};

export default ProfileReviewListContainer;
