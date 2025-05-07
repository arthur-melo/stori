import Button from '@/components/shared/Button';

import RightArrowFull from '@/public/assets/icons/RightArrowFull.svg';
import Flower from '@/components/draws/Flower';

const Home = () => {
  return (
    <div className="flex h-full w-full items-center justify-center gap-6 md:flex-col md:content-center">
      <div className="max-w-96 shrink-0 sm:max-h-1/3 md:max-w-80">
        <Flower className="h-full max-w-full object-contain" />
      </div>
      <div className="flex flex-col items-end gap-8 md:gap-4">
        <div className="flex flex-col items-end justify-center gap-4">
          <p className="text-tertiary-500 dark:text-tertiary-100 text-5xl font-bold lg:text-4xl">
            Let your imagination flow
          </p>
          <p className="text-tertiary-500 dark:text-tertiary-100 text-3xl font-normal lg:text-2xl">
            Looking for a new book? We got you.
          </p>
        </div>

        <div className="block lg:hidden">
          <Button
            href="/catalog"
            size="lg"
            intent="primary"
            alt="Right arrow"
            icon={RightArrowFullProdBuildWrapper}>
            Explore
          </Button>
        </div>
        <div className="hidden lg:block">
          <Button
            href="/catalog"
            size="lg"
            intent="primary"
            alt="Right arrow"
            icon={RightArrowFullProdBuildWrapper}>
            Explore
          </Button>
        </div>
      </div>
    </div>
  );
};

// When running `npm build`, NextJS breaks if the `<RightArrowFull />` component is called twice.
// This only happens on this server component, I couldn't find the source of the error, but it might be
// related on how the SVGR plugin handles the file import on webpack and the internal SSR behavior.
const RightArrowFullProdBuildWrapper = <RightArrowFull />;

export default Home;
