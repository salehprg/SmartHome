import { HttpClient } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { DomSanitizer } from "@angular/platform-browser";
import { environment } from "src/environments/environment";

@Component({
  selector: 'sh-devices',
  templateUrl: './devices.component.html'
})
export class DevicesComponent implements OnInit{
  ip: string;
  wifi: string = '';
  param = {wifi : this.wifi}
  image  ;
  isLoading : boolean;
  hasError : boolean;

  constructor(private http: HttpClient , private sanitizer : DomSanitizer) {

  }

  ngOnInit(){
    this.isLoading = true
    this.hasError = false

    // this.http.get(`${envir}`)

    this.http.patch(`${environment.apiUrl}/Wifi` , {

    }).subscribe(
      (res:any) => {

        this.ip = res.ip
        this.wifi = res.apName
        this.image = this.sanitizer.bypassSecurityTrustHtml(`${res.image}`);
        this.isLoading = false
      } ,
      err => {
        this.hasError = true
      }

    )
  }
}
