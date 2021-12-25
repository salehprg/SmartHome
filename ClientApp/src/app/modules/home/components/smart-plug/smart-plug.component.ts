import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HomeService } from '../../_services/home.service';

@Component({
  selector: 'sh-smart-plug',
  templateUrl: './smart-plug.component.html',
  styleUrls: ['./smart-plug.component.scss'],
})
export class SmartPlugComponent implements OnInit {
  @Input() id: number;
  @Input() smartplug: any;
  public isDangerous: boolean;
  public consumed: any;
  constructor(private homeService: HomeService , private http : HttpClient) {}

  ngOnInit() {
    this.http.post(`${environment.apiUrl}/Smartplug` , {
      format : '1h' ,
      fromDate : '' ,
      toDate : ''
    }).subscribe(
      (res:any)=>{
        let sp = res.outputs.find(o => o.baseDevice.id === this.id)
        this.consumed = sp.price
      }
    )
  }
}
