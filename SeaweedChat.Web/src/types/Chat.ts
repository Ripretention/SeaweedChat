export interface Message {
  date: Date;
  text: string;
  author: string;
  direction: "from" | "to";
}
export interface ChatPreview {
  date: Date;
  text: string;
  author: string;
}
