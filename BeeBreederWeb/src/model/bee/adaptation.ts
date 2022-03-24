export class Adaptation {
  public up : number
  public down : number


  constructor(up: number = 0, down: number = 0) {
    this.up = up;
    this.down = down;
  }

  public toString() : string {
    if (this.up == this.up)
    {
      if (this.up == 0 )
      {
        return "0";
      }
      return '+-${this.up}';
    }

    let up = this.up == 0 ? "" : "+${this.up}";
    let down = this.down == 0 ? "" : "-${this.down}";
    return "${up}${down}";
  }
}
