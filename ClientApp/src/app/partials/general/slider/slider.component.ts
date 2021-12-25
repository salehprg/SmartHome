import { DomSanitizer } from '@angular/platform-browser';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { TranslationService } from 'src/app/modules/i18n/translation.service';

@Component({
  selector: 'sh-slider',
  templateUrl: './slider.component.html'
})
export class SliderComponent implements OnInit {
  @Input() value: number;
  @Output() valueChange = new EventEmitter<number>();
  @Output() onChange = new EventEmitter<undefined>();
  bg: any;

  constructor(
    private sanitizer: DomSanitizer,
    public translationService: TranslationService,
  ) {}

  ngOnInit() {
    this.slide();
  }

  slide(v = -1) {
    if (v != -1) {
      this.value = v;
    }
    this.bg = this.sanitizer.bypassSecurityTrustStyle(`linear-gradient(to right, var(--primary-color) 0%, var(--primary-color) ${this.value}%, var(--surface-0) ${this.value}%, var(--surface-0) 100%)`);
    this.valueChange.emit(this.value);
  }

  goRight() {
    this.value = 100;
    this.slide();
    this.onChange.emit();
   }

   goLeft() {
    this.value = 0;
    this.slide();
    this.onChange.emit();
   }
}
