import { useState, useEffect, RefObject } from 'react';

const useResizeObserver = (
  elementRef: RefObject<HTMLElement | null>,
  threshold: number,
) => {
  const [isHeightExceeded, setIsHeightExceeded] = useState(false);

  useEffect(() => {
    const element = elementRef.current;
    const resizeObserver = new ResizeObserver(() => {
      if (element && element.offsetHeight > threshold) {
        setIsHeightExceeded(true);
      } else {
        setIsHeightExceeded(false);
      }
    });

    if (element) {
      resizeObserver.observe(element);
    }
    return () => {
      if (element) {
        resizeObserver.unobserve(element);
      }
    };
  });

  return isHeightExceeded;
};

export default useResizeObserver;
