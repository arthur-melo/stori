'use client';

import { ToastContainer as ToastContainerReactToastify } from 'react-toastify';

const ToastContainer = () => {
  return (
    <ToastContainerReactToastify
      position="bottom-right"
      autoClose={5000}
      hideProgressBar={false}
      newestOnTop={false}
      closeOnClick
      rtl={false}
      pauseOnFocusLoss
      draggable
      pauseOnHover
      theme="colored"
    />
  );
};

export default ToastContainer;
