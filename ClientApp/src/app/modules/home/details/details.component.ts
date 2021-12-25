import { Component } from "@angular/core";

@Component({
    selector : 'sh-details' ,
    templateUrl : './details.component.html' ,
    styleUrls : ['./details.component.scss']
})

export class DetailsComponent{
    constructor(){}

    restart()
    {
        window.location.reload(false)
    }
}