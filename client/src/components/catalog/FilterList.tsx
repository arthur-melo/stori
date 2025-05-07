'use client';

import { useState } from 'react';
import dynamic from 'next/dynamic';

import breakpoints from '@/styles/breakpoints';

import type { paths } from '@/typings/api';

import Button from '@/components/shared/Button';
import FilterListDropdown from '@/components/catalog/FilterListDropdown';

import Filter from '@/public/assets/icons/Filter.svg';

const MediaQuery = dynamic(() => import('react-responsive'), {
  ssr: false,
});

const FilterList = ({
  searchParams = {},
}: {
  searchParams?: paths['/api/v1/books']['get']['parameters']['query'];
}) => {
  const [showFilterlistDropdown, setShowFilterlistDropdown] = useState(
    Object.keys(searchParams).length === 0 ? false : true,
  );

  const handleToggleFilterlist = () =>
    setShowFilterlistDropdown(!showFilterlistDropdown);

  return (
    <>
      <div>
        <MediaQuery maxWidth={breakpoints.sm}>
          {(isSM: boolean) => (
            <Button
              onClick={handleToggleFilterlist}
              size={isSM ? 'sm' : 'md'}
              icon={<Filter />}
              intent="primary">
              Filters
            </Button>
          )}
        </MediaQuery>
      </div>

      {showFilterlistDropdown && (
        <div className="flex w-full flex-col gap-4">
          <FilterListDropdown searchParams={searchParams} />
        </div>
      )}
    </>
  );
};

export default FilterList;
