import Link from 'next/link';

import SignupForm from '@/components/auth/SignupForm';

import Phone1 from '@/components/draws/Phone1';

const Signup = () => {
  return (
    <div className="mb-8 flex h-full w-full items-center justify-center">
      <div className="flex w-full justify-center gap-6 md:flex-col md:items-center">
        <div className="flex items-center">
          <Phone1 className="h-full max-w-full object-contain" />
        </div>

        <div className="flex w-3/12 flex-col items-end gap-8 sm:w-full md:w-8/12">
          <div className="flex w-full flex-col gap-8">
            <p className="text-tertiary-500 dark:text-tertiary-100 text-3xl font-bold">
              Sign up
            </p>
            <div className="flex gap-2 md:flex-wrap">
              <p className="text-tertiary-500 dark:text-tertiary-100 text-xl font-normal">
                Already have an account?
              </p>
              <Link
                href="/auth/signin"
                className="text-tertiary-500 dark:text-tertiary-100 text-xl font-bold underline">
                Sign in
              </Link>
            </div>
          </div>
          <div className="w-full">
            <SignupForm />
          </div>
        </div>
      </div>
    </div>
  );
};

export default Signup;
