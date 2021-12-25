import { SocketService } from './../../../core/socket.service';
import { Injectable, OnDestroy } from '@angular/core';
import { BehaviorSubject, interval, of, Subscription } from 'rxjs';
import { catchError, tap, finalize } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { RoomModel } from '../../../core/models/room.model';
import { HomeModel } from '../../../core/models/home.model';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HomeService implements OnDestroy {
  _home$ = new BehaviorSubject<HomeModel>(null);
  _hasError$ = new BehaviorSubject<boolean>(false);
  _scenarios$ = new BehaviorSubject<any>(null);
  _smartPlugs$ = new BehaviorSubject<any>(null);
  _isRaspberry$ = new BehaviorSubject<boolean>(true);
  _selectedRoom$ = new BehaviorSubject<RoomModel>(null);
  _isLoading$ = new BehaviorSubject<boolean>(false);
  _scenarioIsLoading$ = new BehaviorSubject<boolean>(false);
  _errorMessage = new BehaviorSubject<string>('');
  lampIsBusy = new BehaviorSubject<boolean>(false);
  API_URL = `${environment.apiUrl}/Home`;
  private _subscriptions: Subscription[] = [];
  private windoors: any = [
    {
      id: 1,
      name: "Entrance",
      situation: 'Opened'
    },
    {
      id: 2,
      name: "Main Window",
      situation: "Closed"
    }
  ]

  constructor(
    private http: HttpClient,
    private socket: SocketService
  ) {
    this.fetchHome();

    this.socket.connection.on('devices', (from, body) => {
      console.log("next from socket", from, body);

      // this._home$.next(from);
      // if (this._selectedRoom$.value) {
      //   const sr = from.rooms.find(room => room.roomId = this._selectedRoom$.value.roomId);
      //   this._selectedRoom$.next(sr);
      // }
    });

    this._home$.subscribe(home => console.log(home));

  }

  getWindoors() {
    return this.windoors
  }


  fetchScenarios() {
    this._scenarioIsLoading$.next(true);
    this._errorMessage.next('');
    this._hasError$.next(false);
    const sRequest = this.http.get<any>(`${environment.apiUrl}/Schedule`).pipe(
      tap((res: any) => {
        const totalRes = res.map(re => {return {...re , situation : 0}})
        this._scenarios$.next(totalRes);
      }),
      catchError(err => {
        this._errorMessage.next(err.message);
        this._hasError$.next(true);
        return of(null);
      }),
      finalize(() => {
        this._scenarioIsLoading$.next(false);
      })
    ).subscribe();
    this._subscriptions.push(sRequest);
  }

  fetchSmartPlugs(){
    const req = this.http.get(`${environment.apiUrl}/Smartplug`).pipe(
      tap((res:any) => {
        this._smartPlugs$.next(res)
      }) ,
      catchError(err => {
        return of(null)
      })
    ).subscribe()

    this._subscriptions.push(req)
  }

  getSelectedScenarios(id){
    return this._scenarios$.value[id]
  }


  fetchHome() {
    this._isLoading$.next(true);
    this._errorMessage.next('');

    const num = interval(1000)
    const request = this.http.get<any>(this.API_URL).pipe(
      tap((res: HomeModel) => {
        console.log("next from get");

        this._home$.next(res);
      }),
      catchError(err => {
        this._errorMessage.next(err.message);
        return of(null);
      }),
      finalize(() => {
        this._isLoading$.next(false);
      })
    ).subscribe();
    this._subscriptions.push(request);
  }

  ngOnDestroy(): void {
    this._subscriptions.forEach(sb => sb.unsubscribe());
  }
}
