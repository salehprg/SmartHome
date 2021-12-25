import { environment } from '../../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { LampModel } from '../../../../core/models/lamp.model';
import { NotifierService } from 'angular-notifier';
import { Router } from '@angular/router';
import { ScenarioSettingsService } from 'src/app/modules/scenario/scenario-settings.service';
import { HomeService } from '../../_services/home.service';

@Component({
  selector: 'sh-lamps',
  templateUrl: './lamps.component.html',
})
export class LampsComponent implements OnInit{
  @ViewChild('notif', { static: true }) notifTemp;
  @Input() lamps: any[];
  @Input() isScenarioSetting: boolean;

  constructor(
    private http: HttpClient,
    private notifierService: NotifierService,
    private router: Router,
    public scenarioSettings: ScenarioSettingsService ,
    private homeService : HomeService
  ) {

  }

  ngOnInit(){
    for(let lamp in this.lamps) {
      this.lamps[lamp]['isOn'] = this.lamps[lamp].status === 'false' ? false : true
    }
  }

  set(lamp: any) {
    const before = !lamp.isOn

    if (this.isScenarioSetting) {
      if (this.scenarioSettings.lampsCheck[lamp.id]) {
        this.scenarioSettings.addAction(lamp.id, lamp.isOn);
      }
    } else {
      this.http
        .patch(`${environment.apiUrl}/Device`, {
          Id : lamp.id ,
          status : lamp.isOn ? 'true' : 'false'
        }, {
          responseType: 'text',
        })
        .subscribe(
          (res) => {},
          (err) => {
            lamp.isOn = before;
            this.notifierService.show({
              type: 'error',
              message :'',
              template: this.notifTemp,
            });
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
      const l = this.lamps.find((l) => l.id === lampId);
      this.scenarioSettings.addAction(l.id, l.isOn);
    } else this.scenarioSettings.removeAction(lampId);
  }
}
