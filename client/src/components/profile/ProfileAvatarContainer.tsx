'use client';

import { useState } from 'react';

import type { components } from '@/typings/api';

import Avatar from '@/components/shared/Avatar';
import Button from '@/components/shared/Button';
import Modal from '@/components/shared/Modal';
import ProfileModalContent from '@/components/profile/ProfileModalContent';

import Edit from '@/public/assets/icons/Edit.svg';

import { useBoundStore } from '@/providers/boundStoreProvider';

const ProfileAvatarContainer = ({
  username,
  userData,
}: Readonly<{
  username: string;
  userData: components['schemas']['UserUnauthorizedResponse'];
}>) => {
  const [showModal, setShowModal] = useState(false);
  const handleEditProfileClick = () => setShowModal(true);
  const handleCloseModal = () => setShowModal(false);
  const user = useBoundStore(state => state.user);

  return (
    <div className="relative flex w-full justify-center sm:flex-col sm:items-end sm:gap-4">
      <div className="mx-auto flex flex-col items-center gap-2 sm:order-2">
        <div className="shrink-0">
          <Avatar
            size="lg"
            name={userData!.name!}
            alt={`Avatar of user ${userData!.username}`}
            src={
              userData!.profileImg &&
              `${process.env.NEXT_PUBLIC_BACKEND_URL}/images/${userData?.profileImg}`
            }
          />
        </div>

        <p className="text-tertiary-500 dark:text-tertiary-100 text-3xl font-bold">
          {userData?.name}
        </p>
        <p className="text-tertiary-500 dark:text-tertiary-100 text-3xl font-light">
          @{userData?.username}
        </p>
      </div>

      {user && user.username === username && (
        <div className="absolute top-0 right-0 sm:relative sm:w-1/2">
          <Button
            size="md"
            intent="secondary"
            icon={<Edit />}
            onClick={handleEditProfileClick}>
            Edit profile
          </Button>
        </div>
      )}

      {showModal && (
        <Modal onClose={handleCloseModal}>
          <ProfileModalContent onClose={handleCloseModal} />
        </Modal>
      )}
    </div>
  );
};

export default ProfileAvatarContainer;
