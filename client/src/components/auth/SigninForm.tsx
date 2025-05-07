'use client';

import { useRouter } from 'next/navigation';
import { useActionState } from 'react';

import postSignin from '@/actions/postSignin';

import useToastWrapper from '@/hooks/useToastWrapper';

import Input from '@/components/shared/Input';
import Button from '@/components/shared/Button';

import RightArrowFull from '@/public/assets/icons/RightArrowFull.svg';

import getAuthedUser from '@/actions/getAuthedUser';
import { useBoundStore } from '@/providers/boundStoreProvider';

const Submit = ({ pending = false }: Readonly<{ pending?: boolean }>) => {
  const router = useRouter();
  const { setUser } = useBoundStore(state => state);
  const getAuthedUserToastWrapper = useToastWrapper(
    getAuthedUser,
    'Error fetching the authed user data from the backend server.',
  );

  const handleClick = async (ev: React.MouseEvent<HTMLButtonElement>) => {
    ev.preventDefault();
    ev.currentTarget.form?.requestSubmit();
    const loggedUser = await getAuthedUserToastWrapper();

    if (!loggedUser) {
      return;
    }

    if (loggedUser?.data) {
      setUser(loggedUser.data!.data?.at(0));
      router.back();
    }
  };

  return (
    <Button
      icon={<RightArrowFull />}
      type="submit"
      disabled={pending}
      onClick={handleClick}>
      Sign in
    </Button>
  );
};

const SigninForm = () => {
  const postSigninWrapper = useToastWrapper(
    postSignin,
    'Error contacting the backend server.',
  );

  const [state, formAction, pending] = useActionState(postSigninWrapper, null);

  return (
    <form action={formAction} className="w-full">
      <div className="flex flex-col gap-8">
        {state?.detail && (
          <p className="text-error text-lg font-normal">{state.detail}</p>
        )}
        <div className="flex flex-col gap-2">
          <Input
            label="Email"
            type="email"
            autoComplete="email"
            error={state?.errors?.email?.toString()}
            required
          />
          <Input
            label="Password"
            type="password"
            autoComplete="new-password"
            error={state?.errors?.password?.toString()}
            required
          />
        </div>
        <div className="w-full">
          <Submit pending={pending} />
        </div>
      </div>
    </form>
  );
};

export default SigninForm;
