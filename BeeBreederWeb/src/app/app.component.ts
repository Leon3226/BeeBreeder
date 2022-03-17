import {Component} from '@angular/core';
import {BeeStack} from "../model/bee/bee-stack";
import {Bee} from "../model/bee/bee";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'BeeBreederWeb';

  bees: BeeStack[] = [];
  selectedBee : Bee = new Bee();

  onBeeSelected(bee: Bee) : void{
    this.selectedBee = bee;
  }

  ngOnInit(): void {
    const fetchPromise = fetch("https://localhost:44327/ApiaryData");
    fetchPromise.then(response => {
      response.text().then(x => {
        let beeObj = JSON.parse(x);
        this.bees = beeObj.bees;
      })
    });
  }
}
