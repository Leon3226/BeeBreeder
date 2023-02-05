import { Inventory } from "./inventory";

export class TransposerWithInventories {
    id : string = "";
    name : string = "Unnamed";
    roofed : boolean = false;
    biome : string = "";
    flowers : string[] = [];
    inventories: Inventory[] = [];
}
