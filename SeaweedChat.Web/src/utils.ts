export const getRandomNumber = (min: number, max: number) =>
  Math.floor(Math.random() * (max - min + 1)) + min;
export function getRandomElement<T>(arr: T[]): T {
  return arr[getRandomNumber(0, arr.length - 1)];
}
