'use client';

import { useRef, useState, useEffect } from 'react';

import Pill from '@/components/shared/Pill';
import ShowMore from '@/components/shared/ShowMore';

import useResizeObserver from '@/hooks/useResizeObserver';

const formatUrl = (route: string, param: string, value: string) => {
  const params = new URLSearchParams();
  params.append(param, value);

  return `${route}?${params.toString()}`;
};

const BookDetailDataList = ({
  title,
  searchParam,
  items,
}: {
  title: string;
  searchParam: string;
  items: string[];
}) => {
  const elementRef = useRef<HTMLDivElement>(null);
  const isHeightExceeded = useResizeObserver(elementRef, 100);
  const [showAllItems, setShowAllItems] = useState(true);

  useEffect(() => {
    setShowAllItems(!isHeightExceeded);
  }, [isHeightExceeded]);

  const handleShowAllItems = () => setShowAllItems(true);

  return (
    <div className="grid w-full grid-flow-row auto-rows-max grid-cols-subgrid gap-4">
      <p className="text-tertiary-500 dark:text-tertiary-100 col-span-full text-xl font-bold">
        {title}
      </p>
      <div
        ref={elementRef}
        style={
          isHeightExceeded && !showAllItems
            ? { maxHeight: `${100 + 1}px`, overflow: 'hidden' }
            : {}
        }
        className={
          'col-span-full flex max-w-full flex-wrap gap-2 overflow-hidden'
        }>
        {items!.map((item, idx) => (
          <Pill
            title={item}
            href={formatUrl('/catalog', searchParam, item)}
            key={idx}>
            {item}
          </Pill>
        ))}
      </div>
      {!showAllItems && <ShowMore onShowMore={handleShowAllItems} />}
    </div>
  );
};

export default BookDetailDataList;
