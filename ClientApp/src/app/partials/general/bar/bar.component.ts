import { Component, EventEmitter, Input, Output } from "@angular/core";

@Component({
  selector: 'sh-bar',
  templateUrl: './bar.component.html'
})
export class BarComponent {
  @Input() options: string[];
  @Input() active: string;
  @Output() activate = new EventEmitter<string>();

  set(option: string) {
    this.activate.emit(option);
  }
}
