import { Component, Input } from "@angular/core";
import { DomSanitizer } from "@angular/platform-browser";

@Component({
    selector : 'sh-gs-card' , 
    templateUrl : './gs-card.component.html' ,
    styleUrls : [ './gs-card.component.scss' ]
})

export class GeneralSettingCard{

    constructor(private dom : DomSanitizer){}
    @Input() title : string ;
    @Input() imagePath : string;
    @Input() color : string;
    @Input() isDark : boolean;
}

