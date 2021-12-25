import { HomeService } from './../../modules/home/_services/home.service';
import { CameraService } from './../../modules/camera/camera.service';
import { AfterViewInit, Component, OnInit } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";

const options = {
  rootMargin: '0px',
  threshold: 0.25
}

@Component({
  selector: 'sh-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {
  observer: any;
  panels= [this.translate.instant('SETTINGS.ADDITIONAL_PANELS.DASHBOARD'),
    this.translate.instant('SETTINGS.ADDITIONAL_PANELS.YOUR_HOME'),
    this.translate.instant('SETTINGS.ADDITIONAL_PANELS.SCENARIOS'),
    this.translate.instant('SETTINGS.ADDITIONAL_PANELS.CAMERAS')];
  activePanel: string;
  sections: any;
  options : any[] = [];

  constructor(public home: HomeService, public translate : TranslateService, private camera: CameraService){}

  ngOnInit() {

    this.options = [
      {
        name : 'SETTINGS.ADDITIONAL_PANELS.DASHBOARD',
        value : 'Dashboard'
      },
      {
        name : 'SETTINGS.ADDITIONAL_PANELS.YOUR_HOME',
        value : 'Your home'
      },
      {
        name : 'SETTINGS.ADDITIONAL_PANELS.SCENARIOS',
        value : 'Scenarios'
      },
      {
        name : 'SETTINGS.ADDITIONAL_PANELS.CAMERAS',
        value : 'Cameras'
      },
      {
        name : 'SETTINGS.ADDITIONAL_PANELS.MUSIC',
        value : 'Music'
      }
    ]

    this.sections = document.querySelectorAll('section');
    this.observer = new IntersectionObserver(this.callback, options);

    this.sections.forEach((section, index) => {
      this.observer.observe(section)
    })

    setTimeout(() => {
      this.activate("Dashboard");
    }, 500);
  }

  callback = (entries) => {
    entries.forEach((entry) => {

      if (entry.intersectionRatio >= 0.25) {
        this.activePanel = entry.target.getAttribute('panel-data')
        if (this.activePanel !== this.translate.instant('SETTINGS.ADDITIONAL_PANELS.CAMERAS')) {
          this.camera.pause();
        } else {
          this.camera.play();
        }
      }
    })
  }

  activate(panel: string) {
    this.activePanel = panel
    let p;
    for (let i = 0; i < this.sections.length; i++) {
      if (this.sections[i].getAttribute('panel-data') === panel) p = this.sections[i];
    }
    p.scrollIntoView();
    window.scrollTo(0, 0);
  }
}
