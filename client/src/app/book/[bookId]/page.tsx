import { notFound } from 'next/navigation';

import getBook from '@/actions/getBook';
import getBookReview from '@/actions/getBookReview';

import BookDetail from '@/components/book/BookDetail';

const BookDetails = async ({
  params,
}: {
  params: Promise<{ bookId: string }>;
}) => {
  const currentBookId = (await params).bookId;
  const { data: bookData, error: bookError } = await getBook(currentBookId);
  const book = bookData?.data?.at(0);

  if (!bookData || !book || bookError) {
    return notFound();
  }

  const { data: reviewData, error: reviewError } = await getBookReview(book.id);

  if (!reviewData || reviewError) {
    return notFound();
  }

  return <BookDetail book={book} reviews={reviewData} />;
};

export default BookDetails;
