import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ContainerImageService {

  constructor() { }

  //TODO: Refactor
  imageLink(containerName: string) : string | undefined {
    switch(containerName){
      case 'minecraft:chest':
        return 'assets/images/containers/chest.png';
      case 'forestry:apiary':
          return 'assets/images/containers/apiary.png';
        default:
          return 'assets/images/containers/unknown.png';
    }
  }
}
