import { Component, OnInit } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { HomeService } from "src/app/modules/home/_services/home.service";

@Component({
    selector : 'sh-all-windoors' ,
    templateUrl : './all-windoors.component.html' ,
    styleUrls : ['./all-windoors.component.scss']
})

export class AllWindoorsComponent implements OnInit{
    public openedWindoors : any[] = [];
    public closedWindoors : any[] = [];
    isEmpty : boolean;
    constructor(private homeService : HomeService , public translate : TranslateService){}

    ngOnInit(){
        let rooms = this.homeService._home$.value.rooms
        let allWindoors = []

        for(let room in rooms){
            if(rooms[room].devices.windoors.length > 0){
                for(let windoor in rooms[room].devices.windoors){
                    allWindoors.push(rooms[room].devices.windoors[windoor])
                }
            }
        }

        if(allWindoors.length === 0){
            this.isEmpty = true
        }

        for(let win in allWindoors){
            if(allWindoors[win].isOpen === true){
                this.openedWindoors.push(allWindoors[win])
            }
            else{
                this.closedWindoors.push(allWindoors[win])
            }
        }

    }
}
