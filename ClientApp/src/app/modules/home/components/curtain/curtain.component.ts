import { SliderComponent } from '../../../../partials/general/slider/slider.component';
import { environment } from './../../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { CurtainModel } from './../../../../core/models/curtain.model';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NotifierService } from 'angular-notifier';
import { Router } from '@angular/router';
import { ScenarioSettingsService } from 'src/app/modules/scenario/scenario-settings.service';

@Component({
  selector: 'sh-curtain',
  templateUrl: './curtain.component.html',
  styleUrls: ['./curtain.component.scss'],
})
export class CurtainComponent implements OnInit {
  @ViewChild('notif', { static: true }) notifTemp;
  @Input() curtain: CurtainModel;
  @Input() isScenarioSetting: boolean;
  @ViewChild('slider', { static: false }) slider: SliderComponent;
  before: number;

  constructor(
    private http: HttpClient,
    private notifierService: NotifierService,
    private router: Router,
    private scenarioSettings: ScenarioSettingsService
  ) {}

  ngOnInit() {
    this.before = this.curtain.range;
  }

  set() {
    if (this.isScenarioSetting) {
      if (this.scenarioSettings.curtainsCheck[this.curtain.id]) {
        this.scenarioSettings.addAction(this.curtain.id, this.curtain.range);
      }
    } else {
      this.http
        .post(`${environment.apiUrl}/curtain`, this.curtain, {
          responseType: 'text',
        })
        .subscribe(
          (res) => {
            this.before = this.curtain.range;
          },
          (err) => {
            this.curtain.range = this.before;
            this.slider.slide(this.before);
            this.notifierService.show({
              type: 'error',
              message: '',
              template: this.notifTemp,
            });
          }
        );
    }
  }

  // stops scroll behavior on sliding!!!!
  doNothing() {}

  goToDetails() {
    this.router.navigateByUrl('/details');
  }

  setActive(value) {
    this.scenarioSettings.curtainsCheck[this.curtain.id] = value;

    if (value) this.scenarioSettings.addAction(this.curtain.id, this.curtain.range);
    else this.scenarioSettings.removeAction(this.curtain.id);
  }
}
