export interface Message {
  date: Date;
  text: string;
  author: string;
  direction: "from" | "to";
}
export interface Chat {
  id: string;
  date: Date;
  text: string;
  author: string;
}
