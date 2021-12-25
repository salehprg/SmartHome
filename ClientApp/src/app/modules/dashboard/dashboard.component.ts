import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { NotifierService } from 'angular-notifier';
import { environment } from 'src/environments/environment';
import { HomeService } from '../home/_services/home.service';
@Component({
  selector: 'sh-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss' ]
})
export class DashboardComponent implements OnInit{

  constructor(public translate : TranslateService , private http: HttpClient , private homeService: HomeService){

  }

  ngOnInit(){
    
  }

}
