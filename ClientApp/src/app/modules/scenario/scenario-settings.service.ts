import { HomeService } from './../home/_services/home.service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { NotifierService } from 'angular-notifier';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root',
})
export class ScenarioSettingsService {
  scenarioId: number = 0;
  scenario: any = {
    scenarioName: '',
    scenarioId: 0,
    cronjob: '0 0 0 ? 1/1 * *',
    running: false,
    deviceStats: [],
    actions: [],
  };
  editMode: boolean = false;
  lampsCheck: any = {};
  curtainsCheck: any = {};
  prevHome: any;

  constructor(
    private notifier: NotifierService,
    private http: HttpClient,
    private router: Router,
    private home: HomeService,
    private translate : TranslateService
  ) {
    this.setScenarioId(0);
   }

  setScenarioId(id: number) {
    this.scenarioId = id;
  }

  setEditMode(sc) {
    if (!sc) this.router.navigateByUrl('/settings');
    else {
      this.scenario = sc;
      this.prevHome = this.home._home$.value;
      this.editMode = true;

      const editedHome = this.home._home$.value;

      sc.actions.forEach((action) => {
        this.lampsCheck[action.deviceId] = action.status;
        this.curtainsCheck[action.deviceId] = action.status;

        editedHome.rooms.forEach((room) => {
          room.devices.leDs.forEach((leD) => {
            if (leD.id === action.deviceId) {
              if (action.status === '1') leD.isOn = true;
              else if (action.status === '0') leD.isOn = false;
            }
          });
          room.devices.curtains.forEach((curtain) => {
            if (curtain.id === action.deviceId) {
              curtain.range = +action.status;
            }
          });
        });
      });

      console.log("next from scenario setting");

      this.home._home$.next(editedHome);
    }
  }

  setScenarioName(value: string) {
    this.scenario.scenarioName = value;
  }

  addAction(deviceId, status) {
    const existing = this.scenario.actions.find((a) => a.deviceId === deviceId);
    if (existing) {
      existing.status = status.toString();
    } else {
      this.scenario.actions.push({
        scenarioId : +this.scenarioId ,
        deviceId,
        delay: 0.1,
        status: status.toString(),
      });
    }
  }

  removeAction(deviceId) {
    const index = this.scenario.actions.find((a) => a.deviceId === deviceId);
    this.scenario.actions.splice(index, 1);
  }

  save() {
    if (this.editMode) {

      this.http
        .post(`${environment.apiUrl}/Schedule`, this.scenario)
        .subscribe(
          (res) => {
            this.notifier.notify('success', this.translate.instant('NOTIFICATION.CHANGED_SUCCESSFULLY'))
          },
          (err) => {
            this.notifier.notify('error', this.translate.instant('NOTIFICATION.SCENARIO_EDIT_ERROR'))
          }
        );
    } else {
      this.http.put(`${environment.apiUrl}/Schedule`, {
        ...this.scenario,
        scenarioId: +this.scenarioId
      }).subscribe(
        (res) => {
          this.notifier.notify('success', this.translate.instant('NOTIFICATION.ADDED_SUCCESSFULLY'));
          this.router.navigateByUrl('/settings');
        },
        (err) => {
          this.notifier.notify('error', this.translate.instant('NOTIFICATION.SCENARIO_ADD_ERROR'))
        }
      );
    }
  }

  reset() {
    console.log("next from reset");

    this.home._home$.next(this.prevHome);
    this.home._selectedRoom$.next(null);
    this.lampsCheck = {};
    this.curtainsCheck = {};
    this.editMode = false;
  }
}
