'use client';

import { ReactNode } from 'react';
import { createPortal } from 'react-dom';

const Modal = ({
  onClose = () => null,
  children,
}: Readonly<{
  onClose: () => void;
  children: ReactNode;
}>) => {
  return createPortal(
    <div className="align-center absolute inset-0 flex justify-center">
      <div className="fixed h-full w-full">
        <div
          onClick={onClose}
          className="absolute inset-0 bg-black opacity-50 dark:bg-white"></div>
        <div className="flex h-full w-full flex-col items-center justify-center">
          <div className="border-tertiary-500 z-50 rounded-lg border bg-white dark:bg-black">
            {children}
          </div>
        </div>
      </div>
    </div>,
    document.body,
  );
};

export default Modal;
