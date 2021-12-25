import { ElementRef, Injectable } from "@angular/core";
import { animate, AnimationBuilder, style } from "@angular/animations";

@Injectable({
  providedIn: 'root'
})
export class SplashScreenService {
  private el: ElementRef;
  private stopped: boolean;

  constructor(private animationBuilder: AnimationBuilder) {}

  init(element: ElementRef) {
    this.el = element;
  }

  hide() {
    if (this.stopped || !this.el) return;

    const player = this.animationBuilder
      .build([style({ opacity: 1 }), animate(800, style({ opacity: 0 }))])
      .create(this.el.nativeElement);

    player.onDone(() => {
      if (typeof this.el.nativeElement.remove === 'function') this.el.nativeElement.remove();
      else this.el.nativeElement.style.display = 'none !important';
      this.stopped = true;
    });

    setTimeout(() => player.play(), 1000);
  }
}
