import { RoomModel } from './room.model';

export interface HomeModel {
  homeId: number;
  rooms: RoomModel[];
  strokedAreas?: {
    stroke : string ,
    id : number
  }[];
}
