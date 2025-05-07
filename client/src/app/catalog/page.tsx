import { notFound } from 'next/navigation';

import getBooks from '@/actions/getBooks';

import List from '@/components/catalog/BookList';
import FilterList from '@/components/catalog/FilterList';
import Houses1 from '@/components/draws/Houses1';

const Catalog = async ({
  searchParams,
}: {
  searchParams: Promise<{ [key: string]: string | string[] }>;
}) => {
  const routeSearchParams = await searchParams;
  const { data: bookData, error } = await getBooks(routeSearchParams);

  if (!bookData || error) {
    return notFound();
  }

  return (
    <div className="flex h-full w-full flex-col gap-8 pt-4 sm:pt-0">
      <div className="flex w-full flex-wrap items-center justify-between gap-4">
        <p className="text-tertiary-500 dark:text-tertiary-100 shrink-0 text-3xl font-bold sm:text-2xl">
          Books ({bookData.totalItems.toLocaleString() ?? 0})
        </p>
        <FilterList searchParams={routeSearchParams} />
      </div>

      {bookData.data.length === 0 ? (
        <div className="flex h-full w-full items-center justify-center">
          <div className="flex flex-col items-end gap-11">
            <Houses1 className="h-full max-w-full object-contain" />
            <div className="flex flex-col items-end gap-4">
              <p className="text-tertiary-500 dark:text-tertiary-100 text-4xl font-bold">
                No book found
              </p>
              <p className="text-tertiary-500 dark:text-tertiary-100 text-end text-2xl font-normal">
                No book with the given filters could be found.
              </p>
            </div>
          </div>
        </div>
      ) : (
        <div className="mb-8 grid w-full grid-cols-[repeat(auto-fill,326px)] justify-center gap-6 sm:grid-cols-[repeat(auto-fill,minmax(108px,326px))]">
          <List
            initialBooks={bookData}
            searchParams={routeSearchParams}
            key={JSON.stringify(routeSearchParams)}
          />
        </div>
      )}
    </div>
  );
};

export default Catalog;
