import Link from 'next/link';

import Icon from '@/components/shared/Icon';
import Bench from '@/components/draws/Bench';

import Compare from '@/public/assets/icons/Compare.svg';
import Mingcute from '@/public/assets/icons/Mingcute.svg';
import Book from '@/public/assets/icons/Book.svg';
import Palmtree from '@/public/assets/icons/Palmtree.svg';
import GitHub from '@/public/assets/icons/GitHub.svg';

const About = () => {
  return (
    <div className="mb-8 flex h-full w-full items-center justify-center">
      <div className="flex items-end gap-6 lg:flex-col lg:items-center">
        <div className="w-6/12 lg:w-full lg:max-w-lg">
          <Bench className="h-full max-w-full object-contain" />
        </div>
        <div className="flex w-6/12 flex-col gap-8 lg:w-full">
          <div className="flex flex-col gap-4">
            <p className="text-tertiary-500 dark:text-tertiary-100 w-full text-3xl font-bold">
              About
            </p>
            <p className="text-tertiary-500 dark:text-tertiary-100 w-full text-lg font-normal">
              Stori is a platform for book lovers to discover, track, and share
              their reading journey with a community of like-minded individuals.
              Whether you&apos;re an avid reader looking for recommendations, or
              a casual explorer wanting to keep track of your books, Stori
              provides a comprehensive and engaging experience that suits all
              needs.
            </p>
          </div>

          <div className="flex flex-col gap-4">
            <p className="text-tertiary-500 dark:text-tertiary-100 w-full text-lg font-normal">
              This website is open source and was built using modern JavaScript
              and C# technologies. The source code is available on GitHub and
              any feedback or bug reports are welcome. If you enjoy this app,
              please give it a star!
            </p>
            <p className="text-tertiary-500 dark:text-tertiary-100 w-full text-lg font-bold">
              Technologies
            </p>
            <div className="flex items-center gap-1">
              <div className="shrink-0">
                <Icon
                  src={<Compare />}
                  size="lg"
                  className="text-tertiary-500 dark:text-tertiary-100"
                />
              </div>

              <p className="text-tertiary-500 dark:text-tertiary-100 text-lg font-normal">
                Next.js 15 — ASP.NET Core 9 — SQL Server — EF Core
              </p>
            </div>
            <div className="flex items-center gap-1">
              <div className="shrink-0">
                <Icon
                  src={<Mingcute />}
                  size="lg"
                  className="text-tertiary-500 dark:text-tertiary-100"
                />
              </div>

              <p className="text-tertiary-500 dark:text-tertiary-100 text-lg font-normal">
                Design assets from Undraw and MingCute
              </p>
            </div>
            <div className="flex items-center gap-1">
              <div className="shrink-0">
                <Icon
                  src={<Book />}
                  size="lg"
                  className="text-tertiary-500 dark:text-tertiary-100"
                />
              </div>

              <p className="text-tertiary-500 dark:text-tertiary-100 text-lg font-normal">
                Book data from goodreads
              </p>
            </div>
            <div className="flex items-center gap-1">
              <div className="shrink-0">
                <Icon
                  src={<Palmtree />}
                  size="lg"
                  className="text-tertiary-500 dark:text-tertiary-100"
                />
              </div>

              <div className="flex items-center gap-1">
                <p className="text-tertiary-500 dark:text-tertiary-100 text-lg font-normal">
                  Project by
                </p>
                <Link
                  target="_blank"
                  href="https://github.com/arthur-melo"
                  className="text-tertiary-500 dark:text-tertiary-100 text-lg font-normal underline">
                  arthur-melo
                </Link>
              </div>
            </div>
            <div className="flex gap-1">
              <div className="shrink-0">
                <Icon
                  src={<GitHub />}
                  size="lg"
                  className="text-tertiary-500 dark:text-tertiary-100"
                />
              </div>

              <div className="flex gap-1">
                <p className="text-tertiary-500 dark:text-tertiary-100 text-lg font-normal">
                  Source code available on
                </p>
                <Link
                  target="_blank"
                  href="https://github.com/arthur-melo/stori"
                  className="text-tertiary-500 dark:text-tertiary-100 text-lg font-normal underline">
                  GitHub
                </Link>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default About;
