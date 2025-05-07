'use client';

import { useState, type MouseEvent } from 'react';
import Link from 'next/link';
import { useTheme } from 'next-themes';
import { useMediaQuery } from 'react-responsive';

import { signout } from '@/lib/auth';

import DropdownItem from '@/components/navbar/DropdownItem';
import Logo from '@/components/navbar/Logo';
import Icon from '@/components/shared/Icon';
import Avatar from '@/components/shared/Avatar';

import Moon from '@/public/assets/icons/Moon.svg';
import Sun from '@/public/assets/icons/Sun.svg';
import Signout from '@/public/assets/icons/Signout.svg';
import Menu from '@/public/assets/icons/Menu.svg';
import Close from '@/public/assets/icons/Close.svg';
import BookmarkFull from '@/public/assets/icons/BookmarkFull.svg';
import Mountain from '@/public/assets/icons/Mountain.svg';

import { useBoundStore } from '@/providers/boundStoreProvider';
import breakpoints from '@/styles/breakpoints';

const NavbarMobile = () => {
  const isSM = useMediaQuery({ maxWidth: breakpoints.sm });
  const [showContextMenu, setShowContextMenu] = useState(false);
  const { setUser } = useBoundStore(state => state);
  const { theme, setTheme } = useTheme();
  const user = useBoundStore(state => state.user);

  const handleToggleTheme = () =>
    setTheme(theme === 'light' ? 'dark' : 'light');

  const handleSignoutClick = async (ev: MouseEvent<HTMLAnchorElement>) => {
    ev.preventDefault();
    await signout();
    setUser(undefined);
  };

  const handleOnBlur = () => {
    if (showContextMenu) {
      setTimeout(() => setShowContextMenu(false), 0);
    }
  };

  return (
    <div
      className="my-4 grid w-full grid-cols-12 gap-x-6 px-10"
      onBlur={handleOnBlur}>
      <div className="col-span-full flex h-12 w-full justify-center">
        <div className="mr-auto">
          <Link href={'/'}>
            <Logo />
          </Link>
        </div>

        <div className="flex items-center gap-4">
          {!showContextMenu ? (
            <button
              onClick={() => setShowContextMenu(true)}
              className="cursor-pointer">
              <Icon
                className="text-tertiary-500 dark:text-tertiary-100"
                src={<Menu />}
                alt="Open menu"
                size={isSM ? 'lg' : 'xl'}
              />
            </button>
          ) : (
            <button
              onClick={() => setShowContextMenu(false)}
              className="cursor-pointer">
              <Icon
                className="text-tertiary-500 dark:text-tertiary-100"
                src={<Close />}
                alt="Close menu"
                size={isSM ? 'lg' : 'xl'}
              />
            </button>
          )}
        </div>
      </div>
      {showContextMenu && (
        <div className="z-10 col-span-full pt-2">
          <div className="border-tertiary-500 dark:border-tertiary-100 flex flex-col overflow-hidden rounded-xl border shadow-2xl dark:bg-black">
            <DropdownItem
              href={'/catalog'}
              icon={<BookmarkFull className="dark:text-tertiary-100" />}
              alt="View book catalog page">
              Catalog
            </DropdownItem>
            <DropdownItem
              href={'/about'}
              icon={<Mountain className="dark:text-tertiary-100" />}
              alt="View about page">
              About
            </DropdownItem>
            {theme === 'light' ? (
              <DropdownItem
                onClick={handleToggleTheme}
                icon={<Moon className="dark:text-tertiary-100" />}
                alt="Toggle theme to light mode">
                Change theme
              </DropdownItem>
            ) : (
              <DropdownItem
                onClick={handleToggleTheme}
                icon={<Sun className="dark:text-tertiary-100" />}
                alt="Toggle theme to dark mode">
                Change theme
              </DropdownItem>
            )}
            {user ? (
              <>
                <DropdownItem
                  href={`/profile/${user.username}`}
                  icon={
                    <Avatar
                      size="xs"
                      name={user.name!}
                      alt={`Avatar of user ${user!.username}`}
                      src={
                        user!.profileImg &&
                        `${process.env.NEXT_PUBLIC_BACKEND_URL}/images/${user?.profileImg}`
                      }
                    />
                  }
                  alt="View profile">
                  View profile
                </DropdownItem>
                <DropdownItem
                  onClick={handleSignoutClick}
                  icon={<Signout className="dark:text-tertiary-100" />}
                  alt="Sign out">
                  Sign out
                </DropdownItem>
              </>
            ) : (
              <>
                <DropdownItem
                  href="/auth/signin"
                  icon={<Signout className="dark:text-tertiary-100" />}
                  alt="Sign in">
                  Sign in / Sign up
                </DropdownItem>
              </>
            )}
          </div>
        </div>
      )}
    </div>
  );
};

export default NavbarMobile;
