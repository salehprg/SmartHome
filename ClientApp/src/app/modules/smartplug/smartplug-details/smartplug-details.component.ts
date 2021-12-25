import { HttpClient } from "@angular/common/http";
import { Component, Input, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { environment } from "src/environments/environment";
import { HomeService } from "../../home/_services/home.service";

@Component({
    selector : 'sh-sp-details' ,
    templateUrl :'./smartplug-details.component.html' ,
    styleUrls : ['./smartplug-details.component.scss']
})

export class SmartPlugDetailsComponent implements OnInit{
    public id:number;
    name :string;
    price: number;

    constructor(private route : ActivatedRoute , private homeService: HomeService , private http: HttpClient){

    }

    ngOnInit(){

        this.http.post(`${environment.apiUrl}/Smartplug` ,{
            format : '24h' ,
            fromDate : '' ,
            toDate : ''
        }).subscribe(
            (res:any) => {
                this.name = res.outputs.find(o => o.baseDevice.id === +this.route.snapshot.params.id).baseDevice.name
              }
        )
    }


}
