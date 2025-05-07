import { useState, useEffect } from 'react';

// eslint-disable-next-line @typescript-eslint/no-explicit-any
const useDebounce = (callback: any, delay: number) => {
  const [debounceValue, setDebounceValue] = useState(callback);
  useEffect(() => {
    const handler = setTimeout(() => {
      setDebounceValue(callback);
    }, delay);

    return () => {
      clearTimeout(handler);
    };
  }, [callback, delay]);

  return debounceValue;
};

export default useDebounce;
