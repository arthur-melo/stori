'use client';

import { useEffect, useState } from 'react';

import ReviewInput from '@/components/shared/ReviewInput';
import Button from '@/components/shared/Button';

import Send from '@/public/assets/icons/Send.svg';
import Close from '@/public/assets/icons/Close.svg';

const LIMIT = 1024;

const ReviewInputContainer = ({
  disabled = false,
  initialText,
  limit = LIMIT,
  onAddReview = () => null,
  onCancel = () => null,
}: Readonly<{
  disabled?: boolean;
  initialText?: string;
  limit?: number;
  onAddReview?: (text: string) => void;
  onCancel?: () => void;
}>) => {
  const [inputData, setInputData] = useState(initialText ?? '');
  const [isLimitReached, setIsLimitReached] = useState(false);

  useEffect(() => {
    setInputData(initialText ?? '');
  }, [initialText]);

  const handleInputChange = (data: string) => {
    setInputData(data);
  };

  const handleOnLimitReached = (data: boolean) => {
    setIsLimitReached(data);
  };

  const handleAddReview = () => {
    onAddReview(inputData);
    setInputData('');
  };

  return (
    <div className="flex w-full items-end justify-center gap-6 lg:flex-col">
      <ReviewInput
        initialText={inputData}
        limit={limit}
        onTextChange={handleInputChange}
        onLimitReached={handleOnLimitReached}
      />

      <div className="flex w-2/12 flex-col gap-4 lg:w-auto lg:flex-row">
        <Button
          size="md"
          intent="secondary"
          variant={'danger'}
          icon={<Close />}
          onClick={() => onCancel()}>
          Cancel
        </Button>
        <Button
          size="md"
          intent="primary"
          icon={<Send />}
          onClick={handleAddReview}
          disabled={isLimitReached || !inputData || disabled}>
          Submit
        </Button>
      </div>
    </div>
  );
};

export default ReviewInputContainer;
