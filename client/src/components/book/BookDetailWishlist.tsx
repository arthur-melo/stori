'use client';

import { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { useMediaQuery } from 'react-responsive';

import type { components } from '@/typings/api';

import breakpoints from '@/styles/breakpoints';

import { useBoundStore } from '@/providers/boundStoreProvider';
import getWishlistByUsernameAndBook from '@/actions/getWishlistByUsernameAndBook';
import deleteWishlist from '@/actions/deleteWishlist';
import postWishlist from '@/actions/postWishlist';

import Button from '@/components/shared/Button';

import Bookmark from '@/public/assets/icons/Bookmark.svg';
import Close from '@/public/assets/icons/Close.svg';

import useClearStore from '@/hooks/useClearStore';

const BookDetailWishlist = ({
  bookId,
}: Readonly<{
  bookId: components['schemas']['BookResponse']['id'];
}>) => {
  const isSM = useMediaQuery({ maxWidth: breakpoints.sm });
  const isLG = useMediaQuery({ maxWidth: breakpoints.lg });
  const [isWishlistLoading, setIsWishlistLoading] = useState(false);
  const [isWishlistItemAdded, setIsWishlistItemAdded] = useState(false);
  const router = useRouter();
  const user = useBoundStore(state => state.user);
  const getWishlistByUsernameAndBookWrapper = useClearStore(
    getWishlistByUsernameAndBook,
  );
  const deleteWishlistWrapper = useClearStore(deleteWishlist);
  const postWishlistWrapper = useClearStore(postWishlist);
  const [isInitialEffect, setIsInitialEffect] = useState(true);

  const buttonSize = isSM ? 'md' : isLG ? 'sm' : 'md';

  useEffect(() => {
    const getWishlistByUsernameAndBookEffectAsyncWrapper = async () => {
      const response = await getWishlistByUsernameAndBookWrapper(
        user!.username,
        bookId,
      );

      if (response?.data) {
        setIsWishlistItemAdded(true);
      } else {
        setIsWishlistItemAdded(false);
      }
    };

    if (user && isInitialEffect) {
      setIsInitialEffect(false);
      getWishlistByUsernameAndBookEffectAsyncWrapper();
    }
  }, [user, bookId, getWishlistByUsernameAndBookWrapper, isInitialEffect]);

  const handleAddToWishlistClick = async () => {
    if (!user) {
      router.push('/auth/signin');
      return;
    }

    setIsWishlistLoading(true);
    if (isWishlistItemAdded) {
      const response = await deleteWishlistWrapper(user.username!, bookId!);

      if (response) {
        setIsWishlistItemAdded(false);
      }
    } else {
      const response = await postWishlistWrapper(user.username!, bookId!);

      if (response) {
        setIsWishlistItemAdded(true);
      }
    }

    setIsWishlistLoading(false);
  };

  return user && isWishlistItemAdded ? (
    <Button
      disabled={isWishlistLoading}
      onClick={handleAddToWishlistClick}
      intent="primary"
      size={buttonSize}
      icon={<Close />}>
      Remove from wishlist
    </Button>
  ) : (
    <Button
      disabled={isWishlistLoading}
      onClick={handleAddToWishlistClick}
      intent="primary"
      size={buttonSize}
      icon={<Bookmark />}>
      Add to wishlist
    </Button>
  );
};

export default BookDetailWishlist;
