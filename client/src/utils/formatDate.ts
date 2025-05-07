import { format } from 'date-fns';

// Helper function to format an ISO date as `MM / DD / YYY`;
const formatDate = (isoDate: string) => {
  const date = new Date(isoDate);
  return format(date, 'MM / dd / yyyy');
};

export default formatDate;
