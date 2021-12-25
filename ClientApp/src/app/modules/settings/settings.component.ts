import { Component, OnInit } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";

const options = {
  rootMargin: '0px',
  threshold: 0.25
}

@Component({
  selector: 'sh-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit {
  observer: any;
  panels= [
    this.translate.instant('SETTINGS.ADDITIONAL_PANELS.GENERAL'),
    this.translate.instant('SETTINGS.ADDITIONAL_PANELS.HOME'), 
    this.translate.instant('SETTINGS.ADDITIONAL_PANELS.SCENARIOS'), 
    this.translate.instant('SETTINGS.ADDITIONAL_PANELS.CAMERAS'), 
    this.translate.instant('SETTINGS.ADDITIONAL_PANELS.ACTIVE_SESSIONS')];
  activePanel: string;
  sections: any;
  options : any[] = [];
  constructor(public translate : TranslateService){}

  ngOnInit() {
    this.sections = document.querySelectorAll('section');
    this.observer = new IntersectionObserver(this.callback, options);

    this.options = [
      {
        name : 'SETTINGS.ADDITIONAL_PANELS.GENERAL' ,
        value : 'General'
      },
      {
        name : 'SETTINGS.ADDITIONAL_PANELS.HOME', 
        value : 'Home'
      },
      {
        name : 'SETTINGS.ADDITIONAL_PANELS.SCENARIOS', 
        value : 'Scenarios'
      },
      {
        name : 'SETTINGS.ADDITIONAL_PANELS.MODULES',
        value : 'Modules'
      },
      {
        name : 'SETTINGS.ADDITIONAL_PANELS.CAMERAS' ,
        value : 'Cameras'
      }
    ]

    this.sections.forEach((section, index) => {
      this.observer.observe(section)
    })
  }

  callback = (entries) => {
    entries.forEach((entry) => {
      if (entry.intersectionRatio >= 0.25) {
        this.activePanel = entry.target.getAttribute('panel-data')
      }
    })
  }

  activate(panel: string) {
    let p;
    for (let i = 0; i < this.sections.length; i++) {
      if (this.sections[i].getAttribute('panel-data') === panel) p = this.sections[i];
    }
    p.scrollIntoView();
    window.scrollTo(0, 0);
  }
}
