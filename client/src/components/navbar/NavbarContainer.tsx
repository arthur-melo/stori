'use client';

import { useEffect, useState } from 'react';
import { useMediaQuery } from 'react-responsive';

import breakpoints from '@/styles/breakpoints';

import NavbarMobile from '@/components/navbar/NavbarMobile';
import NavbarDesktop from '@/components/navbar/NavbarDesktop';

const NavbarContainer = () => {
  const [isClient, setIsClient] = useState(false);
  const isMD = useMediaQuery({ maxWidth: breakpoints.md });

  useEffect(() => {
    setIsClient(true);
  }, [isClient]);

  if (!isClient) {
    return;
  }

  return isMD ? <NavbarMobile /> : <NavbarDesktop />;
};
export default NavbarContainer;
