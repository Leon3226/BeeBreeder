import { Directive, ElementRef, OnInit } from "@angular/core";

@Directive({
  selector: "[header-less-tab]",
})
export class HeaderLessTabsDirective implements OnInit {
  constructor(private eleRef: ElementRef) {}

  ngOnInit(): void {
    this.eleRef.nativeElement.children[0].style.display = "none";
  }
}