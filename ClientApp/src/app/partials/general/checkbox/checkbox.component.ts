import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';

@Component({
  selector: 'sh-checkbox',
  templateUrl: './checkbox.component.html',
  styleUrls: ['./checkbox.component.scss'],
})
export class CheckBoxComponent implements OnInit {
  @Input() text: string;
  @Input() disabled: boolean;
  @Input() checked: boolean;
  @Output() onChange = new EventEmitter<boolean>();
  value: boolean;

  ngOnInit() {
    this.value = this.checked;
  }

  set() {
    this.onChange.emit(this.value);
  }
}
