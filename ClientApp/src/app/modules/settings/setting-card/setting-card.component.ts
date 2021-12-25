import { HttpClient } from "@angular/common/http";
import { Component, Input, Output , OnInit, EventEmitter } from "@angular/core";
import { Router } from "@angular/router";
import { TranslateService } from "@ngx-translate/core";
import { NotifierService } from "angular-notifier";
import { environment } from "src/environments/environment";
import { HomeService } from "../../home/_services/home.service";
import { ModuleService } from "../../home/_services/module.service";

@Component({
    selector: 'sh-setting-card',
    templateUrl: './setting-card.component.html',
    styleUrls: ['./setting-card.component.scss']
})

export class SettingCardComponent implements OnInit {
    @Input() icon: string;
    @Input() prop: any;
    @Input() title: string;
    @Input() editable: boolean;
    @Input() notDelete: boolean;
    @Input() isVertical: boolean;
    @Output() removeCamera : EventEmitter<any> = new EventEmitter();
    isDelete: boolean;
    name: string;
    constructor(private router: Router,
        private homeService: HomeService,
        private moduleService : ModuleService,
        private notifierService: NotifierService,
        private http: HttpClient,
        public translate: TranslateService) {
        this.isDelete = false

    }

    ngOnInit() {
        if (this.prop.module) {
            this.name = this.prop.module.name
        }
        else {
            this.name = this.prop.name
        }

    }

    setToDelete() {
        if (!this.isDelete) {
            this.isDelete = true

            setTimeout(() => { this.isDelete = false }, 3000)
        }
        else {
            this.deleteItem()
        }
    }

    edit() {
        if (this.prop.deviceType === 1) {
            this.router.navigateByUrl(`settings/LED/${this.prop.id}`)
        }
        else if (this.prop.deviceType === 2) {
            this.router.navigateByUrl(`settings/Curtain/${this.prop.id}`)
        }
        else if (this.prop.deviceType === 3) {
            this.router.navigateByUrl(`settings/Windoor/${this.prop.id}`)
        }
        else if (this.prop.scenarioId) {
            this.router.navigateByUrl(`scenario/${this.prop.id}`)
        }
        else if(this.prop.baseDevice){
            this.router.navigateByUrl(`settings/SmartPlug/${this.prop.id}`)
        }
        else if(this.prop.moduleType !== null && this.prop.moduleType !== undefined){
            this.router.navigateByUrl(`settings/Module/${this.prop.id}`)
        }
        else {
            this.router.navigateByUrl(`settings/editCamera/${this.prop.id}`)
        }


    }

    deleteItem() {
        this.isDelete = false
        let type;

        if(this.prop.moduleType !== null && this.prop.moduleType !== undefined){
            type = 'Module'
        }
        else if(this.prop.scenarioId){
            type = 'Schedule'
        }
        else if(this.prop.deviceType ){

            type = 'Device'
        }
        else{
            type = 'Camera'
        }

        if(type !== 'Camera'){
            this.http.delete(`${environment.apiUrl}/${type}`, {
                params: {
                    id: this.prop.id
                }
            }).subscribe(
                (res:any) => {
                    const newHome = this.homeService._home$.value

                    if (this.prop.deviceType === 1) {
                        for (let room in this.homeService._home$.value.rooms) {

                            // let ledId = newHome.rooms[room].modules.leDs.findIndex(led => led.id === this.prop.id)


                            let ledId = newHome.rooms[room].devices.leDs.findIndex(led => led.id === this.prop.id)

                            if (ledId > -1) {
                                newHome.rooms[room].devices.leDs.splice(ledId, 1)
                            }

                        }
                    }
                    else if (this.prop.deviceType === 2) {
                        for (let room in this.homeService._home$.value.rooms) {


                            let curId = newHome.rooms[room].devices.curtains.findIndex(curtain => curtain.id === this.prop.id)

                            if (curId > -1) {
                                newHome.rooms[room].devices.curtains.splice(curId, 1)

                            }

                        }
                    }
                    else if (this.prop.deviceType === 3) {
                        for (let room in this.homeService._home$.value.rooms) {

                            let winId = newHome.rooms[room].devices.windoors.findIndex(win => win.id === this.prop.id)
                            if (winId > -1) {
                                newHome.rooms[room].devices.windoors.splice(winId, 1)
                            }

                        }
                    }
                    else if(this.prop.moduleType !== null && this.prop.moduleType !== undefined){
                        const newModules = this.moduleService._modules$.value
                        const module = newModules.findIndex(m => m.id === res.id)

                        newModules.splice(module , 1)

                        this.moduleService._modules$.next(newModules)

                    }
                    else {

                        const newScenarios = this.homeService._scenarios$.value

                        const scenarioId = newScenarios.findIndex(scenario => scenario.id === this.prop.id)
                        if (scenarioId > -1) {
                            newScenarios.splice(scenarioId, 1)
                        }
                    }
                    this.notifierService.notify('success', this.translate.instant('NOTIFICATION.DELETED_SUCCESSFULLY'))
                    this.homeService._home$.next(newHome)
                    console.log("next from setting card");


                },
                err => {
                    this.notifierService.notify('error' , this.translate.instant('NOTIFICATION.DELETE_ERROR'))
                }
            )
        }
        else{
            this.http.delete(`${environment.apiUrl}/Camera`,{
                params :{
                    camId : this.prop.id
                }
            }).subscribe(
                res=>{
                    this.removeCamera.emit(res)
                    this.notifierService.notify('success' , this.translate.instant('NOTIFICATION.DELETED_SUCCESSFULLY'))
                },
                err =>{
                    this.notifierService.notify('error' , this.translate.instant('NOTIFICATION.DELETE_ERROR'))

                }
            )
        }




    }
}
