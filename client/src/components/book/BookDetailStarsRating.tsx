'use client';

import { useMediaQuery } from 'react-responsive';
import { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';

import type { components } from '@/typings/api';

import breakpoints from '@/styles/breakpoints';

import { useBoundStore } from '@/providers/boundStoreProvider';
import getUserRatingByUsernameAndBook from '@/actions/getUserRatingByUsernameAndBook';
import deleteUserRating from '@/actions/deleteUserRating';
import postUserRating from '@/actions/postUserRating';

import StarsRatingInteractive from '@/components/shared/StarsRatingInteractive';
import StarsRating from '@/components/shared/StarsRating';

import useClearStore from '@/hooks/useClearStore';

const BookDetailStarsRating = ({
  bookId,
}: Readonly<{
  bookId: components['schemas']['BookResponse']['id'];
}>) => {
  const [rating, setRating] = useState<number>();
  const [isRatingLoading, setIsRatingLoading] = useState(true);
  const isLG = useMediaQuery({ maxWidth: breakpoints.lg });
  const isMD = useMediaQuery({ maxWidth: breakpoints.md });

  const iconSize = isMD ? 'xl' : isLG ? 'md' : 'xl';

  const router = useRouter();
  const user = useBoundStore(state => state.user);

  const getUserRatingByUsernameAndBookWrapper = useClearStore(
    getUserRatingByUsernameAndBook,
  );
  const deleteUserRatingWrapper = useClearStore(deleteUserRating);
  const postUserRatingWrapper = useClearStore(postUserRating);
  const [isInitialEffect, setIsInitialEffect] = useState(true);

  useEffect(() => {
    const getUserRatingByUsernameAndBookEffectAsyncWrapper = async () => {
      const response = await getUserRatingByUsernameAndBookWrapper(
        user!.username!,
        bookId!,
      );

      if (response?.data) {
        setRating(response.data.data!.at(0)!.rating);
      } else {
        setRating(undefined);
      }
      setIsRatingLoading(false);
    };

    if (user && isInitialEffect) {
      setIsInitialEffect(false);
      getUserRatingByUsernameAndBookEffectAsyncWrapper();
    }
  }, [
    user,
    setRating,
    bookId,
    getUserRatingByUsernameAndBookWrapper,
    isInitialEffect,
  ]);

  const handleStarsRatingClick = async (newRating: number) => {
    if (!user) {
      router.push('/auth/signin');
      return;
    }

    setIsRatingLoading(true);

    let response;
    if (rating === newRating) {
      response = await deleteUserRatingWrapper(user.username!, bookId!);

      if (response) {
        setRating(undefined);
      }
    } else {
      response = await postUserRatingWrapper(
        user.username!,
        bookId!,
        newRating,
      );

      if (response) {
        setRating(newRating);
      }
    }

    setIsRatingLoading(false);

    if (response) {
      router.refresh();
    }
  };

  if (!user) {
    return (
      <StarsRatingInteractive
        onClick={handleStarsRatingClick}
        className="text-secondary-500"
        size={iconSize}
        key="loggedOff"
      />
    );
  }

  return isRatingLoading ? (
    <StarsRating
      value={rating}
      className="w-full text-neutral-300"
      size={iconSize}
    />
  ) : (
    <StarsRatingInteractive
      onClick={handleStarsRatingClick}
      value={rating}
      className="text-secondary-500"
      size={iconSize}
      key={`loggedIn${iconSize}${rating}`}
    />
  );
};

export default BookDetailStarsRating;
