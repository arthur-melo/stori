'use client';

import { ChangeEvent, useState, useEffect } from 'react';

import cn from '@/utils/classNames';

const ReviewInput = ({
  initialText,
  limit,
  onTextChange,
  onLimitReached,
}: Readonly<{
  initialText?: string;
  limit: number;
  onTextChange: (data: string) => void;
  onLimitReached: (data: boolean) => void;
}>) => {
  const [inputData, setInputData] = useState(initialText ?? '');
  const [totalCharacters, setTotalCharacters] = useState(
    initialText?.length ?? 0,
  );
  const [isOverLimit, setIsOverLimit] = useState(totalCharacters > limit);

  useEffect(() => {
    setInputData(initialText ?? '');
  }, [initialText]);

  useEffect(() => {
    setTotalCharacters(inputData.length);

    if (inputData.length > limit) {
      if (isOverLimit != true) {
        setIsOverLimit(true);
        onLimitReached(true);
      }
    } else {
      if (isOverLimit != false) {
        setIsOverLimit(false);
        onLimitReached(false);
      }
    }
  }, [inputData, setIsOverLimit, limit, onLimitReached, isOverLimit]);

  const handleInputChange = (ev: ChangeEvent<HTMLTextAreaElement>) => {
    onTextChange(ev.target.value);
    setInputData(ev.target.value);
  };

  return (
    <div
      className={cn(
        'flex h-64 max-h-64 w-full max-w-2xl flex-col items-end gap-2 rounded-2xl border p-4',
        isOverLimit
          ? 'border-secondary-500'
          : 'border-tertiary-500 dark:border-tertiary-100',
      )}>
      <textarea
        className="text-tertiary-500 dark:text-tertiary-100 h-full w-full resize-none text-base font-medium focus:outline-hidden"
        placeholder="Type your review..."
        value={initialText ?? 0}
        autoFocus
        onChange={handleInputChange}
      />
      <p
        className={cn(
          'text-base font-bold',
          isOverLimit
            ? 'text-secondary-500'
            : 'text-tertiary-500 dark:text-tertiary-100 opacity-50',
        )}>
        {totalCharacters}/{limit} characters
      </p>
    </div>
  );
};

export default ReviewInput;
