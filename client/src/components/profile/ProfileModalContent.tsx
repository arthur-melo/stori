'use client';

import { useActionState, useState, useRef, useEffect } from 'react';
import { useRouter } from 'next/navigation';

import { useBoundStore } from '@/providers/boundStoreProvider';
import patchUserForm from '@/actions/patchUserForm';
import getAuthedUser from '@/actions/getAuthedUser';

import useToastWrapper from '@/hooks/useToastWrapper';

import Avatar from '@/components/shared/Avatar';
import Button from '@/components/shared/Button';
import Input from '@/components/shared/Input';

import Plus from '@/public/assets/icons/Plus.svg';
import Minus from '@/public/assets/icons/Minus.svg';
import Check from '@/public/assets/icons/Check.svg';
import Close from '@/public/assets/icons/Close.svg';

const Submit = ({ disabled = false }: Readonly<{ disabled?: boolean }>) => {
  const router = useRouter();
  const setUser = useBoundStore(state => state.setUser);
  const getAuthedUserToastWrapper = useToastWrapper(
    getAuthedUser,
    'Error contacting the server to save the user data.',
  );

  const handleClick = async (ev: React.MouseEvent<HTMLButtonElement>) => {
    ev.preventDefault();
    ev.currentTarget.form?.requestSubmit();

    const loggedUser = await getAuthedUserToastWrapper();

    if (loggedUser === undefined) {
      return;
    }

    if (!loggedUser) {
      setUser(undefined);
      router.push('/auth/signin');
      return;
    }

    if (loggedUser?.data) {
      setUser(loggedUser.data!.data?.at(0));
      router.push(`/profile/${loggedUser.data!.data?.at(0)!.username}`);
    }
  };

  return (
    <Button
      type="button"
      size="md"
      intent="primary"
      icon={<Check />}
      disabled={disabled}
      onClick={handleClick}>
      Save changes
    </Button>
  );
};

const ProfileModalContent = ({
  onClose = () => null,
}: Readonly<{
  onClose: () => void;
}>) => {
  const user = useBoundStore(state => state.user);
  const [file, setFile] = useState<string | null>(null);
  const [clearImage, setClearImage] = useState<boolean | undefined>(undefined);
  const fileInputRef = useRef<HTMLInputElement>(null);
  const [state, formAction, pending] = useActionState(
    patchUserForm.bind(null, user!),
    null,
  );

  useEffect(() => {
    resetImageState(undefined);
  }, [state?.profileImg]);

  const handleCancelClick = () => onClose();

  const handleUploadPhotoClick = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files) {
      setFile(URL.createObjectURL(e.target.files[0]));
    }
    setClearImage(false);
  };

  const resetImageState = (clear: boolean | undefined) => {
    setFile(null);
    setClearImage(clear);
    if (fileInputRef) {
      fileInputRef.current!.value = '';
    }
  };

  const handleRemovePhotoClick = () => resetImageState(true);

  return (
    <form action={formAction} className="flex flex-col gap-8 px-16 py-8">
      <div className="flex flex-col gap-2">
        <div className="flex justify-center gap-4 sm:flex-col sm:items-center">
          <Avatar
            size="lg"
            name={user?.name ?? ''}
            alt={`Avatar of user ${user?.username}`}
            src={
              clearImage
                ? null
                : file
                  ? file
                  : user?.profileImg &&
                    `${process.env.NEXT_PUBLIC_BACKEND_URL}/images/${user?.profileImg}`
            }
          />
          <div className="flex w-full flex-col justify-center gap-4">
            <Button
              as="label"
              htmlFor="image-upload"
              size="md"
              intent="primary"
              className=""
              icon={<Plus />}>
              Upload profile picture
              <input
                ref={fileInputRef}
                accept="image/png, image/jpeg"
                id="image-upload"
                name="ProfileImg"
                type="file"
                className="hidden"
                onChange={handleUploadPhotoClick}
              />
            </Button>
            <Button
              type="button"
              size="md"
              intent="secondary"
              variant="danger"
              icon={<Minus />}
              onClick={handleRemovePhotoClick}>
              Remove current photo
              <input
                type="hidden"
                name="ClearProfileImg"
                value={String(clearImage || '')}
              />
            </Button>
          </div>
        </div>
        {state?.profileImg && (
          <p className="text-error text-lg font-normal">
            {state?.profileImg.toString()}
          </p>
        )}
      </div>
      <div className="flex w-full flex-col gap-2">
        <Input
          label="Name"
          initialValue={user?.name ?? undefined}
          error={state?.name?.toString()}
        />
        <Input
          label="Email"
          initialValue={user?.email ?? undefined}
          error={state?.email?.toString()}
        />
        <Input
          label="Username"
          initialValue={user?.username ?? undefined}
          error={state?.username?.toString()}
        />
      </div>

      {state?.detail && (
        <p className="text-error text-lg font-normal">{state.detail}</p>
      )}

      <div className="flex justify-end gap-4 sm:flex-col">
        <div>
          <Button
            type="button"
            size="md"
            intent="secondary"
            variant="danger"
            icon={<Close />}
            onClick={handleCancelClick}>
            Cancel
          </Button>
        </div>

        <div>
          <Submit disabled={pending} />
        </div>
      </div>
    </form>
  );
};

export default ProfileModalContent;
