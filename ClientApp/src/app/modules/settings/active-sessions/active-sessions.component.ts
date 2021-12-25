import { HttpClient } from "@angular/common/http";
import { Component, OnDestroy, OnInit } from "@angular/core";
import { of, Subscription } from "rxjs";
import { catchError, tap } from "rxjs/operators";
import { environment } from "src/environments/environment";

@Component({
    selector : 'sh-active-sessions' ,
    templateUrl : './active-sessions.component.html' ,
    styleUrls : ['./active-sessions.component.scss']
})

export class ActiveSessionsComponent implements OnInit , OnDestroy{
    sessions : any;
    _subscriptions : Subscription[] = []

    constructor(private http : HttpClient){}

    ngOnInit(){
        const request = this.http.get(`${environment.apiUrl}/active`).pipe(
            tap((res:any) =>{
                this.sessions = res
            }) ,
            catchError((err:any) =>{
                return of(null)
            })
        ).subscribe()

        this._subscriptions.push(request)
    }

    ngOnDestroy(){
        this._subscriptions.forEach(sb => sb.unsubscribe())
    }
}
