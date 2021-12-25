import { WindoorModel } from './windoor.model';
import { CurtainModel } from './curtain.model';
import { LampModel } from './lamp.model';

export interface RoomModel {
  roomId: string;
  name: {
    text: string;
    x: number;
    y: number;
  };
  // area: {
  //   d: string;
  // };
  area : string;
  // border: {
  //   d: string;
  // };
  border : string ;
  devices :{
    leDs?: LampModel[],
    curtains?: CurtainModel[],
    windoors?: WindoorModel[]
  } ,
  roomDevices : any[];
}
