'use client';

import { useActionState, useEffect } from 'react';
import { useRouter } from 'next/navigation';

import postSignup from '@/actions/postSignup';

import Input from '@/components/shared/Input';
import Button from '@/components/shared/Button';

import RightArrowFull from '@/public/assets/icons/RightArrowFull.svg';

import useToastWrapper from '@/hooks/useToastWrapper';

const SignupForm = () => {
  const router = useRouter();

  const postSignupWrapper = useToastWrapper(
    postSignup,
    'Error contacting the backend server to save the user data.',
  );

  const [state, formAction, pending] = useActionState(postSignupWrapper, null);

  useEffect(() => {
    if (state === undefined) {
      router.push('/auth/signin');
      return;
    }
  }, [state, router]);

  return (
    <form action={formAction} className="w-full">
      <div className="flex flex-col gap-8">
        {state?.detail && (
          <p className="text-error text-lg font-normal">{state.detail}</p>
        )}
        <div className="flex flex-col gap-2">
          <Input
            label="Username"
            autoComplete="username"
            error={state?.errors?.username?.toString()}
            required
          />
          <Input
            label="Name"
            autoComplete="name"
            error={state?.errors?.name?.toString()}
            required
          />
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
          <Button icon={<RightArrowFull />} type="submit" disabled={pending}>
            Sign up
          </Button>
        </div>
      </div>
    </form>
  );
};

export default SignupForm;
