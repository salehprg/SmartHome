import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { ScenarioModel } from "../../../core/models/scenario.model";

@Component({
    selector : 'sh-scenario-card' ,
    templateUrl : './scenario-card.component.html' ,
    styleUrls : [ './scenario-card.component.scss']
})

export class ScenarioCardComponent implements OnInit {
    @Input() scenario : any;
    @Input() situation : number;
    @Output() changeScenarioSituation : EventEmitter<number> = new EventEmitter();
    constructor(){
        
    }

    ngOnInit(){        
        this.situation = 0 ;              
    }

    changeSituation(){
        
        this.changeScenarioSituation.emit(this.scenario.id);
    }
}