import { Component, Input, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { RGBService } from "./_services/rgb.service";

@Component({
    selector : 'sh-rgb-lamp' ,
    templateUrl : './rgb.component.html' ,
    styleUrls : ['./rgb.component.scss']
})

export class RgbLampComponent implements OnInit{
    @Input() isScenarioSetting:boolean;
    color : string = "#0100ff";
    presetColors : any ;
    addable : boolean = true;

    @Input() isOn : boolean ;

    constructor(private router:Router , private rgbService:RGBService){}

    ngOnInit(){

        this.rgbService.fetchColors()

        if(!localStorage.getItem('preset-colors')){
            let colors = {
                colors : []
            }
            localStorage.setItem('preset-colors' , JSON.stringify(colors))
        }

        if(JSON.parse(localStorage.getItem('preset-colors')).colors.length >= 4){
            this.rgbService._addable$.next(false)
        }
        this.rgbService._preset_colors$.subscribe(colors => this.presetColors = colors)
        this.rgbService._addable$.subscribe(addable => this.addable = addable)

        this.rgbService._color$.subscribe(color => this.color = color)
    }



    changeColor(event){
        this.rgbService._color$.next(event.color.hex)
         this.rgbService._switch_checked$.next(true)
    }

    doNothing(){}

    goToDetails(){
        this.router.navigateByUrl('/details')
      }
}
