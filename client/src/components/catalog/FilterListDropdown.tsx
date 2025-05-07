import FilterListContainer from '@/components/catalog/FilterListContainer';
import FilterListTextItem from '@/components/catalog/FilterListTextItem';
import FilterListComboboxItem from '@/components/catalog/FilterListComboboxItem';

import type { paths } from '@/typings/api';

import { useRouter } from 'next/navigation';
import { ListItemInputChange } from '@/typings/components';

const FilterListDropdown = ({
  searchParams,
}: {
  searchParams?: paths['/api/v1/books']['get']['parameters']['query'];
}) => {
  const router = useRouter();

  const handleInputChange = ({
    searchParamName,
    value,
  }: ListItemInputChange) => {
    const url = new URL(window.location.href);

    if (!value) {
      url.searchParams.delete(searchParamName);
    } else {
      url.searchParams.set(searchParamName, value);
    }

    router.push(url.toString());
  };

  return (
    <div className="grid w-full grid-cols-[repeat(auto-fit,minmax(326px,1fr))] gap-6 sm:flex sm:flex-col">
      <FilterListContainer label="Title">
        <FilterListTextItem
          label="Title"
          searchParamName="title"
          queryParamName="titles"
          value={searchParams?.title}
          onInputChange={handleInputChange}
        />
      </FilterListContainer>

      <FilterListContainer label="Genre">
        <FilterListTextItem
          label="Genre"
          searchParamName="genre"
          queryParamName="genres"
          value={searchParams?.genre}
          onInputChange={handleInputChange}
        />
      </FilterListContainer>

      <FilterListContainer label="Character">
        <FilterListTextItem
          label="Character"
          searchParamName="character"
          queryParamName="characters"
          value={searchParams?.character}
          onInputChange={handleInputChange}
        />
      </FilterListContainer>

      <FilterListContainer label="Setting">
        <FilterListTextItem
          label="Setting"
          searchParamName="setting"
          queryParamName="settings"
          value={searchParams?.setting}
          onInputChange={handleInputChange}
        />
      </FilterListContainer>

      <FilterListContainer label="Award">
        <FilterListTextItem
          label="Award"
          searchParamName="award"
          queryParamName="awards"
          value={searchParams?.award}
          onInputChange={handleInputChange}
        />
      </FilterListContainer>

      <FilterListContainer label="Order By">
        <FilterListComboboxItem
          label="Order By"
          searchParamName="orderBy"
          value={searchParams?.orderBy || 'rating'}
          options={['Rating', 'Date']}
          onInputChange={handleInputChange}
        />
      </FilterListContainer>
    </div>
  );
};

export default FilterListDropdown;
