import { environment } from '../../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { LampModel } from '../../../../core/models/lamp.model';
import { NotifierService } from 'angular-notifier';
import { Router } from '@angular/router';
import { ScenarioSettingsService } from 'src/app/modules/scenario/scenario-settings.service';
import { HomeService } from '../../_services/home.service';

@Component({
  selector: 'sh-lamp',
  templateUrl: './lamp.component.html',
})
export class LampComponent implements OnInit{
  @ViewChild('notif', { static: true }) notifTemp;
  @Input() lamp: LampModel;
  @Input() isScenarioSetting: boolean;
  isLoading: boolean;

  constructor(
    private http: HttpClient,
    private notifierService: NotifierService,
    private router: Router,
    public scenarioSettings: ScenarioSettingsService ,
    public homeService : HomeService
  ) {

  }

  ngOnInit(){
    this.lamp.isOn = this.lamp.status !== 'false'
  }

  set() {
    const before = !this.lamp.isOn

    if (this.isScenarioSetting) {
      if (this.scenarioSettings.lampsCheck[this.lamp.id]) {
        this.scenarioSettings.addAction(this.lamp.id, this.lamp.isOn);
      }
    } else {
      this.homeService.lampIsBusy.next(true)
        this.isLoading = true;
      this.http
        .patch(`${environment.apiUrl}/Device`, {
          Id : this.lamp.id ,
          status : this.lamp.isOn ? 'true' : 'false'
        }, {
          responseType: 'text',
        })
        .subscribe(
          (res) => {
            this.isLoading = false;
            this.homeService.lampIsBusy.next(false);
          },
          (err) => {
            this.lamp.isOn = before;
            this.notifierService.show({
              type: 'error',
              message :'',
              template: this.notifTemp,
            });
            this.isLoading = false;
            this.homeService.lampIsBusy.next(false);
          }
        );
    }
  }

  goToDetails() {
    this.router.navigateByUrl('/details');
  }

  setActive(lampId, value) {
    this.scenarioSettings.lampsCheck[lampId] = value;

    if (value) {
      this.scenarioSettings.addAction(this.lamp.id, this.lamp.isOn);
    } else this.scenarioSettings.removeAction(lampId);
  }
}
