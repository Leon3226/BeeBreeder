import {Genotype} from "./genotype";

export class Bee {
  public gender: boolean = false;
  public genotype: Genotype = new Genotype();
  public generation: number = 0;
}
