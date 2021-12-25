import { HttpClient } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { environment } from "src/environments/environment";

@Component({
    selector : 'sh-cameras-settings' ,
    templateUrl : './cameras-settings.component.html',
    styleUrls : ['./cameras-settings.component.scss']
})

export class CamerasSettingsComponent implements OnInit{
    cameras : any[] = []
    isLoading : boolean = false;
    hasError : boolean = false;

    constructor(private http: HttpClient){

    }

    remove(camera){
        const camId = this.cameras.findIndex(cam => cam.id === camera.id)

        this.cameras.splice(camId , 1)


    }

    ngOnInit(){
        this.isLoading =true

        this.http.get(`${environment.apiUrl}/Camera`).subscribe(
            (res:any) => {
                res.forEach(r =>{
                    this.cameras.push({...r , 'name' : r.cameraName})
                })
                this.isLoading = false
                this.hasError = false

            },
            err =>{
                this.isLoading = false
                this.hasError = true
            }
        )
    }

}
