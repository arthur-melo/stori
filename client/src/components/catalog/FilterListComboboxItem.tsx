'use client';

import { useState } from 'react';

import { ListItemInputChange } from '@/typings/components';
import Icon from '@/components/shared/Icon';

import DownArrow from '@/public/assets/icons/DownArrow.svg';
import UpArrow from '@/public/assets/icons/UpArrow.svg';

// Helper function to capitalize the first letter of a given word.
const capitalizeFirstLetter = (word: string) =>
  word
    .split('')
    .map((char, idx) => (idx === 0 ? char.toUpperCase() : char))
    .join('');

const FilterListComboboxItem = ({
  label,
  value,
  searchParamName,
  options = ['Item'],
  onInputChange = () => null,
}: Readonly<{
  label: string;
  searchParamName: string;
  value?: string;
  error?: string;
  options: Array<string>;
  onInputChange: (params: ListItemInputChange) => void;
}>) => {
  const [inputValue, setInputValue] = useState(
    capitalizeFirstLetter(value || options[0]),
  );
  const [showDropdown, setShowDropdown] = useState(false);

  const handleToggleDropdown = () => setShowDropdown(!showDropdown);

  const handleDropdownClick = (value: string) => {
    setInputValue(value);
    handleToggleDropdown();
    onInputChange({ searchParamName, value: value.toLowerCase() });
  };

  return (
    <div className="border-tertiary-500 dark:border-tertiary-100 relative flex min-h-12 w-full items-center rounded-[4px] border">
      <button
        onClick={handleToggleDropdown}
        className="flex h-full w-full items-center px-2">
        <input
          id={label}
          name={searchParamName}
          type="text"
          value={inputValue}
          disabled
          className="text-tertiary-500 dark:text-tertiary-100 pointer-events-none w-full rounded-[4px] [background-color:transparent]"
        />
        <div>
          {showDropdown ? (
            <Icon
              src={<UpArrow />}
              className="text-tertiary-500 dark:text-tertiary-100"
              size="md"
            />
          ) : (
            <Icon
              src={<DownArrow />}
              className="text-tertiary-500 dark:text-tertiary-100"
              size="md"
            />
          )}
        </div>
      </button>

      {showDropdown && (
        <div className="absolute top-full bottom-0 left-0 z-20 w-full pt-1">
          <div className="border-tertiary-500 dark:border-tertiary-100 flex w-full flex-col items-center justify-start overflow-hidden rounded-[4px] border bg-white dark:bg-black">
            {options.map((item, idx) => (
              <button
                className="h-full min-h-12 w-full hover:bg-neutral-200 dark:hover:bg-neutral-400"
                onClick={() => handleDropdownClick(item)}
                key={idx}>
                <p className="text-tertiary-500 dark:text-tertiary-100 flex items-center px-2 text-base font-medium">
                  {item}
                </p>
              </button>
            ))}
          </div>
        </div>
      )}
    </div>
  );
};

export default FilterListComboboxItem;
