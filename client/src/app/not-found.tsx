import Houses2 from '@/components/draws/Houses2';

const NotFound = () => {
  return (
    <div className="flex h-full w-full items-center justify-center">
      <div className="flex flex-col items-end gap-11">
        <Houses2 className="h-full max-w-full object-contain" />
        <div className="flex flex-col items-end gap-4">
          <p className="text-tertiary-500 dark:text-tertiary-100 text-4xl font-bold">
            Page not found
          </p>
          <p className="text-tertiary-500 dark:text-tertiary-100 text-end text-2xl font-normal">
            The requested page does not exist.
          </p>
        </div>
      </div>
    </div>
  );
};

export default NotFound;
