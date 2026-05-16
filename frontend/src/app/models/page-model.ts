export default interface PageModel<T> {
  data: T[];
  totalCount: number;
  hasNextPage: boolean;
  hasPrevPage: boolean;
}
