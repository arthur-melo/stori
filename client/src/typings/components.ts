export enum FilterEndpoints {
  awards = 'awards',
  characters = 'characters',
  genres = 'genres',
  settings = 'settings',
  titles = 'titles',
}

export interface ListItemInputChange {
  searchParamName: string;
  value?: string;
}
