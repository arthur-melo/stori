import { twMerge } from 'tailwind-merge';
import { cx } from 'class-variance-authority';
import type { ClassValue } from 'clsx';

// Helper function to merge CLSX's classes into tailwind-merge, avoiding naming conflicts.
const classNames = (...inputs: ClassValue[]) => twMerge(cx(inputs));

export default classNames;
