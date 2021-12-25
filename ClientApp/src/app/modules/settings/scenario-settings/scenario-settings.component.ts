import { HttpClient } from "@angular/common/http";
import { ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { of, Subscription } from "rxjs";
import { catchError, finalize, tap } from "rxjs/operators";
import { environment } from "src/environments/environment";
import { HomeService } from "../../home/_services/home.service";

@Component({
    selector: 'sh-scenario-settings',
    templateUrl: './scenario-settings.component.html',
    styleUrls: ['./scenario-settings.component.scss']
})

export class ScenarioSettingsComponent implements OnInit, OnDestroy {
    constructor(private http: HttpClient, private _ref: ChangeDetectorRef, private homeService: HomeService) { }

    scenarios: any;
    hasError: boolean;
    isLoading: boolean;
    fetchedScenarios : any;
    _subscriptions: Subscription[] = [];
    ngOnInit() {
        this.isLoading = true
        this._ref.detectChanges()

        this.homeService.fetchScenarios()
        this.fetchedScenarios = this.homeService._scenarios$.subscribe(s => {
            this.scenarios = s

            for (let scenario in this.scenarios) {
                this.scenarios[scenario]['name'] = this.scenarios[scenario].scenarioName
                this.scenarios[scenario]['id'] = this.scenarios[scenario].scenarioId
            }
            this.isLoading = false
            this._ref.detectChanges()
        })
    }

    ngOnDestroy() {
        this._subscriptions.forEach(sb => sb.unsubscribe())
        this.fetchedScenarios.unsubscribe()
    }

}
