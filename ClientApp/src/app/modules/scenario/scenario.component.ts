import { HomeService } from '../home/_services/home.service';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, NgZone, OnDestroy, OnInit } from '@angular/core';
import { ScenarioModel } from '../../core/models/scenario.model';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { catchError, finalize, tap } from 'rxjs/operators';
import { of, Subscription } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { HomeModel } from '../../core/models/home.model'


@Component({
    selector: 'sh-scenarios',
    templateUrl: './scenario.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class ScenariosComponent implements OnInit, OnDestroy {
    public scenarios: any;
    fetchedScenarios: any[] = [];
    private _subscriptions: Subscription[] = [];
    isLoading: boolean;
    hasError: boolean;
    constructor(public homeService: HomeService,
        private http: HttpClient,
        private _ref: ChangeDetectorRef ,
        public translate : TranslateService) { }

    ngOnInit() {
        this.isLoading = true
        this._ref.detectChanges()

        this.homeService.fetchScenarios()

        this.fetchedScenarios.push(this.homeService._scenarios$.subscribe(s => {
            this.scenarios = s
            this.isLoading = false
            this._ref.detectChanges()
        }))
    }

    selectScenario(id) {
        let selectedScenario = this.scenarios.find(s => s.scenarioId === id)

        if(selectedScenario.situation !== 3){
            selectedScenario.situation++;
        }

        for (let i = 0; i < this.scenarios.length; i++) {
            if (this.scenarios[i].scenarioId !== selectedScenario.scenarioId) {
                this.scenarios[i].situation = 0;
            }
        }

        if (selectedScenario.situation === 1) {
            setTimeout(() => {
                selectedScenario.situation = 0;
                this._ref.detectChanges()
            }, 3000)
        }
        else if (selectedScenario.situation === 2) {
            const setScenario = this.http.patch(`${environment.apiUrl}/Schedule` , {
                scenarioId : id
            }).subscribe(
                (res:HomeModel) => {
                    res.rooms.forEach(room => {
                        room.devices.leDs.forEach(lamp => {
                            lamp['isOn'] = lamp.status !== '0'
                        })
                    })
                    console.log("next from scenario");

                    this.homeService._home$.next(res)

                    if (this.homeService._selectedRoom$.value) {
                      const sr = res.rooms.find(r => r.roomId === this.homeService._selectedRoom$.value.roomId)

                    this.homeService._selectedRoom$.next(sr)
                    }
                    selectedScenario.situation = 3
                    this._ref.detectChanges()

                    setTimeout(() => {
                        selectedScenario.situation = 0;
                        this._ref.detectChanges()
                        // this.scenarios[id-1].situation = 0;
                        // for(let scenario in this.scenarios){
                        //   // this.scenarios[scenario].situation = 0;
                        // }

                    }, 3000)

                } ,
                err => {
                    selectedScenario.situation = 4
                    this._ref.detectChanges()

                    setTimeout(() => {
                        selectedScenario.situation = 0;
                        this._ref.detectChanges()
                        // this.scenarios[id-1].situation = 0;
                        // for(let scenario in this.scenarios){
                        //   // this.scenarios[scenario].situation = 0;
                        // }

                    }, 3000)
                }

            );

            this._subscriptions.push(setScenario);


        }

    }

    ngOnDestroy() {
        this._subscriptions.forEach(sb => sb.unsubscribe());
        this.fetchedScenarios.forEach(s => s.unsubscribe());
    }
}
