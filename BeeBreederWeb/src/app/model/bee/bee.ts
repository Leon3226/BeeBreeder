import {Genotype} from "./genotype";
import {Chromosome} from "./chromosome";

export class Bee {
  public gender: boolean = false;
  public genotype: Genotype = new Genotype();
  public generation: number = 0;
  specieChromosome : any;
}
