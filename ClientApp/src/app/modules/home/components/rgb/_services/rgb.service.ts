import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

@Injectable({
    providedIn : 'root'
})

export class RGBService{
    _color$ = new BehaviorSubject<string>('');
    _preset_colors$ = new BehaviorSubject<string[]>([]);
    _addable$ = new BehaviorSubject<boolean>(true);
    _switch_checked$ = new BehaviorSubject<boolean>(false);

    fetchColors(){
        if(localStorage.getItem('preset-colors')){
            this._preset_colors$.next(JSON.parse(localStorage.getItem('preset-colors')).colors)

        }
    }

    deleteColor(color){

        let newObject = {
            colors : []
        }

        const newColors = this._preset_colors$.value.filter(col => col !== color)
        newObject.colors = newColors

        localStorage.setItem('preset-colors' , JSON.stringify(newObject))

        if(newColors.length < 4) this._addable$.next(true)

        this._preset_colors$.next(newColors)

    }
}
