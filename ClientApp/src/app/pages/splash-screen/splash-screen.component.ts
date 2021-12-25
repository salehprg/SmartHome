import { Component, ElementRef, OnInit, ViewChild } from "@angular/core";
import { SplashScreenService } from './splash-screen.service';

@Component({
  selector: 'sh-splash-screen',
  templateUrl: './splash-screen.component.html',
  styleUrls: ['./splash-screen.component.scss']
})
export class SplashScreenComponent implements OnInit {
  @ViewChild('splashScreen', { static: true }) splashScreen: ElementRef;

  constructor(private splashScreenService: SplashScreenService) {}

  ngOnInit() {
    this.splashScreenService.init(this.splashScreen);
  }
}
