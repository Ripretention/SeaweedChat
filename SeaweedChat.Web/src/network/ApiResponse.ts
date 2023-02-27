export class ApiResponse<T> {
  constructor(public readonly status: number, public readonly data: T) {}

  public get ok() {
    let code = this.status / 100;
    return code != 4 && code != 5;
  }
}
