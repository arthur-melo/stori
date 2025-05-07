'use server';

import { components } from '@/typings/api';

import patchUser from '@/actions/patchUser';
import postUserImageUpload from '@/actions/postUserImageUpload';
import deleteUserProfileImage from '@/actions/deleteUserProfileImage';

async function patchUserForm(
  user: components['schemas']['UserAuthorizedResponse'],
  _: unknown,
  formData: FormData,
) {
  if (!user) {
    return null;
  }

  const rawFormData = {
    email: formData.get('Email')?.toString(),
    name: formData.get('Name')?.toString(),
    password: formData.get('Password')?.toString(),
    username: formData.get('Username')?.toString(),
    profileImg: formData.get('ProfileImg') as File | undefined,
    clearProfileImg: formData.get('ClearProfileImg')?.toString(),
  };

  if (rawFormData.email === user.email) delete rawFormData.email;
  if (rawFormData.name === user.name) delete rawFormData.name;
  if (rawFormData.username === user.username) delete rawFormData.username;
  if (rawFormData?.profileImg?.size === 0) delete rawFormData.profileImg;
  if (rawFormData.clearProfileImg === '') delete rawFormData.clearProfileImg;

  const allKeyValuesUndefinedOrNull = Object.values(rawFormData).every(
    value => value === undefined || value === null,
  );

  if (allKeyValuesUndefinedOrNull) {
    return null;
  }

  const resultPatchUser = await patchUser(
    user.username!,
    rawFormData.email,
    rawFormData.password,
    rawFormData.username,
    rawFormData.name,
  );

  let resultUserImage;
  if (rawFormData.clearProfileImg || rawFormData.profileImg) {
    const shouldClear = rawFormData.clearProfileImg === 'true';

    if (shouldClear) {
      resultUserImage = await deleteUserProfileImage(user.username!);
    } else {
      resultUserImage = await postUserImageUpload(
        user.username!,
        rawFormData.profileImg!,
      );
    }
  }

  const errors = {
    ...resultPatchUser?.error?.errors,
    ...resultUserImage?.error?.errors,
  };

  if (errors) {
    return errors;
  }
}

export default patchUserForm;
