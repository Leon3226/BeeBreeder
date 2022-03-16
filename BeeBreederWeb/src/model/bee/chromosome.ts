import {Gene} from "./gene";

export class Chromosome {
  property : string;
  primary : Gene;
  secondary: Gene;
  clean : boolean = false;
  resultantAttribute: string;
}
