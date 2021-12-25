import { ModuleModel } from './module.model';

export interface LampModel extends ModuleModel {
  isOn: boolean;
  status : string;
}
