'use client';

import { toast } from 'react-toastify';

// Helper hook to show a toast message when a backend endpoint fetch call fails.
const useToastWrapper = <T, A extends unknown[]>(
  callback: (...args: A) => Promise<T>,
  message = 'Error contacting the backend server, please reload the page.',
) => {
  const wrappedCallback = async (...args: A): Promise<T | undefined> => {
    try {
      return await callback(...args);
    } catch (error) {
      toast.error(message);
      console.log(error);
    }
  };

  return wrappedCallback;
};

export default useToastWrapper;
