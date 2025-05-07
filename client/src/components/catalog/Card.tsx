import StarsRating from '@/components/shared/StarsRating';
import Image from '@/components/shared/Image';

import formatDate from '@/utils/formatDate';

const Card = ({
  title,
  publishDate,
  description,
  starsAverage = 0,
  starsTotal = 0,
  imageUrl,
}: Readonly<{
  title: string;
  description?: string;
  publishDate?: string;
  starsAverage?: number;
  starsTotal?: number;
  imageUrl: string;
}>) => {
  return (
    <div className="group relative flex w-full flex-col content-center items-center gap-4 px-8 py-6">
      <div className="bg-tertiary-500 dark:bg-tertiary-100 absolute top-0 left-0 -z-10 h-full w-full rounded-[32px] opacity-5"></div>

      <div className="relative h-full w-full">
        <div className="aspect-1/1.5 relative w-full">
          <Image src={imageUrl} alt={`Book cover image for ${title}`} fill />
        </div>

        {description && (
          <>
            <div className="absolute top-0 left-0 z-10 hidden h-full w-full bg-neutral-200 opacity-95 group-hover:block dark:bg-neutral-400"></div>
            <div className="absolute top-0 left-0 z-10 hidden h-full w-full rounded-[32px] group-hover:block">
              <div className="flex h-full w-full flex-col content-end items-end gap-4 p-2">
                {publishDate && (
                  <p className="text-tertiary-500 dark:text-tertiary-100 text-base font-light">
                    Publish date: {formatDate(publishDate)}
                  </p>
                )}

                <p
                  className={`text-tertiary-500 dark:text-tertiary-100 max-w-full text-base font-medium ${publishDate ? 'line-clamp-13' : 'line-clamp-15'}`}>
                  {description}
                </p>
              </div>
            </div>
          </>
        )}
      </div>

      <div className="z-20 flex h-full w-full flex-col gap-2">
        <p className="text-tertiary-500 dark:text-tertiary-100 w-full overflow-hidden text-2xl font-bold text-nowrap text-ellipsis sm:text-xl">
          {title}
        </p>
        <div className="flex w-full gap-2 sm:flex-wrap">
          <div className="mr-auto flex">
            <StarsRating
              value={starsAverage}
              size="md"
              className="text-secondary-500"
            />
          </div>
          <p className="text-tertiary-500 dark:text-tertiary-100 overflow-hidden text-base font-light text-nowrap text-ellipsis">
            Votes:{' '}
            <span className="font-medium">
              {starsTotal === 0 ? 'N/A' : starsTotal.toLocaleString()}
            </span>
          </p>
        </div>
      </div>
    </div>
  );
};

export default Card;
