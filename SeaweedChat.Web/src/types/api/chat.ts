export interface Chat {
  id: string;
  text: string;
  title: string;
  type: ChatType;
}
export enum ChatType {
  Chat,
  Channel,
}
export interface ChatCreateParams {
  title: string;
  type: ChatType;
}
