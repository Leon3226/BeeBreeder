import { Computer } from "./computer";

export class Apiary {
  id: number = 0;
  name: string;
  description : string;
  computers : Computer [] = [];
  mods : number[] = [];
}
