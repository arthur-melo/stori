const ShowMore = ({
  onShowMore = () => null,
}: Readonly<{ onShowMore: () => void }>) => (
  <div className="group col-span-full flex w-full justify-center">
    <button
      onClick={onShowMore}
      className="flex w-full cursor-pointer items-center justify-center gap-2 text-neutral-200 group-hover:text-neutral-300">
      <hr className="w-full rounded-full border-2" />
      <p className="shrink-0 text-lg font-light text-neutral-500 dark:text-neutral-200">
        Show more
      </p>
      <hr className="w-full rounded-full border-2 text-neutral-200 group-hover:text-neutral-300" />
    </button>
  </div>
);

export default ShowMore;
