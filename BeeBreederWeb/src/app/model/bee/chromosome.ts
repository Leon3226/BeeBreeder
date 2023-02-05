import {Gene} from "./gene";

export class Chromosome {
  key : string = "";
  value : {
    property : string;
    primary : Gene;
    secondary: Gene;
    clean : boolean;
    resultantAttribute: any;
    stringResultantAttribute: string;
  } = {property: "", primary: new Gene(), secondary: new Gene(), clean: false, resultantAttribute: "", stringResultantAttribute: "" };

}
