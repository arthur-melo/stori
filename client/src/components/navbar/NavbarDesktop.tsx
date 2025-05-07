'use client';

import { useState, type MouseEvent } from 'react';
import Link from 'next/link';
import { useTheme } from 'next-themes';

import { signout } from '@/lib/auth';

import TextItem from '@/components/navbar/TextItem';
import DropdownItem from '@/components/navbar/DropdownItem';
import Logo from '@/components/navbar/Logo';
import Icon from '@/components/shared/Icon';
import Avatar from '@/components/shared/Avatar';

import Moon from '@/public/assets/icons/Moon.svg';
import Sun from '@/public/assets/icons/Sun.svg';
import Profile from '@/public/assets/icons/Profile.svg';
import Signout from '@/public/assets/icons/Signout.svg';

import { useBoundStore } from '@/providers/boundStoreProvider';

const NavbarDesktop = () => {
  const { setUser } = useBoundStore(state => state);
  const { theme, setTheme } = useTheme();
  const [disableBlur, setDisableBlur] = useState(false);

  const [showContextMenu, setShowContextMenu] = useState(false);
  const user = useBoundStore(state => state.user);

  const handleToggleTheme = () =>
    setTheme(theme === 'light' ? 'dark' : 'light');

  const handleToggleMenu = () => setShowContextMenu(!showContextMenu);
  const handleSignoutClick = async (ev: MouseEvent<HTMLAnchorElement>) => {
    ev.preventDefault();
    await signout();
    setUser(undefined);
  };

  const handleOnBlur = () => {
    if (showContextMenu && !disableBlur) {
      setTimeout(() => setShowContextMenu(false), 0);
    }
  };

  return (
    <div
      className="z-30 my-4 grid w-full grid-cols-12 gap-x-6 px-20 md:px-10"
      onBlur={handleOnBlur}>
      <div className="col-span-full flex h-12 w-full justify-center">
        <div className="mr-auto flex items-center gap-4">
          <TextItem route="/catalog">Catalog</TextItem>
          <TextItem route="/about">About</TextItem>
        </div>
        <div className="absolute">
          <Link href={'/'}>
            <Logo />
          </Link>
        </div>

        <div className="flex items-center gap-4">
          {theme === 'light' ? (
            <button onClick={handleToggleTheme} className="cursor-pointer">
              <Icon
                className="text-tertiary-500 dark:text-tertiary-100"
                src={<Moon />}
                alt="Switch theme"
                size="xl"
              />
            </button>
          ) : (
            <button onClick={handleToggleTheme} className="cursor-pointer">
              <Icon
                className="text-tertiary-500 dark:text-tertiary-100"
                src={<Sun />}
                alt="Switch theme"
                size="xl"
              />
            </button>
          )}
          <button className="cursor-pointer" onClick={handleToggleMenu}>
            {user ? (
              <Avatar
                size="sm"
                name={user.name!}
                alt={`Avatar of user ${user!.username}`}
                src={
                  user!.profileImg &&
                  `${process.env.NEXT_PUBLIC_BACKEND_URL}/images/${user?.profileImg}`
                }
              />
            ) : (
              <Icon
                className="text-tertiary-500 dark:text-tertiary-100"
                src={<Profile />}
                alt="Profile"
                size="xl"
              />
            )}
          </button>
        </div>
      </div>
      {showContextMenu && (
        <div className="relative z-10 col-start-10 -col-end-1 bg-white pt-2 lg:col-start-8 dark:bg-black">
          <div className="border-b-tertiary-500 dark:border-b-tertiary-100 absolute top-0.5 right-4 h-0 w-0 border-r-8 border-b-8 border-l-8 border-transparent"></div>
          <div
            className="border-tertiary-500 dark:border-tertiary-100 relative flex flex-col overflow-hidden rounded-xl border shadow-2xl"
            onMouseEnter={() => setDisableBlur(true)}
            onMouseLeave={() => setDisableBlur(false)}
            onClick={handleToggleMenu}>
            {user ? (
              <>
                <DropdownItem
                  href={`/profile/${user.username}`}
                  icon={<Profile className="dark:text-tertiary-100" />}
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
              <DropdownItem
                href="/auth/signin"
                icon={<Signout className="dark:text-tertiary-100" />}
                alt="Sign in">
                Sign in / Sign up
              </DropdownItem>
            )}
          </div>
        </div>
      )}
    </div>
  );
};

export default NavbarDesktop;
