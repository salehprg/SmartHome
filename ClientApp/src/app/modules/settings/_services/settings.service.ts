import { HttpClient } from "@angular/common/http";
import { Injectable, OnDestroy } from "@angular/core";
import { Subscription } from "rxjs";
import { environment } from "src/environments/environment";
import { HomeService } from "../../home/_services/home.service";

@Injectable({
    providedIn: 'root'
})
export class SettingsService implements OnDestroy {
    value: string;
    deviceType : string;

    module: any;
    private _subscriptions: Subscription[] = [];

    constructor(private http: HttpClient, private homeService: HomeService) { }

    setValue(val: string) {
        this.value = val
    }

    setModule(module: any) {
        this.module = module
    }

    setModuleType(type:string){
        this.deviceType = type
    }

    edit() {
        let type = this.module.deviceType === 1 ? 'LED' : 'Curtain'

        if (this.value && this.value.length > 0) {
            const request = this.http.post(`${environment.apiUrl}/${type}`, {
                id: this.module.id,
                name: this.value
            }).subscribe(
                res => {
                    let newHome = this.homeService._home$.value
                    let room = newHome.rooms.filter(room => room.roomId === this.module.roomId)

                    if (type === 'LED') {
                        let selectedModule = room[0].devices.leDs.filter(led => led.id === this.module.id)
                        selectedModule[0].name = this.value
                    }
                    else {
                        let selectedModule = room[0].devices.curtains.filter(curtain => curtain.id === this.module.id)
                        selectedModule[0].name = this.value
                    }
                    console.log("next from settings service");

                    this.homeService._home$.next(newHome)
                },
                err => {}

            )

            this._subscriptions.push(request)
        }
    }

    add(){

        const newHome = this.homeService._home$.value

        const roomId = newHome.rooms.findIndex(room => room.roomId === this.homeService._selectedRoom$.value.roomId)


        switch (this.deviceType) {
            case 'led':
                const addReq = this.http.put(`${environment.apiUrl}/LED` , {
                    name : this.value ,
                    roomId : roomId
                }).subscribe(
                    res => {} ,
                    err => {}
                )

                this._subscriptions.push(addReq)
                break;
            case 'curtain' :
                break;
            case 'windoor' :
                break;
            default:
                break;
        }
    }

    ngOnDestroy() {
        this._subscriptions.forEach(sb => sb.unsubscribe())
    }
}
