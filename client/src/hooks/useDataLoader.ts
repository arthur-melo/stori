'use client';

import { useEffect, useState } from 'react';
import { toast } from 'react-toastify';

export type GenericPaginatedListEnvelope<T> = {
  data: Array<T>;
  /** Format: int32 */
  pageNumber: number;
  /** Format: int32 */
  totalPages: number;
  /** Format: int32 */
  totalItems: number;
  readonly hasPreviousPage: boolean;
  readonly hasNextPage: boolean;
};

export type GenericPaginatedListEnvelopeRequestParams = {
  pageSize: number;
  pageNumber: number;
};

const useDataLoader = <T>(
  initialData: GenericPaginatedListEnvelope<T> | null,
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  callback: any,
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  requestParams?: any,
) => {
  const [data, setData] = useState(initialData?.data ?? []);
  const [pageNumber, setPageNumber] = useState(initialData?.pageNumber ?? 0);
  const [hasNextPage, setHasNextPage] = useState(
    initialData?.hasNextPage ?? null,
  );
  const [error, setError] = useState<unknown>(undefined);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    if (error) {
      toast.error('Error fetching content, please reload the page.');
      console.log(error);
      setError(undefined);
    }
  }, [error]);

  useEffect(() => {
    setData(initialData?.data ?? []);
    setPageNumber(initialData?.pageNumber ?? 0);
    setHasNextPage(initialData?.hasNextPage ?? null);
    setError(undefined);
  }, [initialData]);

  const loadMoreData = async () => {
    if (isLoading) {
      return;
    }

    setIsLoading(true);
    if (hasNextPage || hasNextPage === null) {
      const nextPageParams: GenericPaginatedListEnvelopeRequestParams = {
        ...requestParams,
        pageNumber: pageNumber + 1,
      };

      try {
        const { data, error } = await callback(nextPageParams);

        if (error) {
          setError(error);
          return;
        }

        setData(previousData => [...previousData, ...(data!.data || [])]);
        setPageNumber(pageNumber + 1);
        setHasNextPage(data!.hasNextPage);
      } catch (error) {
        setError(error);
        return;
      }
    } else {
      setHasNextPage(false);
    }
    setIsLoading(false);
  };

  return { loadMoreData, data, hasNextPage, error };
};

export default useDataLoader;
