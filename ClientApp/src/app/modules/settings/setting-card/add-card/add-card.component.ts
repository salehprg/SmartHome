import { Component, Input } from "@angular/core";
import { Router } from "@angular/router";
import { HomeService } from "src/app/modules/home/_services/home.service";

@Component({
    selector : 'sh-add-card' ,
    templateUrl : './add-card.component.html' ,
    styleUrls : ['./add-card.component.scss']
})

export class AddCardComponent {
    @Input() title : string ;
    @Input() isVertical : boolean;
    @Input() isScenario : boolean;
    @Input() addModule : boolean;
    @Input() addCamera : boolean;
    constructor(private route:Router , private home : HomeService){}

    handleRoute(){
        if(this.isScenario){
            this.route.navigateByUrl('/scenario/add')
        }

        else if(this.addModule){
            this.route.navigateByUrl('/settings/add/module')
        }
        else if(this.addCamera) this.route.navigateByUrl('/settings/addCamera')
        else{
            this.route.navigateByUrl('/settings/add')
        }
    }
}