import type { ReactNode } from 'react';

const FilterListContainer = ({
  label,
  error,
  children,
}: Readonly<{
  label: string;
  error?: string;
  children: ReactNode;
}>) => {
  return (
    <div className="flex w-full flex-col">
      <label
        htmlFor={label}
        className="text-tertiary-500 dark:text-tertiary-100 text-lg font-bold">
        {label}
      </label>

      {children}

      {error && <p className="text-error text-lg font-normal">{error}</p>}
    </div>
  );
};

export default FilterListContainer;
