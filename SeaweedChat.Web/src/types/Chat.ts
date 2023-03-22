export interface Message {
  id: string;
  text: string;
  ownerId: string;
  ownerUsername: string;
  createdAt: Date;
  editAt: Date;
}
export interface Chat {
  id: string;
  text: string;
  title: string;
}
