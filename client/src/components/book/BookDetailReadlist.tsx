'use client';

import { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { useMediaQuery } from 'react-responsive';

import type { components } from '@/typings/api';

import breakpoints from '@/styles/breakpoints';

import { useBoundStore } from '@/providers/boundStoreProvider';
import getReadlistByUsernameAndBook from '@/actions/getReadlistByUsernameAndBook';
import deleteReadlist from '@/actions/deleteReadlist';
import postReadlist from '@/actions/postReadlist';

import Button from '@/components/shared/Button';

import Check from '@/public/assets/icons/Check.svg';
import Close from '@/public/assets/icons/Close.svg';

import useClearStore from '@/hooks/useClearStore';

const BookDetailReadlist = ({
  bookId,
}: Readonly<{
  bookId: components['schemas']['BookResponse']['id'];
}>) => {
  const isSM = useMediaQuery({ maxWidth: breakpoints.sm });
  const isLG = useMediaQuery({ maxWidth: breakpoints.lg });
  const [isReadlistLoading, setIsReadlistLoading] = useState(false);
  const [isReadlistItemAdded, setIsReadlistItemAdded] = useState(false);
  const router = useRouter();
  const user = useBoundStore(state => state.user);
  const getReadlistByUsernameAndBookWrapper = useClearStore(
    getReadlistByUsernameAndBook,
  );
  const deleteReadlistWrapper = useClearStore(deleteReadlist);
  const postReadlistWrapper = useClearStore(postReadlist);
  const [isInitialEffect, setIsInitialEffect] = useState(true);

  const buttonSize = isSM ? 'md' : isLG ? 'sm' : 'md';

  useEffect(() => {
    const getReadlistByUsernameAndBookEffectAsyncWrapper = async () => {
      const response = await getReadlistByUsernameAndBookWrapper(
        user!.username,
        bookId,
      );

      if (response?.data) {
        setIsReadlistItemAdded(true);
      } else {
        setIsReadlistItemAdded(false);
      }
    };

    if (user && isInitialEffect) {
      setIsInitialEffect(false);
      getReadlistByUsernameAndBookEffectAsyncWrapper();
    }
  }, [user, bookId, getReadlistByUsernameAndBookWrapper, isInitialEffect]);

  const handleAddToReadlistClick = async () => {
    if (!user) {
      router.push('/auth/signin');
      return;
    }

    setIsReadlistLoading(true);
    if (isReadlistItemAdded) {
      const response = await deleteReadlistWrapper(user.username!, bookId!);

      if (response) {
        setIsReadlistItemAdded(false);
      }
    } else {
      const response = await postReadlistWrapper(user.username!, bookId!);

      if (response) {
        setIsReadlistItemAdded(true);
      }
    }

    setIsReadlistLoading(false);
  };

  return user && isReadlistItemAdded ? (
    <Button
      disabled={isReadlistLoading}
      onClick={handleAddToReadlistClick}
      intent="primary"
      size={buttonSize}
      icon={<Close />}>
      Remove from readlist
    </Button>
  ) : (
    <Button
      disabled={isReadlistLoading}
      onClick={handleAddToReadlistClick}
      intent="primary"
      size={buttonSize}
      icon={<Check />}>
      Mark as read
    </Button>
  );
};

export default BookDetailReadlist;
