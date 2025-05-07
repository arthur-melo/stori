import Icon from '@/components/shared/Icon';

import Spinner from '@/public/assets/icons/Spinner.svg';

const Loading = () => {
  return (
    <div className="flex h-full w-full items-center justify-center">
      <Icon
        className="text-tertiary-500 dark:text-tertiary-100 animate-spin"
        src={<Spinner />}
        size="xl"
        alt="Loading spinner"
      />
    </div>
  );
};

export default Loading;
