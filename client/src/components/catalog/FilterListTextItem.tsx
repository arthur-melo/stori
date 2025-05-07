'use client';

import {
  useRef,
  useState,
  useEffect,
  type ChangeEvent,
  type FocusEvent,
} from 'react';
import { useInView } from 'react-intersection-observer';
import { toast } from 'react-toastify';

import { ListItemInputChange } from '@/typings/components';
import useDebounce from '@/hooks/useDebounce';

import getEndpointDataList from '@/actions/getFilteredList';

import type { paths } from '@/typings/api';
import type { FilterEndpoints } from '@/typings/components';

type PaginatedListRequest =
  paths[`/api/v1/${FilterEndpoints}`]['get']['parameters']['query'];

const FilterListTextItem = ({
  label,
  value,
  searchParamName,
  queryParamName,
  onInputChange = () => null,
}: Readonly<{
  label: string;
  searchParamName: string;
  queryParamName: keyof typeof FilterEndpoints;
  value?: string;
  error?: string;
  onInputChange: (params: ListItemInputChange) => void;
}>) => {
  const [inputValue, setInputValue] = useState(value || '');
  const [searchResults, setSearchResults] = useState<string[]>([]);
  const previousInputValue = useRef(inputValue);
  const debounceValue = useDebounce(inputValue, 1000);

  const [showDropdown, setShowDropdown] = useState(false);

  // TODO: Port this to the useDataLoader hook.
  const [pageNumber, setPageNumber] = useState(0);
  const [hasNextPage, setHasNextPage] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<unknown | undefined>(undefined);
  const { ref, inView } = useInView();

  useEffect(() => {
    if (error) {
      toast.error('Error fetching content, please reload the page.');
      console.log(error);
      setError(undefined);
    }
  }, [error]);

  // TODO: Move to hook, listen to escape to close
  useEffect(() => {
    window.addEventListener('keydown', handleKeyDown);

    return () => {
      window.removeEventListener('keydown', handleKeyDown);
    };
  }, []);

  useEffect(() => {
    if (previousInputValue.current !== inputValue) {
      handleSearchData(queryParamName, debounceValue);
      onInputChange({ searchParamName, value: debounceValue });
    }
  }, [debounceValue]);

  const updateInputValue = (value: string) => {
    previousInputValue.current = inputValue;
    setInputValue(value);
  };

  const handleInputChange = (ev: ChangeEvent<HTMLInputElement>) =>
    updateInputValue(ev.target.value);

  const handleDropdownClick = (value: string) => {
    updateInputValue(value);
    setShowDropdown(false);
    onInputChange({ searchParamName, value });
  };

  // Populate the dropdown data from the external api
  const handleSearchData = async (
    endpoint: keyof typeof FilterEndpoints,
    query: string,
  ) => {
    try {
      const { data, error } = await getEndpointDataList(endpoint, {
        name: query,
      });

      if (!data || error) {
        setError(error);
        return;
      }

      setSearchResults(data!.data!);
      setHasNextPage(data!.hasNextPage!);
      setPageNumber(data!.pageNumber!);
    } catch (error) {
      setError(error);
    }
  };

  // Close dropdown on clicking outside the focused div
  const handleBlur = (e: FocusEvent<HTMLDivElement>) => {
    const currentTarget = e.currentTarget;

    requestAnimationFrame(() => {
      if (!currentTarget.contains(document.activeElement)) {
        setShowDropdown(false);
      }
    });
  };

  // Close dropdown on pressing ESC
  const handleKeyDown = (ev: KeyboardEvent) => {
    if (ev.key === 'Escape') {
      ev.preventDefault();
      setShowDropdown(false);
    }
  };

  // Scroll on dropdown infinite loader behavior:
  // TODO: Extract to hook, async load
  useEffect(() => {
    if (inView && hasNextPage && !isLoading) {
      setIsLoading(true);
      loadMoreItems();
      setIsLoading(false);
    }
  }, [inView, hasNextPage, isLoading]);

  const loadMoreItems = async () => {
    if (isLoading) {
      return;
    }

    setIsLoading(true);
    if (hasNextPage) {
      const nextPageParams: PaginatedListRequest = {
        name: inputValue,
        pageNumber: pageNumber + 1,
      };

      try {
        const { data, error } = await getEndpointDataList(
          queryParamName,
          nextPageParams,
        );

        if (!data || error) {
          setError(error);
          return;
        }

        setSearchResults(items => [...items, ...(data!.data || [])]);
        setHasNextPage(data!.hasNextPage!);
        setPageNumber(pageNumber + 1);
      } catch (error) {
        setError(error);
      }
    } else {
      setHasNextPage(false);
    }
    setIsLoading(false);
  };

  return (
    <div
      onBlur={handleBlur}
      className="border-tertiary-500 dark:border-tertiary-100 relative flex min-h-12 w-full rounded-[4px] border">
      <input
        id={label}
        name={searchParamName}
        type="text"
        value={inputValue}
        onClick={async () => {
          await handleSearchData(queryParamName, inputValue);
          setShowDropdown(true);
        }}
        onChange={handleInputChange}
        className="text-tertiary-500 dark:text-tertiary-100 focus:outline-primary-500 dark:focus:outline-tertiary-100 w-full rounded-[4px] px-2 focus:outline-3"
      />

      {showDropdown && (
        <div className="absolute top-full bottom-0 left-0 z-20 w-full pt-1">
          <div className="border-tertiary-500 dark:border-tertiary-100 flex max-h-44 w-full flex-col items-center justify-start overflow-auto rounded-[4px] border bg-white dark:bg-black">
            {searchResults.length === 0 ? (
              <button
                className="h-full min-h-12 w-full hover:bg-neutral-200 dark:hover:bg-neutral-400"
                disabled>
                <p className="text-tertiary-500 dark:text-tertiary-100 flex items-center overflow-hidden px-2 text-base font-medium text-nowrap text-ellipsis">
                  No items found
                </p>
              </button>
            ) : (
              <>
                {searchResults.map((item, idx) => (
                  <button
                    className="h-full min-h-12 w-full hover:bg-neutral-200 dark:hover:bg-neutral-400"
                    onClick={() => handleDropdownClick(item)}
                    key={idx}>
                    <p className="text-tertiary-500 dark:text-tertiary-100 flex items-center overflow-hidden px-2 text-base font-medium text-nowrap text-ellipsis">
                      {item}
                    </p>
                  </button>
                ))}
                <div ref={ref}></div>
              </>
            )}
          </div>
        </div>
      )}
    </div>
  );
};

export default FilterListTextItem;
