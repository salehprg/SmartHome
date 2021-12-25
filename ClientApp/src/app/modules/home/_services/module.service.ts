import { HttpClient } from "@angular/common/http";
import { Injectable, OnDestroy } from "@angular/core";
import { BehaviorSubject, Subscription } from "rxjs";
import { environment } from "src/environments/environment";

@Injectable({
    providedIn : 'root'
})
export class ModuleService implements OnDestroy{
    private _subscriptions: Subscription[] = [];
    _modules$ = new BehaviorSubject<any>(null);

    constructor(private http: HttpClient){}

    fetchModules(){
        this.http.get(`${environment.apiUrl}/Module`).subscribe(
            (res:any) =>{
                const newRes = res.map(re => {return {...re , name : re.serialNumber}})
                this._modules$.next(newRes)
            },
            err => {}
        )
    }

    ngOnDestroy(){
        this._subscriptions.forEach(sb=> sb.unsubscribe())
    }
}
