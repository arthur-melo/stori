'use client';

import { FC, InputHTMLAttributes, useState, type ChangeEvent } from 'react';

import cn from '@/utils/classNames';

interface InputProps extends InputHTMLAttributes<HTMLInputElement> {
  label: string;
  error?: string;
  initialValue?: string | undefined;
}

const Input: FC<InputProps> = ({
  label,
  initialValue = '',
  error,
  type = 'text',
  ...props
}) => {
  const [inputValue, setInputValue] = useState(initialValue);

  const handleInputChange = (ev: ChangeEvent<HTMLInputElement>) =>
    setInputValue(ev.target.value);

  return (
    <div className="flex w-full flex-col gap-1">
      <label
        htmlFor={label}
        className={cn(
          'text-lg font-bold',
          error ? 'text-error' : 'text-tertiary-500 dark:text-tertiary-100',
        )}>
        {label}
      </label>

      <div
        className={cn(
          'flex min-h-12 w-full rounded-[4px] border',
          error
            ? 'border-error'
            : 'border-tertiary-500 dark:border-tertiary-100',
        )}>
        <input
          id={label}
          name={label}
          type={type}
          value={inputValue}
          onChange={handleInputChange}
          className="text-tertiary-500 dark:text-tertiary-100 focus:outline-primary-500 dark:focus:outline-tertiary-100 w-full rounded-[4px] px-2 focus:outline-3"
          {...props}
        />
      </div>

      {error && <p className="text-error text-lg font-normal">{error}</p>}
    </div>
  );
};

export default Input;
