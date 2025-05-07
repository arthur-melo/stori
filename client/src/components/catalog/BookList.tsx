'use client';

import { useEffect } from 'react';
import { useInView } from 'react-intersection-observer';
import Link from 'next/link';

import type { paths, components } from '@/typings/api';

import Card from '@/components/catalog/Card';

import getBooks from '@/actions/getBooks';

import useDataLoader from '@/hooks/useDataLoader';

const BookList = ({
  initialBooks,
  searchParams,
}: {
  searchParams?: paths['/api/v1/books']['get']['parameters']['query'];
  initialBooks: components['schemas']['BookListResponsePaginatedListEnvelope'];
}) => {
  const { loadMoreData, data, hasNextPage } = useDataLoader<
    components['schemas']['BookListResponse']
  >(initialBooks, getBooks, searchParams);
  const { ref, inView } = useInView();

  useEffect(() => {
    const loadMoreDataWrapper = async () => await loadMoreData();

    if (inView && hasNextPage) {
      loadMoreDataWrapper();
    }
  }, [inView, hasNextPage, loadMoreData]);

  const booksList = data.map((book, idx) => (
    <Link href={`/book/${book.bookId}`} key={idx}>
      <Card
        title={book.title}
        imageUrl={book.coverImg}
        publishDate={book.publishDate ?? undefined}
        description={book.description ?? undefined}
        starsAverage={book.rating?.starsAverage ?? undefined}
        starsTotal={book.rating?.starsTotal ?? undefined}
      />
    </Link>
  ));

  return (
    <>
      {booksList}
      {/* Intersection observer ref for infinite scroll */}
      <div ref={ref}></div>
    </>
  );
};

export default BookList;
