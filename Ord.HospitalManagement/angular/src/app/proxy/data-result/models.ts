export interface DataResult<T> {
  isOk: boolean;
  msg?: string;
  data: T;
  listData: T[];
  errorData: T[];
}
