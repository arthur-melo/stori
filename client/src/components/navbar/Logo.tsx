import BookLogo from '@/public/assets/icons/BookLogo.svg';

import Icon from '@/components/shared/Icon';

const Logo = () => {
  return (
    <div className="flex items-center gap-1">
      <Icon
        className="text-tertiary-500 dark:text-secondary-500"
        size="xl"
        alt="Stori logo"
        src={<BookLogo />}
      />
      <p className="text-tertiary-500 dark:text-tertiary-100 font-serif text-4xl font-bold italic">
        Stori
      </p>
    </div>
  );
};

export default Logo;
