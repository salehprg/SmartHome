import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { RGBService } from "../_services/rgb.service";

@Component({
    selector: 'sh-rgb-preset-color',
    templateUrl: './preset-color.component.html',
    styleUrls: ['./preset-color.component.scss']
})

export class RgbPresetColorComponent implements OnInit {
    @Input() color: string;
    @Input() forAdd: boolean;
    @Output() changeChecked = new EventEmitter<string>();

    selectedColor: string;
    situation:number = 0;
    constructor(private rgbService: RGBService) {

    }

    ngOnInit() {
        this.rgbService._color$.subscribe(color => this.selectedColor = color)
    }

    check() {
        if (this.forAdd) {
            if (localStorage.getItem('preset-colors')) {
                if (this.selectedColor.length > 0) {
                    let localColors = JSON.parse(localStorage.getItem('preset-colors'))

                    const idx = localColors.colors.findIndex(c => c === this.selectedColor)

                    if (idx === -1 && localColors.colors.length < 4) {
                        localColors.colors.push(this.selectedColor)
                        localStorage.setItem('preset-colors', JSON.stringify(localColors))

                        if(localColors.colors.length >= 4){
                            this.rgbService._addable$.next(false)
                        }
                        
                    }

                    this.rgbService._preset_colors$.next(JSON.parse(localStorage.getItem('preset-colors')).colors)
                }
            }
        }
        else{
            this.situation++

            this.changeChecked.emit(this.color)

            if(this.situation === 1){
                this.rgbService._color$.next(this.color)
                this.rgbService._switch_checked$.next(true)

                setTimeout(()=>{
                    this.situation = 0
                } , 3000)
            }

            else if(this.situation === 2){

                setTimeout(()=> {
                    this.situation = 0
                } , 3000)
                
            }
            else if(this.situation === 3){
                this.rgbService.deleteColor(this.color)
            }
        }
    }
}