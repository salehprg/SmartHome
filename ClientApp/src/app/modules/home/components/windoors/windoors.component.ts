import { Component, Input, OnInit } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";

@Component({
    selector : 'sh-windoors' ,
    templateUrl : './windoors.component.html' ,
    styleUrls : ['./windoors.component.scss']
})

export class WindoorsComponent implements OnInit{

    @Input() windoors : any;
    
    constructor(public translate : TranslateService){}

    ngOnInit(){
        
    }
}