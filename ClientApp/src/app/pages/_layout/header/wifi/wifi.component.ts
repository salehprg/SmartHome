import { HttpClient } from "@angular/common/http";
import { Component, OnDestroy, OnInit } from "@angular/core";
import { environment } from "src/environments/environment";

@Component({
    selector : 'sh-wifi-selector' ,
    templateUrl : './wifi.component.html' ,
    styleUrls : ['./wifi.component.scss']
})

export class WifiSelectorComponent implements OnInit , OnDestroy{
    wifis : any;
    time : any;
    connectedWifi : string ;

    constructor(private http:HttpClient){}

    ngOnInit(){
        this.getWifis()

        this.time = setInterval(() => {
            this.getWifis()
        } , 10000)

    }

    ngOnDestroy(){
        clearInterval(this.time)
    }

    getWifis(){
        this.http.get(`${environment.apiUrl}/Wifi`).subscribe(
            res => {
                this.wifis = res
            },
            err =>{


            }
        )

        this.http.patch(`${environment.apiUrl}/Wifi` , {}).subscribe(
            (res:any) =>{
                this.connectedWifi = res.apName

            }
        )
    }

}
