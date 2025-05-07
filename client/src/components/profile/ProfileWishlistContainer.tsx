'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';

import type { components } from '@/typings/api';

import ProfileReviewCard from '@/components/profile/ProfileReviewCard';
import ShowMore from '@/components/shared/ShowMore';

import getWishlists from '@/actions/getWishlists';
import deleteWishlist from '@/actions/deleteWishlist';

import useDataLoader from '@/hooks/useDataLoader';
import useClearStore from '@/hooks/useClearStore';

type WishlistResponse = components['schemas']['WishlistResponse'];

const ProfileWishlistContainer = ({
  username,
  wishlist,
}: Readonly<{
  username: string;
  wishlist: components['schemas']['WishlistResponsePaginatedListEnvelope'];
}>) => {
  const { loadMoreData, data, hasNextPage } = useDataLoader<WishlistResponse>(
    wishlist,
    getWishlists.bind(null, username),
  );
  const [isDeleteLoading, setIsDeleteLoading] = useState(false);
  const deleteWishlistWrapper = useClearStore(deleteWishlist);
  const router = useRouter();

  const handleShowMore = async () => {
    await loadMoreData();
  };

  const handleDeleteWishlistItemClick = async (id: number) => {
    setIsDeleteLoading(true);
    const response = await deleteWishlistWrapper(username, id);
    setIsDeleteLoading(false);

    if (response) {
      router.refresh();
    }
  };

  return (
    <div className="grid w-full auto-rows-min grid-cols-12 gap-x-6 gap-y-8 sm:grid-cols-4 md:grid-cols-8">
      <div className="col-span-full flex">
        <p className="text-tertiary-500 dark:text-tertiary-100 mr-auto text-3xl font-bold">
          Wishlist ({wishlist?.totalItems})
        </p>
      </div>

      <div className="col-span-full grid grid-cols-subgrid gap-6 md:flex md:flex-wrap md:justify-between">
        {data!.map((wishlist, idx) => (
          <div
            key={idx}
            className="col-span-2 grid auto-rows-min grid-cols-subgrid sm:min-h-max sm:w-5/12 md:block md:w-3/10 lg:col-span-3">
            <ProfileReviewCard
              username={username}
              disabled={isDeleteLoading}
              data={wishlist.book!}
              onDelete={handleDeleteWishlistItemClick}
            />
          </div>
        ))}

        {hasNextPage && <ShowMore onShowMore={handleShowMore} />}
      </div>
    </div>
  );
};

export default ProfileWishlistContainer;
