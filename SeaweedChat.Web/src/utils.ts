export const getRandomNumber = (min: number, max: number) =>
  Math.floor(Math.random() * (max - min + 1)) + min;
export function getRandomElement<T>(arr: T[]): T {
  return arr[getRandomNumber(0, arr.length - 1)];
}
export function prettyShortDate(d: Date): string {
  return [d.getHours(), d.getSeconds()]
    .map((s) => s.toString().padStart(2, "0"))
    .join(":");
}
export function prettyDate(d: Date): string {
  return `${prettyShortDate(d)} ${d.getDate()}.${
    d.getMonth() + 1
  }.${d.getFullYear()}`;
}
