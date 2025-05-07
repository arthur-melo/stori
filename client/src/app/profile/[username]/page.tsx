import ProfileAvatarContainer from '@/components/profile/ProfileAvatarContainer';
import ProfileReviewListContainer from '@/components/profile/ProfileReviewListContainer';
import ProfileWishlistContainer from '@/components/profile/ProfileWishlistContainer';
import ProfileReadlistContainer from '@/components/profile/ProfileReadlistContainer';
import Article from '@/components/draws/Article';
import Houses1 from '@/components/draws/Houses1';

import getUser from '@/actions/getUser';
import getBookReviewByUsername from '@/actions/getBookReviewByUsername';
import getWishlists from '@/actions/getWishlists';
import getReadlists from '@/actions/getReadlists';

import type { components } from '@/typings/api';

const noDataComponent = (
  <div className="flex h-full w-full items-center justify-center">
    <div className="flex flex-col items-end gap-11">
      <div className="flex w-full justify-center">
        <Article className="h-full max-w-full object-contain" />
      </div>
      <div className="flex flex-col items-end gap-4">
        <p className="text-tertiary-500 dark:text-tertiary-100 text-4xl font-bold">
          No items to show
        </p>
        <p className="text-tertiary-500 dark:text-tertiary-100 text-end text-2xl font-normal lg:text-xl">
          This user has not written any reviews, added books to their wishlist
          or readlist yet.
        </p>
      </div>
    </div>
  </div>
);

const errorComponent = (title: string, detail: string) => (
  <div className="flex h-full w-full items-center justify-center">
    <div className="flex flex-col items-end gap-11">
      <Houses1 className="h-full max-w-full object-contain" />
      <div className="flex flex-col items-end gap-4">
        <p className="text-tertiary-500 dark:text-tertiary-100 text-4xl font-bold">
          {title}
        </p>
        <p className="text-tertiary-500 dark:text-tertiary-100 text-2xl font-normal">
          {detail}
        </p>
      </div>
    </div>
  </div>
);

type ProblemDetailsError =
  components['schemas']['HttpValidationProblemDetails'];

const Profile = async ({
  params,
}: {
  params: Promise<{ username: string }>;
}) => {
  const username = (await params).username;
  const { data: userData, error: userError } = await getUser(username);
  const { data: reviewByUsernameData, error: reviewByUsernameError } =
    await getBookReviewByUsername(username);
  const { data: wishlistData, error: wishlistError } =
    await getWishlists(username);
  const { data: readlistData, error: readlistError } =
    await getReadlists(username);

  if (userError || reviewByUsernameError || wishlistError || readlistError) {
    return errorComponent(
      (userError! as ProblemDetailsError)!.title!,
      (userError! as ProblemDetailsError)!.detail!,
    );
  }

  return (
    <div className="mx-auto flex h-full w-full flex-col gap-8 pb-8">
      <ProfileAvatarContainer
        username={username}
        userData={userData!.data!.at(0)!}
      />

      {!reviewByUsernameData?.data?.length &&
      !wishlistData?.data?.length &&
      !readlistData?.data?.length ? (
        noDataComponent
      ) : (
        <div className="flex w-full flex-col gap-8">
          {!!reviewByUsernameData!.data!.length && (
            <ProfileReviewListContainer
              username={username}
              reviews={reviewByUsernameData!}
            />
          )}

          {!!wishlistData!.data!.length && (
            <ProfileWishlistContainer
              username={username}
              wishlist={wishlistData!}
            />
          )}

          {!!readlistData!.data!.length && (
            <ProfileReadlistContainer
              username={username}
              readlist={readlistData!}
            />
          )}
        </div>
      )}
    </div>
  );
};

export default Profile;
