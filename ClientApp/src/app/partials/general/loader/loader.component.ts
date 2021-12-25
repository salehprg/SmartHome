import { Component, Input } from '@angular/core';

@Component({
  selector: 'sh-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.scss']
})
export class LoaderComponent {
  @Input('class') class: string;
}
