import Link from 'next/link';

import SigninForm from '@/components/auth/SigninForm';
import Phone2 from '@/components/draws/Phone2';

const Signin = () => {
  return (
    <div className="mb-8 flex h-full w-full items-center justify-center">
      <div className="flex w-full justify-center gap-6 md:flex-col md:items-center">
        <div className="flex items-center">
          <Phone2 className="h-full max-w-full object-contain" />
        </div>

        <div className="flex w-3/12 flex-col gap-8 sm:w-full md:w-8/12">
          <div className="flex w-full flex-col gap-8">
            <p className="text-tertiary-500 dark:text-tertiary-100 text-3xl font-bold">
              Sign in
            </p>
            <div className="flex gap-2 md:flex-wrap">
              <p className="text-tertiary-500 dark:text-tertiary-100 text-xl font-normal">
                Don&apos;t have an account?
              </p>
              <Link
                href="/auth/signup"
                className="text-tertiary-500 dark:text-tertiary-100 text-xl font-bold underline">
                Sign up
              </Link>
            </div>
          </div>
          <div className="w-full">
            <SigninForm />
          </div>
        </div>
      </div>
    </div>
  );
};

export default Signin;
