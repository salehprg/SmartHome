import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Console } from "console";
import { environment } from "src/environments/environment";

@Injectable({
    providedIn: 'root'
})

export class WifiService {
    // connectedWifi: any;
    name : string;

    constructor(private http: HttpClient) { }

    verify(pass) {
        return this.http.post(`${environment.apiUrl}/Wifi`, {
            apname: this.name,
            password: pass
        })
    }

    setWifiName(name){
        this.name = name
    }

    disconnect() {
        // this.connectedWifi = null
    }
}
